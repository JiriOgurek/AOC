using System.Diagnostics;

namespace AOC2025.Day01
{
    internal static class Day01
    {
        public static void Solve()
        {
            var input = File.ReadAllLines(@"Day01\input.txt");

            var leftList = new List<int>();
            var rightList = new List<int>();

            foreach (var s in input)
            {
                var n = s.Split("   ");
                leftList.Add(int.Parse(n[0]));
                rightList.Add(int.Parse(n[1]));
            }

            var stopwatch = Stopwatch.StartNew();
            PartOne(leftList, rightList);
            stopwatch.Stop();
            Console.WriteLine($"PartOne execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch = Stopwatch.StartNew();
            PartTwo(leftList, rightList);
            stopwatch.Stop();
            Console.WriteLine($"PartTwo execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");
        }

        private static void PartTwo(List<int> leftList, List<int> rightList)
        {
            var sum = leftList.Sum(i => rightList.Count(t => t == i) * i);

            Console.WriteLine(sum);
        }

        private static void PartOne(List<int> leftList, List<int> rightList)
        {
            leftList.Sort();
            rightList.Sort();

            var sum = leftList.Select((t, i) => t >= rightList[i] ? t - rightList[i] : rightList[i] - t).Sum();

            Console.WriteLine(sum);
        }
    }
}