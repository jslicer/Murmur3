// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3A.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the Murmur3 32 x86 hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

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
        /// Initializes a new instance of the <see cref="Murmur3A"/> class.
        /// </summary>
        /// <param name="seed">The seed value.</param>
        public Murmur3A(in int seed = 0x00000000)
            : base(32, seed) =>
            this.Init();

        /// <inheritdoc />
        /// <summary>
        /// Initializes an implementation of the <see cref="Murmur3A" /> class.
        /// </summary>
        public override void Initialize() => this.Init();

        /// <inheritdoc />
        /// <summary>
        /// Initializes the hash for this instance.
        /// </summary>
        protected override void Init()
        {
            this._h1 = this.Seed;
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
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            this.Length += cbSize;

            const int BlockSizeInBytes = 4;
            int remainder = cbSize & (BlockSizeInBytes - 1);
            int alignedLength = ibStart + (cbSize - remainder);

            for (int i = ibStart; i < alignedLength; i += BlockSizeInBytes)
            {
                this._h1 ^= C2 * RotateLeft(C1 * ToUInt32(array, i), 15);
                this._h1 = (5 * RotateLeft(this._h1, 13)) + 0xE6546B64;
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
            this._h1 = FMix(this._h1 ^ (uint)this.Length);
            return GetBytes(this._h1);
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
        /// <param name="tail">The byte array being hashed.</param>
        /// <param name="position">The position in the byte array where the tail starts.</param>
        /// <param name="remainder">The number of bytes remaining to process.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Tail(in byte[] tail, in int position, in int remainder)
        {
            uint k1 = 0x00000000U;

            switch (remainder)
            {
                case 3:
                    k1 ^= (uint)tail[position + 2] << 16;
                    k1 ^= (uint)tail[position + 1] << 8;
                    k1 ^= tail[position];
                    this._h1 ^= C2 * RotateLeft(C1 * k1, 15);
                    break;
                case 2:
                    k1 ^= (uint)tail[position + 1] << 8;
                    k1 ^= tail[position];
                    this._h1 ^= C2 * RotateLeft(C1 * k1, 15);
                    break;
                case 1:
                    k1 ^= tail[position];
                    this._h1 ^= C2 * RotateLeft(C1 * k1, 15);
                    break;
            }
        }
    }
}