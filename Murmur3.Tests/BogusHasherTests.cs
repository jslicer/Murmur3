// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BogusHasherTests.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Tests a bogus hash class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3.Tests;

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests a bogus hash class.
/// </summary>
/// <seealso cref="Murmur3TestsBase" />
[TestClass]
public sealed class BogusHasherTests : Murmur3TestsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BogusHasherTests" /> class.
    /// </summary>
    public BogusHasherTests()
        : base(typeof(BogusHasher))
    {
        // Intentionally empty.
    }

    /// <summary>
    /// Tests that when the hash algorithm is not a proper descendant of Murmur3 it throws the appropriate
    /// exception.
    /// </summary>
    /// <returns>An asynchronous <see cref="Task" />.</returns>
    [TestMethod]
    public async Task EnsureNonMurmur3HasherCaughtAsync() =>
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await TestSmHasherAsync("The quick brown fox jumps over the lazy dog").ConfigureAwait(false)).ConfigureAwait(true);

    /// <summary>
    /// A bogus hash class.
    /// </summary>
    /// <seealso cref="HashAlgorithm" />
    private sealed class BogusHasher : HashAlgorithm
    {
        /// <inheritdoc />
        /// <summary>
        /// Resets the hash algorithm to its initial state.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public override void Initialize()
        {
            // Intentionally empty.
        }

        /// <inheritdoc />
        /// <summary>
        /// When overridden in a derived class, routes data written to the object into the hash algorithm for computing
        /// the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
#pragma warning disable VSSpell001 // Spell Check
        [ExcludeFromCodeCoverage]
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
#pragma warning restore VSSpell001 // Spell Check
        {
            // Intentionally empty.
        }

        /// <summary>
        /// When overridden in a derived class, finalizes the hash computation after the last data is processed by the
        /// cryptographic hash algorithm.
        /// </summary>
        /// <returns>
        /// The computed hash code.
        /// </returns>
#pragma warning disable IDE0301 // Simplify collection initialization
        [ExcludeFromCodeCoverage]
        //// ReSharper disable once UseCollectionExpression
        protected override byte[] HashFinal() => Array.Empty<byte>();
#pragma warning restore IDE0301 // Simplify collection initialization
    }
}