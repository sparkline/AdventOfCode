using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day25 : Solver
    {
        public Day25() : base(2021, 25) { }

        protected override object PartA(string input)
        {
            (int xSize, int ySize, HashSet<(int x, int y)> ltr, HashSet<(int x, int y)> ttb) cumcumbers = readData(input);
            HashSet<(int x, int y)> ltrToMove = new HashSet<(int x, int y)>();
            HashSet<(int x, int y)> ttbToMove = new HashSet<(int x, int y)>();
            int iter = 0;
            do
            {
                /*
                Console.WriteLine($"========== Iter {iter}");
                for (int y = 0; y < cumcumbers.ySize; y++)
                {
                    for (int x = 0; x < cumcumbers.xSize; x++)
                    {
                        if (cumcumbers.ltr.Contains((x, y))) Console.Write('>');
                        else if (cumcumbers.ttb.Contains((x, y))) Console.Write('v');
                        else Console.Write('.');
                    }
                    Console.WriteLine();
                }
                */
                iter++;
                ltrToMove.Clear();
                ttbToMove.Clear();
                foreach (var cucumber in cumcumbers.ltr)
                {
                    (int x, int y) nextCoord = (x: (cucumber.x + 1) % cumcumbers.xSize, y: cucumber.y);
                    if (!cumcumbers.ltr.Contains(nextCoord) && !cumcumbers.ttb.Contains(nextCoord))
                    {
                        ltrToMove.Add(cucumber);
                    }
                }
                foreach (var cucumber in ltrToMove)
                {
                    cumcumbers.ltr.Remove(cucumber);
                    cumcumbers.ltr.Add(((cucumber.x + 1) % cumcumbers.xSize, cucumber.y));
                }

                foreach (var cucumber in cumcumbers.ttb)
                {
                    (int x, int y) nextCoord = (x: cucumber.x, y: (cucumber.y + 1) % cumcumbers.ySize);
                    if (!cumcumbers.ltr.Contains(nextCoord) && !cumcumbers.ttb.Contains(nextCoord))
                    {
                        ttbToMove.Add(cucumber);
                    }
                }
                foreach (var cucumber in ttbToMove)
                {
                    cumcumbers.ttb.Remove(cucumber);
                    cumcumbers.ttb.Add((cucumber.x, (cucumber.y + 1) % cumcumbers.ySize));
                }


            } while (ltrToMove.Count > 0 || ttbToMove.Count > 0);


            return iter;
        }

        protected override object PartB(string input)
        {
            throw new NotImplementedException();
        }

        protected (int xSize, int ySize, HashSet<(int x, int y)> ltr, HashSet<(int x, int y)> ttb) readData(string input)
        {
            int Xsize, Ysize;
            HashSet<(int x, int y)> ltr = new HashSet<(int x, int y)>();
            HashSet<(int x, int y)> ttb = new HashSet<(int x, int y)>();

            var lines = input.SplitOnNewline();
            Ysize = lines.Count;
            Xsize = lines[0].Length;
            for (int y = 0; y < Ysize; y++)
            {
                for (int x = 0; x < Xsize; x++)
                {
                    switch (lines[y][x])
                    {
                        case '>':
                            ltr.Add((x, y));
                            break;
                        case 'v':
                            ttb.Add((x, y));
                            break;
                    }
                }
            }
            return (xSize: Xsize, ySize: Ysize, ltr: ltr, ttb: ttb);
        }
    }
}
