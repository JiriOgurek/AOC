using System.Diagnostics;

namespace AOC2024.Day02
{
    internal static class Day02
    {
        public static void Solve()
        {
            var input = File.ReadAllLines(@"Day02\input.txt");

            var reportList = input.Select(s => s.Split(' ')).Select(n => n.Select(int.Parse).ToList()).ToList();

            var stopwatch = Stopwatch.StartNew();
            PartOne(reportList);
            stopwatch.Stop();
            Console.WriteLine($"PartOne execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch = Stopwatch.StartNew();
            PartTwo(reportList);
            stopwatch.Stop();
            Console.WriteLine($"PartTwo execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");

        }

        private static void PartOne(List<List<int>> reportList)
        {
            var safeCount = 0;
            foreach (var report in from report in reportList let order = CheckOrder(report) where order != 0 select report)
            {
                var n = 0;
                for (var i = 0; i < report.Count - 1; i++)
                {
                    var delta = Math.Abs(report[i] - report[i + 1]);
                    if (delta is >= 1 and <= 3)
                        n++;
                }

                if (n == report.Count - 1)
                    safeCount++;
            }

            Console.WriteLine(safeCount);
        }

        private static void PartTwo(List<List<int>> reportList)
        {
            for (var i = 0; i < reportList.Count; i++)
            {
                if (!reportList[i].IsValid())
                {
                    for (var j = 0; j < reportList[i].Count; j++)
                    {
                        var l = new List<int>(reportList[i]);
                        l.RemoveAt(j);
                        if (l.IsValid())
                        {
                            reportList[i] = l;
                            break;
                        }
                    }
                }
            }

            PartOne(reportList);
        }

        private static bool IsValid(this List<int> report)
        {
            if (CheckOrder(report) == 0)
                return false;

            for (var i = 0; i < report.Count - 1; i++)
            {
                var delta = Math.Abs(report[i] - report[i + 1]);
                if (delta is < 1 or > 3)
                    return false;
            }
            return true;
        }

        private static int CheckOrder(List<int> list)
        {
            var isAscending = true;
            var isDescending = true;

            for (var i = 1; i < list.Count; i++)
            {
                if (list[i] < list[i - 1])
                {
                    isAscending = false;
                }
                if (list[i] > list[i - 1])
                {
                    isDescending = false;
                }
            }

            if (isAscending)
            {
                return 1;
            }

            if (isDescending)
            {
                return -1;
            }

            return 0;
        }
    }
}
