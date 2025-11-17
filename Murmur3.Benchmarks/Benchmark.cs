// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Benchmark.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Benchmark the Murmur3 32 x86, Murmur3 128 x86, and Murmur3 128 x64 hashing algorithm variants.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Murmur3.Benchmarks;

using System.IO.Hashing;

using BenchmarkDotNet.Attributes;

/// <summary>
/// Benchmark the Murmur3 32 x86, Murmur3 128 x86, and Murmur3 128 x64 hashing algorithm variants.
/// </summary>
#pragma warning disable CA1515 // Consider making public types internal
[Config(typeof(BenchmarkConfig))]
public class Benchmark
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// The size of the random byte array to hash.
    /// </summary>
    private const int N = 100000;

    /// <summary>
    /// The random byte array to hash.
    /// </summary>
    private readonly byte[] _data;

    /// <summary>
    /// The Murmur3 32 x86 hasher.
    /// </summary>
    private readonly NonCryptographicHashAlgorithm _murmur3A = new Murmur3A();

    /// <summary>
    /// The Murmur3 128 x86 hasher.
    /// </summary>
    private readonly NonCryptographicHashAlgorithm _murmur3C = new Murmur3C();

    /// <summary>
    /// The Murmur3 128 x64 hasher.
    /// </summary>
    private readonly NonCryptographicHashAlgorithm _murmur3F = new Murmur3F();

    /// <summary>
    /// Initializes a new instance of the <see cref="Benchmark" /> class.
    /// </summary>
    public Benchmark()
    {
        _data = new byte[N];
#pragma warning disable CA5394 // Do not use insecure randomness
        Random.Shared.NextBytes(_data);
#pragma warning restore CA5394 // Do not use insecure randomness
    }

    /// <summary>
    /// Benchmark the Murmur3 32 x86 hashing algorithm variant.
    /// </summary>
    /// <returns>The resulting hash value of the random byte array.</returns>
    [Benchmark]
    public byte[] Murmur3A()
    {
        _murmur3A.Reset();
        _murmur3A.Append(_data);
        return _murmur3A.GetCurrentHash();
    }

    /// <summary>
    /// Benchmark the Murmur3 128 x86 hashing algorithm variant.
    /// </summary>
    /// <returns>The resulting hash value of the random byte array.</returns>
    [Benchmark]
    public byte[] Murmur3C()
    {
        _murmur3C.Reset();
        _murmur3C.Append(_data);
        return _murmur3C.GetCurrentHash();
    }

    /// <summary>
    /// Benchmark the Murmur3 128 x64 hashing algorithm variant.
    /// </summary>
    /// <returns>The resulting hash value of the random byte array.</returns>
    [Benchmark]
    public byte[] Murmur3F()
    {
        _murmur3F.Reset();
        _murmur3F.Append(_data);
        return _murmur3F.GetCurrentHash();
    }
}