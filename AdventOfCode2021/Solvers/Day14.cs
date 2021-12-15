using AdventOfCode2021.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solvers
{
    public class Day14 : Solver
    {
        public Day14() : base(2021, 14) { }
        protected override object PartA(string input)
        {
            return BuildMeAPolymer(input, 10);
        }

        private static object BuildMeAPolymer(string input, int iterations)
        {
            var lines = input.SplitOnNewline();
            List<char> polymer = new List<char>(lines[0]);
            Dictionary<string, char> transformations = new Dictionary<string, char>();
            foreach (string line in lines.Skip(1))
            {
                var p = line.Split("->", StringSplitOptions.TrimEntries);
                transformations.Add(p[0], p[1][0]);
            }

            for (int iteration = 1; iteration <= iterations; iteration++)
            {
                for (int i = polymer.Count - 1; i > 0; i--)
                {
                    var key = "" + polymer[i - 1] + polymer[i];
                    if (transformations.TryGetValue(key, out char value))
                    {
                        polymer.Insert(i, value);
                    }
                }
            }

            var grouped = polymer.GroupBy(c => c).OrderBy(c => c.Count()).Select(a => (key: a.Key, count: a.Count()));
            long min = grouped.First().count;
            long max = grouped.Last().count;
            return (max - min);
        }

        protected override object PartB(string input)
        {
            var lines = input.SplitOnNewline();
            string polymer = lines[0];
            Dictionary<string,List<string>> children = new Dictionary<string,List<string>>();
            Dictionary<string, Dictionary<char, long>> occurences = new Dictionary<string, Dictionary<char, long>>();
            Dictionary<string, Dictionary<char, long>> firstStep = new Dictionary<string, Dictionary<char, long>>();

            // Add Keys
            foreach (string line in lines.Skip(1))
            {
                var t = line.Split("->", StringSplitOptions.TrimEntries);
                occurences.Add(t[0], new Dictionary<char, long> { { t[1][0], 1L } });
                firstStep.Add(t[0], new Dictionary<char, long> { { t[1][0], 1L } });
                children.Add(t[0], new List<string> { t[0][0] + t[1], t[1] + t[0][1] });
            }

            // Prune nonexisting children
            foreach (var child in children)
            {
                children[child.Key] = child.Value.Where(c => occurences.ContainsKey(c)).ToList();
            }

            // Iterate
            for (int i = 1; i < 40; i++)
            {
                Dictionary<string, Dictionary<char, long>> newOccurences = new Dictionary<string, Dictionary<char, long>>();
               
                // Voodoo...
                newOccurences = children.ToDictionary(c => c.Key,
                    c => c.Value.Select(childName => occurences[childName])
                    .Aggregate((result, next) => result.Concat(next).GroupBy(c => c.Key).ToDictionary(c => c.Key, c => c.Sum(b => b.Value)))
                    .Concat(firstStep[c.Key]).GroupBy(c => c.Key).ToDictionary(c => c.Key, c => c.Sum(b => b.Value)));
                // Neh just kidding, we are merging the char-dicts of the children for each of the symbols. Which is the new char-dict of our symbol
                occurences = newOccurences;
            }

            // Let's see what characters we see in our input
            Dictionary<char, long> zheResult = polymer.GroupBy(c => c).ToDictionary(c => c.Key, c => (long)c.Count());

            // Now we are going to resolve our iterated symbols
            for (int i = 1; i< polymer.Length; i++)
            {
                string key = polymer.Substring(i-1,2);

                // Add the iterations
                zheResult = zheResult.Concat(occurences[key]).GroupBy(c => c.Key).ToDictionary(c => c.Key, c => c.Sum(b => b.Value));
            }

            long min = zheResult.Min(c => c.Value);
            long max = zheResult.Max(c => c.Value);

            return max - min;
        }

    }
}
