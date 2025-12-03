using System.Diagnostics;

namespace AOC2025.Day01
{
    internal static class Day01
    {
        public static void Solve()
        {
            var input = File.ReadAllLines(@"Day01\input.txt");


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
            var i = 50;
            var zeroed = 0;

            foreach (var line in input)
            {
                i += line[0] == 'R' ? int.Parse(line[1..]) : -int.Parse(line[1..]);

                if (i % 100 == 0)
                    zeroed++;
            }

            Console.WriteLine($"Part One Result: {zeroed}");
        }

        private static void PartTwo(string[] input)
        {
            var i = 50;
            var zeroed = 0;

            foreach (var line in input)
            {
                var old = i;

                var m = int.Parse(line[1..]);
                zeroed += m / 100;
                m %= 100;

                i += line[0] == 'R' ? m : -m;

                if (i % 100 == 0 || (old < 0 && i > 0) || (old > 0 && i < 0) || (old < 100 && i > 100) || (old > 100 && i < 100))
                {
                    zeroed++;
                }

                switch (i)
                {
                    case < 0:
                        i += 100;
                        break;
                    case > 99:
                        i -= 100;
                        break;
                }
                
                //Console.WriteLine($"old: {old}, i: {i}, zeroed: {zeroed}");
            }

            Console.WriteLine($"Part Two Result: {zeroed}");
        }
    }
}