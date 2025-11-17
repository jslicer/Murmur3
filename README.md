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

BenchmarkDotNet v0.15.6, Linux Ubuntu 24.04.3 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.100
  [Host]     : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v4
  DefaultJob : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v4


```
| Method   | Mean     | Error    | StdDev   |
|--------- |---------:|---------:|---------:|
| Murmur3A | 66.30 μs | 0.070 μs | 0.062 μs |
| Murmur3C | 29.67 μs | 0.079 μs | 0.066 μs |
| Murmur3F | 18.60 μs | 0.126 μs | 0.112 μs |
<!-- BENCHMARK_RESULTS_END -->
