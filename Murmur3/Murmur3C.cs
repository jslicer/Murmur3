﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3C.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the Murmur3 128 x86 hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3;

using System;
using System.IO.Hashing;
using System.Runtime.CompilerServices;

using static System.BitConverter;

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
    public Murmur3C(in int seed = 0x00000000)
        : base(128, seed) =>
        Init();

    /// <inheritdoc />
    /// <summary>
    /// Initializes an implementation of the <see cref="Murmur3C" /> class.
    /// </summary>
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
        byte[] array = source.ToArray();

        for (int i = 0; i < alignedLength; i += BlockSizeInBytes)
        {
            uint k1 = ToUInt32(array, i);
            //// ReSharper disable ComplexConditionExpression
            uint k2 = ToUInt32(array, i + (BlockSizeInBytes / 4));
            uint k3 = ToUInt32(array, i + (BlockSizeInBytes / 2));
            uint k4 = ToUInt32(array, i + (3 * BlockSizeInBytes / 4));
            //// ReSharper restore ComplexConditionExpression

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

        _h1 ^= (uint)Length;
        _h2 ^= (uint)Length;
        _h3 ^= (uint)Length;
        _h4 ^= (uint)Length;

        _h1 += _h2;
        _h1 += _h3;
        _h1 += _h4;

        _h2 += _h1;
        _h3 += _h1;
        _h4 += _h1;

        _h1 = FMix(_h1);
        _h2 = FMix(_h2);
        _h3 = FMix(_h3);
        _h4 = FMix(_h4);

        _h1 += _h2;
        _h1 += _h3;
        _h1 += _h4;

        _h2 += _h1;
        _h3 += _h1;
        _h4 += _h1;
    }

    /// <inheritdoc />
    /// <summary>
    /// Initializes the hash for this instance.
    /// </summary>
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
    // ReSharper disable once MethodTooLong
    protected override void GetCurrentHashCore(Span<byte> destination)
    {
        byte[] b1 = GetBytes(_h1);
        byte[] b2 = GetBytes(_h2);
        byte[] b3 = GetBytes(_h3);
        byte[] b4 = GetBytes(_h4);
        //// ReSharper disable once ComplexConditionExpression
        byte[] bytes = new byte[b1.Length + b2.Length + b3.Length + b4.Length];

        b1.CopyTo(bytes, 0);
        b2.CopyTo(bytes, b1.Length);
        b3.CopyTo(bytes, b1.Length + b2.Length);
        b4.CopyTo(bytes, b1.Length + b2.Length + b3.Length);
        bytes.CopyTo(destination);
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
    //// ReSharper disable once MethodTooLong
    //// ReSharper disable once CognitiveComplexity
    private void Tail(in ReadOnlySpan<byte> tail, in int position, in int remainder)
    {
        uint k1 = 0x00000000U;
        uint k2 = 0x00000000U;
        uint k3 = 0x00000000U;
        uint k4 = 0x00000000U;

        switch (remainder)
        {
            case 15:
                k4 ^= (uint)tail[position + 14] << 16;
                k4 ^= (uint)tail[position + 13] << 8;
                k4 ^= tail[position + 12];
                _h4 ^= C1 * RotateLeft(C4 * k4, 18);
                k3 ^= (uint)tail[position + 11] << 24;
                k3 ^= (uint)tail[position + 10] << 16;
                k3 ^= (uint)tail[position + 9] << 8;
                k3 ^= tail[position + 8];
                _h3 ^= C4 * RotateLeft(C3 * k3, 17);
                k2 ^= (uint)tail[position + 7] << 24;
                k2 ^= (uint)tail[position + 6] << 16;
                k2 ^= (uint)tail[position + 5] << 8;
                k2 ^= tail[position + 4];
                _h2 ^= C3 * RotateLeft(C2 * k2, 16);
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 14:
                k4 ^= (uint)tail[position + 13] << 8;
                k4 ^= tail[position + 12];
                _h4 ^= C1 * RotateLeft(C4 * k4, 18);
                k3 ^= (uint)tail[position + 11] << 24;
                k3 ^= (uint)tail[position + 10] << 16;
                k3 ^= (uint)tail[position + 9] << 8;
                k3 ^= tail[position + 8];
                _h3 ^= C4 * RotateLeft(C3 * k3, 17);
                k2 ^= (uint)tail[position + 7] << 24;
                k2 ^= (uint)tail[position + 6] << 16;
                k2 ^= (uint)tail[position + 5] << 8;
                k2 ^= tail[position + 4];
                _h2 ^= C3 * RotateLeft(C2 * k2, 16);
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 13:
                k4 ^= tail[position + 12];
                _h4 ^= C1 * RotateLeft(C4 * k4, 18);
                k3 ^= (uint)tail[position + 11] << 24;
                k3 ^= (uint)tail[position + 10] << 16;
                k3 ^= (uint)tail[position + 9] << 8;
                k3 ^= tail[position + 8];
                _h3 ^= C4 * RotateLeft(C3 * k3, 17);
                k2 ^= (uint)tail[position + 7] << 24;
                k2 ^= (uint)tail[position + 6] << 16;
                k2 ^= (uint)tail[position + 5] << 8;
                k2 ^= tail[position + 4];
                _h2 ^= C3 * RotateLeft(C2 * k2, 16);
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 12:
                k3 ^= (uint)tail[position + 11] << 24;
                k3 ^= (uint)tail[position + 10] << 16;
                k3 ^= (uint)tail[position + 9] << 8;
                k3 ^= tail[position + 8];
                _h3 ^= C4 * RotateLeft(C3 * k3, 17);
                k2 ^= (uint)tail[position + 7] << 24;
                k2 ^= (uint)tail[position + 6] << 16;
                k2 ^= (uint)tail[position + 5] << 8;
                k2 ^= tail[position + 4];
                _h2 ^= C3 * RotateLeft(C2 * k2, 16);
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 11:
                k3 ^= (uint)tail[position + 10] << 16;
                k3 ^= (uint)tail[position + 9] << 8;
                k3 ^= tail[position + 8];
                _h3 ^= C4 * RotateLeft(C3 * k3, 17);
                k2 ^= (uint)tail[position + 7] << 24;
                k2 ^= (uint)tail[position + 6] << 16;
                k2 ^= (uint)tail[position + 5] << 8;
                k2 ^= tail[position + 4];
                _h2 ^= C3 * RotateLeft(C2 * k2, 16);
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 10:
                k3 ^= (uint)tail[position + 9] << 8;
                k3 ^= tail[position + 8];
                _h3 ^= C4 * RotateLeft(C3 * k3, 17);
                k2 ^= (uint)tail[position + 7] << 24;
                k2 ^= (uint)tail[position + 6] << 16;
                k2 ^= (uint)tail[position + 5] << 8;
                k2 ^= tail[position + 4];
                _h2 ^= C3 * RotateLeft(C2 * k2, 16);
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 9:
                k3 ^= tail[position + 8];
                _h3 ^= C4 * RotateLeft(C3 * k3, 17);
                k2 ^= (uint)tail[position + 7] << 24;
                k2 ^= (uint)tail[position + 6] << 16;
                k2 ^= (uint)tail[position + 5] << 8;
                k2 ^= tail[position + 4];
                _h2 ^= C3 * RotateLeft(C2 * k2, 16);
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 8:
                k2 ^= (uint)tail[position + 7] << 24;
                k2 ^= (uint)tail[position + 6] << 16;
                k2 ^= (uint)tail[position + 5] << 8;
                k2 ^= tail[position + 4];
                _h2 ^= C3 * RotateLeft(C2 * k2, 16);
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 7:
                k2 ^= (uint)tail[position + 6] << 16;
                k2 ^= (uint)tail[position + 5] << 8;
                k2 ^= tail[position + 4];
                _h2 ^= C3 * RotateLeft(C2 * k2, 16);
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 6:
                k2 ^= (uint)tail[position + 5] << 8;
                k2 ^= tail[position + 4];
                _h2 ^= C3 * RotateLeft(C2 * k2, 16);
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 5:
                k2 ^= tail[position + 4];
                _h2 ^= C3 * RotateLeft(C2 * k2, 16);
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
            case 4:
                k1 ^= (uint)tail[position + 3] << 24;
                k1 ^= (uint)tail[position + 2] << 16;
                k1 ^= (uint)tail[position + 1] << 8;
                k1 ^= tail[position];
                _h1 ^= C2 * RotateLeft(C1 * k1, 15);
                break;
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