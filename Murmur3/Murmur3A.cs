// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3A.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the Murmur3 32 x86 hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// Ignore Spelling: ib
namespace Murmur3
{
    using System;
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
            Init();

        /// <inheritdoc />
        /// <summary>
        /// Initializes an implementation of the <see cref="Murmur3A" /> class.
        /// </summary>
        public override void Initialize() => Init();

        /// <inheritdoc />
        /// <summary>
        /// Initializes the hash for this instance.
        /// </summary>
        protected override void Init()
        {
            _h1 = Seed;
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
            Length += cbSize;

            const int BlockSizeInBytes = 4;
            int remainder = cbSize & (BlockSizeInBytes - 1);
            int alignedLength = ibStart + (cbSize - remainder);

            for (int i = ibStart; i < alignedLength; i += BlockSizeInBytes)
            {
                _h1 ^= C2 * RotateLeft(C1 * ToUInt32(array, i), 15);
                _h1 = (5 * RotateLeft(_h1, 13)) + 0xE6546B64;
            }

            if (remainder > 0)
            {
                Tail(array, alignedLength, remainder);
            }
        }

        /// <summary>
        /// Routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="source">The input to compute the hash code for.</param>
        protected override void HashCore(ReadOnlySpan<byte> source)
        {
            Length += source.Length;

            const int BlockSizeInBytes = 4;
            int remainder = source.Length & (BlockSizeInBytes - 1);
            int alignedLength = source.Length - remainder;
            byte[] array = source.ToArray();

            for (int i = 0; i < alignedLength; i += BlockSizeInBytes)
            {
                _h1 ^= C2 * RotateLeft(C1 * ToUInt32(array, i), 15);
                _h1 = (5 * RotateLeft(_h1, 13)) + 0xE6546B64;
            }

            if (remainder > 0)
            {
                Tail(source, alignedLength, remainder);
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
            _h1 = FMix(_h1 ^ (uint)Length);
            return GetBytes(_h1);
        }

        /// <summary>
        /// Attempts to finalize the hash computation after the last data is processed by the hash algorithm.
        /// </summary>
        /// <param name="destination">The buffer to receive the hash value.</param>
        /// <param name="bytesWritten">When this method returns, the total number of bytes written into
        /// <paramref name="destination" />. This parameter is treated as uninitialized.</param>
        /// <returns><see langword="true" /> if <paramref name="destination" /> is long enough to receive the hash
        /// value; otherwise, <see langword="false" />.</returns>
        protected override bool TryHashFinal(Span<byte> destination, out int bytesWritten)
        {
            _h1 = FMix(_h1 ^ (uint)Length);

            byte[] bytes = GetBytes(_h1);

            bytes.CopyTo(destination);
            bytesWritten = bytes.Length;
            return true;
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
}