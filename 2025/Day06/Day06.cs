using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AOC2025.Day06
{
    internal static class Day06
    {
        public static void Solve()
        {
            var input = File.ReadAllLines(@"Day06\input.txt");


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