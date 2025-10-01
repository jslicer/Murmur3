// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Murmur3Base.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Implements the common Murmur3 hashing algorithm functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3;

using System.IO.Hashing;
using System.Runtime.CompilerServices;

/// <inheritdoc />
/// <summary>
/// Implements the common Murmur3 hashing algorithm functionality.
/// </summary>
/// <seealso cref="NonCryptographicHashAlgorithm" />
public abstract class Murmur3Base : NonCryptographicHashAlgorithm
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Murmur3Base" /> class.
    /// </summary>
    /// <param name="hashSizeValue">The hash size value in bits.</param>
    /// <param name="seed">The seed value.</param>
    protected Murmur3Base(in int hashSizeValue, in int seed = 0x00000000)
        : base(hashSizeValue >> 3) =>
        Seed = unchecked((uint)seed);

    /// <summary>
    /// Gets the seed value.
    /// </summary>
    /// <value>
    /// The seed value.
    /// </value>
    protected uint Seed { get; }

    /// <summary>
    /// Gets or sets the data length.
    /// </summary>
    /// <value>
    /// The data length.
    /// </value>
    protected int Length { get; set; }

    /// <summary>
    /// Initializes the hash for this instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void Init() => Length = 0;
}