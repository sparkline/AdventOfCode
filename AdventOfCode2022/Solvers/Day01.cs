using AdventOfCode2022.Common;
using System;

namespace AdventOfCode2022.Solvers
{
    public class Day01 : Solver
    {
        public Day01() : base(1) { }

        protected override object PartA(string input)
        {
            var groupedInputs = input.Split("\n\n").Select(subList => subList.ToIntList()).ToList();
            var sums = groupedInputs.Select(x => x.Sum());

            var maxValue = sums.Max();

            return maxValue;
        }

        protected override object PartB(string input)
        {
            var groupedInputs = input.Split("\n\n").Select(subList => subList.ToIntList()).ToList();
            var sums = groupedInputs.Select(x => x.Sum());
            var topThreeSum = sums.OrderByDescending(x => x).Take(3).Sum();

            return topThreeSum; 
        }
    }
}
