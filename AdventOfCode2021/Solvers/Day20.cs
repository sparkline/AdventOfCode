using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day20 : Solver
    {
        public Day20() : base(2021, 20) { }
        protected override object PartA(string input)
        {
            return Enhance(input, 2);
        }

        private object Enhance(string input, int times)
        {
            var lines = input.SplitOnNewline();
            bool flickeringUniverse = lines[0][0] == '#' && lines[0][511] == '.';
            bool[] key = lines[0].Select(c => c == '#').ToArray();

            HashSet<(int x, int y)> picture = new HashSet<(int x, int y)>();
            var pictureInput = lines.Skip(1).ToList();
            for (int y = 0; y < pictureInput.Count(); y++)
            {
                for (int x = 0; x < pictureInput[y].Count(); x++)
                {
                    if (pictureInput[y][x] == '#')
                    {
                        picture.Add((x, y));
                    }
                }
            }

            // Apply algorithm

            // Iteration
            bool invertedInput = false;
            for (int iteration = 0; iteration < times; iteration++)
            {
                bool invertedResult = flickeringUniverse && (iteration % 2 == 0);

                // Bounding box
                int minX = picture.Min(p => p.x) - 2;
                int minY = picture.Min(p => p.y) - 2;
                int maxX = picture.Max(p => p.x) + 2;
                int maxY = picture.Max(p => p.y) + 2;
                HashSet<(int x, int y)> _picture = new HashSet<(int x, int y)>();
                for (int x = minX; x < maxX; x++)
                {
                    for (int y = minY; y < maxY; y++)
                    {
                        uint value = GetNumber(picture, invertedInput, x, y);
                        bool light = key[value];
                        if (light ^ invertedResult)
                        {
                            _picture.Add((x, y));
                        }
                    }
                }
                picture = _picture;
                invertedInput = invertedResult;
            }

            return picture.Count();
        }

        private uint GetNumber(HashSet<(int x, int y)> picture, bool defaultLight, int x, int y)
        {
            uint result = 0;
            for (int _y = y - 1; _y <= y + 1; _y++)
            {
                for (int _x = x - 1; _x <= x + 1; _x++)
                {
                    result = (result << 1);
                    result += (picture.Contains((_x, _y)) ^ defaultLight) ? 1u : 0u;
                }
            }
            return result;
        }

        protected override object PartB(string input)
        {
            return Enhance(input, 50);
        }
    }
}
