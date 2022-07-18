// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3TestsBase.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the common functionality to test all the Murmur3 hashing algorithm variants.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3.Tests;

using System.Numerics;
using System.Security.Cryptography;

using static System.Globalization.CultureInfo;
using static System.Globalization.NumberStyles;
using static System.Numerics.BigInteger;

using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

/// <summary>
/// Implements the common functionality to test all the Murmur3 hashing algorithm variants.
/// </summary>
public abstract class Murmur3TestsBase
{
    /// <summary>
    /// Type of the Murmur3 hashing algorithm variant.
    /// </summary>
    private readonly Type _algType;

    /// <summary>
    /// Initializes a new instance of the <see cref="Murmur3TestsBase"/> class.
    /// </summary>
    /// <param name="algType">Type of the Murmur3 hashing algorithm variant.</param>
    /// <exception cref="ArgumentNullException"><paramref name="algType" /> cannot be
    /// <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="algType" /> must be a descendant of
    /// Murmur3Base.</exception>
    protected Murmur3TestsBase(in Type algType)
    {
        if (algType is null)
        {
            throw new ArgumentNullException(nameof(algType));
        }

        if (!algType.IsAssignableTo(typeof(Murmur3Base)))
        {
            throw new InvalidOperationException("The algorithm type must be a descendant of Murmur3Base.");
        }

        this._algType = algType;
    }

    /// <summary>
    /// Tests a byte array using the Murmur3 hashing algorithm variant.
    /// </summary>
    /// <param name="expected">The expected result.</param>
    /// <param name="input">The input byte array.</param>
    /// <param name="message">The message to show if the test fails.</param>
    /// <param name="seed">The seed value.</param>
    // ReSharper disable once TooManyArguments
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    protected void Test(in string expected, in byte[] input, in string message, in int seed = 0x00000000) =>
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        AreEqual(Parse(expected, AllowHexSpecifier, InvariantCulture), this.Hash(input, seed), message);

    /// <summary>
    /// Tests a UTF-8 string using the Murmur3 hashing algorithm variant.
    /// </summary>
    /// <param name="expected">The expected result.</param>
    /// <param name="input">The input string.</param>
    /// <param name="message">The message to show if the test fails.</param>
    /// <param name="seed">The seed value.</param>
    // ReSharper disable once TooManyArguments
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters
    protected void Test(in string expected, in string input, in string message, in int seed = 0x00000000) =>
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        AreEqual(
            Parse(expected, AllowHexSpecifier, InvariantCulture),
            this.Hash(System.Text.Encoding.UTF8.GetBytes(input), seed),
            message);

    /// <summary>
    /// ReSharper disable CommentTypo
    /// Tests using the SMHasher KeysetTest VerificationTest.
    /// ReSharper restore CommentTypo.
    /// </summary>
    /// <param name="expected">The expected.</param>
    /// <exception cref="InvalidOperationException">Hash algorithm constructor not found.</exception>
    /// <exception cref="InvalidOperationException">Hash invalid.</exception>
    // ReSharper disable once MethodTooLong
    protected void TestSmHasher(in string expected)
    {
        using HashAlgorithm? alg = this.GetAlgorithm();
        if (alg is null)
        {
            throw new InvalidOperationException("Hash algorithm constructor not found.");
        }

        byte[] key = new byte[256];

        using CryptoStream cryptoStream = new (Stream.Null, alg, CryptoStreamMode.Write);
        for (int i = 0; i < key.Length; i++)
        {
            key[i] = (byte)i;
            using HashAlgorithm? alg2 = this.GetAlgorithm(key.Length - i);
            if (alg2 is null)
            {
                throw new InvalidOperationException("Hash algorithm constructor not found.");
            }

            cryptoStream.Write(alg2.ComputeHash(key, 0, i));
        }

        cryptoStream.FlushFinalBlock();
        if (alg.Hash is null)
        {
            throw new InvalidOperationException("Hash invalid.");
        }

        AreEqual(
            Parse(expected, AllowHexSpecifier, InvariantCulture),
            new BigInteger(alg.Hash),
            "SMHasher hash verification");
    }

    /// <summary>
    /// Hashes the specified input bytes.
    /// </summary>
    /// <param name="input">The input byte array.</param>
    /// <param name="seed">The seed value.</param>
    /// <returns>The result of applying the specified Murmur3 hashing algorithm variant to the input byte
    /// array.</returns>
    private BigInteger Hash(in byte[] input, in int seed = 0x00000000)
    {
        using HashAlgorithm? alg = this.GetAlgorithm(seed);
        return alg is null
            ? throw new InvalidOperationException("Hash algorithm constructor not found.")
            : new BigInteger(alg.ComputeHash(input));
    }

    /// <summary>
    /// Gets a new instance of the specified Murmur3 hashing algorithm variant.
    /// </summary>
    /// <param name="seed">The seed value.</param>
    /// <returns>A new instance of specified Murmur3 hashing algorithm variant, or <see langword="null" /> if one could
    /// not be found.</returns>
    private HashAlgorithm? GetAlgorithm(in int seed = 0x00000000)
    {
        System.Reflection.ConstructorInfo? constructor =
            this._algType.GetConstructor(new[] { typeof(int).MakeByRefType() });

        return constructor?.Invoke(new object[] { seed }) as HashAlgorithm;
    }
}