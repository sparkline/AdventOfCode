using AdventOfCode2022.Common;
using System;

namespace AdventOfCode2022.Solvers
{
    public class Day04 : Solver
    {
        public Day04() : base(4) { }


        protected override object PartA(string input)
        {
            var pairs = input.SplitOnNewline();
            int overlappingPairs = 0;
            foreach (var pair in pairs)
            {
                var values = pair.Split(new[] { ',', '-' }).Select(s => int.Parse(s)).ToList();
                if ((values[0] >= values[2] && values[1] <= values[3]) || (values[0] <= values[2] && values[1] >= values[3]))
                { overlappingPairs++; }
            }

            return overlappingPairs;
        }

        protected override object PartB(string input)
        {
            var pairs = input.SplitOnNewline();
            int overlappingPairs = 0;
            foreach (var pair in pairs)
            {
                var values = pair.Split(new[] { ',', '-' }).Select(s => int.Parse(s)).ToList();
                if (values[0] <= values[3] && values[2] <= values[1]) 
                { overlappingPairs++; }
            }

            return overlappingPairs;
        }
    }


    
    
}
