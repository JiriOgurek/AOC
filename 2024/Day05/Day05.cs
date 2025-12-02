namespace AOC2024.Day05
{
    internal static class Day05
    {
        public static void Solve()
        {
            var rules = File.ReadAllText(@"Day05\Day05Rules.txt");
            var pages = File.ReadAllText(@"Day05\Day05Pages.txt");
            PartOne(rules, pages);
            PartTwo(rules, pages);
        }

        private static List<(int, List<int>)> ParseRules(string rules)
        {
            var ruleList = new List<(int, List<int>)>();
            var ruleLines = rules.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var ruleLine in ruleLines)
            {
                var ruleValues = ruleLine.Split("|");
                var key = int.Parse(ruleValues[0]);
                var value = int.Parse(ruleValues[1]);
                if (ruleList.Select(x => x.Item1).Contains(key))
                {
                    ruleList[ruleList.FindIndex(x => x.Item1 == key)].Item2.Add(value);
                }
                else
                {
                    var rule = (key, new List<int> { value });
                    ruleList.Add(rule);
                }
            }
            return ruleList;
        }

        private static void PartOne(string _rules, string _pages)
        {
            var rules = ParseRules(_rules);
            var sum = 0;

            foreach (var pageLine in _pages.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var pages = ParsePageLine(pageLine);
                var valid = ValidatePage(rules, pages);

                if (valid)
                {
                    sum += pages[(pages.Count - 1) / 2];
                }
            }

            Console.WriteLine(sum);
        }

        private static bool ValidatePage(List<(int, List<int>)> rules, List<int> pages)
        {
            for (var i = 0; i < pages.Count; i++)
            {
                var doms = GetDominants(rules, pages[i]);
                var subs = GetSubmissives(rules, pages[i]);

                // check if all dominants are before the current page
                for (var j = 0; j < i; j++)
                {
                    if (!doms.Contains(pages[j]))
                    {
                        return false;
                    }
                }

                // check if all submissives are after the current page
                for (var j = i + 1; j < pages.Count; j++)
                {
                    if (!subs.Contains(pages[j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static List<int> GetDominants(List<(int, List<int>)> rules, int x)
        {
            return rules
                .Where(rule => rule.Item2.Contains(x))
                .Select(rule => rule.Item1)
                .ToList();
        }

        private static List<int> GetSubmissives(List<(int, List<int>)> rules, int x)
        {
            return rules
                .Where(rule => rule.Item1 == x)
                .SelectMany(rule => rule.Item2)
                .ToList();
        }

        private static List<int> ParsePageLine(string pageLine)
        {
            var pages = pageLine.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return pages.Select(int.Parse).ToList();
        }

        private static void PartTwo(string _rules, string _pages)
        {
            var rules = ParseRules(_rules);
            var invalidLines = new List<string>();

            foreach (var pageLine in _pages.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var pages = ParsePageLine(pageLine);
                var valid = ValidatePage(rules, pages);

                if (!valid)
                {
                    invalidLines.Add(pageLine);
                }
            }

            for (var i = 0; i < invalidLines.Count; i++)
            {
                do
                    invalidLines[i] = CorrectLine(invalidLines[i], rules);
                while (!ValidatePage(rules, ParsePageLine(invalidLines[i])));
            }

            var sum = 0;

            foreach (var pageLine in invalidLines)
            {
                var pages = ParsePageLine(pageLine);
                sum += pages[(pages.Count - 1) / 2];
            }

            Console.WriteLine(sum);
        }

        private static string CorrectLine(string line, List<(int, List<int>)> rules)
        {
            var pages = ParsePageLine(line);

            for (var i = 0; i < pages.Count; i++)
            {
                var doms = GetDominants(rules, pages[i]);
                var subs = GetSubmissives(rules, pages[i]);
                // check if all dominants are before the current page
                for (var j = 0; j < i; j++)
                {
                    if (!doms.Contains(pages[j]))
                    {
                        (pages[i], pages[j]) = (pages[j], pages[i]);
                    }
                }
                // check if all submissives are after the current page
                for (var j = i + 1; j < pages.Count; j++)
                {
                    if (!subs.Contains(pages[j]))
                    {
                        (pages[i], pages[j]) = (pages[j], pages[i]);
                    }
                }
            }

            return string.Join(",", pages);
        }
    }
}
