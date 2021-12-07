using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day07 : Solver
    {
        public Day07() : base(2021, 7, CodeType.Original) { }

        protected override object PartA(string input)
        {
            int[] crabPositions = input.ToIntList(Extensions.SeparatorOption.Comma).ToArray();

            int minPos = crabPositions.Min();
            int maxPos = crabPositions.Max();

            int minDistance = int.MaxValue;
            for (int targetPos = minPos; targetPos <= maxPos; targetPos++)
            {
                // Try for each position
                int distance = 0;
                // Overkill? Anyway, added an extra stop clause to prevent unnessecary iterations
                for (int crab = 0; crab < crabPositions.Length && distance < minDistance; crab++)
                {
                    distance += Math.Abs(crabPositions[crab] - targetPos);
                }

                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }

            return minDistance;
        }

        protected override object PartB(string input)
        {
            int[] crabPositions = input.ToIntList(Extensions.SeparatorOption.Comma).ToArray();

            int minPos = crabPositions.Min();
            int maxPos = crabPositions.Max();

            int minDistance = int.MaxValue;
            for (int targetPos = minPos; targetPos <= maxPos; targetPos++)
            {
                // Try for each position
                int distance = 0;
                for (int crab = 0; crab < crabPositions.Length && distance < minDistance; crab++)
                {
                    int movement = Math.Abs(crabPositions[crab] - targetPos);
                    int fuelSpent = ((1 + movement) * movement) / 2; // Average cost times steps. Moved division outside to prevent casting to double
                    distance += fuelSpent;
                }
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }

            return minDistance;
        }
    }
}
