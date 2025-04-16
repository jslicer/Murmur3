// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BogusMurmurTests1.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Tests a bogus Murmur3 algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests a bogus Murmur3 algorithm variant.
/// </summary>
/// <seealso cref="Murmur3TestsBase" />
[TestClass]
public sealed class BogusMurmurTests1 : Murmur3TestsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BogusMurmurTests1" /> class.
    /// </summary>
    public BogusMurmurTests1()
        : base(typeof(BogusMurmurHasher1))
    {
        // Intentionally empty.
    }

    /// <summary>
    /// Tests that a Murmur3 hash algorithm derivative that returns <see langword="null" /> throws the appropriate
    /// exception.
    /// </summary>
    /// <returns>An asynchronous <see cref="Task" />.</returns>
    [TestMethod]
    public async Task EnsureHashOfNullCaughtAsync() =>
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(
            async () => await TestSmHasherAsync("The quick brown fox jumps over the lazy dog").ConfigureAwait(false)).ConfigureAwait(true);

    /// <inheritdoc />
    /// <summary>
    /// Creates a bogus Murmur3 hashing algorithm that returns a <see langword="null" /> hash result.
    /// </summary>
    /// <seealso cref="Murmur3Base" />
    /// <remarks>
    /// Initializes a new instance of the <see cref="BogusMurmurHasher1"/> class.
    /// </remarks>
    /// <param name="seed">The seed value.</param>
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
    private sealed class BogusMurmurHasher1(in int seed = 0) : Murmur3Base(0, in seed)
#pragma warning restore SA1009 // Closing parenthesis should be spaced correctly
#pragma warning restore IDE0079 // Remove unnecessary suppression
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets the value of the computed hash code.
        /// </summary>
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
        public override byte[]? Hash => null;
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
#pragma warning restore IDE0079 // Remove unnecessary suppression

        /// <inheritdoc />
        /// <summary>
        /// Resets the hash algorithm to its initial state.
        /// </summary>
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
        // ReSharper disable once UseCollectionExpression
#pragma warning disable IDE0301 // Simplify collection initialization
        protected override byte[] HashFinal() => Array.Empty<byte>();
#pragma warning restore IDE0301 // Simplify collection initialization
    }
}