using AdventOfCode2021.Common;
using System.Text;

namespace AdventOfCode2021.Solvers
{
    public class Day13 : Solver
    {
        public Day13() : base(2021, 13) { }
        protected override object PartA(string input)
        {
            var lines = input.SplitOnNewline();
            List<(int x, int y)> dots = new List<(int x, int y)>();

            var dotsInput = lines.TakeWhile(s => !s.StartsWith("fold")).ToList();
            var foldsInput = lines.Skip(dotsInput.Count).ToList();

            dots = dotsInput.Select(dot => (x: Int32.Parse(dot.Split(',')[0]), y: Int32.Parse(dot.Split(',')[1]))).ToList();
            List<(char dimension, int value)> folds = foldsInput.Select(fold => (dimension: fold.Split('=')[0].Last(), value: Int32.Parse(fold.Split('=')[1]))).ToList();

            dots = Fold(dots, folds.Take(1).ToList());

            return dots.Count();
        }

        private List<(int x, int y)> Fold(List<(int x, int y)> dots, List<(char dimension, int value)> folds)
        {
            var result = dots;
            foreach (var fold in folds)
            {
                if (fold.dimension == 'x')
                {
                    result = result.Select(d => d.x > fold.value ? (x: (2 * fold.value) - d.x, y: d.y) : d).Distinct().ToList();
                }
                if (fold.dimension == 'y')
                {
                    result = result.Select(d => d.y > fold.value ? (x: d.x, y: (2 * fold.value) - d.y) : d).Distinct().ToList();
                }
            }
            return result;
        }

        protected override object PartB(string input)
        {
            var lines = input.SplitOnNewline();
            List<(int x, int y)> dots = new List<(int x, int y)>();

            var dotsInput = lines.TakeWhile(s => !s.StartsWith("fold")).ToList();
            var foldsInput = lines.Skip(dotsInput.Count).ToList();

            dots = dotsInput.Select(dot => (x: Int32.Parse(dot.Split(',')[0]), y: Int32.Parse(dot.Split(',')[1]))).ToList();
            List<(char dimension, int value)> folds = foldsInput.Select(fold => (dimension: fold.Split('=')[0].Last(), value: Int32.Parse(fold.Split('=')[1]))).ToList();

            dots = Fold(dots, folds);

            StringBuilder answer = new StringBuilder();
            answer.AppendLine();
            for (int y = 0; y <= dots.Max(d => d.y); y++)
            {
                answer.Append("\t");
                for (int x = 0; x <= dots.Max(d => d.x); x++)
                {
                    answer.Append(dots.Contains((x, y)) ? "#" : ".");
                }
                answer.AppendLine();
            }

            return answer;
        }
    }
}
