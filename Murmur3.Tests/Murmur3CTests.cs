// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3CTests.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Tests the Murmur3 128 x86 hashing algorithm variant.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// Ignore Spelling: Lorem Ipsum
namespace Murmur3.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Tests the Murmur3 128 x86 hashing algorithm variant.
/// </summary>
/// <seealso cref="Murmur3TestsBase" />
[TestClass]
//// ReSharper disable once ClassTooBig
public sealed class Murmur3CTests : Murmur3TestsBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Murmur3CTests" /> class.
    /// </summary>
    public Murmur3CTests()
        : base(typeof(Murmur3C))
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
        "10918848739FA4FE56BA3BAD573F53AB",
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
        "E5E91D2C5D7BF66CECEE2C672F1583C3",
        "The quick brown fox jumps over the lazy dog",
        "lengthy string interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxPeriodInputSeed0() => Test(
        "9B627B552BBF0FBB7DD6ED5E6CBB6099",
        "The quick brown fox jumps over the lazy dog.",
        "lengthy string terminated with a period interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxTwoPeriodsInputSeed0() => Test(
        "46D9869E8B44740D36D45DACCB28467F",
        "The quick brown fox jumps over the lazy dog..",
        "lengthy string terminated with two periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxThreePeriodsInputSeed0() => Test(
        "776D65E1B507CC5EB934BC20E96FB75B",
        "The quick brown fox jumps over the lazy dog...",
        "lengthy string terminated with three periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxFourPeriodsInputSeed0() => Test(
        "73430BBCDD00D016A5D9C973992CCF9C",
        "The quick brown fox jumps over the lazy dog....",
        "lengthy string terminated with four periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxFivePeriodsInputSeed0() => Test(
        "55D4DFDCD757F91E93F59A3BC48FC2E4",
        "The quick brown fox jumps over the lazy dog.....",
        "lengthy string terminated with five periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxSixPeriodsInputSeed0() => Test(
        "359E4A4A8B938FB90FFC6C28A59582E8",
        "The quick brown fox jumps over the lazy dog......",
        "lengthy string terminated with six periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxSevenPeriodsInputSeed0() => Test(
        "D66032D0CA66A6CBDDF7CD05ACFEBEF2",
        "The quick brown fox jumps over the lazy dog.......",
        "lengthy string terminated with seven periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxEightPeriodsInputSeed0() => Test(
        "10CF26226AA52EDB099F88121753C20B",
        "The quick brown fox jumps over the lazy dog........",
        "lengthy string terminated with eight periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxNinePeriodsInputSeed0() => Test(
        "CE7118471214B7DE9C7B9ED148425B34",
        "The quick brown fox jumps over the lazy dog.........",
        "lengthy string terminated with nine periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxTenPeriodsInputSeed0() => Test(
        "31AA767F508A9C474366F4A51095C591",
        "The quick brown fox jumps over the lazy dog..........",
        "lengthy string terminated with ten periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxElevenPeriodsInputSeed0() => Test(
        "96079342DC3D180A674ACA58996785C2",
        "The quick brown fox jumps over the lazy dog...........",
        "lengthy string terminated with eleven periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxTwelvePeriodsInputSeed0() => Test(
        "32D4B4397F1C3EA2CAC587A65CDD2756",
        "The quick brown fox jumps over the lazy dog............",
        "lengthy string terminated with twelve periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxThirteenPeriodsInputSeed0() => Test(
        "3D82ECDA91BE7726A9CE122907F52FB9",
        "The quick brown fox jumps over the lazy dog.............",
        "lengthy string terminated with thirteen periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxFourteenPeriodsInputSeed0() => Test(
        "8723B270C9CBF5E87F949E7C75B27D54",
        "The quick brown fox jumps over the lazy dog..............",
        "lengthy string terminated with thirteen periods interpreted as UTF-8");

    /// <summary>
    /// Tests a common string ("The quick brown fox") with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestQuickBrownFoxFifteenPeriodsInputSeed0() => Test(
        "6A0781B1FE80628E7518AD5C73C2E863",
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
        "CDB6793E8EA73A9C4CB861718AD4D55E",
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
        "E8F88FA36A97662B3A378F614AA93B4F",
        //// ReSharper disable once StyleCop.SA1118
#pragma warning restore SA1114 // Parameter list should follow declaration
#pragma warning restore IDE0079 // Remove unnecessary suppression
        "The quick brown fox jumps over the lazy dog",
        "lengthy string interpreted as UTF-8 with seed",
        unchecked((int)0xC58F1A7BU));

    /// <summary>
    /// Tests using the SMHasher KeysetTest VerificationTest.
    /// </summary>
    [TestMethod]
    public void TestSmHasher() => TestSmHasher("F4AFBC3F0A9C1DE0E7DA6F515A670AB9");
}