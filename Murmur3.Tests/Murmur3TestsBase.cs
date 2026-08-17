// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3TestsBase.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the common functionality to test all the Murmur3 hashing algorithm variants.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// Ignore Spelling: alg Hasher
namespace Murmur3.Tests;

using System.Globalization;
using System.IO.Hashing;
using System.Numerics;
using System.Text;

using static System.Globalization.CultureInfo;
using static System.Globalization.NumberStyles;
using static System.Numerics.BigInteger;

#pragma warning disable IDE0001
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
#pragma warning restore IDE0001

/// <summary>
/// Implements the common functionality to test all the Murmur3 hashing algorithm variants.
/// </summary>
#pragma warning disable CA1515 // Consider making public types internal
public abstract class Murmur3TestsBase
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// The empty hash value used for testing.
    /// </summary>
    private static readonly byte[] _EmptyHash = new byte[4];

    /// <summary>
    /// Type of the Murmur3 hashing algorithm variant.
    /// </summary>
    private readonly Type _algType;

    /// <summary>
    /// Initializes a new instance of the <see cref="Murmur3TestsBase" /> class.
    /// </summary>
    /// <param name="algType">Type of the Murmur3 hashing algorithm variant.</param>
    /// <exception cref="ArgumentNullException"><paramref name="algType" /> cannot be
    /// <see langword="null" />.</exception>
    protected Murmur3TestsBase(Type algType)
    {
        ArgumentNullException.ThrowIfNull(algType);
        _algType = algType;
    }

    /// <summary>
    /// Tests a byte array using the Murmur3 hashing algorithm variant.
    /// </summary>
    /// <param name="expected">The expected result.</param>
    /// <param name="input">The input byte array.</param>
    /// <param name="message">The message to show if the test fails.</param>
    /// <param name="seed">The seed value.</param>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    /// <exception cref="AssertFailedException">Thrown if <paramref name="expected" /> is not equal to
    /// actual.</exception>
    // ReSharper disable once TooManyArguments
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    protected void Test(string expected, byte[] input, string message, int seed = 0x00000000) =>
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
#pragma warning restore IDE0079 // Remove unnecessary suppression
        AreEqual(Parse(expected, AllowHexSpecifier, InvariantCulture), Hash(input, seed), message);

    /// <summary>
    /// Tests a UTF-8 string using the Murmur3 hashing algorithm variant.
    /// </summary>
    /// <param name="expected">The expected result.</param>
    /// <param name="input">The input string.</param>
    /// <param name="message">The message to show if the test fails.</param>
    /// <param name="seed">The seed value.</param>
    /// <exception cref="ArgumentNullException">s is <see langword="null" />.</exception>
    /// <exception cref="EncoderFallbackException">A fallback occurred (for more information, see Character Encoding in
    /// .NET)
    ///  -and-
    ///  <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.</exception>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    /// <exception cref="AssertFailedException">Thrown if <paramref name="expected" /> is not equal to
    /// actual.</exception>
    // ReSharper disable once TooManyArguments
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    protected void Test(string expected, string input, string message, int seed = 0x00000000) =>
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
#pragma warning restore IDE0079 // Remove unnecessary suppression
        AreEqual(
            Parse(expected, AllowHexSpecifier, InvariantCulture),
            Hash(Encoding.UTF8.GetBytes(input), seed),
            message);

    /// <summary>
    /// ReSharper disable CommentTypo
    /// Tests using the SMHasher KeysetTest VerificationTest.
    /// ReSharper restore CommentTypo.
    /// </summary>
    /// <param name="expected">The expected value.</param>
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
    /// <exception cref="AssertFailedException">Thrown if <paramref name="expected" /> is not equal to
    /// actual.</exception>
    // ReSharper disable once MethodTooLong
    protected void TestSmHasher(string expected)
    {
        // ReSharper disable once UnthrowableException
        NonCryptographicHashAlgorithm alg =
            GetAlgorithm() ?? throw new MissingMethodException("Hash algorithm constructor not found.");
        byte[] key = new byte[256];

        alg.Reset();
        for (int i = 0; i < key.Length; i++)
        {
            // ReSharper disable once UnthrowableException
            NonCryptographicHashAlgorithm alg2 = GetAlgorithm(key.Length - i)
                ?? throw new MissingMethodException("Hash algorithm constructor not found.");

            alg2.Reset();
            key[i] = (byte)i;
            alg2.Append(key.AsSpan(0, i));
            alg.Append(alg2.GetCurrentHash());
        }

        byte[] currentHash = alg.GetCurrentHash();

        if (currentHash.SequenceEqual(_EmptyHash))
        {
            throw new InvalidOperationException("Hash invalid.");
        }

        AreEqual(
            Parse(expected, AllowHexSpecifier, InvariantCulture),
            new(currentHash),
            "SMHasher hash verification");
    }

    /// <summary>
    /// Hashes the specified input bytes.
    /// </summary>
    /// <param name="input">The input byte array.</param>
    /// <param name="seed">The seed value.</param>
    /// <returns>The result of applying the specified Murmur3 hashing algorithm variant to the input byte
    /// array.</returns>
    private BigInteger Hash(ReadOnlySpan<byte> input, int seed = 0x00000000)
    {
        NonCryptographicHashAlgorithm alg =
            GetAlgorithm(seed) ?? throw new InvalidOperationException("Hash algorithm constructor not found.");

        // ReSharper disable once ComplexConditionExpression
        Span<byte> destination = stackalloc byte[alg.HashLengthInBytes];

        alg.Append(input);
        _ = alg.GetCurrentHash(destination);
        return new(destination);
    }

    /// <summary>
    /// Gets a new instance of the specified Murmur3 hashing algorithm variant.
    /// </summary>
    /// <param name="seed">The seed value.</param>
    /// <returns>A new instance of specified Murmur3 hashing algorithm variant, or <see langword="null" /> if one could
    /// not be found.</returns>
    /// <exception cref="InvalidOperationException"><see cref="_algType" /> must be a descendant of
    /// Murmur3Base.</exception>
    private NonCryptographicHashAlgorithm? GetAlgorithm(int seed = 0x00000000)
    {
        if (!_algType.IsAssignableTo(typeof(Murmur3Base)))
        {
            throw new InvalidOperationException("The algorithm type must be a descendant of Murmur3Base.");
        }

        System.Reflection.ConstructorInfo? constructor =
            _algType.GetConstructor([typeof(int)]);

        return constructor?.Invoke([seed]) as NonCryptographicHashAlgorithm;
    }
}