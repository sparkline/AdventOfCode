using AdventOfCode2022.Common;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solvers
{
    public class Day05 : Solver
    {
        public Day05() : base(5) { }


        protected override object PartA(string input)
        {
            var blocks = input.Split("\n\n")[0].SplitOnNewline();
            int stackCount = blocks[^1].ToIntList(Extensions.SeparatorOption.Spaces).Max();
            var onlyBlocks = blocks.ToArray()[..^1].Reverse();
            
            List<Stack<char>> stacks = new List<Stack<char>>();
            for (int i = 0; i < stackCount; i++)
            {
                stacks.Add(new Stack<char>());
            }
            
            foreach (var line in onlyBlocks)
            {
                for (int i = 0; i < stackCount; i++)
                {
                    char block = line[(i * 4) + 1];
                    if (block != ' ')
                    {
                        stacks[i].Push(block);
                    }
                }
            }

            var operations = input.Split("\n\n")[1].SplitOnNewline().Select(line => Regex.Split(line, @"\D+").Where(d => d != String.Empty).Select(d => int.Parse(d)).ToList());
            foreach (var operation in operations)
            {
                int numBlocks = operation[0];
                int from = operation[1]-1;
                int to = operation[2]-1;

                for (int i = 0; i < numBlocks; i++)
                {
                    stacks[to].Push(stacks[from].Pop());
                }

            }

            string result = "";
            foreach ( var stack in stacks)
            {
                result += stack.Count > 0 ? stack.Peek() : "";
            }
            return result;
        }

        protected override object PartB(string input)
        {
            var blocks = input.Split("\n\n")[0].SplitOnNewline();
            int stackCount = blocks[^1].ToIntList(Extensions.SeparatorOption.Spaces).Max();
            var onlyBlocks = blocks.ToArray()[..^1].Reverse().ToArray();
            int stackHeight = onlyBlocks.Length;

            List<List<char>> stacks = new List<List<char>>();
            for (int i = 0; i < stackCount; i++)
            {
                stacks.Add(new List<char>());
            }

            foreach (var line in onlyBlocks)
            {
                for (int i = 0; i < stackCount; i++)
                {
                    char block = line[(i * 4) + 1];
                    if (block != ' ')
                    {
                        stacks[i].Add(block);
                    }
                }
            }

            var operations = input.Split("\n\n")[1].SplitOnNewline().Select(line => Regex.Split(line, @"\D+").Where(d => d != String.Empty).Select(d => int.Parse(d)).ToList());
            foreach (var operation in operations)
            {
                int numBlocks = operation[0];
                int from = operation[1] - 1;
                int to = operation[2] - 1;

                var liftIt = stacks[from].ToArray()[^numBlocks..];
                stacks[to].AddRange(liftIt);
                stacks[from] = stacks[from].ToArray()[..^numBlocks].ToList();
            }

            string result = "";
            foreach (var stack in stacks)
            {
                result += stack.Count > 0 ? stack[^1] : "";
            }
            return result;
        }
    }


    
    
}
