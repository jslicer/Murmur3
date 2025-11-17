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
