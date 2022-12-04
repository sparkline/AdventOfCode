using AdventOfCode2022.Common;
using System;

namespace AdventOfCode2022.Solvers
{
    public class Day03 : Solver
    {
        public Day03() : base(3) { }

        private int letterValue(char symbol)
        {
            return symbol switch
            {
                >= 'a' and <= 'z' => (int)symbol - 'a' + 1,
                >= 'A' and <= 'Z' => (int)symbol - 'A' + 27,
                _ => throw new NotImplementedException()
            };
        }

        protected override object PartA(string input)
        {
            var rucksacks = input.SplitOnNewline();
            int totalValue = 0;

            foreach (var rucksack in rucksacks)
            {
                var lhs = rucksack.Take(rucksack.Length / 2);
                var rhs = rucksack.Skip(rucksack.Length / 2);

                var doubleElement = lhs.Intersect(rhs);

                totalValue += doubleElement.Sum(symbol => letterValue(symbol));
            }


            return totalValue;
        }

        protected override object PartB(string input)
        {
            var rucksacks = input.SplitOnNewline();
            int totalValue = 0;
            for (int i = 0; i < rucksacks.Count / 3; i++)
            {
                var intersection = rucksacks[i * 3].Intersect(rucksacks[(i * 3) + 1]).Intersect(rucksacks[(i * 3) + 2]);
                totalValue += intersection.Sum(s => letterValue(s));
            }

            return totalValue;
        }
    }


    
    
}
