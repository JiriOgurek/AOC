using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace AOC2025.Day07
{
    internal static class Day07
    {
        public static void Solve()
        {
            var input = File.ReadAllLines(@"Day07\input.txt");

            var stopwatch = Stopwatch.StartNew();
            PartOne(input);
            stopwatch.Stop();
            Console.WriteLine($"PartOne execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");

            //stopwatch = Stopwatch.StartNew();
            //PartTwo(input);
            //stopwatch.Stop();
            //Console.WriteLine($"PartTwo execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");
        }

        private static void PartOne(string[] input)
        {

        }
    }
}
