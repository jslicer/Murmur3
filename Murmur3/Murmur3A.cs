// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3A.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the Murmur3 32 x86 hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3;

using System;
using System.Buffers.Binary;
using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using static System.BitConverter;

/// <inheritdoc />
/// <summary>
/// Implements the Murmur3 32 x86 hashing algorithm variant.
/// </summary>
/// <seealso cref="Murmur3Base" />
public sealed class Murmur3A : Murmur3Base
{
    /// <summary>
    /// First hash multiplication constant.
    /// </summary>
    private const uint C1 = 0xCC9E2D51U;

    /// <summary>
    /// Second hash multiplication constant.
    /// </summary>
    private const uint C2 = 0x1B873593U;

    /// <summary>
    /// The hash value.
    /// </summary>
    private uint _h1;

    /// <summary>
    /// Initializes a new instance of the <see cref="Murmur3A" /> class.
    /// </summary>
    /// <param name="seed">The seed value.</param>
    public Murmur3A(in int seed = 0x00000000)
        : base(32, seed) =>
        Init();

    /// <inheritdoc />
    /// <summary>
    /// Initializes an implementation of the <see cref="Murmur3A" /> class.
    /// </summary>
    public override void Reset() => Init();

    /// <inheritdoc />
    /// <summary>
    ///   When overridden in a derived class,
    ///   appends the contents of <paramref name="source" /> to the data already
    ///   processed for the current hash computation.
    /// </summary>
    /// <param name="source">The data to process.</param>
    public override void Append(ReadOnlySpan<byte> source)
    {
        Length += source.Length;

        const int BlockSizeInBytes = 4;
        int remainder = source.Length & (BlockSizeInBytes - 1);
        int alignedLength = source.Length - remainder;

        for (int i = 0; i < alignedLength; i += BlockSizeInBytes)
        {
            uint k = BinaryPrimitives.ReadUInt32LittleEndian(source.Slice(i, BlockSizeInBytes));

            _h1 ^= C2 * RotateLeft(C1 * k, 15);
            _h1 = (5 * RotateLeft(_h1, 13)) + 0xE6546B64;
        }

        if (remainder > 0)
        {
            Tail(source, alignedLength, remainder);
        }

        _h1 = FMix(_h1 ^ (uint)Length);
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
    protected override void GetCurrentHashCore(Span<byte> destination)
    {
        byte[] bytes = GetBytes(_h1);

        bytes.CopyTo(destination);
    }

    /// <inheritdoc />
    /// <summary>
    /// Initializes the hash for this instance.
    /// </summary>
    protected override void Init()
    {
        _h1 = Seed;
        base.Init();
    }

    /// <summary>
    /// Rotates the bits left in an unsigned int.
    /// </summary>
    /// <param name="x">The value to rotate.</param>
    /// <param name="r">The number of bits to rotate (maximum 32 bits).</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint RotateLeft(in uint x, in byte r) => (x << r) | (x >> (32 - r));

    /// <summary>
    /// Finalization mix - force all bits of a hash block to avalanche.
    /// </summary>
    /// <param name="k">The value to mix.</param>
    /// <returns>The mixed value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint FMix(in uint k)
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Tail(in ReadOnlySpan<byte> tail, in int position, in int remainder)
    {
        uint k1 = 0x00000000U;

        switch (remainder)
        {
            case 3:
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 2:
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 1:
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
        }
    }
}