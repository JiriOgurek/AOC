using System.Text.RegularExpressions;

namespace AOC2024.Day03
{
    internal static class Day03
    {
        public static void Solve()
        {
            var input = File.ReadAllText(@"Day03\input.txt");
            PartOne(input);
            PartTwo(input);
        }

        private static void PartOne(string input)
        {
            var matches = FindMulInstances(input, @"mul\((\d+),(\d+)\)");
            var sum = matches.Sum(match => match.X * match.Y);
            Console.WriteLine(sum);
        }

        private static void PartTwo(string input)
        {
            var matches = FindMulInstances(input, @"mul\((\d+),(\d+)\)");
            var donts = FindIndexes(input, @"don't\(\)");
            var dos = FindIndexes(input, @"do\(\)");

            var validIndexes = GetValidIndexes(dos, donts, input.Length);

            var sum = matches.Where(match => validIndexes[match.Index].Item2).Sum(match => match.X * match.Y);

            Console.WriteLine(sum);
        }

        private static List<(int, bool)> GetValidIndexes(List<int> dos, List<int> donts, int length)
        {
            var result = new List<(int, bool)>();
            var state = true;
            for (var i = 0; i < length; i++)
            {
                if (donts.Contains(i))
                    state = false;
                else
                    if (dos.Contains(i))
                    state = true;

                result.Add((i, state));
            }
            return result;
        }

        private static List<MulInstance> FindMulInstances(string input, string pattern)
        {
            var regex = new Regex(pattern);
            var matches = regex.Matches(input);

            var result = new List<MulInstance>();
            foreach (Match match in matches)
            {
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                var index = match.Index;

                result.Add(new MulInstance
                {
                    X = x,
                    Y = y,
                    Index = index
                });
            }

            return result;
        }

        private static List<int> FindIndexes(string input, string pattern)
        {
            var regex = new Regex(pattern);
            var matches = regex.Matches(input);

            var result = new List<int>();
            foreach (Match match in matches)
            {
                result.Add(match.Index);
            }

            return result;
        }
    }

    internal class MulInstance
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Index { get; set; }
    }
}
