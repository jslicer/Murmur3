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
