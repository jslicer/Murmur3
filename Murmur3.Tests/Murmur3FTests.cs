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
    /// Tests an empty byte array with an input seed of 0x00000000.
    /// </summary>
    [TestMethod]
    public void TestEmptyInputSeed0() => this.Test(
        "00000000000000000000000000000000",
        Array.Empty<byte>(),
        "with zero data and zero seed, everything becomes zero");

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
    public void TestQuickBrownFoxInputSeed0() => this.Test(
        "7A433CA9C49A9347E34BBC7BBC071B6C",
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
    public void TestQuickBrownFoxInputSeedC58f1a7b() => this.Test(
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
    public async Task TestSmHasherAsync() =>
        await this.TestSmHasherAsync("7192878CE684ED2D63F3DE036384BA69").ConfigureAwait(false);
}