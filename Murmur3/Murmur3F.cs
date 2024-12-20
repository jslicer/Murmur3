// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3F.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the Murmur3 128 x64 hashing algorithm variant.
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
            : base(128, seed) =>
            Init();

        /// <inheritdoc />
        /// <summary>
        /// Initializes an implementation of the <see cref="Murmur3Base" /> class.
        /// </summary>
        public override void Initialize() => Init();

        /// <inheritdoc />
        /// <summary>
        /// Initializes the hash for this instance.
        /// </summary>
        protected override void Init()
        {
            _h1 = Seed;
            _h2 = Seed;
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
            Length += cbSize;

            const int BlockSizeInBytes = 16;
            int remainder = cbSize & (BlockSizeInBytes - 1);
            int alignedLength = ibStart + (cbSize - remainder);

            for (int i = ibStart; i < alignedLength; i += BlockSizeInBytes)
            {
                ulong k1 = ToUInt64(array, i);
                //// ReSharper disable once ComplexConditionExpression
                ulong k2 = ToUInt64(array, i + (BlockSizeInBytes / 2));

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
                Tail(array, alignedLength, remainder);
            }
        }

        /// <summary>
        /// Routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="source">The input to compute the hash code for.</param>
        // ReSharper disable once MethodTooLong
        protected override void HashCore(ReadOnlySpan<byte> source)
        {
            Length += source.Length;

            const int BlockSizeInBytes = 16;
            int remainder = source.Length & (BlockSizeInBytes - 1);
            int alignedLength = source.Length - remainder;
            byte[] array = source.ToArray();

            for (int i = 0; i < alignedLength; i += BlockSizeInBytes)
            {
                ulong k1 = ToUInt64(array, i);
                //// ReSharper disable once ComplexConditionExpression
                ulong k2 = ToUInt64(array, i + (BlockSizeInBytes / 2));

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
            _h1 ^= (ulong)Length;
            _h2 ^= (ulong)Length;

            _h1 += _h2;
            _h2 += _h1;

            _h1 = FMix(_h1);
            _h2 = FMix(_h2);

            _h1 += _h2;
            _h2 += _h1;

            byte[] b1 = GetBytes(_h1);
            byte[] b2 = GetBytes(_h2);
            byte[] hash = new byte[b1.Length + b2.Length];

            b1.CopyTo(hash, 0);
            b2.CopyTo(hash, b1.Length);
            return hash;
        }

        /// <summary>
        /// Attempts to finalize the hash computation after the last data is processed by the hash algorithm.
        /// </summary>
        /// <param name="destination">The buffer to receive the hash value.</param>
        /// <param name="bytesWritten">When this method returns, the total number of bytes written into
        /// <paramref name="destination" />. This parameter is treated as uninitialized.</param>
        /// <returns><see langword="true" /> if <paramref name="destination" /> is long enough to receive the hash
        /// value; otherwise, <see langword="false" />.</returns>
        // ReSharper disable once MethodTooLong
        protected override bool TryHashFinal(Span<byte> destination, out int bytesWritten)
        {
            _h1 ^= (ulong)Length;
            _h2 ^= (ulong)Length;

            _h1 += _h2;
            _h2 += _h1;

            _h1 = FMix(_h1);
            _h2 = FMix(_h2);

            _h1 += _h2;
            _h2 += _h1;

            byte[] b1 = GetBytes(_h1);
            byte[] b2 = GetBytes(_h2);
            byte[] bytes = new byte[b1.Length + b2.Length];

            b1.CopyTo(bytes, 0);
            b2.CopyTo(bytes, b1.Length);
            bytes.CopyTo(destination);
            bytesWritten = bytes.Length;
            return true;
        }

        /// <summary>
        /// Rotates the bits left in an unsigned long.
        /// </summary>
        /// <param name="x">The value to rotate.</param>
        /// <param name="r">The number of bits to rotate (maximum 64 bits).</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong RotateLeft(in ulong x, in byte r) => (x << r) | (x >> (64 - r));

        /// <summary>
        /// Finalization mix - force all bits of a hash block to avalanche.
        /// </summary>
        /// <param name="k">The value to mix.</param>
        /// <returns>The mixed value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
}