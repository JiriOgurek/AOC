using System.Collections.Generic;
using System.Diagnostics;

namespace AOC2025.Day02
{
    internal static class Day02
    {
        public static void Solve()
        {
            var input = File.ReadAllText(@"Day02\input.txt").Split(',', StringSplitOptions.RemoveEmptyEntries);


            var stopwatch = Stopwatch.StartNew();
            PartOne(input);
            stopwatch.Stop();
            Console.WriteLine($"PartOne execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch = Stopwatch.StartNew();
            PartTwo(input);
            stopwatch.Stop();
            Console.WriteLine($"PartTwo execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");
        }

        private static void PartOne(string[] input)
        {
            long sum = 0;
            
            foreach (var line in input)
            {
                var num = line.Split('-').Select(long.Parse).ToArray();
                for (var i = num[0]; i <= num[1]; i++)
                {
                    var s = i.ToString();

                    if (s.Length % 2 == 0)
                    {
                        if (s[..(s.Length / 2)] == s[(s.Length / 2)..])
                        {
                            //Console.WriteLine(i);
                            sum += i;
                        }
                    }
                }
            }

            Console.WriteLine(sum);
        }

        private static void PartTwo(string[] input)
        {
            long sum = 0;

            foreach (var line in input)
            {
                var num = line.Split('-').Select(long.Parse).ToArray();
                for (var i = num[0]; i <= num[1]; i++)
                {
                    var s = i.ToString();

                    if (s.Length == 1)
                        continue;

                    var delitele = VlastniDelitele(s.Length);

                    foreach (var delitel in delitele)
                    {
                        var l = SplitStringByNumberOfChars(s, delitel);
                        if (l.Distinct().Count() <= 1)
                        {
                            //Console.WriteLine(i);
                            sum += i;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine(sum);
        }

        private static List<string> SplitStringByNumberOfChars(string s, int chunkSize)
        {
            var result = new List<string>();
            for (var i = 0; i < s.Length; i += chunkSize)
            {
                result.Add(s.Substring(i, Math.Min(chunkSize, s.Length - i)));
            }
            return result;
        }

        public static List<int> VlastniDelitele(int n)
        {
            if (n == 1)
                return [0];

            var vysledek = new List<int>();

            for (var i = 1; i < n; i++)
            {
                if (n % i == 0)
                    vysledek.Add(i);
            }

            return vysledek;
        }

    }
}