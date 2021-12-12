using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day09 : Solver
    {
        public Day09() : base(2021, 9) { }
        protected override object PartA(string input)
        {
            List<string> lines = input.SplitOnNewline();
            int heigth = lines.Count;
            int width = lines[0].Length;
            int[,] board = new int[heigth, width];
            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    board[y, x] = Int32.Parse(lines[y][x] + "");
                }
            }

            int sum = 0;
            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int val = board[y, x];
                    if ((x == 0 || board[y, x - 1] > val) &&
                         (x == width - 1 || board[y, x + 1] > val) &&
                         (y == 0 || board[y - 1, x] > val) &&
                         (y == heigth - 1 || board[y + 1, x] > val))
                    {
                        sum += val + 1;
                    }
                }
            }

            return sum;
        }

        protected override object PartB(string input)
        {
            List<string> lines = input.SplitOnNewline();
            int heigth = lines.Count;
            int width = lines[0].Length;
            bool[,] board = new bool[heigth, width];
            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    board[y, x] = Int32.Parse(lines[y][x] + "") == 9;
                }
            }

            List<int> basinSizes = new List<int>();
            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool partOfBasin = !board[y, x];
                    if (partOfBasin)
                    {
                        int basinSize = DetermineBasinSize(board, x, y);
                        basinSizes.Add(basinSize);
                    }
                }
            }

            return basinSizes.OrderByDescending(i => i).Take(3).Aggregate(1, (total, next) => total * next);
        }

        private int DetermineBasinSize(bool[,] board, int x, int y)
        {
            int basinSize = 0;
            if (x < 0 || y < 0 || y >= board.GetLength(0) || x >= board.GetLength(1))
            {
                // out of bounds
                return basinSize;
            }
            bool partOfBasin = !board[y, x];
            if (partOfBasin)
            {
                basinSize++;
                board[y, x] = true; // mark as visited
                basinSize += DetermineBasinSize(board, x - 1, y);
                basinSize += DetermineBasinSize(board, x + 1, y);
                basinSize += DetermineBasinSize(board, x, y - 1);
                basinSize += DetermineBasinSize(board, x, y + 1);
            }
            return basinSize;
        }
    }
}
