// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3ATests.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Tests the Murmur3 32 x86 hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests the Murmur3 32 x86 hashing algorithm variant.
/// </summary>
/// <seealso cref="Murmur3TestsBase" />
[TestClass]
public sealed class Murmur3ATests : Murmur3TestsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Murmur3ATests" /> class.
    /// </summary>
    public Murmur3ATests()
        : base(typeof(Murmur3A))
    {
        // Intentionally empty.
    }

    /// <summary>
    /// Tests an empty byte array with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestEmptyInputSeed0() => this.Test(
        "00000000",
        Array.Empty<byte>(),
        "with zero data and zero seed, everything becomes zero");

    /// <summary>
    /// Tests an empty byte array with an input seed of 0x00000001.
    /// </summary>
    [TestMethod]
    public void TestEmptyInputSeed1() =>
        this.Test("514E28B7", Array.Empty<byte>(), "ignores nearly all the math", 0x00000001);

    /// <summary>
    /// ReSharper disable once CommentTypo
    /// Tests an empty byte array with an input seed of 0xFFFFFFFF.
    /// </summary>
    [TestMethod]
    //// ReSharper disable once InconsistentNaming
    //// ReSharper disable IdentifierTypo
    public void TestEmptyInputSeedFFFFFFFF() => this.Test(
    //// ReSharper restore IdentifierTypo
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "81F16F39",
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        Array.Empty<byte>(),
        "make sure your seed uses unsigned 32-bit math",
        unchecked((int)0xFFFFFFFFU));

    /// <summary>
    /// ReSharper disable once CommentTypo
    /// Tests an array with bytes 0xFFFFFFFF with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    //// ReSharper disable once InconsistentNaming
    //// ReSharper disable IdentifierTypo
    public void TestFFFFFFFFInputSeed0() => this.Test(
    //// ReSharper restore IdentifierTypo
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "76293B50",
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        new[] { (byte)0xFF, (byte)0xFF, (byte)0xFF, (byte)0xFF },
        "make sure 4-byte chunks use unsigned math");

    /// <summary>
    /// Tests an array with bytes 0x87654321 with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void Test21436587InputSeed0() => this.Test(
        "F55B516B",
        new[] { (byte)0x21, (byte)0x43, (byte)0x65, (byte)0x87 },
        "Endian order. UInt32 should end up as 0x87654321");

    /// <summary>
    /// ReSharper disable once CommentTypo
    /// Tests an array with bytes 0x87654321 with an input seed of 0x5082EDEE.
    /// </summary>
    [TestMethod]
    //// ReSharper disable once InconsistentNaming
    //// ReSharper disable IdentifierTypo
    public void Test21436587InputSeed5082EDEE() => this.Test(
    //// ReSharper restore IdentifierTypo
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "2362F9DE",
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        new[] { (byte)0x21, (byte)0x43, (byte)0x65, (byte)0x87 },
        "Special seed value eliminates initial key with xor",
        0x5082EDEE);

    /// <summary>
    /// Tests an array with bytes 0x654321 with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void Test214365InputSeed0() => this.Test(
        "7E4A8634",
        new[] { (byte)0x21, (byte)0x43, (byte)0x65 },
        "Only three bytes. Should end up as 0x654321");

    /// <summary>
    /// Tests an array with bytes 0x4321 with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void Test2143InputSeed0() => this.Test(
        "A0F7B07A",
        new[] { (byte)0x21, (byte)0x43 },
        "Only two bytes. Should end up as 0x4321");

    /// <summary>
    /// Tests an array with byte 0x21 with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void Test21InputSeed0() =>
        this.Test("72661CF4", new[] { (byte)0x21 }, "Only one byte. Should end up as 0x21");

    /// <summary>
    /// Tests an array with bytes 0x00000000 with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void Test00000000InputSeed0() =>
        this.Test("2362F9DE", new byte[4], "Make sure compiler doesn't see zero and convert to null");

    /// <summary>
    /// Tests an array with bytes 0x000000 with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void Test000000InputSeed0() =>
        this.Test("85F0B427", new byte[3], "Make sure compiler doesn't see zero and convert to null");

    /// <summary>
    /// Tests an array with bytes 0x0000 with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void Test0000InputSeed0() =>
        this.Test("30F4C306", new byte[2], "Make sure compiler doesn't see zero and convert to null");

    /// <summary>
    /// Tests an array with bytes 0x00 with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void Test00InputSeed0() =>
        this.Test("514E28B7", new byte[1], "Make sure compiler doesn't see zero and convert to null");

    /// <summary>
    /// ReSharper disable CommentTypo
    /// Tests a common string ("Lorem ipsum") with an input seed of 0x00000000.
    /// ReSharper restore CommentTypo.
    /// </summary>
    [TestMethod]
    //// ReSharper disable IdentifierTypo
    public void TestLoremIpsumInputSeed0() => this.Test(
    //// ReSharper restore IdentifierTypo
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "3BF7E870",
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        //// ReSharper disable StringLiteralTypo
        "Lorem ipsum dolor sit amet, consectetur adipisicing elit",
        //// ReSharper restore StringLiteralTypo
        "Lengthy string interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxInputSeed0() => this.Test(
        "2E4FF723",
        "The quick brown fox jumps over the lazy dog",
        "lengthy string interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x9747B28C.
    /// </summary>
    [TestMethod]
    //// ReSharper disable InconsistentNaming
    public void TestQuickBrownFoxInputSeed9747b28c() => this.Test(
    //// ReSharper restore InconsistentNaming
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "2FA826CD",
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        "The quick brown fox jumps over the lazy dog",
        "lengthy string interpreted as UTF-8 with seed",
        unchecked((int)0x9747B28CU));

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0xC58F1A7B.
    /// </summary>
    [TestMethod]
    //// ReSharper disable InconsistentNaming
    public void TestQuickBrownFoxInputSeedC58f1a7b() => this.Test(
    //// ReSharper restore InconsistentNaming
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "75B422D6",
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        "The quick brown fox jumps over the lazy dog",
        "lengthy string interpreted as UTF-8 with seed",
        unchecked((int)0xC58F1A7BU));

    /// <summary>
    /// Tests using the SMHasher KeysetTest VerificationTest.
    /// </summary>
    [TestMethod]
    public void TestSmHasher() => this.TestSmHasher("B0F57EE3");
}