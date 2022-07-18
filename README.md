# Murmur3
Murmur3 hash algorithm in C#

This small project is an implementation of the Murmur3 hash algorithm for 32-bit x86, 128-bit x86, and 128-bit x64 variants.
All implemented classes descend from the CLR's HashAlgotithm, which should make for easy adoption.

Example:

```cs
namespace Murmur3Test
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    
    using Murmur3;
    
    public static class Program
    {
        public static void Main()
        {
            using (HashAlgorithm alg = new Murmur3F())
            {
                Console.WriteLine(((ulong)BitConverter.ToInt64(alg.ComputeHash(Encoding.UTF8.GetBytes("foobar")), 0)).ToString("X8", CultureInfo.InvariantCulture));
            }
        }
    }
}
```

This will output BDD2AE7116C85A45 as the Murmur3 128-bit x64 hash of the string "foobar".
