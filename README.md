# Murmur3
Murmur3 hash algorithm in C#

This small project is an implementation of the Murmur3 hash algorithm for 32-bit x86, 128-bit x86, and 128-bit x64 variants.
All implemented classes descend from [System.IO.Hashing](https://learn.microsoft.com/en-us/dotnet/api/system.io.hashing)'s [NonCryptographicHashAlgorithm](https://learn.microsoft.com/en-us/dotnet/api/system.io.hashing.noncryptographichashalgorithm), which should make for easy adoption.

Example:

```cs
namespace Murmur3Test
{
    using System;
    using System.Globalization;
    using System.IO.Hashing;
    using System.Text;
    
    using Murmur3;
    
    public static class Program
    {
        public static void Main()
        {
            NonCryptographicHashAlgorithm alg = new Murmur3F();

            alg.Append(Encoding.UTF8.GetBytes("foobar"));
            Console.WriteLine(((ulong)BitConverter.ToInt64(alg.GetCurrentHash(), 0)).ToString("X8", CultureInfo.InvariantCulture));
        }
    }
}
```

This will output BDD2AE7116C85A45 as the Murmur3 128-bit x64 hash of the string "foobar".

## Benchmark Results

<!-- BENCHMARK_RESULTS_START -->
```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon 6973P-C 2.60GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  DefaultJob : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4


```
| Method   | Mean      | Error    | StdDev   |
|--------- |----------:|---------:|---------:|
| Murmur3H | 106.21 μs | 0.214 μs | 0.190 μs |
| Murmur3A |  36.67 μs | 0.075 μs | 0.066 μs |
| Murmur3C |  20.64 μs | 0.049 μs | 0.041 μs |
| Murmur3F |  15.33 μs | 0.030 μs | 0.027 μs |
<!-- BENCHMARK_RESULTS_END -->
