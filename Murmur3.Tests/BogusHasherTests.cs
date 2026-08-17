// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BogusHasherTests.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Tests a bogus hash class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// Ignore Spelling: Hasher
namespace Murmur3.Tests;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO.Hashing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static System.Globalization.NumberStyles;

/// <summary>
/// Tests a bogus hash class.
/// </summary>
/// <seealso cref="Murmur3TestsBase" />
[TestClass]
#pragma warning disable CA1515 // Consider making public types internal
public sealed class BogusHasherTests : Murmur3TestsBase
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BogusHasherTests" /> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">algType cannot be <see langword="null" />.</exception>
    public BogusHasherTests()
        : base(typeof(BogusHasher))
    {
        // Intentionally empty.
    }

    /// <summary>
    /// Tests that when the hash algorithm is not a proper descendant of Murmur3 it throws the appropriate
    /// exception.
    /// </summary>
    /// <exception cref="AssertFailedException">Thrown if action does not throw exception of type
    /// TException.</exception>
    /// <exception cref="MissingMethodException">Hash algorithm constructor not found.</exception>
    /// <exception cref="InvalidOperationException">Hash invalid.</exception>
    /// <exception cref="OverflowException">The array is multidimensional and contains more than
    /// <see cref="int.MaxValue">Int32.MaxValue</see> elements.</exception>
    /// <exception cref="ArrayTypeMismatchException">array is covariant, and the array's type is not exactly
    /// <see langword="T[]" />".</exception>
    /// <exception cref="ArgumentOutOfRangeException">start, length, or start + length> is not in the range of
    /// array.</exception>
    /// <exception cref="ArgumentNullException">source is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    [TestMethod]
    public void EnsureNonMurmur3HasherCaught() =>
        Assert.ThrowsExactly<InvalidOperationException>(() =>
            TestSmHasher("The quick brown fox jumps over the lazy dog"));

    /// <summary>
    /// A bogus hash class.
    /// </summary>
    /// <seealso cref="NonCryptographicHashAlgorithm" />
    [method: ExcludeFromCodeCoverage]
#pragma warning disable CA1812 // This is an internal class that is apparently never instantiated
    private sealed class BogusHasher() : NonCryptographicHashAlgorithm(32)
#pragma warning restore CA1812 // This is an internal class that is apparently never instantiated
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