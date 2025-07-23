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
using System.IO.Hashing;

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
    [TestMethod]
    public void EnsureNonMurmur3HasherCaught() =>
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            TestSmHasher("The quick brown fox jumps over the lazy dog"));

    /// <summary>
    /// A bogus hash class.
    /// </summary>
    /// <seealso cref="NonCryptographicHashAlgorithm" />
    [method: ExcludeFromCodeCoverage]
    private sealed class BogusHasher() : NonCryptographicHashAlgorithm(32)
    {
        /// <inheritdoc />
        /// <summary>
        /// When overridden in a derived class, appends the contents of <paramref name="source" /> to the data already
        /// processed for the current hash computation.
        /// </summary>
        /// <param name="source">The data to process.</param>
        [ExcludeFromCodeCoverage]
        public override void Append(ReadOnlySpan<byte> source)
        {
            // Intentionally empty.
        }

        /// <inheritdoc />
        /// <summary>
        /// Resets the hash algorithm to its initial state.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public override void Reset()
        {
            // Intentionally empty.
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
        [ExcludeFromCodeCoverage]
        protected override void GetCurrentHashCore(Span<byte> destination) => destination.Clear();
    }
}