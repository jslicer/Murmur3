// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3H.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the Murmur3 16-bit hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3;

using System.Buffers.Binary;
using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/// <inheritdoc />
/// <summary>
/// "Murmur3H": a 16-bit (half-width) variant of Murmur3, designed to run efficiently on a 16-bit CPU such as the
/// TMS9900. Every operation is a 16-bit add, xor, shift, rotate or 16x16->16 multiply. Blocks are 2 bytes, read
/// little-endian; the result is a 16-bit hash written little-endian.
/// </summary>
/// <remarks>
/// <para>
///   Not the same function as any published Murmur3 variant. Constants were chosen by avalanche search (see
///   MURMUR3H.md). The length is folded in modulo 2^16.
/// </para>
/// <para>
///   Implementation note: this class computes in 32-bit registers because that is what the host CPU is fastest at.
///   Add, multiply, xor and left shift never let high bits influence low bits, so only the low 16 bits of any
///   intermediate value are meaningful and the upper 16 bits are ignored. Values are truncated to 16 bits only where
///   it matters: before a right shift and when the result is stored or written out.
/// </para>
/// </remarks>
/// <seealso cref="Murmur3Base" />
public sealed class Murmur3H : Murmur3Base
{
    /// <summary>
    /// First hash multiplication constant.
    /// </summary>
    private const ushort C1 = 0x49DB;

    /// <summary>
    /// Second hash multiplication constant.
    /// </summary>
    private const ushort C2 = 0x3C4F;

    /// <summary>
    /// Additive constant applied after each block (the low half of Murmur3-32's 0xE6546B64).
    /// </summary>
    private const ushort N = 0x6B64;

    /// <summary>
    /// First finalization multiplication constant.
    /// </summary>
    private const ushort F1 = 0xA42B;

    /// <summary>
    /// Second finalization multiplication constant.
    /// </summary>
    private const ushort F2 = 0xE6CB;

    /// <summary>
    /// The number of bits the mixed block is rotated by in <see cref="MixK" />.
    /// </summary>
    private const byte R1 = 7;

    /// <summary>
    /// The number of bits the running hash is rotated by in <see cref="MixBlock" />.
    /// </summary>
    private const byte R2 = 6;

    /// <summary>
    /// The size, in bytes, of a single hash block.
    /// </summary>
    private const int BlockSizeInBytes = 2;

    /// <summary>
    /// The size, in bytes, of the wide read used to process two blocks per loop iteration.
    /// </summary>
    private const int WideSizeInBytes = 4;

    /// <summary>
    /// The hash value.
    /// </summary>
    private ushort _h;

    /// <summary>
    /// The pending low byte of a block that was split across calls to <see cref="Append" />.
    /// </summary>
    private byte _tail;

    /// <summary>
    /// A value indicating whether <see cref="_tail" /> holds a pending byte.
    /// </summary>
    private bool _hasTail;

    /// <summary>
    /// Initializes a new instance of the <see cref="Murmur3H" /> class.
    /// </summary>
    /// <param name="seed">The seed value (only the low 16 bits are used).</param>
    public Murmur3H(int seed = 0x0000)
        : base(16, seed) =>
        Init();

    /// <inheritdoc />
    /// <summary>
    /// Initializes an implementation of the <see cref="Murmur3H" /> class.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void Reset() => Init();

    /// <inheritdoc />
    /// <summary>
    ///   When overridden in a derived class,
    ///   appends the contents of <paramref name="source" /> to the data already
    ///   processed for the current hash computation.
    /// </summary>
    /// <param name="source">The data to process.</param>
    /// <exception cref="ArgumentOutOfRangeException">start is less than zero or greater than
    /// <see cref="Span{T}" />.</exception>
    /// <exception cref="OverflowException">The Length property of the new <see cref="ReadOnlySpan{T}" /> would exceed
    /// MaxValue.</exception>
    /// <exception cref="ArgumentException">TFrom or TTo contains managed object references.</exception>
    // ReSharper disable once MethodTooLong
    public override void Append(ReadOnlySpan<byte> source)
    {
        Length += source.Length;
        if (source.IsEmpty)
        {
            return;
        }

        // Keep the running hash in a local so it stays in a register for the whole loop instead of round-tripping
        // through the field on every block.
        uint h = _h;

        // Complete a block that was split across calls: the pending byte is the low half, the first new byte the high.
        if (_hasTail)
        {
            // ReSharper disable once ComplexConditionExpression
            h = MixBlock(h, (uint)(_tail | (source[0] << 8)));
            source = source[1..];
            _hasTail = false;
        }

        int remainder = source.Length & (BlockSizeInBytes - 1);
        int alignedLength = source.Length - remainder;

        // ReSharper disable once ComplexConditionExpression
        int wideLength = alignedLength & ~(WideSizeInBytes - 1);

        // Two blocks per iteration. MixBlock only looks at the low 16 bits of its block argument, so the low block is
        // passed as-is and the high block is the upper half shifted down.
        foreach (uint wide in MemoryMarshal.Cast<byte, uint>(source[..wideLength]))
        {
            // Blocks are defined as little-endian; swap on big-endian hosts so the output never changes.
            uint pair = BitConverter.IsLittleEndian ? wide : BinaryPrimitives.ReverseEndianness(wide);

            h = MixBlock(h, pair);
            h = MixBlock(h, pair >> 16);
        }

        // At most one whole 2-byte block can be left over after the wide loop.
        if (wideLength < alignedLength)
        {
            h = MixBlock(h, BinaryPrimitives.ReadUInt16LittleEndian(source[wideLength..]));
        }

        _h = (ushort)h;
        if (remainder > 0)
        {
            Tail(source, alignedLength);
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///   When overridden in a derived class,
    ///   writes the computed hash value to <paramref name="destination" />
    ///   without modifying accumulated state.
    /// </summary>
    /// <param name="destination">The buffer that receives the computed hash value.</param>
    /// <remarks>
    ///   <para>
    ///     Implementations of this method must write exactly
    ///     <see cref="NonCryptographicHashAlgorithm.HashLengthInBytes" /> bytes to <paramref name="destination" />.
    ///     Do not assume that the buffer was zero-initialized.
    ///   </para>
    ///   <para>
    ///     The <see cref="NonCryptographicHashAlgorithm" /> class validates the
    ///     size of the buffer before calling this method, and slices the span
    ///     down to be exactly <see cref="NonCryptographicHashAlgorithm.HashLengthInBytes" /> in length.
    ///   </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination" /> is too small to contain a
    /// <see cref="ushort" />.</exception>
    protected override void GetCurrentHashCore(Span<byte> destination)
    {
        ushort h = _h;

        // Fold in a pending single byte without modifying the accumulated state.
        if (_hasTail)
        {
            h ^= (ushort)MixK(_tail);
        }

        // The length is folded in modulo 2^16 by design.
        ushort length = unchecked((ushort)Length);

        BinaryPrimitives.WriteUInt16LittleEndian(destination, FMix((ushort)(h ^ length)));
    }

    /// <inheritdoc />
    /// <summary>
    /// Initializes the hash for this instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void Init()
    {
        _h = unchecked((ushort)Seed);
        _tail = 0;
        _hasTail = false;
        base.Init();
    }

    /// <summary>
    /// Rotates the low 16 bits of a value left by the given amount.
    /// </summary>
    /// <param name="x">The value to rotate; only its low 16 bits are significant.</param>
    /// <param name="r">The number of bits to rotate (maximum 16 bits).</param>
    /// <returns>A value whose low 16 bits are the rotated result; the upper 16 bits are undefined.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint RotateLeft(uint x, byte r) => (x << r) | ((x & 0xFFFF) >> (16 - r));

    /// <summary>
    /// Mixes a single 16-bit block: multiply by <see cref="C1" />, rotate left by <see cref="R1" />, then multiply
    /// by <see cref="C2" />.
    /// </summary>
    /// <param name="k">The block to mix; only its low 16 bits are significant.</param>
    /// <returns>The mixed block in the low 16 bits; the upper 16 bits are undefined.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint MixK(uint k)
    {
        unchecked
        {
            k *= C1;
            k = RotateLeft(k, R1);
            return k * C2;
        }
    }

    /// <summary>
    /// Folds a full 16-bit block into the running hash.
    /// </summary>
    /// <param name="h">The running hash value; only its low 16 bits are significant.</param>
    /// <param name="k">The block to fold in; only its low 16 bits are significant.</param>
    /// <returns>The updated hash in the low 16 bits; the upper 16 bits are undefined.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint MixBlock(uint h, uint k)
    {
        unchecked
        {
            h ^= MixK(k);
            h = RotateLeft(h, R2);
            return (h * 5) + N;
        }
    }

    /// <summary>
    /// Finalization mix - force all bits of a hash block to avalanche.
    /// </summary>
    /// <param name="h">The value to mix.</param>
    /// <returns>The mixed value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ushort FMix(ushort h)
    {
        unchecked
        {
            h ^= (ushort)(h >> 8);
            h = (ushort)(h * F1);
            h ^= (ushort)(h >> 7);
            h = (ushort)(h * F2);
            h ^= (ushort)(h >> 9);
            return h;
        }
    }

    /// <summary>
    /// Stores the remaining byte (the "tail") of an incomplete block so it can be completed by the next
    /// <see cref="Append" /> call, or folded in by <see cref="GetCurrentHashCore" />.
    /// </summary>
    /// <param name="tail">The read-only span of bytes being hashed.</param>
    /// <param name="position">The position in the read-only span of bytes where the tail starts.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Tail(ReadOnlySpan<byte> tail, int position)
    {
        _tail = tail[position];
        _hasTail = true;
    }
}