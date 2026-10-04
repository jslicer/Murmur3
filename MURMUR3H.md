# Murmur3H — a 16-bit Murmur3 variant

## Why a new function rather than "Murmur3 truncated"
Murmur3-32 relies on 32-bit multiplies, rotates by 15/13 and 16-bit-wide fmix shifts. On a 16-bit CPU
(TMS9900) each 32-bit multiply is ~3 MPYs plus carries. Murmur3H shrinks every word to 16 bits so each
step is one native instruction, while keeping the same structure.

## Specification (all arithmetic mod 2^16)

| Item | Murmur3-32 | Murmur3H |
|---|---|---|
| Block | 4 bytes LE | **2 bytes LE** |
| c1 / c2 | 0xCC9E2D51 / 0x1B873593 | **0x49DB / 0x3C4F** |
| rotl(k) / rotl(h) | 15 / 13 | **7 / 6** |
| h update | h*5 + 0xE6546B64 | h*5 + **0x6B64** |
| Tail | 1-3 bytes | **0-1 byte** |
| Length | 32-bit | **16-bit (mod 65,536)** |
| fmix | >>16, *85EBCA6B, >>13, *C2B2AE35, >>16 | **>>8, *A42B, >>7, *E6CB, >>9** |

    h = seed
    for each block k:  k*=C1; k=rotl(k,7); k*=C2; h^=k; h=rotl(h,6); h=h*5+0x6B64
    if odd:            k=last byte; k*=C1; k=rotl(k,7); k*=C2; h^=k
    h ^= len;  h = fmix16(h)

## Design reasoning
* **Odd multipliers** make every multiply a bijection on 16 bits; xor, rotate and add are bijections too.
  Consequence: any single 2-byte message maps to a unique hash (unit-tested exhaustively).
* **Rotates 7 and 6** (not 8): 8 only moves whole bytes, so low-byte and high-byte entropy would stay in
  separate lanes. On the TMS9900 they are still cheap: `SWPB` + `SRC 1` (ROTL 7) and `SWPB` + `SRC 2` (ROTL 6).
* **h*5** is `SLA 2` + `A` (shift/add), cheaper than MPY (52 clocks).
* **fmix16 constants** came from a random search over odd multiplier pairs and shifts, scored by avalanche
  bias over the *entire* 65,536-value domain (the domain is small enough to test exhaustively).
  Block constants c1/c2 were chosen from 60 random pairs scored by whole-hash avalanche on 1-8 byte inputs.
* **Measured quality** (3,000 random inputs/length, lengths 1-16): RMS avalanche bias 1.6-2.0%
  (noise floor at that sample size is ~1.8%); 50,000 keys into 65,536 buckets gave 14,889 / 14,916
  collisions vs 15,023 expected for an ideal random function.

## Limits (be honest about 16 bits)
* Only 65,536 outputs: ~300 keys gives a 50% birthday collision. Use it for small hash tables, bloom-filter
  indices, checksums of small records, 64KB-address-space structures — not for identity.
* `hash("", seed 0) == 0`, as with Murmur3-32 (fmix(0) = 0). Use a non-zero seed if that matters.
* Length is mod 65,536, so inputs differing only by multiples of 64 KiB of zero padding aren't distinguished by length.
* Not cryptographic, not DoS-resistant.

## Endianness
Murmur3 is little-endian; the TMS9900 is big-endian. The assembly assembles each block with
`MOVB / SWPB / MOVB`, which also works at odd addresses (a `MOV` word load would silently force even alignment).

## TI-99/4A performance notes
* Per 2-byte block the two MPYs (52 clocks each) dominate; the full loop body is roughly 330 clocks
  by the data-book table, i.e. ~9,000 blocks/s (~18 kB/s) at 3 MHz in scratchpad. These are estimates;
  not measured on hardware.
* Keep the workspace at >8300 (16-bit zero-wait-state RAM). Expansion RAM and cartridge space sit behind the
  8-bit multiplexer and cost 4 extra wait states per word access, which slows the data loads but not the register math.

## Verification status
* Reference vectors generated from the Python reference; C# tests embed the same table.
* The assembly was run in a small purpose-built TMS9900 interpreter (my own, subset of instructions): all 13
  vectors pass, and a deliberately broken rotate is caught. **It has not been assembled with xas99/E-A or run
  on real hardware or in MAME/Classic99.** The C# was not compiled in my environment either (no .NET SDK available).
