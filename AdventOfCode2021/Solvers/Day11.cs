using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day11 : Solver
    {
        public Day11() : base(2021, 11, CodeType.Original) { }
        protected override object PartA(string input)
        {
            var numbers = input.SplitOnNewline();

            int rows = numbers.Count;
            int columns = numbers.Max(l => l.Length);

            // build grid of ints
            Dictionary<(int x, int y), int> dumbos = new Dictionary<(int x, int y), int>();
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int value = Int32.Parse("" + numbers[y][x]);
                    dumbos.Add((x, y), value);
                }
            }

            const int EXHAUSTED = -1;
            const int FLASHPOINT = 9;
            int flashes = 0;
            for (int step = 1; step <= 100; step++)
            {
                Queue<(int x, int y)> checkForFlash = new Queue<(int x, int y)>(dumbos.Keys);
                while (checkForFlash.Count > 0)
                {
                    var dumbo = checkForFlash.Dequeue();
                    int value = dumbos[dumbo];
                    if (value == EXHAUSTED)
                    {
                        // Ignore, has already flashed this round
                        continue;
                    }
                    else
                    {
                        // Increase by one
                        dumbos[dumbo] = ++value;
                    }

                    // Propagate
                    if (value > FLASHPOINT)
                    {
                        // Exhaust, we just flashed
                        dumbos[dumbo] = EXHAUSTED;

                        // Propagate
                        for (int x = Math.Max(0, dumbo.x - 1); x <= Math.Min(columns - 1, dumbo.x + 1); x++)
                        {
                            for (int y = Math.Max(0, dumbo.y - 1); y <= Math.Min(rows - 1, dumbo.y + 1); y++)
                            {
                                if (x == dumbo.x && y == dumbo.y)
                                {
                                    continue;
                                }
                                else
                                {
                                    checkForFlash.Enqueue((x, y));
                                }
                            }
                        }
                    }
                }

                var dumbosThatFlashed = dumbos.Where(x => x.Value == EXHAUSTED).Select(x => x.Key);
                flashes += dumbosThatFlashed.Count();
                foreach (var update in dumbosThatFlashed)
                {
                    dumbos[update] = 0;
                }

            }

            return flashes;
        }

        protected override object PartB(string input)
        {
            var numbers = input.SplitOnNewline();

            int rows = numbers.Count;
            int columns = numbers.Max(l => l.Length);

            // build grid of ints
            Dictionary<(int x, int y), int> dumbos = new Dictionary<(int x, int y), int>();
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int value = Int32.Parse("" + numbers[y][x]);
                    dumbos.Add((x, y), value);
                }
            }

            const int EXHAUSTED = -1;
            const int FLASHPOINT = 9;
            int step = 0;
            while (true)
            {
                step++;
                Queue<(int x, int y)> checkForFlash = new Queue<(int x, int y)>(dumbos.Keys);
                while (checkForFlash.Count > 0)
                {
                    var dumbo = checkForFlash.Dequeue();
                    int value = dumbos[dumbo];
                    if (value == EXHAUSTED)
                    {
                        // Ignore, has already flashed this round
                        continue;
                    }
                    else
                    {
                        // Increase by one
                        dumbos[dumbo] = ++value;
                    }

                    // Propagate
                    if (value > FLASHPOINT)
                    {
                        // Exhaust, we just flashed
                        dumbos[dumbo] = EXHAUSTED;

                        // Propagate
                        for (int x = Math.Max(0, dumbo.x - 1); x <= Math.Min(columns - 1, dumbo.x + 1); x++)
                        {
                            for (int y = Math.Max(0, dumbo.y - 1); y <= Math.Min(rows - 1, dumbo.y + 1); y++)
                            {
                                if (x == dumbo.x && y == dumbo.y)
                                {
                                    continue;
                                }
                                else
                                {
                                    checkForFlash.Enqueue((x, y));
                                }
                            }
                        }
                    }
                }

                var dumbosThatFlashed = dumbos.Where(x => x.Value == EXHAUSTED).Select(x => x.Key);
                int flashes = dumbosThatFlashed.Count();
                foreach (var update in dumbosThatFlashed)
                {
                    dumbos[update] = 0;
                }

                if (flashes == rows * columns)
                {
                    return step;
                }

            }
        }
    }
}
