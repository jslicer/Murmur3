// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3FTests.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Tests the Murmur3 128 x64 hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// Ignore Spelling: Lorem Ipsum
namespace Murmur3.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests the Murmur3 128 x64 hashing algorithm variant.
/// </summary>
/// <seealso cref="Murmur3TestsBase" />
[TestClass]
//// ReSharper disable once ClassTooBig
public sealed class Murmur3FTests : Murmur3TestsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Murmur3FTests" /> class.
    /// </summary>
    public Murmur3FTests()
        : base(typeof(Murmur3F))
    {
        // Intentionally empty.
    }

    /// <summary>
    /// Tests an empty <see langword="byte" /> array with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestEmptyInputSeed0() => Test(
        "00000000000000000000000000000000",
        [],
        "with zero data and zero seed, everything becomes zero");

    /// <summary>
    /// ReSharper disable CommentTypo
    /// Tests a common string ("Lorem ipsum") with an input seed of 0x00000000.
    /// ReSharper restore CommentTypo.
    /// </summary>
    [TestMethod]
    //// ReSharper disable IdentifierTypo
    public void TestLoremIpsumInputSeed0() => Test(
        //// ReSharper restore IdentifierTypo
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "46704160FFF09DE66FDC5EFD2CB05C6F",
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
    [TestMethod]
    public void TestQuickBrownFoxInputSeed0() => Test(
        "7A433CA9C49A9347E34BBC7BBC071B6C",
        "The quick brown fox jumps over the lazy dog",
        "lengthy string interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxPeriodInputSeed0() => Test(
        "695DA1A38987B6E7CD99481F9EE902C9",
        "The quick brown fox jumps over the lazy dog.",
        "lengthy string terminated with a period interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxTwoPeriodsInputSeed0() => Test(
        "C223E9BAA9E88CBE5E157F5CF160B380",
        "The quick brown fox jumps over the lazy dog..",
        "lengthy string terminated with two periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxThreePeriodsInputSeed0() => Test(
        "5298F65A5EB449336587A20B11FCCB07",
        "The quick brown fox jumps over the lazy dog...",
        "lengthy string terminated with three periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxFourPeriodsInputSeed0() => Test(
        "28DDB16FB6A6B6B2EC01139A7A722680",
        "The quick brown fox jumps over the lazy dog....",
        "lengthy string terminated with four periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxFivePeriodsInputSeed0() => Test(
        "08CA8F73395928ACBAE6A92A3DF6B312",
        "The quick brown fox jumps over the lazy dog.....",
        "lengthy string terminated with five periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxSixPeriodsInputSeed0() => Test(
        "933A71892F0F32F58C4614ADD7E407EA",
        "The quick brown fox jumps over the lazy dog......",
        "lengthy string terminated with six periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxSevenPeriodsInputSeed0() => Test(
        "E84E129A47F01AF21B89D9B92E931A40",
        "The quick brown fox jumps over the lazy dog.......",
        "lengthy string terminated with seven periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxEightPeriodsInputSeed0() => Test(
        "E72D267D8A24E3AE4399239217BD6C2F",
        "The quick brown fox jumps over the lazy dog........",
        "lengthy string terminated with eight periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxNinePeriodsInputSeed0() => Test(
        "0D52349174C183AF24E0C47644D2F488",
        "The quick brown fox jumps over the lazy dog.........",
        "lengthy string terminated with nine periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxTenPeriodsInputSeed0() => Test(
        "47A02D677C2E06615C9FB5CC1F62B44D",
        "The quick brown fox jumps over the lazy dog..........",
        "lengthy string terminated with ten periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxElevenPeriodsInputSeed0() => Test(
        "B9B9C05FD54992B290904D16DDA6E4C1",
        "The quick brown fox jumps over the lazy dog...........",
        "lengthy string terminated with eleven periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxTwelvePeriodsInputSeed0() => Test(
        "8CDF9B1300705FB773AFFBBA72F9A33E",
        "The quick brown fox jumps over the lazy dog............",
        "lengthy string terminated with twelve periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxThirteenPeriodsInputSeed0() => Test(
        "8EFCCC016B0FB773A7B7F7B28562C5ED",
        "The quick brown fox jumps over the lazy dog.............",
        "lengthy string terminated with thirteen periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxFourteenPeriodsInputSeed0() => Test(
        "ED38223BD3583EE31AA17B12FEC33C6C",
        "The quick brown fox jumps over the lazy dog..............",
        "lengthy string terminated with thirteen periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxFifteenPeriodsInputSeed0() => Test(
        "6A9C394DCFBD278BDA4103A8B6E46FEF",
        "The quick brown fox jumps over the lazy dog...............",
        "lengthy string terminated with fifteen periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x9747B28C.
    /// </summary>
    [TestMethod]
    //// ReSharper disable InconsistentNaming
    public void TestQuickBrownFoxInputSeed9747b28c() => Test(
        //// ReSharper restore InconsistentNaming
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "F94573727EC016E5738A7F3BD2633121",
        //// ReSharper disable once StyleCop.SA1118
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
    public void TestQuickBrownFoxInputSeedC58f1a7b() => Test(
        //// ReSharper restore InconsistentNaming
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1114 // Parameter list should follow declaration
        "38935C52DEEFF526AC1F40EED20C9DFF",
        //// ReSharper disable once StyleCop.SA1118
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        "The quick brown fox jumps over the lazy dog",
        "lengthy string interpreted as UTF-8 with seed",
        unchecked((int)0xC58F1A7BU));

    /// <summary>
    /// Tests using the SMHasher KeysetTest VerificationTest.
    /// </summary>
    /// <returns>An asynchronous <see cref="Task" />.</returns>
    [TestMethod]
    public async Task TestSmHasherAsync()
    {
        using CancellationTokenSource cts = new ();
        await TestSmHasherAsync("7192878CE684ED2D63F3DE036384BA69", cts.Token).ConfigureAwait(false);
    }
}