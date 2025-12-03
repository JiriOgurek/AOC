using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AOC2025.Day03
{
    internal static class Day03
    {
        public static void Solve()
        {
            var input = File.ReadAllLines(@"Day03\input.txt");


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
            var sum = input.Sum(s => long.Parse(GetBiggestNumber(s, 2)));

            Console.WriteLine(sum);
        }

        private static void PartTwo(string[] input)
        {
            var sum = input.Sum(s => long.Parse(GetBiggestNumber(s, 12)));

            Console.WriteLine(sum);
        }

        public static string GetBiggestNumber(string inputNumber, int n)
        {
            if (string.IsNullOrEmpty(inputNumber) || n <= 0) return "";
            if (n >= inputNumber.Length) return inputNumber;

            var digitsToRemove = inputNumber.Length - n;

            var resultStack = new StringBuilder();

            foreach (var currentDigit in inputNumber)
            {
                while (digitsToRemove > 0 &&
                       resultStack.Length > 0 &&
                       resultStack[^1] < currentDigit)
                {
                    resultStack.Length--;
                    digitsToRemove--;
                }

                resultStack.Append(currentDigit);
            }

            if (resultStack.Length > n)
            {
                resultStack.Length = n;
            }

            return resultStack.ToString();
        }
    }
}