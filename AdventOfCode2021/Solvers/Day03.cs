using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day03 : Solver
    {
        public Day03() : base(2021, 3, CodeType.Original) { }

        protected override object PartA(string input)
        {
            var numbers = input.SplitOnNewline();

            int rows = numbers.Count;
            int columns = numbers.Max(l => l.Length);
            bool[] mostCommon = new bool[columns];

            // Get most common bit per column
            for (int x = 0; x < columns; x++)
            {
                mostCommon[x] = numbers.Count(line => line[x] == '1') > (double)rows / 2;
            }

            // invert
            bool[] leastCommon = mostCommon.Select(b => !b).ToArray();

            // construct binary string
            string gammaString = string.Join(null, mostCommon.Select(b => b ? '1' : '0'));
            string epsilonString = string.Join(null, leastCommon.Select(b => b ? '1' : '0'));

            // convert to int
            int gamma = Convert.ToInt32(gammaString, 2);
            int epsilon = Convert.ToInt32(epsilonString, 2);

            return epsilon * gamma;
        }

        protected override object PartB(string input)
        {
            var numbers = input.SplitOnNewline();

            int rows = numbers.Count;
            int columns = numbers.Max(l => l.Length);

            // build grid of bools            
            bool[,] grid = new bool[columns, rows];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    grid[x, y] = (numbers[y][x] == '1');
                }
            }

            // Filter all candidates untill we find oxygen
            HashSet<int> oxygenCandidates = new HashSet<int>(Enumerable.Range(0, rows));
            int oxygen = 0;
            for (int x = 0; x < columns; x++)
            {
                bool mostCommon = oxygenCandidates.Count(c => grid[x, c]) >= (double)oxygenCandidates.Count() / 2;

                oxygenCandidates.RemoveWhere(c => grid[x, c] ^ mostCommon);
                if (oxygenCandidates.Count == 1)
                {
                    // build binary string and convert to int.
                    string oxygenString = numbers[oxygenCandidates.Single()];
                    oxygen = Convert.ToInt32(oxygenString, 2);
                }
            }

            // Filter all candidates untill we find co2
            HashSet<int> co2Candidates = new HashSet<int>(Enumerable.Range(0, rows));
            int co2 = 0;
            for (int x = 0; x < columns; x++)
            {
                bool leastCommon = co2Candidates.Count(c => grid[x, c]) < (double)co2Candidates.Count() / 2;

                co2Candidates.RemoveWhere(c => grid[x, c] ^ leastCommon);
                if (co2Candidates.Count == 1)
                {
                    // build binary string and convert to int.
                    string co2String = numbers[co2Candidates.Single()];
                    co2 = Convert.ToInt32(co2String, 2);
                    break;
                }
            }

            return oxygen * co2;
        }
    }
}
