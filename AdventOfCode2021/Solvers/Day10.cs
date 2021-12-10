using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day10 : Solver
    {
        public Day10() : base(2021, 10, CodeType.Original) { }
        protected override object PartA(string input)
        {
            var lines = input.SplitOnNewline();
            int errorScore = 0;

            foreach (var line in lines)
            {
                errorScore += ErrorScore(line);
            }


            return errorScore;
        }

        private int ErrorScore(string line)
        {
            Dictionary<char, int> points = new Dictionary<char, int>() { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
            (char open, char close)[] chunkIdentifiers = new (char open, char close)[] { ('(', ')'), ('[', ']'), ('{', '}'), ('<', '>') };
            Stack<(char open, char close)> stack = new Stack<(char open, char close)>();

            foreach (char c in line)
            {
                var identifier = chunkIdentifiers.FirstOrDefault(id => id.open == c);
                if (identifier != default((char open, char close)))
                {
                    // we don't even check if the char is any character other than an opening or closing bracket
                    stack.Push(identifier);
                }
                else
                {
                    // we don't even check if the stack is the right size. Just trusting we are getting the right input
                    var expected = stack.Pop();
                    if (c != expected.close)
                    {
                        return points[c];
                    }
                }
            }
            return 0;
        }

        protected override object PartB(string input)
        {
            var lines = input.SplitOnNewline();
            List<long> scores = new List<long>();

            foreach (var line in lines)
            {
                int errorScore = ErrorScore(line);
                if (errorScore == 0)
                {
                    // valid line!
                    long closingScore = ClosingScore(line);
                    scores.Add(closingScore);
                }
            }

            scores.Sort();
            long result = scores.Skip(scores.Count / 2).First();

            return result;

        }

        private long ClosingScore(string line)
        {
            Dictionary<char, int> points = new Dictionary<char, int>() { { ')', 1 }, { ']', 2 }, { '}', 3 }, { '>', 4 } };
            (char open, char close)[] chunkIdentifiers = new (char open, char close)[] { ('(', ')'), ('[', ']'), ('{', '}'), ('<', '>') };
            Stack<(char open, char close)> stack = new Stack<(char open, char close)>();

            foreach (char c in line)
            {
                var identifier = chunkIdentifiers.FirstOrDefault(id => id.open == c);
                if (identifier != default((char open, char close)))
                {
                    // we don't even check if the char is any character other than an opening or closing bracket
                    stack.Push(identifier);
                }
                else
                {
                    // we don't even check if the stack is the right size or if this is the right value. Just trusting we are getting the right input.
                    stack.Pop();
                }
            }

            long score = 0;
            while (stack.Count > 0)
            {
                char value = stack.Pop().close;
                int point = points[value];
                score *= 5;
                score += point;
            }

            return score;
        }

    }
}
