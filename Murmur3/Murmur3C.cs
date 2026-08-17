// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3C.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the Murmur3 128 x86 hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3;

using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

/// <inheritdoc />
/// <summary>
/// Implements the Murmur3 128 x86 hashing algorithm variant.
/// </summary>
/// <seealso cref="Murmur3Base" />
public sealed class Murmur3C : Murmur3Base
{
    /// <summary>
    /// First hash multiplication constant.
    /// </summary>
    private const uint C1 = 0x239B961BU;

    /// <summary>
    /// Second hash multiplication constant.
    /// </summary>
    private const uint C2 = 0xAB0E9789U;

    /// <summary>
    /// Third hash multiplication constant.
    /// </summary>
    private const uint C3 = 0x38B34AE5U;

    /// <summary>
    /// Fourth hash multiplication constant.
    /// </summary>
    private const uint C4 = 0xA1E38B93U;

    /// <summary>
    /// The hash value, part 1.
    /// </summary>
    private uint _h1;

    /// <summary>
    /// The hash value, part 2.
    /// </summary>
    private uint _h2;

    /// <summary>
    /// The hash value, part 3.
    /// </summary>
    private uint _h3;

    /// <summary>
    /// The hash value, part 4.
    /// </summary>
    private uint _h4;

    /// <summary>
    /// Initializes a new instance of the <see cref="Murmur3C" /> class.
    /// </summary>
    /// <param name="seed">The seed value.</param>
    /// <exception cref="ArgumentOutOfRangeException">hashLengthInBytes is less than 1.</exception>
    public Murmur3C(int seed = 0x00000000)
        : base(128, seed) =>
        Init();

    /// <inheritdoc />
    /// <summary>
    /// Initializes an implementation of the <see cref="Murmur3C" /> class.
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
    /// <see cref="Span{T}.Length" />.</exception>
    /// <exception cref="OverflowException">The Length property of the new <see cref="ReadOnlySpan{T}" /> would exceed
    /// MaxValue.</exception>
    /// <exception cref="ArgumentException">TFrom or TTo contains managed object references.</exception>
    /// <exception cref="IndexOutOfRangeException">index is less than zero or greater than or equal to
    /// <see cref="ReadOnlySpan{T}.Length" />.</exception>
    // ReSharper disable once MethodTooLong
    public override void Append(ReadOnlySpan<byte> source)
    {
        Length += source.Length;

        // ReSharper disable once InconsistentNaming
        const int BlockSizeInBytes = 16;
        int remainder = source.Length & (BlockSizeInBytes - 1);
        int alignedLength = source.Length - remainder;
        ReadOnlySpan<uint> blocks = MemoryMarshal.Cast<byte, uint>(source[..alignedLength]);

        for (int i = 0; i < blocks.Length; i += 4)
        {
            uint k1 = blocks[i];
            uint k2 = blocks[i + 1];
            uint k3 = blocks[i + 2];
            uint k4 = blocks[i + 3];

            _h1 ^= C2 * RotateLeft(C1 * k1, 15);
            _h1 = RotateLeft(_h1, 19);
            _h1 += _h2;
            _h1 = (5 * _h1) + 0x561CCD1BU;

            _h2 ^= C3 * RotateLeft(C2 * k2, 16);
            _h2 = RotateLeft(_h2, 17);
            _h2 += _h3;
            _h2 = (5 * _h2) + 0x0BCAA747U;

            _h3 ^= C4 * RotateLeft(C3 * k3, 17);
            _h3 = RotateLeft(_h3, 15);
            _h3 += _h4;
            _h3 = (5 * _h3) + 0x96CD1C35U;

            _h4 ^= C1 * RotateLeft(C4 * k4, 18);
            _h4 = RotateLeft(_h4, 13);
            _h4 += _h1;
            _h4 = (5 * _h4) + 0x32AC3B17U;
        }

        if (remainder > 0)
        {
            Tail(source, alignedLength, remainder);
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Initializes the hash for this instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override void Init()
    {
        _h1 = Seed;
        _h2 = Seed;
        _h3 = Seed;
        _h4 = Seed;
        base.Init();
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
    /// <exception cref="OverflowException">The Length property of the new <see cref="ReadOnlySpan{T}" /> would exceed
    /// MaxValue.</exception>
    /// <exception cref="ArgumentException">TFrom or TTo contains managed object references.</exception>
    /// <exception cref="IndexOutOfRangeException">index is less than zero or greater than or equal to
    /// <see cref="Span{T}.Length" />.</exception>
    // ReSharper disable once MethodTooLong
    protected override void GetCurrentHashCore(Span<byte> destination)
    {
        uint h1 = _h1;
        uint h2 = _h2;
        uint h3 = _h3;
        uint h4 = _h4;

        h1 ^= (uint)Length;
        h2 ^= (uint)Length;
        h3 ^= (uint)Length;
        h4 ^= (uint)Length;

        h1 += h2;
        h1 += h3;
        h1 += h4;

        h2 += h1;
        h3 += h1;
        h4 += h1;

        h1 = FMix(h1);
        h2 = FMix(h2);
        h3 = FMix(h3);
        h4 = FMix(h4);

        h1 += h2;
        h1 += h3;
        h1 += h4;

        h2 += h1;
        h3 += h1;
        h4 += h1;

        Span<uint> writer = MemoryMarshal.Cast<byte, uint>(destination);

        writer[0] = h1;
        writer[1] = h2;
        writer[2] = h3;
        writer[3] = h4;
    }

    /// <summary>
    /// Rotates the bits left in an unsigned int.
    /// </summary>
    /// <param name="x">The value to rotate.</param>
    /// <param name="r">The number of bits to rotate (maximum 32 bits).</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static uint RotateLeft(uint x, byte r) => (x << r) | (x >> (32 - r));

    /// <summary>
    /// Finalization mix - force all bits of a hash block to avalanche.
    /// </summary>
    /// <param name="k">The value to mix.</param>
    /// <returns>The mixed value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static uint FMix(uint k)
    {
        //// ReSharper disable ComplexConditionExpression
        uint k1 = 0x85EBCA6BU * (k ^ (k >> 16));
        uint k2 = 0xC2B2AE35U * (k1 ^ (k1 >> 13));
        //// ReSharper restore ComplexConditionExpression

        return k2 ^ (k2 >> 16);
    }

    /// <summary>
    /// Processes the remaining bytes (the "tail") of an aligned block.
    /// </summary>
    /// <param name="tail">The read-only span of bytes being hashed.</param>
    /// <param name="position">The position in the read-only span of bytes where the tail starts.</param>
    /// <param name="remainder">The number of bytes remaining to process.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //// ReSharper disable once MethodTooLong
    //// ReSharper disable once CognitiveComplexity
    private void Tail(ReadOnlySpan<byte> tail, int position, int remainder)
    {
        if (remainder == 0)
        {
            return;
        }

        Span<byte> buffer = stackalloc byte[16];

        buffer.Clear();
        tail.Slice(position, remainder).CopyTo(buffer);

        uint k1 = MemoryMarshal.Read<uint>(buffer);
        uint k2 = MemoryMarshal.Read<uint>(buffer[4..]);
        uint k3 = MemoryMarshal.Read<uint>(buffer[8..]);
        uint k4 = MemoryMarshal.Read<uint>(buffer[12..]);

        if (remainder > 12)
        {
            _h4 ^= C1 * RotateLeft(C4 * k4, 18);
        }

        if (remainder > 8)
        {
            _h3 ^= C4 * RotateLeft(C3 * k3, 17);
        }

        if (remainder > 4)
        {
            _h2 ^= C3 * RotateLeft(C2 * k2, 16);
        }

        _h1 ^= C2 * RotateLeft(C1 * k1, 15);
    }
}