```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.3 LTS (Noble Numbat)
AMD EPYC 7763 2.45GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.102
  [Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3


```
| Method   | Mean     | Error    | StdDev   |
|--------- |---------:|---------:|---------:|
| Murmur3A | 33.38 μs | 0.099 μs | 0.088 μs |
| Murmur3C | 31.58 μs | 0.058 μs | 0.048 μs |
| Murmur3F | 18.58 μs | 0.034 μs | 0.031 μs |
