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
            var ranges = new List<Interval>();
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
                        ranges.Add(new Interval(from, to));
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

            var intervals = new List<Interval>();

            foreach (var r in ranges) 
            {
                InsertInterval(intervals, r.Start, r.End);
            }

            var sum = intervals.Sum(interval => (interval.End - interval.Start + 1));

            Console.WriteLine($"Part Two: {sum}");
        }

        public class Interval
        {
            public long Start { get; set; }
            public long End { get; set; }

            public Interval(long start, long end)
            {
                if (start > end)
                    throw new ArgumentException("Start must be <= End");

                Start = start;
                End = end;
            }

            public override string ToString() => $"[{Start}, {End}]";
        }


        public static void InsertInterval(List<Interval> intervals, long newStart, long newEnd)
        {
            if (newStart > newEnd)
                throw new ArgumentException("newStart must be <= newEnd");

            var mergedStart = newStart;
            var mergedEnd = newEnd;

            // Sem si budeme pamatovat, kam nový/rozšířený interval vložíme
            var insertIndex = 0;

            // Projdeme seznam a najdeme všechny intervaly, které se
            // s novým intervalem překrývají nebo s ním sousedí.
            for (var i = 0; i < intervals.Count;)
            {
                var current = intervals[i];

                // 1) Aktuální interval je úplně vpravo, bez dotyku
                //    [mergedStart, mergedEnd] ... [current.Start, current.End]
                if (current.Start > mergedEnd + 1)
                {
                    // Už jsme za všemi, kterých se to týká → skončíme cyklus.
                    insertIndex = i;
                    break;
                }

                // 2) Aktuální interval je úplně vlevo, bez dotyku
                //    [current.Start, current.End] ... [mergedStart, mergedEnd]
                if (current.End < mergedStart - 1)
                {
                    // Tento interval necháme být, jdeme dál.
                    insertIndex = i + 1;
                    i++;
                    continue;
                }

                // 3) Jinak se buď překrývá, je uvnitř, nebo sousedí zleva/zprava:
                //    sloučíme ho do merged intervalu.
                mergedStart = Math.Min(mergedStart, current.Start);
                mergedEnd = Math.Max(mergedEnd, current.End);

                // Odebereme current, protože bude „pohlcen“ merged intervalem.
                intervals.RemoveAt(i);
                // Neinkrementujeme i, protože se seznam zkrátil, na tomto indexu je nový prvek
            }

            // Vložíme výsledný merged interval na správnou pozici
            intervals.Insert(insertIndex, new Interval(mergedStart, mergedEnd));
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