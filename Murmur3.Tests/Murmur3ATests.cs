// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3ATests.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Tests the Murmur3 32 x86 hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// Ignore Spelling: Lorem Ipsum Hasher EDEE
namespace Murmur3.Tests;

using System.Globalization;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static System.Globalization.NumberStyles;

/// <summary>
/// Tests the Murmur3 32 x86 hashing algorithm variant.
/// </summary>
/// <seealso cref="Murmur3TestsBase" />
[TestClass]
//// ReSharper disable once UnusedType.Global
#pragma warning disable CA1515 // Consider making public types internal
public sealed class Murmur3ATests : Murmur3TestsBase
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// The make sure compiler doesn't see zero and convert to null.
    /// </summary>
    // ReSharper disable once IdentifierTypo
    private const string MakeSureCompilerDoesntSeeZeroAndConvertToNull =
        "Make sure compiler doesn't see zero and convert to null";

    /// <summary>
    /// Initializes a new instance of the <see cref="Murmur3ATests" /> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">algType cannot be <see langword="null" />.</exception>
    public Murmur3ATests()
        : base(typeof(Murmur3A))
    {
        // Intentionally empty.
    }

    /// <summary>
    /// Tests an empty <see langword="byte" /> array with an input seed of 0x00000000.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void TestEmptyInputSeed0() => Test(
        "00000000",
        [],
        "with zero data and zero seed, everything becomes zero");

    /// <summary>
    /// Tests an empty <see langword="byte" /> array with an input seed of 0x00000001.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void TestEmptyInputSeed1() =>
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
        Test("514E28B7", [], "ignores nearly all the math", 0x00000001);
#pragma warning restore SA1010 // Opening square brackets should be spaced correctly
#pragma warning restore IDE0079 // Remove unnecessary suppression

    /// <summary>
    /// ReSharper disable once CommentTypo
    /// Tests an empty <see langword="byte" /> array with an input seed of 0xFFFFFFFF.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once InconsistentNaming
    //// ReSharper disable IdentifierTypo
    //// ReSharper disable once UnusedMember.Global
    public void TestEmptyInputSeedFFFFFFFF() => Test(
    //// ReSharper restore IdentifierTypo
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "81F16F39",
        //// ReSharper disable once StyleCop.SA1118
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        [],
        "make sure your seed uses unsigned 32-bit math",
        unchecked((int)0xFFFFFFFFU));

    /// <summary>
    /// ReSharper disable once CommentTypo
    /// Tests an array with bytes 0xFFFFFFFF with an input seed of 0x00000000.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once InconsistentNaming
    //// ReSharper disable IdentifierTypo
    //// ReSharper disable once UnusedMember.Global
    public void TestFFFFFFFFInputSeed0() => Test(
    //// ReSharper restore IdentifierTypo
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "76293B50",
        //// ReSharper disable once StyleCop.SA1118
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        //// ReSharper disable once StyleCop.SA1502
        [0xFF, 0xFF, 0xFF, 0xFF],
        "make sure 4-byte chunks use unsigned math");

    /// <summary>
    /// Tests an array with bytes 0x87654321 with an input seed of 0x00000000.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void Test21436587InputSeed0() => Test(
        "F55B516B",
        //// ReSharper disable once StyleCop.SA1502
        [0x21, 0x43, 0x65, 0x87],
        "Endian order. UInt32 should end up as 0x87654321");

    /// <summary>
    /// ReSharper disable once CommentTypo
    /// Tests an array with bytes 0x87654321 with an input seed of 0x5082EDEE.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once InconsistentNaming
    //// ReSharper disable IdentifierTypo
    //// ReSharper disable once UnusedMember.Global
    public void Test21436587InputSeed5082EDEE() => Test(
    //// ReSharper restore IdentifierTypo
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "2362F9DE",
        //// ReSharper disable once StyleCop.SA1118
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        //// ReSharper disable once StyleCop.SA1502
        [0x21, 0x43, 0x65, 0x87],
        "Special seed value eliminates initial key with xor",
        0x5082EDEE);

    /// <summary>
    /// Tests an array with bytes 0x654321 with an input seed of 0x00000000.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void Test214365InputSeed0() => Test(
        "7E4A8634",
        //// ReSharper disable once StyleCop.SA1502
        [.. "!Ce"u8],
        "Only three bytes. Should end up as 0x654321");

    /// <summary>
    /// Tests an array with bytes 0x4321 with an input seed of 0x00000000.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void Test2143InputSeed0() => Test(
        "A0F7B07A",
        //// ReSharper disable once StyleCop.SA1502
        [.. "!C"u8],
        "Only two bytes. Should end up as 0x4321");

    /// <summary>
    /// Tests an array with <see langword="byte" /> 0x21 with an input seed of 0x00000000.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void Test21InputSeed0() =>
        //// ReSharper disable once StyleCop.SA1502
        Test("72661CF4", [.. "!"u8], "Only one byte. Should end up as 0x21");

    /// <summary>
    /// Tests an array with <see langword="byte" />s 0x00000000 with an input seed of 0x00000000.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void Test00000000InputSeed0() =>
        Test("2362F9DE", new byte[4], MakeSureCompilerDoesntSeeZeroAndConvertToNull);

    /// <summary>
    /// Tests an array with <see langword="byte" />s 0x000000 with an input seed of 0x00000000.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void Test000000InputSeed0() =>
        Test("85F0B427", new byte[3], MakeSureCompilerDoesntSeeZeroAndConvertToNull);

    /// <summary>
    /// Tests an array with <see langword="byte" />s 0x0000 with an input seed of 0x00000000.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void Test0000InputSeed0() =>
        Test("30F4C306", new byte[2], MakeSureCompilerDoesntSeeZeroAndConvertToNull);

    /// <summary>
    /// Tests an array with <see langword="byte" />s 0x00 with an input seed of 0x00000000.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void Test00InputSeed0() =>
        Test("514E28B7", new byte[1], MakeSureCompilerDoesntSeeZeroAndConvertToNull);

    /// <summary>
    /// ReSharper disable CommentTypo
    /// Tests a common string ("Lorem ipsum") with an input seed of 0x00000000.
    /// ReSharper restore CommentTypo.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    /// <exception cref="EncoderFallbackException">A fallback occurred (for more information, see Character Encoding in
    /// .NET)
    ///  -and-
    ///  <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.</exception>
    [TestMethod]
    //// ReSharper disable IdentifierTypo
    //// ReSharper disable once UnusedMember.Global
    public void TestLoremIpsumInputSeed0() => Test(
    //// ReSharper restore IdentifierTypo
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "3BF7E870",
        //// ReSharper disable once StyleCop.SA1118
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        //// ReSharper disable StringLiteralTypo
        "Lorem ipsum dolor sit amet, consectetur adipisicing elit",
        //// ReSharper restore StringLiteralTypo
        "Lengthy string interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    /// <exception cref="EncoderFallbackException">A fallback occurred (for more information, see Character Encoding in
    /// .NET)
    ///  -and-
    ///  <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void TestQuickBrownFoxInputSeed0() => Test(
        "2E4FF723",
        "The quick brown fox jumps over the lazy dog",
        "lengthy string interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x9747B28C.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    /// <exception cref="EncoderFallbackException">A fallback occurred (for more information, see Character Encoding in
    /// .NET)
    ///  -and-
    ///  <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.</exception>
    [TestMethod]
    //// ReSharper disable InconsistentNaming
    //// ReSharper disable once UnusedMember.Global
    public void TestQuickBrownFoxInputSeed9747b28c() => Test(
    //// ReSharper restore InconsistentNaming
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "2FA826CD",
        //// ReSharper disable once StyleCop.SA1118
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        "The quick brown fox jumps over the lazy dog",
        "lengthy string interpreted as UTF-8 with seed",
        unchecked((int)0x9747B28CU));

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0xC58F1A7B.
    /// </summary>
    /// <exception cref="ArgumentException">style is not a <see cref="NumberStyles" /> value.
    ///  -or-
    ///  style includes the <see cref="AllowHexSpecifier" /> or <see cref="HexNumber" /> flag along with another
    /// value.</exception>
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    /// <exception cref="FormatException">value does not comply with the input pattern specified by style.</exception>
    /// <exception cref="ArgumentNullException">value is <see langword="null" />.</exception>
    /// <exception cref="EncoderFallbackException">A fallback occurred (for more information, see Character Encoding in
    /// .NET)
    ///  -and-
    ///  <see cref="EncoderFallback" /> is set to <see cref="EncoderExceptionFallback" />.</exception>
    [TestMethod]
    //// ReSharper disable InconsistentNaming
    //// ReSharper disable once UnusedMember.Global
    public void TestQuickBrownFoxInputSeedC58f1a7b() => Test(
    //// ReSharper restore InconsistentNaming
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "75B422D6",
        //// ReSharper disable once StyleCop.SA1118
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        "The quick brown fox jumps over the lazy dog",
        "lengthy string interpreted as UTF-8 with seed",
        unchecked((int)0xC58F1A7BU));

    /// <summary>
    /// Tests using the SMHasher KeysetTest VerificationTest.
    /// </summary>
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
    /// <exception cref="AssertFailedException">Thrown if expected is not equal to actual.</exception>
    [TestMethod]
    //// ReSharper disable once UnusedMember.Global
    public void TestSmHasher() => TestSmHasher("B0F57EE3");
}