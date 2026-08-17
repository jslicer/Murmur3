// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BogusMurmurTests2.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Tests a bogus Murmur3 algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3.Tests;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static System.Globalization.NumberStyles;

/// <summary>
/// Tests a bogus Murmur3 algorithm variant.
/// </summary>
/// <seealso cref="Murmur3TestsBase" />
[TestClass]
#pragma warning disable CA1515 // Consider making public types internal
public sealed class BogusMurmurTests2 : Murmur3TestsBase
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BogusMurmurTests2" /> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">algType cannot be <see langword="null" />.</exception>
    public BogusMurmurTests2()
        : base(typeof(BogusMurmurHasher2))
    {
        // Intentionally empty.
    }

    /// <summary>
    /// Tests that a Murmur3 hash algorithm derivative that has a bad constructor throws the appropriate
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
    public void EnsureBadConstructorCaught() =>
        _ = Assert.ThrowsExactly<MissingMethodException>(() => TestSmHasher("The quick brown fox jumps over the lazy dog"));

    /// <inheritdoc />
    /// <summary>
    /// Creates a bogus Murmur3 hashing algorithm that has a bad constructor signature.
    /// </summary>
    /// <seealso cref="Murmur3Base" />
    /// <remarks>
    /// Initializes a new instance of the <see cref="BogusMurmurHasher2" /> class.
    /// </remarks>
    [method: ExcludeFromCodeCoverage]
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
#pragma warning disable CA1812 // This is an internal class that is apparently never instantiated
    private sealed class BogusMurmurHasher2() : Murmur3Base(32)
#pragma warning restore CA1812 // This is an internal class that is apparently never instantiated
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