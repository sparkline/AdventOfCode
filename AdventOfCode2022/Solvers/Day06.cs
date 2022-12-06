using AdventOfCode2022.Common;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solvers
{
    public class Day06 : Solver
    {
        public Day06() : base(6) { }


        protected override object PartA(string input)
        {
            return Enumerable.Range(0, input.Length - 3).First(i => input.Skip(i).Take(4).Distinct().Count() == 4) + 4;
        }

        protected override object PartB(string input)
        {
            return Enumerable.Range(0, input.Length - 13).First(i => input.Skip(i).Take(14).Distinct().Count() == 14) + 14;
        }
    }


    
    
}
