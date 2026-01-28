```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.3 LTS (Noble Numbat)
Intel Xeon Platinum 8370C CPU 2.80GHz (Max: 2.79GHz), 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.102
  [Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v4
  DefaultJob : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v4


```
| Method   | Mean     | Error    | StdDev   |
|--------- |---------:|---------:|---------:|
| Murmur3A | 67.38 μs | 0.060 μs | 0.050 μs |
| Murmur3C | 25.77 μs | 0.013 μs | 0.010 μs |
| Murmur3F | 21.77 μs | 0.004 μs | 0.003 μs |
