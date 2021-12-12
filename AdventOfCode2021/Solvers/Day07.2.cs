using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day07_2 : Solver
    {
        public Day07_2() : base(2021, 7, 2, "faster") { }

        protected override object PartA(string input)
        {
            int[] crabPositions = input.ToIntList(Extensions.SeparatorOption.Comma).OrderBy(c => c).ToArray();

            // The optimum is the median, which is the value in the middle of the (sorted) array.
            // argmin[x] Sum(|Si-x|) ==> minimum for Sum(sign(Si-x)), which is the median
            int median = crabPositions.Length / 2;

            return crabPositions.Sum(x => Math.Abs(x - crabPositions[median]));
        }

        protected override object PartB(string input)
        {
            int[] crabPositions = input.ToIntList(Extensions.SeparatorOption.Comma).OrderBy(c => c).ToArray();

            // Braindump:
            // The old optimum is the median, which is the value in the middle of the array.
            // However since there is a distance^2 element the value we search must be on the right
            // We can analyze again:
            // argmin[x] Sum(|Si-x|*(1+|Si-x|)*0.5) ==> minimum for Sum(Si-x-0.5*Sign(Si-x))
            // The 0.5*Sign(Si-x) part would solve to the median
            // The Si-x part would solve to the average (i=0..N for Si = N*avg(S))
            // Therefore our solution must be between the average and the median
            // In a sorted list of integers the average >= median
            // Solve it:    0 = Sum(Si) - Sum(x)    - Sum(0.5*Sign(Si-x))
            //              0 = N*avg   - N*x       - (median - x)
            //              x = (N*avg - median) / (N - 1)
            //              x = (N*avg - N/2) / (N - 1)
            //              x = (N*(avg-0.5)) / (N - 1)
            // 
            // Since this is almost the average, and we have to round the average down to go to the median. Our safe bet is that with large numbers the answer will be the average
            // 


            int median = crabPositions.Length / 2;
            double average = crabPositions.Average();
            int N = crabPositions.Length;

            int answerLocation = (int)Math.Floor((N * (average - 0.5)) / (N - 1));
            return crabPositions.Sum(x => Math.Abs(x - answerLocation) * (1 + Math.Abs(x - answerLocation)) * 0.5);
        }
    }
}
