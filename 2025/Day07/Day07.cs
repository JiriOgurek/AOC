using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace AOC2025.Day07
{
    internal static class Day07
    {
        public static void Solve()
        {
            var input = File.ReadAllLines(@"Day07\input.txt");

            var stopwatch = Stopwatch.StartNew();
            //PartOne(input);
            stopwatch.Stop();
            Console.WriteLine($"PartOne execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");

            stopwatch = Stopwatch.StartNew();
            PartTwo(input);
            stopwatch.Stop();
            Console.WriteLine($"PartTwo execution time: {stopwatch.Elapsed.TotalMilliseconds} ms");
        }

        // Jednoduchá struktura pro pozici částice
        struct ParticleState
        {
            public int X; // Sloupec
            public int Y; // Řádek
            // Můžeme přidat "string History", pokud bychom chtěli vypsat cestu,
            // ale pro pouhé počítání to není třeba.

            public ParticleState(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        static long CountActiveTimelines(string[] map)
        {
            int rows = map.Length;
            int cols = map[0].Length;
            long completedTimelines = 0;

            // Najdeme startovní pozici 'S'
            ParticleState startState = new ParticleState(-1, -1);
            for (int r = 0; r < rows; r++)
            {
                int c = map[r].IndexOf('S');
                if (c != -1)
                {
                    startState = new ParticleState(c, r);
                    break;
                }
            }

            if (startState.X == -1)
            {
                Console.WriteLine("Chyba: Startovní bod 'S' nenalezen.");
                return 0;
            }

            // 2. INICIALIZACE FRONTY (Seznam aktivních realit)
            Queue<ParticleState> activeTimelines = new Queue<ParticleState>();
            activeTimelines.Enqueue(startState);

            // 3. HLAVNÍ SMYČKA (Běh času)
            while (activeTimelines.Count > 0)
            {
                // Vybereme jednu realitu k simulaci
                ParticleState current = activeTimelines.Dequeue();

                // Fyzika pohybu: Předpokládáme, že čas plyne "dolů" (Y + 1)
                int nextY = current.Y + 1;

                // KONTROLA KONCE: Pokud jsme pod mapou, linie je dokončena
                if (nextY >= rows)
                {
                    completedTimelines++;
                    continue;
                }

                // Zjistíme, na jakém políčku se nacházíme v příštím kroku
                // Musíme ošetřit, aby částice nevyletěla do stran mimo pole
                if (current.X < 0 || current.X >= cols) continue; // Částice zanikla mimo boční hranice

                // Podíváme se, co je na aktuální pozici (kde částice PRÁVĚ JE, než se pohne dál)
                // Poznámka: V zadání se štěpí na 'splitteru'.
                char currentTile = map[current.Y][current.X];

                // Pokud stojíme na startu 'S' nebo cestě '|' nebo prázdnu '.', padáme rovně dolů.
                // Pokud stojíme na splitteru '^', štěpíme se.

                // Logika: Podíváme se na políčko POD námi nebo na aktuální interakci?
                // Vizuálně to vypadá jako Galtonova deska (pyramida). 
                // Pokud narazím na '^', v dalším kroku jdu VLEVO-DOLŮ a VPRAVO-DOLŮ.
                // Pokud narazím na cokoliv jiného, jdu DOLŮ.

                // Zde implementujeme logiku "Když narazím na ^, větvím se"

                // Musíme se podívat, na co jsme "dopadli" v tomto kroku.
                char tileInteraction = map[current.Y][current.X];

                if (tileInteraction == '^')
                {
                    // KVANTOVÉ ŠTĚPENÍ
                    // Vytvoříme dvě nové reality pro DALŠÍ krok

                    // Realita 1: Jde vlevo dolů
                    if (IsWithinBounds(current.X - 1, nextY, rows, cols))
                        activeTimelines.Enqueue(new ParticleState(current.X - 1, nextY));

                    // Realita 2: Jde vpravo dolů
                    if (IsWithinBounds(current.X + 1, nextY, rows, cols))
                        activeTimelines.Enqueue(new ParticleState(current.X + 1, nextY));
                }
                else
                {
                    // KLASICKÝ POHYB (Padáme rovně dolů)
                    // Toto platí pro 'S', '|', '.'

                    // Pozor: V příkladu se zdá, že po rozštěpení se částice pohybuje po diagonále?
                    // Nebo se vrátí k vertikálnímu pádu?
                    // Dle ASCII: `...|^...` -> další řádek `...|...`
                    // Implementujeme základní gravitaci: Pokud to není splitter, padám dolů.

                    if (IsWithinBounds(current.X, nextY, rows, cols))
                    {
                        activeTimelines.Enqueue(new ParticleState(current.X, nextY));
                    }
                }
            }

            return completedTimelines;
        }

        // Pomocná metoda pro kontrolu hranic
        static bool IsWithinBounds(int x, int y, int rows, int cols)
        {
            return x >= 0 && x < cols && y < rows; // y >= 0 je zaručeno logikou
        }


        private static void PartTwo(string[] input)
        {
            Console.WriteLine("Spouštím simulaci kvantového rozdělovače...");
            long totalTimelines = CountActiveTimelines(input);
            Console.WriteLine($"------------------------------------------");
            Console.WriteLine($"Celkový počet aktivních časových linií: {totalTimelines}");
        }


        private static void PartOne(string[] input)
        {
            var suma = 0;
            var loader = new HerniPoleLoader();
            loader.NactiZeSouboru(input);

            var beams = new List<(int, List<int>)> { (0, [70]) };

            for (var y = 0; y < loader.VyskaPole - 1; y++)
            {
                var (index, beamList) = beams[y];
                var nove = new List<int>();
                foreach (var beam in beamList)
                {
                    if (loader.ZnakNaPozici(beam, y + 1) == '^')
                    {
                        if (beam - 1 >= 0 && !nove.Contains(beam - 1)){
                            nove.Add(beam - 1);
                        }

                        if (beam + 1 < loader.SirkaPole && !nove.Contains(beam + 1)){
                            nove.Add(beam + 1);
                        }

                        suma++;
                    }
                    else
                    {
                        if (!nove.Contains(beam))
                            nove.Add(beam);
                    }
                }

                beams.Add((index + 1, nove));
                //Console.WriteLine($"Beam at {index}, new positions: {string.Join(", ", nove)}");
            }
            Console.WriteLine($"Suma: {suma}");
        }

        public class HerniPoleLoader
        {
            public char[,] HerniPole { get; private set; }

            public int SirkaPole => HerniPole.GetLength(0);
            public int VyskaPole => HerniPole.GetLength(1);


            public void NactiZeSouboru(string[] radky)
            {
                if (radky.Length == 0)
                    throw new InvalidOperationException("Soubor je prázdný.");

                var vyska = radky.Length;
                var sirka = radky[0].Length;

                // Volitelná kontrola, že všechny řádky mají stejnou délku
                for (var i = 1; i < vyska; i++)
                {
                    if (radky[i].Length != sirka)
                        throw new InvalidOperationException("Všechny řádky mapy musí mít stejnou délku.");
                }

                HerniPole = new char[vyska, sirka];

                for (var y = 0; y < vyska; y++)
                {
                    for (var x = 0; x < sirka; x++)
                    {
                        HerniPole[y, x] = radky[y][x];
                    }
                }
            }

            public char ZnakNaPozici(int x, int y)
            {
                if (HerniPole == null)
                    throw new InvalidOperationException("Nejprve zavolej NactiZeSouboru.");

                if (y < 0 || y >= HerniPole.GetLength(0) ||
                    x < 0 || x >= HerniPole.GetLength(1))
                {
                    throw new ArgumentOutOfRangeException("Souřadnice jsou mimo herní pole.");
                }

                return HerniPole[y, x];
            }
        }
    }
}
