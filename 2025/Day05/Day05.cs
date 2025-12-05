using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AOC2025.Day05
{
    internal static class Day05
    {
        public static void Solve()
        {
            var input = File.ReadAllLines(@"Day05\input.txt");


            var stopwatch = Stopwatch.StartNew();
            PartOne(input);
            stopwatch.Stop();
            Console.WriteLine($"PartOne execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch = Stopwatch.StartNew();
            PartTwo(input);
            stopwatch.Stop();
            Console.WriteLine($"PartTwo execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");
        }

        private static void PartTwo(string[] input)
        {
            var secondHalf = false;

            //read all ranges and ids
            var ranges = new List<LongRange>();
            var ids = new List<long>();

            foreach (var line in input)
            {
                if (!secondHalf)
                {
                    // first half of the input - ranges
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        secondHalf = true;
                        continue;
                    }

                    var parts = line.Split("-", StringSplitOptions.TrimEntries);
                    if (parts.Length == 2 &&
                        long.TryParse(parts[0], out var from) &&
                        long.TryParse(parts[1], out var to))
                    {
                        ranges.Add(new LongRange(from, to));
                    }
                }
                else
                {
                    // second half of the input - ids
                    if (long.TryParse(line, out var id))
                    {
                        ids.Add(id);
                    }
                }
            }

            var l = new List<long>();
            foreach (var r in ranges) {
                for (var i = r.From; i <= r.To; i++) {
                    if (!l.Contains(i))
                        l.Add(i);
                }
            }

            Console.WriteLine($"Part Two: {l.Count}");
        }

        private static void PartOne(string[] input)
        {
            var secondHalf = false;

            //read all ranges and ids
            var ranges = new List<LongRange>();
            var ids = new List<long>();

            foreach (var line in input)
            {
                if (!secondHalf)
                {
                    // first half of the input - ranges
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        secondHalf = true;
                        continue;
                    }

                    var parts = line.Split("-", StringSplitOptions.TrimEntries);
                    if (parts.Length == 2 &&
                        long.TryParse(parts[0], out var from) &&
                        long.TryParse(parts[1], out var to))
                    {
                        ranges.Add(new LongRange(from, to));
                    }
                }
                else
                {
                    // second half of the input - ids
                    if (long.TryParse(line, out var id))
                    {
                        ids.Add(id);
                    }
                }
            }

            var fresh = 0;

            foreach (var id in ids)
            {
                var isValid = false;
                foreach (var range in ranges)
                {
                    if (range.Contains(id))
                    {
                        isValid = true;
                        break;
                    }
                }
                
                if (isValid)
                {
                    fresh++;
                }
            }

            Console.WriteLine($"Part One: {fresh}");
        }

        public readonly struct LongRange
        {
            public long From { get; }
            public long To { get; }

            public LongRange(long from, long to)
            {
                if (from > to)
                    throw new ArgumentException("From must be <= To.");

                From = from;
                To = to;
            }

            public bool Contains(long value)
                => value >= From && value <= To;

            public override string ToString() => $"{From}..{To}";
        }
    }
}