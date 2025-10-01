// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3F.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the Murmur3 128 x64 hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3;

using System;
using System.Buffers.Binary;
using System.IO.Hashing;
using System.Runtime.CompilerServices;

using static System.BitConverter;

/// <inheritdoc />
/// <summary>
/// Implements the Murmur3 128 x64 hashing algorithm variant.
/// </summary>
/// <seealso cref="Murmur3Base" />
public sealed class Murmur3F : Murmur3Base
{
    /// <summary>
    /// First hash multiplication constant.
    /// </summary>
    private const ulong C1 = 0x87C37B91114253D5UL;

    /// <summary>
    /// Second hash multiplication constant.
    /// </summary>
    private const ulong C2 = 0x4CF5AD432745937FUL;

    /// <summary>
    /// The hash value, part 1.
    /// </summary>
    private ulong _h1;

    /// <summary>
    /// The hash value, part 2.
    /// </summary>
    private ulong _h2;

    /// <summary>
    /// Initializes a new instance of the <see cref="Murmur3F" /> class.
    /// </summary>
    /// <param name="seed">The seed value.</param>
    public Murmur3F(in int seed = 0x00000000)
        : base(128, seed) =>
        Init();

    /// <inheritdoc />
    /// <summary>
    /// Initializes an implementation of the <see cref="Murmur3Base" /> class.
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
    // ReSharper disable once MethodTooLong
    public override void Append(ReadOnlySpan<byte> source)
    {
        Length += source.Length;

        const int BlockSizeInBytes = 16;
        int remainder = source.Length & (BlockSizeInBytes - 1);
        int alignedLength = source.Length - remainder;

        for (int i = 0; i < alignedLength; i += BlockSizeInBytes)
        {
            ulong k1 = BinaryPrimitives.ReadUInt64LittleEndian(source.Slice(i, 8));
            ulong k2 = BinaryPrimitives.ReadUInt64LittleEndian(source.Slice(i + 8, 8));

            _h1 ^= C2 * RotateLeft(C1 * k1, 31);
            _h1 = RotateLeft(_h1, 27);
            _h1 += _h2;
            _h1 = (5 * _h1) + 0x0000000052DCE729UL;

            _h2 ^= C1 * RotateLeft(C2 * k2, 33);
            _h2 = RotateLeft(_h2, 31);
            _h2 += _h1;
            _h2 = (5 * _h2) + 0x0000000038495AB5UL;
        }

        if (remainder > 0)
        {
            Tail(source, alignedLength, remainder);
        }

        _h1 ^= (ulong)Length;
        _h2 ^= (ulong)Length;

        _h1 += _h2;
        _h2 += _h1;

        _h1 = FMix(_h1);
        _h2 = FMix(_h2);

        _h1 += _h2;
        _h2 += _h1;
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
    // ReSharper disable once MethodTooLong
    protected override void GetCurrentHashCore(Span<byte> destination)
    {
        byte[] b1 = GetBytes(_h1);
        byte[] b2 = GetBytes(_h2);
        byte[] bytes = new byte[b1.Length + b2.Length];

        b1.CopyTo(bytes, 0);
        b2.CopyTo(bytes, b1.Length);
        bytes.CopyTo(destination);
    }

    /// <summary>
    /// Rotates the bits left in an unsigned long.
    /// </summary>
    /// <param name="x">The value to rotate.</param>
    /// <param name="r">The number of bits to rotate (maximum 64 bits).</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static ulong RotateLeft(in ulong x, in byte r) => (x << r) | (x >> (64 - r));

    /// <summary>
    /// Finalization mix - force all bits of a hash block to avalanche.
    /// </summary>
    /// <param name="k">The value to mix.</param>
    /// <returns>The mixed value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static ulong FMix(in ulong k)
    {
        //// ReSharper disable ComplexConditionExpression
        ulong k1 = 0xFF51AFD7ED558CCDUL * (k ^ (k >> 33));
        ulong k2 = 0xC4CEB9FE1A85EC53UL * (k1 ^ (k1 >> 33));
        //// ReSharper restore ComplexConditionExpression

        return k2 ^ (k2 >> 33);
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
    private void Tail(in ReadOnlySpan<byte> tail, in int position, in int remainder)
    {
        ulong k1 = 0x0000000000000000UL;
        ulong k2 = 0x0000000000000000UL;

        switch (remainder)
        {
            case 15:
                k2 ^= (ulong)tail[position + 14] << 48;
                k2 ^= (ulong)tail[position + 13] << 40;
                k2 ^= (ulong)tail[position + 12] << 32;
                k2 ^= (ulong)tail[position + 11] << 24;
                k2 ^= (ulong)tail[position + 10] << 16;
                k2 ^= (ulong)tail[position + 9] << 8;
                k2 ^= tail[position + 8];
                _h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 14:
                k2 ^= (ulong)tail[position + 13] << 40;
                k2 ^= (ulong)tail[position + 12] << 32;
                k2 ^= (ulong)tail[position + 11] << 24;
                k2 ^= (ulong)tail[position + 10] << 16;
                k2 ^= (ulong)tail[position + 9] << 8;
                k2 ^= tail[position + 8];
                _h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 13:
                k2 ^= (ulong)tail[position + 12] << 32;
                k2 ^= (ulong)tail[position + 11] << 24;
                k2 ^= (ulong)tail[position + 10] << 16;
                k2 ^= (ulong)tail[position + 9] << 8;
                k2 ^= tail[position + 8];
                _h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 12:
                k2 ^= (ulong)tail[position + 11] << 24;
                k2 ^= (ulong)tail[position + 10] << 16;
                k2 ^= (ulong)tail[position + 9] << 8;
                k2 ^= tail[position + 8];
                _h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 11:
                k2 ^= (ulong)tail[position + 10] << 16;
                k2 ^= (ulong)tail[position + 9] << 8;
                k2 ^= tail[position + 8];
                _h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 10:
                k2 ^= (ulong)tail[position + 9] << 8;
                k2 ^= tail[position + 8];
                _h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 9:
                k2 ^= tail[position + 8];
                _h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 8:
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 7:
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 6:
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 5:
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 4:
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 3:
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 2:
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 1:
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
        }
    }
}