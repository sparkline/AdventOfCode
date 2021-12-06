using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day06 : Solver
    {
        public Day06() : base(2021, 6, CodeType.Original) { }

        protected override object PartA(string input)
        {
            List<int> fishies = input.ToIntList(Extensions.SeparatorOption.Comma);
            int totalPeriod = 80;
            int coolDown = 6; // seven days including 0
            int initialCooldown = coolDown + 2;

            for (int day = 1; day <= totalPeriod; day++)
            {
                int fishiesDue = fishies.Count(dueDay => dueDay == 0);
                // Next day, Rejuvinate fishies and age fishies
                fishies = fishies.Select(dueDay => dueDay <= 0 ? coolDown : dueDay - 1).ToList();
                // Add new fish
                fishies.AddRange(Enumerable.Repeat(initialCooldown, fishiesDue));
            }

            int totalFishies = fishies.Count();
            return totalFishies;
        }

        protected override object PartB(string input)
        {
            List<int> fishies = input.ToIntList(Extensions.SeparatorOption.Comma);
            int totalPeriod = 256;
            int coolDown = 7;
            int initialCooldown = coolDown + 2;
            int ageRange = initialCooldown;

            double[] population = new double[ageRange];
            foreach (var fish in fishies)
            {
                population[fish]++;
            }

            for (int day = 0; day < totalPeriod; day++)
            {
                int dueFishIndex = day % ageRange;
                int resetFishIndex = (day + coolDown) % ageRange;

                population[resetFishIndex] += population[dueFishIndex]; // reset fish to new cycle
                // fish that are at dueAge will be at ageRange days older next round
            }

            double totalFishies = population.Sum();
            return totalFishies;
        }
    }
}
