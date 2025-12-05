using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AOC2025.Day04
{
    internal static class Day04
    {
        public static void Solve()
        {
            var input = File.ReadAllLines(@"Day04\input.txt");


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
            var x = 137;
            var y = 137;
            var sum = 0;

            var board = new Board(x, y);
            board.LoadBoard(input);

            for (var row = 0; row < y; row++)
            {
                for (var col = 0; col < x; col++)
                {
                    var pos = new Position(col, row);
                    if (board[pos] == Board.PIECE)
                    {
                        var adjacentCount = board.GetNumberOfAdjacentRolls(pos);
                        if (adjacentCount < 4)
                        {
                            sum++;
                            //Console.WriteLine($"Piece at {pos} has {adjacentCount} adjacent pieces.");
                        }
                    }
                }
            }

            Console.WriteLine($"Total rolls to be removed found: {sum}");
        }

        private static void PartTwo(string[] input)
        {
            var x = 137;
            var y = 137;
            var sum = 0;

            var board = new Board(x, y);
            board.LoadBoard(input);

            var removed = 0;

            do
            {
                removed = 0;
                
                for (var row = 0; row < y; row++)
                {
                    for (var col = 0; col < x; col++)
                    {
                        var pos = new Position(col, row);
                        if (board[pos] == Board.PIECE)
                        {
                            var adjacentCount = board.GetNumberOfAdjacentRolls(pos);
                            if (adjacentCount < 4)
                            {
                                board[pos] = Board.EMPTY;
                                sum++;
                                removed++;
                                //Console.WriteLine($"Piece at {pos} has {adjacentCount} adjacent pieces.");
                            }
                        }
                    }
                }
            } while (removed > 0);

            Console.WriteLine($"Total rolls to be removed found: {sum}");
        }

        // 1. SOUŘADNICE (Zůstává stejné - je to velmi užitečné)
        public readonly record struct Position(int X, int Y)
        {
            public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
            public override string ToString() => $"[{X}, {Y}]";
        }

        public class Board
        {
            public int Width { get; }
            public int Height { get; }

            public const char EMPTY = '.';
            public const char PIECE = '@';

            private readonly char[,] _grid;

            public Board(int columns, int rows)
            {
                Width = columns;
                Height = rows;
                _grid = new char[columns, rows];

                // Inicializace tečkami
                for (var x = 0; x < Width; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        _grid[x, y] = EMPTY;
                    }
                }
            }

            public int GetNumberOfAdjacentRolls(Position p)
            {
                return GetAdjacentPositions(p).Count(direction => IsValidPosition(direction) && this[direction] == PIECE);
            }

            private IEnumerable<Position> GetAdjacentPositions(Position position)
            {
                var directions = new[]
                {
                    new Position(-1, 0), // Left
                    new Position(1, 0),  // Right
                    new Position(0, -1), // Up
                    new Position(0, 1),  // Down
                    new Position(-1, -1), // Left, Up
                    new Position(1, -1),  // Right, Up
                    new Position(-1, 1),  // Left, Down
                    new Position(1, 1)    // Right, Down
                };

                foreach (var dir in directions)
                {
                    var newPos = position + dir;
                    if (IsValidPosition(newPos))
                    {
                        yield return newPos;
                    }
                }
            }

            public void LoadBoard(string[] input)
            {
                for (var y = 0; y < Height; y++)
                {
                    var line = input[y];
                    for (var x = 0; x < Width; x++)
                    {
                        _grid[x, y] = line[x];
                    }
                }
            }

            // Indexer pro přístup přes souřadnice x, y
            public char this[int x, int y]
            {
                get
                {
                    if (!IsValidPosition(x, y)) return '#'; // Nebo vyhodit výjimku, '#' značí zeď/mimo
                    return _grid[x, y];
                }
                set
                {
                    if (IsValidPosition(x, y)) _grid[x, y] = value;
                }
            }

            // Indexer pro přístup přes Position
            public char this[Position pos]
            {
                get => this[pos.X, pos.Y];
                set => this[pos.X, pos.Y] = value;
            }

            public bool IsValidPosition(int x, int y)
            {
                return x >= 0 && x < Width && y >= 0 && y < Height;
            }

            public bool IsValidPosition(Position pos) => IsValidPosition(pos.X, pos.Y);
        }
    }
}