﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3F.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the Murmur3 128 x64 hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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
    /// Initializes a new instance of the <see cref="Murmur3F"/> class.
    /// </summary>
    /// <param name="seed">The seed value.</param>
    public Murmur3F(in int seed = 0x00000000)
        : base(128, seed) => this.Init();

    /// <inheritdoc />
    /// <summary>
    /// Initializes an implementation of the <see cref="Murmur3Base" /> class.
    /// </summary>
    public override void Initialize() => this.Init();

    /// <inheritdoc />
    /// <summary>
    /// Initializes the hash for this instance.
    /// </summary>
    protected override void Init()
    {
        this._h1 = this.Seed;
        this._h2 = this.Seed;
        base.Init();
    }

    /// <inheritdoc />
    /// <summary>
    /// When overridden in a derived class, routes data written to the object into the hash algorithm for computing
    /// the hash.
    /// </summary>
    /// <param name="array">The input to compute the hash code for.</param>
    /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
    /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
#pragma warning disable IDE0079 // Remove unnecessary suppression
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
    //// ReSharper disable once MethodTooLong
    protected override void HashCore(byte[] array, int ibStart, int cbSize)
    {
        this.Length += cbSize;

        const int BlockSizeInBytes = 16;
        int remainder = cbSize & (BlockSizeInBytes - 1);
        int alignedLength = ibStart + (cbSize - remainder);

        for (int i = ibStart; i < alignedLength; i += BlockSizeInBytes)
        {
            ulong k1 = BitConverter.ToUInt64(array, i);
            //// ReSharper disable once ComplexConditionExpression
            ulong k2 = BitConverter.ToUInt64(array, i + (BlockSizeInBytes / 2));

            this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
            this._h1 = RotateLeft(this._h1, 27);
            this._h1 += this._h2;
            this._h1 = (5 * this._h1) + 0x0000000052DCE729UL;

            this._h2 ^= C1 * RotateLeft(C2 * k2, 33);
            this._h2 = RotateLeft(this._h2, 31);
            this._h2 += this._h1;
            this._h2 = (5 * this._h2) + 0x0000000038495AB5UL;
        }

        if (remainder > 0)
        {
            this.Tail(array, alignedLength, remainder);
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// When overridden in a derived class, finalizes the hash computation after the last data is processed by the
    /// cryptographic stream object.
    /// </summary>
    /// <returns>
    /// The computed hash code.
    /// </returns>
    protected override byte[] HashFinal()
    {
        this._h1 ^= (ulong)this.Length;
        this._h2 ^= (ulong)this.Length;

        this._h1 += this._h2;
        this._h2 += this._h1;

        this._h1 = FMix(this._h1);
        this._h2 = FMix(this._h2);

        this._h1 += this._h2;
        this._h2 += this._h1;

        byte[] b1 = BitConverter.GetBytes(this._h1);
        byte[] b2 = BitConverter.GetBytes(this._h2);
        byte[] hash = new byte[b1.Length + b2.Length];

        b1.CopyTo(hash, 0);
        b2.CopyTo(hash, b1.Length);
        return hash;
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
    /// <param name="tail">The byte array being hashed.</param>
    /// <param name="position">The position in the byte array where the tail starts.</param>
    /// <param name="remainder">The number of bytes remaining to process.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //// ReSharper disable once MethodTooLong
    //// ReSharper disable once CognitiveComplexity
    private void Tail(in byte[] tail, in int position, in int remainder)
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
                this._h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 14:
                k2 ^= (ulong)tail[position + 13] << 40;
                k2 ^= (ulong)tail[position + 12] << 32;
                k2 ^= (ulong)tail[position + 11] << 24;
                k2 ^= (ulong)tail[position + 10] << 16;
                k2 ^= (ulong)tail[position + 9] << 8;
                k2 ^= tail[position + 8];
                this._h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 13:
                k2 ^= (ulong)tail[position + 12] << 32;
                k2 ^= (ulong)tail[position + 11] << 24;
                k2 ^= (ulong)tail[position + 10] << 16;
                k2 ^= (ulong)tail[position + 9] << 8;
                k2 ^= tail[position + 8];
                this._h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 12:
                k2 ^= (ulong)tail[position + 11] << 24;
                k2 ^= (ulong)tail[position + 10] << 16;
                k2 ^= (ulong)tail[position + 9] << 8;
                k2 ^= tail[position + 8];
                this._h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 11:
                k2 ^= (ulong)tail[position + 10] << 16;
                k2 ^= (ulong)tail[position + 9] << 8;
                k2 ^= tail[position + 8];
                this._h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 10:
                k2 ^= (ulong)tail[position + 9] << 8;
                k2 ^= tail[position + 8];
                this._h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 9:
                k2 ^= tail[position + 8];
                this._h2 ^= C1 * RotateLeft(C2 * k2, 33);
                k1 ^= (ulong)tail[position + 7] << 56;
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
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
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 7:
                k1 ^= (ulong)tail[position + 6] << 48;
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 6:
                k1 ^= (ulong)tail[position + 5] << 40;
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 5:
                k1 ^= (ulong)tail[position + 4] << 32;
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 4:
                k1 ^= (ulong)tail[position + 3] << 24;
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 3:
                k1 ^= (ulong)tail[position + 2] << 16;
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 2:
                k1 ^= (ulong)tail[position + 1] << 8;
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
            case 1:
                k1 ^= tail[position];
                this._h1 ^= C2 * RotateLeft(C1 * k1, 31);
                break;
        }
    }
}