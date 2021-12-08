using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day08 : Solver
    {


        public Day08() : base(2021, 8, CodeType.Original)
        {
        }

        protected override object PartA(string input)
        {
            const StringSplitOptions trimAndRemove = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
            List<List<string>[]> digits = input.SplitOnNewline().Select(line => line.Split('|', trimAndRemove).Select(e => e.Split(' ', trimAndRemove).ToList()).ToArray()).ToList();
            int[] uniqueLengths = new int[] { 2, 4, 3, 7 };

            int uniqueOutputs = 0;
            foreach (var line in digits)
            {
                List<string> output = line[1];
                uniqueOutputs += output.Count(s => uniqueLengths.Contains(s.Length));
            }

            return uniqueOutputs;
        }

        protected override object PartB(string input)
        {
            const StringSplitOptions trimAndRemove = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
            // Yeah, good luck reading this beauty...
            List<List<string>[]> digits = input.SplitOnNewline().Select(line => line.Split('|', trimAndRemove).Select(e => e.Split(' ', trimAndRemove).Select(s => String.Concat(s.OrderBy(c => c))).ToList()).ToArray()).ToList();

            int outputSum = 0;
            foreach (var line in digits)
            {
                string[] rosetta = new string[10];
                List<string> numbers = line[0];
                List<string> output = line[1];

                rosetta[1] = numbers.Single(s => s.Length == 2);
                rosetta[4] = numbers.Single(s => s.Length == 4);
                rosetta[7] = numbers.Single(s => s.Length == 3);
                rosetta[8] = numbers.Single(s => s.Length == 7);
                rosetta[3] = numbers.Single(s => s.Length == 5 && IsSubset(s, rosetta[1]));
                rosetta[6] = numbers.Single(s => s.Length == 6 && !IsSubset(s, rosetta[1]));
                rosetta[9] = numbers.Single(s => s.Length == 6 && IsSubset(s, rosetta[4]));
                rosetta[5] = numbers.Single(s => s.Length == 5 && IsSubset(rosetta[6], s));
                rosetta[2] = numbers.Single(s => s.Length == 5 && s != rosetta[5] && s != rosetta[3]);
                rosetta[0] = numbers.Single(s => s.Length == 6 && s != rosetta[6] && s != rosetta[9]);

                string outputNumber = String.Concat(output.Select(d => Array.IndexOf(rosetta, d).ToString()));
                outputSum += Int32.Parse(outputNumber);
            }

            return outputSum;
        }

        private bool IsSubset(string superset, string subset)
        {
            return !subset.Except(superset.ToCharArray()).Any();
        }
    }
}
