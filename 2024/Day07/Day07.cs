namespace AOC2024.Day07
{
    internal static class Day07
    {
        public static void Solve()
        {
            PartOne();
            //PartTwo();
        }

        private static void PartOne()
        {
            var equations = GetEquations();

            long sum = 0;
            foreach (var equation in equations)
            {
                if (IsValid(equation.Item1, equation.Item2))
                {
                    sum += equation.Item1;
                }
            }

            Console.WriteLine(sum);
        }

        private static bool IsValid(long left, List<long> right)
        {
            var combinations = GetCombinations(right);
            foreach (var combination in combinations)
            {
                var result = EvaluateExpression(combination);
                if (result == left)
                {
                    return true;
                }
            }

            return false;
        }

        static long EvaluateExpression(string expression)
        {
            var tokens = expression.Split(new char[] { '+', '*' });
            var operators = new List<char>();
            foreach (var ch in expression)
            {
                if (ch == '+' || ch == '*')
                {
                    operators.Add(ch);
                }
            }

            var result = long.Parse(tokens[0]);
            for (var i = 0; i < operators.Count; i++)
            {
                var nextValue = long.Parse(tokens[i + 1]);
                if (operators[i] == '+')
                {
                    result += nextValue;
                }
                else if (operators[i] == '*')
                {
                    result *= nextValue;
                }
            }

            return result;
        }

        private static List<string> GetCombinations(List<long> numbers)
        {
            var results = new List<string>();
            GenerateCombinations(numbers, 0, "", results);
            return results;
        }

        private static void GenerateCombinations(List<long> numbers, int index, string current, List<string> results)
        {
            if (index == numbers.Count - 1)
            {
                results.Add(current + numbers[index]);
                return;
            }

            GenerateCombinations(numbers, index + 1, current + numbers[index] + "+", results);
            GenerateCombinations(numbers, index + 1, current + numbers[index] + "*", results);
        }

        private static List<(long, List<long>)> GetEquations()
        {
            var input = File.ReadAllLines("Day07/input.txt");

            var equations = new List<(long, List<long>)>();

            foreach (var line in input)
            {
                var equation = ((long)0, new List<long>());
                var elements = line.Split(" ");

                for (var index = 0; index < elements.Length; index++)
                {
                    var element = elements[index];
                    if (index == 0)
                        equation.Item1 = long.Parse(element.Replace(":", ""));
                    else
                        equation.Item2.Add(long.Parse(element));
                }

                equations.Add(equation);
            }

            return equations;
        }
    }
}
