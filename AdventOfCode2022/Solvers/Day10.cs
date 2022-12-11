using AdventOfCode2022.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solvers
{
    public class Day10 : Solver
    {
        public Day10() : base(10) { }


        protected override object PartA(string input)
        {
            var lines = input.SplitOnNewline();
            Dictionary<int, int> register = new Dictionary<int, int>();
            int clockTick = 0;
            register.Add(clockTick, 1); // register tick ==> x
            foreach (var line in lines)
            {
                var cmd = line.Split()[0];
                var args = line.Split()[1..];
                switch(cmd)
                {
                    case "addx":
                        int increment = int.Parse(args[0]);
                        int currentValue = register[register.Keys.Max(i => i)];
                        clockTick += 2;
                        register.Add(clockTick+1, increment+currentValue); // effect visible after clocktick
                        break;
                    case "noop":
                        clockTick++;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            long totalScore = 0;
            for (int clock = 20; clock <= 220; clock+=40)
            {
                int value = getValue(register, clock);
                int signalStrength = clock * value;
                totalScore += signalStrength;
            }

            return totalScore;
        }

        private int getValue(Dictionary<int, int> register, int clock)
        {
            int index = register.Keys.Where(k => k <= clock).Max();
            return register[index];
        }

        protected override object PartB(string input)
        {
            var lines = input.SplitOnNewline();
            Dictionary<int, int> register = new Dictionary<int, int>();
            int clockTick = 0;
            register.Add(clockTick, 1); // register tick ==> x
            foreach (var line in lines)
            {
                var cmd = line.Split()[0];
                var args = line.Split()[1..];
                switch (cmd)
                {
                    case "addx":
                        int increment = int.Parse(args[0]);
                        int currentValue = register[register.Keys.Max(i => i)];
                        clockTick += 2;
                        register.Add(clockTick + 1, increment + currentValue); // effect visible after clocktick
                        break;
                    case "noop":
                        clockTick++;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            StringBuilder screen = new StringBuilder();
            for (int i = 0; i < 240; i++)
            {
                if ((i - 1) % 40 == 39) // lol
                {
                    screen.AppendLine();
                }
 
                int xPos = i % 40;
                int clock = i + 1;
                int xValue = getValue(register, clock);
                bool lit = ((xPos - xValue).Abs() <= 1);
                screen.Append(lit ? '#' : '.');
            }

            return screen.ToString();

        }

    }
    
}
