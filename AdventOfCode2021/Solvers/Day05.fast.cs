using AdventOfCode2021.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Solvers
{
    public class Day05f : Solver
    {
        public Day05f() : base(2021, 5, CodeType.Fastest) { }

        protected override object PartA(string input)
        {
            Dictionary<(int x, int y), int> board = new Dictionary<(int x, int y), int>();
            string pattern = @"(?<x1>\d+),(?<y1>\d+)\s->\s(?<x2>\d+),(?<y2>\d+)[\r\n]*";
            Regex r = new Regex(pattern);
            var matches = r.Matches(input);
            foreach (Match match in matches)
            {
                int x1 = Int32.Parse(match.Groups["x1"].Value);
                int y1 = Int32.Parse(match.Groups["y1"].Value);
                int x2 = Int32.Parse(match.Groups["x2"].Value);
                int y2 = Int32.Parse(match.Groups["y2"].Value);
                if (x1 == x2 || y1 == y2)
                {
                    AddVector(board, x1, y1, x2, y2);
                }
            }

            int sum = board.Where(v => v.Value > 1).Count();
            return sum;
        }

        protected override object PartB(string input)
        {
            Dictionary<(int x, int y), int> board = new Dictionary<(int x, int y), int>();
            string pattern = @"(?<x1>\d+),(?<y1>\d+)\s->\s(?<x2>\d+),(?<y2>\d+)[\r\n]*";
            Regex r = new Regex(pattern);
            var matches = r.Matches(input);
            foreach (Match match in matches)
            {
                int x1 = Int32.Parse(match.Groups["x1"].Value);
                int y1 = Int32.Parse(match.Groups["y1"].Value);
                int x2 = Int32.Parse(match.Groups["x2"].Value);
                int y2 = Int32.Parse(match.Groups["y2"].Value);
                AddVector(board, x1, y1, x2, y2);
            }

            int sum = board.Where(v => v.Value > 1).Count();
            return sum;
        }

        private void AddVector(Dictionary<(int x, int y), int> board, int x1, int y1, int x2, int y2)
        {
            int length = Math.Max(Math.Abs(x2 - x1), Math.Abs(y2 - y1));
            int deltax = Math.Sign(x2 - x1);
            int deltay = Math.Sign(y2 - y1);

            for (int i = 0; i <= length; i++)
            {
                (int x, int y) point = (x1 + (i * deltax), y1 + (i * deltay));
                if (!board.TryAdd(point, 1))
                {
                    board[point] += 1;
                }
            }
        }

    }
}

