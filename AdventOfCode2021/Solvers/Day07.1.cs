﻿using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day07_1 : Solver
    {
        public Day07_1() : base(2021, 7, 1, "brief") { }

        // 3x Slower
        protected override object PartA(string input)
        {
            List<int> crabPositions = input.ToIntList(Extensions.SeparatorOption.Comma);

            int minDistance = Enumerable.Range(crabPositions.Min(), crabPositions.Max())
                .Min(targetPosition => crabPositions
                    .Sum(crabPosition => Math.Abs(crabPosition - targetPosition)));

            return minDistance;
        }

        // 4x Slower
        protected override object PartB(string input)
        {
            List<int> crabPositions = input.ToIntList(Extensions.SeparatorOption.Comma);

            int minDistance = Enumerable.Range(crabPositions.Min(), crabPositions.Max())
                .Min(targetPosition => crabPositions
                    .Sum(crabPosition => Math.Abs(crabPosition - targetPosition) * (1 + Math.Abs(crabPosition - targetPosition)) / 2));

            return minDistance;
        }
    }
}
