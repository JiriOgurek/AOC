namespace AOC2024.Day04
{
    internal static class Day04
    {
        public static void Solve()
        {
            var input = File.ReadAllText(@"Day04\input.txt");
            PartOne(input);
            PartTwo(input);
        }

        private static void PartOne(string input)
        {
            var array = CreateArray(@"Day04\input.txt");

            var positions = FindWord(array, "XMAS", [
                (-1, 0), (1, 0), (0, -1), (0, 1), // Up, Down, Left, Right
                (-1, -1), (-1, 1), (1, -1), (1, 1) // Diagonals
            ]);
            var sum = positions.Count();

            Console.WriteLine(sum);
        }

        private static void PartTwo(string input)
        {
            var array = CreateArray(@"Day04\input.txt");

            var listDownRight = GetCrossWords(array, (1, 1));
            var listDownLeft = GetCrossWords(array, (1, -1));

            var sum = 0;
            foreach (var pos in listDownRight)
            {
                //Console.WriteLine($"{pos.Item1}:{pos.Item2}");

                if (listDownLeft.Contains(new Tuple<int, int>(pos.Item1, pos.Item2 + 2)))
                    sum++;
            }

            Console.WriteLine(sum);
        }

        private static List<Tuple<int, int>> GetCrossWords(char[,] array, (int, int) direction)
        {
            var posMas = FindWord(array, "MAS", [
                direction // Diagonals down-right
            ]);
            var posSam = FindWord(array, "SAM", [
                direction // Diagonals down-left
            ]);
            var listDownRight = posMas.Union(posSam).ToList();
            return listDownRight;
        }

        private static char[,] CreateArray(string input)
        {
            // Read all lines from the input
            var lines = File.ReadAllLines(input);

            // Determine the number of rows and columns
            var rows = lines.Length;
            var cols = lines[0].Length;

            // Initialize the two-dimensional array
            var array = new char[rows, cols];

            // Populate the array with characters from the input
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    array[i, j] = lines[i][j];
                }
            }

            return array;
        }

        private static List<Tuple<int, int>> FindWord(char[,] array, string word, (int, int)[] directions)
        {
            var positions = new List<Tuple<int, int>>();
            var rows = array.GetLength(0);
            var cols = array.GetLength(1);
            var wordLength = word.Length;

            for (var x = 0; x < rows; x++)
            {
                for (var y = 0; y < cols; y++)
                {
                    foreach (var (dx, dy) in directions)
                    {
                        if (IsWordAtPosition(array, word, x, y, dx, dy))
                        {
                            positions.Add(Tuple.Create(x, y));
                        }
                    }
                }
            }

            return positions;
        }

        private static bool IsWordAtPosition(char[,] array, string word, int startX, int startY, int dx, int dy)
        {
            var rows = array.GetLength(0);
            var cols = array.GetLength(1);
            var wordLength = word.Length;

            for (var i = 0; i < wordLength; i++)
            {
                var newX = startX + i * dx;
                var newY = startY + i * dy;

                if (newX < 0 || newX >= rows || newY < 0 || newY >= cols || array[newX, newY] != word[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
