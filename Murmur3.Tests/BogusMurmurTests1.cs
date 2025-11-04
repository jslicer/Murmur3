// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BogusMurmurTests1.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Tests a bogus Murmur3 algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3.Tests;

using System.Diagnostics.CodeAnalysis;

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
    [TestMethod]
    public void EnsureHashOfNullCaught() =>
        Assert.ThrowsExactly<InvalidOperationException>(
            () => TestSmHasher("The quick brown fox jumps over the lazy dog"));

    /// <inheritdoc />
    /// <summary>
    /// Creates a bogus Murmur3 hashing algorithm that returns a <see langword="null" /> hash result.
    /// </summary>
    /// <seealso cref="Murmur3Base" />
    /// <remarks>
    /// Initializes a new instance of the <see cref="BogusMurmurHasher1" /> class.
    /// </remarks>
    /// <param name="seed">The seed value.</param>
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
    private sealed class BogusMurmurHasher1(int seed = 0) : Murmur3Base(32, seed)
#pragma warning restore SA1009 // Closing parenthesis should be spaced correctly
#pragma warning restore IDE0079 // Remove unnecessary suppression
    {
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

        [ExcludeFromCodeCoverage]
        protected override void GetCurrentHashCore(Span<byte> destination)
        {
            // Intentionally empty.
        }
    }
}