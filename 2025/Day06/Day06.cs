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
            var l = new List<List<int>>();
            var cols = 0;

            for (var index = 0; index < input.Length - 1; index++)
            {
                var s = input[index];
                var e = s.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (cols == 0)
                    cols = e.Length;
                else
                {
                    if (e.Length != cols)
                        throw new Exception("Invalid input");
                }

                var i = e.Select(n => int.Parse(n)).ToList();

                l.Add(i);
            }

            var ops = input[^1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            long sum = 0;

            for (var i = 0; i < ops.Length; i++)
            {
                long s = 0;
                long ss = 1;

                var op = ops[i];
                switch (op)
                {
                    case "+":
                    {
                        for (var index = 0; index < l.Count; index++)
                            s += l[index][i];
                        sum += s;
                            break;
                    }
                    case "*":
                    {
                        for (var index = 0; index < l.Count; index++)
                            ss *= l[index][i];
                        sum += ss;
                            break;
                    }
                    default:
                        throw new Exception("Invalid operation");
                }
            }

            Console.WriteLine($"PartOne: {sum}");
        }
    }
}