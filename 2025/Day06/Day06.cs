using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

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

            stopwatch = Stopwatch.StartNew();
            PartTwo(input);
            stopwatch.Stop();
            Console.WriteLine($"PartTwo execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");
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

        private static void PartTwo(string[] input)
        {
            var l = new List<List<int>>();
            var m = ParseToMatrix(input);

            for (var i = 0; i < m[0].Count; i++)
            {
                l.Add(GetVerticalSlicesFromColumn(input, i));
            }

            var ops = input[^1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            long sum = 0;

            for (var i = 0; i < l.Count; i++)
            {
                long s = 0;
                long ss = 1;

                var op = ops[l.Count - 1 - i];
                switch (op)
                {
                    case "+":
                    {
                        for (var index = 0; index < l[i].Count; index++)
                            s += l[i][index];
                        sum += s;
                            break;
                    }
                    case "*":
                    {
                        for (var index = 0; index < l[i].Count; index++)
                            ss *= l[i][index];
                        sum += ss;
                            break;
                    }
                    default:
                        throw new Exception("Invalid operation");
                }
            }

            Console.WriteLine($"PartOne: {sum}");
        }

        // Pomocná třída pro uchování informace o pozici čísla v textu
        private class Token
        {
            public string Text { get; set; }
            public int StartIndex { get; set; }
            public int EndIndex { get; set; } // Start + Length
        }

        private enum Alignment { Left, Right }

        // Funkce pro standardní parsování čísel
        public static List<List<int>> ParseToMatrix(string[] input)
        {
            var result = new List<List<int>>();
            for (var index = 0; index < input.Length - 1; index++)
            {
                var line = input[index];
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Rozdělíme podle mezer a parsujeme
                var numbers = Regex.Split(line.Trim(), @"\s+")
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(int.Parse)
                    .ToList();
                result.Add(numbers);
            }

            return result;
        }

        // Funkce pro získání "vertikálních čísel"
        public static List<int> GetVerticalSlicesFromColumn(string[] input, int colIndexFromRight)
        {
            // 1. Získáme tokeny (text + pozice) pro všechny řádky
            var rowsOfTokens = new List<List<Token>>();
            for (var index = 0; index < input.Length - 1; index++)
            {
                var line = input[index];
                if (string.IsNullOrWhiteSpace(line)) continue;

                var matches = Regex.Matches(line, @"\S+");
                var rowTokens = new List<Token>();
                foreach (Match m in matches)
                {
                    rowTokens.Add(new Token
                    {
                        Text = m.Value,
                        StartIndex = m.Index,
                        EndIndex = m.Index + m.Length
                    });
                }

                rowsOfTokens.Add(rowTokens);
            }

            // 2. Vybereme konkrétní sloupec
            // Předpokládáme, že všechny řádky mají dostatek sloupců
            var columnTokens = new List<Token>();
            foreach (var row in rowsOfTokens)
            {
                var index = row.Count - 1 - colIndexFromRight;
                if (index >= 0 && index < row.Count)
                {
                    columnTokens.Add(row[index]);
                }
            }

            if (columnTokens.Count == 0) return new List<int>();

            // 3. Detekce zarovnání (Klíčová část pro váš výsledek)
            // Pokud mají všechna čísla stejný StartIndex -> Left Aligned (např. poslední sloupec ve vašem příkladu)
            // Pokud mají všechna čísla stejný EndIndex -> Right Aligned (např. předposlední sloupec)
            var align = DetectAlignment(columnTokens);

            // 4. Normalizace délek (padding)
            var maxLen = columnTokens.Max(t => t.Text.Length);
            var paddedStrings = columnTokens.Select(t =>
                align == Alignment.Right
                    ? t.Text.PadLeft(maxLen, ' ')
                    : t.Text.PadRight(maxLen, ' ')
            ).ToList();

            // 5. Čtení vertikálně ZPRAVA DOLEVA (podle pořadí ve vašem výsledku)
            var results = new List<int>();

            // Iterujeme sloupce znaků od posledního (vpravo) k prvnímu (vlevo)
            for (var charCol = maxLen - 1; charCol >= 0; charCol--)
            {
                var sb = new StringBuilder();
                foreach (var str in paddedStrings)
                {
                    var c = str[charCol];
                    if (c != ' ') // Ignorujeme mezery vzniklé paddingem
                    {
                        sb.Append(c);
                    }
                }

                if (sb.Length > 0)
                {
                    results.Add(int.Parse(sb.ToString()));
                }
            }

            return results;
        }

        private static Alignment DetectAlignment(List<Token> tokens)
        {
            // Heuristika: Pokud se startovní indexy liší méně než koncové, je to Left, jinak Right.
            // Ve vašem příkladu:
            // Poslední sloupec (64, 23, 314) začíná na stejné pozici -> Left Aligned -> Výsledek 4, 431, 623
            // Předposlední (51, 387, 215) končí na stejné pozici -> Right Aligned -> Výsledek 175, 581, 32

            var distinctStarts = tokens.Select(t => t.StartIndex).Distinct().Count();
            var distinctEnds = tokens.Select(t => t.EndIndex).Distinct().Count();

            if (distinctStarts <= distinctEnds) return Alignment.Left;
            return Alignment.Right;
        }
    }
}
