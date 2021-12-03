using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day02 : Solver
    {
        public Day02() : base(2021, 2, CodeType.Original) { }

        protected override object PartA(string input)
        {
            int horizontal = 0;
            int depth = 0;
            var lines = input.SplitOnNewline();
            foreach (var line in lines)
            {
                var splitted = line.Split(' ');
                string command = splitted[0];
                int amount = int.Parse(splitted[1]);
                switch (command)
                {
                    case "forward":
                        horizontal += amount;
                        break;
                    case "down":
                        depth += amount;
                        break;
                    case "up":
                        depth -= amount;
                        break;
                    default:
                        throw new ArgumentException("Command not known");
                }
            }

            int total = depth * horizontal;
            return total;
        }

        protected override object PartB(string input)
        {
            int horizontal = 0;
            int depth = 0;
            int aim = 0;
            var lines = input.SplitOnNewline();
            foreach (var line in lines)
            {
                var splitted = line.Split(' ');
                string command = splitted[0];
                int amount = int.Parse(splitted[1]);
                switch (command)
                {
                    case "forward":
                        horizontal += amount;
                        depth += (amount * aim);
                        break;
                    case "down":
                        aim += amount;
                        break;
                    case "up":
                        aim -= amount;
                        break;
                    default:
                        throw new ArgumentException("Command not known");
                }
            }

            int total = depth * horizontal;
            return total;
        }
    }
}
