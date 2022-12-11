using AdventOfCode2022.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solvers
{
    public class Day09 : Solver
    {
        public Day09() : base(9) { }


        protected override object PartA(string input)
        {
            var lines = input.SplitOnNewline();
            List<(int x, int y)> deltaMoves = lines.SelectMany(line => getMoves(line.Split()[0][0], int.Parse(line.Split()[1]))).ToList();
            List<(int x, int y)> moves = new List<(int x, int y)>();
            (int x, int y) headPosition = (0, 0);
            foreach (var move in deltaMoves)
            {
                headPosition = headPosition.Sum(move);
                moves.Add(headPosition);
            }

            (int x, int y) currentTailPosition = (0, 0);
            HashSet<(int x, int y)> visited = new HashSet<(int x, int y)> { currentTailPosition };
            foreach (var move in moves)
            {
                currentTailPosition = moveYaBooty(currentTailPosition, move, ref visited);
            }

            int visitedPlaces = visited.Count;
            
            return visitedPlaces;
        }

        private (int x, int y) moveYaBooty((int x, int y) currentTailPosition, (int x, int y) move, ref HashSet<(int x, int y)> visited)
        {
            var delta = move.Subtract(currentTailPosition);
            if (delta.x.Abs() <= 1 && delta.y.Abs() <= 1)
            {
                // We are in range, don't do anything
                return currentTailPosition;
            } 
            else
            {
                var vector = delta.NormalVector();
                var newTailPosition = vector.Sum(currentTailPosition);
                visited.Add(newTailPosition); 
                return newTailPosition;
            }
        }

        private IEnumerable<(int x, int y)> getMoves(char dir, int amount)
        {
            return dir switch 
            {
                'L' => Enumerable.Repeat((-1, 0),amount),
                'R' => Enumerable.Repeat((1, 0),amount),
                'U' => Enumerable.Repeat((0, -1),amount),
                'D' => Enumerable.Repeat((0, 1), amount),
                _ => throw new NotImplementedException(),
            };
        }

        protected override object PartB(string input)
        {
            var lines = input.SplitOnNewline();
            List<(int x, int y)> deltaMoves = lines.SelectMany(line => getMoves(line.Split()[0][0], int.Parse(line.Split()[1]))).ToList();
            List<(int x, int y)> moves = new List<(int x, int y)>();
            (int x, int y) headPosition = (0, 0);
            foreach (var move in deltaMoves)
            {
                headPosition = headPosition.Sum(move);
                moves.Add(headPosition);
            }

            int tailLength = 9;
            (int x, int y)[] currentTailPositions = Enumerable.Repeat((0,0), tailLength).ToArray();
            HashSet<(int x, int y)> visited = new HashSet<(int x, int y)> { currentTailPositions[tailLength-1] };
            foreach (var move in moves)
            {
                moveYaBooties(currentTailPositions, move, ref visited);
            }

            int visitedPlaces = visited.Count;
            return visitedPlaces;
        }

        private void moveYaBooties((int x, int y)[] currentTailPositions, (int x, int y) move, ref HashSet<(int x, int y)> visited)
        {
            (int x, int y) myLeader = move;
            for (int tailSection = 0; tailSection < currentTailPositions.Length; tailSection++)
            {
                var currentTailPosition = currentTailPositions[tailSection];
                var delta = myLeader.Subtract(currentTailPosition);
                if (delta.x.Abs() <= 1 && delta.y.Abs() <= 1)
                {
                    // We are in range, don't do anything
                    myLeader = currentTailPosition;
                }
                else
                {
                    // Move one step to neighbour
                    var vector = delta.NormalVector();
                    var newTailPosition = vector.Sum(currentTailPosition);
                    myLeader = newTailPosition;
                    currentTailPositions[tailSection] = newTailPosition;
                    if (tailSection == currentTailPositions.Length-1)
                    {
                        // keep track of the tip
                        visited.Add(newTailPosition);
                    }
                }
            }
        }
    }
    
}
