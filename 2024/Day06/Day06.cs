namespace AOC2024.Day06
{
    internal class Day06
    {
        private char[,] Array { get; set; }
        private (int, int, char) _state;
        private (int, int, char) State
        {
            get => _state;
            set => SetCoords(value);
        }
        private List<(int, int, char)> Visited { get; set; }

        private void SetCoords((int, int, char) value)
        {
            if (value.Item1 != _state.Item1 || value.Item2 != _state.Item2)
                Visited.Add(value);

            _state = value;
        }

        private (int, int, char) InitState;

        public void Solve()
        {
            PartOne();
            PartTwo();
        }

        private void PartTwo()
        {
            Array = CreateArray(@"Day06\input.txt");
            Visited = [];
            State = GetInitialState(Array);
            InitState = State;

            var loops = 0;

            var rows = Array.GetLength(0);
            var cols = Array.GetLength(1);
            for (var x = 0; x < rows; x++)
            {
                for (var y = 0; y < cols; y++)
                {
                    if (Array[x, y] == '.')
                    {
                        Array[x, y] = '#';

                        State = InitState;
                        Visited = [];

                        if (GetStepsCount() == -1)
                            loops++;

                        Array[x, y] = '.';
                    }

                    if ((x + 1) * (y + 1) % (rows * cols / 100) == 0)
                        Console.WriteLine($"{x * y / (rows * cols / 100)}% done");
                }
            }

            Console.WriteLine(loops);
        }

        private void PartOne()
        {
            Array = CreateArray(@"Day06\input.txt");
            Visited = [];
            State = GetInitialState(Array);

            Console.WriteLine(GetStepsCount());
        }

        private int GetStepsCount()
        {
            while (IsInBounds())
            {
                Array[State.Item1, State.Item2] = '.';
                Move();
                if (Visited.Where(x => x == State).GroupBy(n => n).Any(g => g.Count() > 1))
                    return -1;
            }

            if (Visited.Any())
                Visited.RemoveAt(Visited.Count - 1);

            return Visited.Select(x => (x.Item1, x.Item2)).Distinct().Count();
        }

        private void Move()
        {
            while (true)
            {
                try
                {

                    switch (State.Item3)
                    {
                        case '^':
                            if (Array[State.Item1 - 1, State.Item2] != '#')
                                State = (State.Item1 - 1, State.Item2, State.Item3);
                            else
                            {
                                State = (State.Item1, State.Item2, '>');
                                continue;
                            }

                            break;
                        case '>':
                            if (Array[State.Item1, State.Item2 + 1] != '#')
                                State = (State.Item1, State.Item2 + 1, State.Item3);
                            else
                            {
                                State = (State.Item1, State.Item2, 'v');
                                continue;
                            }

                            break;
                        case 'v':
                            if (Array[State.Item1 + 1, State.Item2] != '#')
                                State = (State.Item1 + 1, State.Item2, State.Item3);
                            else
                            {
                                State = (State.Item1, State.Item2, '<');
                                continue;
                            }

                            break;
                        case '<':
                            if (Array[State.Item1, State.Item2 - 1] != '#')
                                State = (State.Item1, State.Item2 - 1, State.Item3);
                            else
                            {
                                State = (State.Item1, State.Item2, '^');
                                continue;
                            }

                            break;
                    }

                    break;
                }
                catch (IndexOutOfRangeException e)
                {
                    State = (-1, -1, '-');
                    return;
                }
            }
        }

        private bool IsInBounds()
        {
            var (x, y, _) = State;
            return x >= 0 && x < Array.GetLength(0) && y >= 0 && y < Array.GetLength(1);
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

        private static (int, int, char) GetInitialState(char[,] array)
        {
            var rows = array.GetLength(0);
            var cols = array.GetLength(1);
            for (var x = 0; x < rows; x++)
            {
                for (var y = 0; y < cols; y++)
                {
                    if (array[x, y] != '.' && array[x, y] != '#')
                    {
                        return (x, y, array[x, y]);
                    }
                }
            }
            return (-1, -1, '-');
        }
    }
}
