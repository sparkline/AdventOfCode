using AdventOfCode2022.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solvers
{
    public class Day14 : Solver
    {
        public Day14() : base(14) { }


        protected override object PartA(string input)
        {
            Dictionary<int, HashSet<(int from, int to)>> horizontalWalls = new Dictionary<int, HashSet<(int from, int to)>>();
            Dictionary<int, HashSet<(int from, int to)>> verticalWalls = new Dictionary<int, HashSet<(int from, int to)>>();

            // Load walls
            LoadWalls(input, ref horizontalWalls, ref verticalWalls);

            // Drop sand
            Dictionary<int, HashSet<int>> happyLittleGrainsOfSand = new Dictionary<int, HashSet<int>>(); // Dictionary(x,Set(y))
            while (droppedGrainOfSand(500, 0, horizontalWalls, verticalWalls, ref happyLittleGrainsOfSand))
            {
                // still dropping sand
            }
            int grainsDropped = happyLittleGrainsOfSand.Sum(columnSet => columnSet.Value.Count);
            return grainsDropped;
        }

        private static void LoadWalls(string input, ref Dictionary<int, HashSet<(int from, int to)>> horizontalWalls, ref Dictionary<int, HashSet<(int from, int to)>> verticalWalls)
        {
            var lines = input.SplitOnNewline();
            foreach (var line in lines)
            {
                int xFrom, xTo, yFrom, yTo;
                var coords = line.Split(" -> ");
                xFrom = int.Parse(coords[0].Split(',')[0]);
                yFrom = int.Parse(coords[0].Split(',')[1]);
                foreach (var coord in coords.Skip(1))
                {
                    xTo = int.Parse(coord.Split(',')[0]);
                    yTo = int.Parse(coord.Split(',')[1]);
                    if (xFrom == xTo)
                    {
                        if (!verticalWalls.TryGetValue(xFrom, out HashSet<(int from, int to)> walls))
                        {
                            walls = new HashSet<(int from, int to)>();
                            verticalWalls.Add(xFrom, walls);
                        }
                        walls.Add((from: Math.Min(yFrom, yTo), to: Math.Max(yFrom, yTo)));
                    }
                    else if (yFrom == yTo)
                    {
                        if (!horizontalWalls.TryGetValue(yFrom, out HashSet<(int from, int to)> walls))
                        {
                            walls = new HashSet<(int from, int to)>();
                            horizontalWalls.Add(yFrom, walls);
                        }
                        walls.Add((from: Math.Min(xFrom, xTo), to: Math.Max(xFrom, xTo)));
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    xFrom = xTo;
                    yFrom = yTo;
                }
            }

            // Simplify walls
            SimplifyWalls(ref horizontalWalls);
            SimplifyWalls(ref verticalWalls);
        }

        private bool droppedGrainOfSand(int sourceX, int sourceY, Dictionary<int, HashSet<(int from, int to)>> horizontalWalls, Dictionary<int, HashSet<(int from, int to)>> verticalWalls, ref Dictionary<int, HashSet<int>> happyLittleGrainsOfSand)
        {
            // get our collision
            int verticalWallCollision = GetVerticalWallCollision(sourceX, sourceY, verticalWalls);
            int horizontalWallCollision = GetHorizontalWallCollision(sourceX, sourceY, horizontalWalls);
            int grainCollision = GetGrainCollision(sourceX, sourceY, ref happyLittleGrainsOfSand);

            int firstCollision = Math.Min(grainCollision,Math.Min(verticalWallCollision, horizontalWallCollision));
            if (firstCollision == int.MaxValue || firstCollision == sourceY) // void, or plugged
            {
                // Could not drop grain of sand
                return false;
            }
            else if (spotFree(sourceX - 1, firstCollision, horizontalWalls, verticalWalls, ref happyLittleGrainsOfSand))
            {
                // drop left
                return droppedGrainOfSand(sourceX - 1, firstCollision, horizontalWalls, verticalWalls, ref happyLittleGrainsOfSand);
            }
            else if (spotFree(sourceX + 1, firstCollision, horizontalWalls, verticalWalls, ref happyLittleGrainsOfSand))
            {
                // drop right
                return droppedGrainOfSand(sourceX + 1, firstCollision, horizontalWalls, verticalWalls, ref happyLittleGrainsOfSand);
            }
            else
            {
                // settle
                if (!happyLittleGrainsOfSand.TryGetValue(sourceX, out HashSet<int> yValues))
                {
                    yValues = new HashSet<int>();
                    happyLittleGrainsOfSand.Add(sourceX, yValues);
                }
                // Drop one spot above the collision
                yValues.Add(firstCollision - 1);
                return true;
            }
        }

        private bool spotFree(int x, int y, Dictionary<int, HashSet<(int from, int to)>> horizontalWalls, Dictionary<int, HashSet<(int from, int to)>> verticalWalls, ref Dictionary<int, HashSet<int>> happyLittleGrainsOfSand)
        {
            if (happyLittleGrainsOfSand.TryGetValue(x, out HashSet<int> yValues) && yValues.Contains(y))
            {
                return false;
            }
            if (verticalWalls.Any(columnSet => columnSet.Key == x && columnSet.Value.Any(wall => wall.from <= y && wall.to >= y)))
            {
                return false;
            }
            if (horizontalWalls.Any(rowSet => rowSet.Key == y && rowSet.Value.Any(wall => wall.from <= x && wall.to >= x)))
            {
                return false;
            }

            return true;
        }

        private int GetGrainCollision(int sourceX, int sourceY, ref Dictionary<int, HashSet<int>> happyLittleGrainsOfSand)
        {
            if (happyLittleGrainsOfSand.TryGetValue(sourceX, out HashSet<int> grainsInMyColumn))
            {
                return grainsInMyColumn.Where(yValue => yValue >= sourceY).DefaultIfEmpty(int.MaxValue).Min();
            }
            return int.MaxValue;
        }

        private static int GetHorizontalWallCollision(int sourceX, int sourceY, Dictionary<int, HashSet<(int from, int to)>> horizontalWalls)
        {
            var horizontalWallsUnderMe = horizontalWalls.Where(s => s.Key >= sourceY).OrderBy(s => s.Key).Where(wallsOnYlevel => wallsOnYlevel.Value.Any(w => w.from <= sourceX && w.to >= sourceX));
            if (horizontalWallsUnderMe.Any())
            {
                int firstHit = horizontalWallsUnderMe.First().Key;
                return firstHit;
            }
            return int.MaxValue;
        }

        private static int GetVerticalWallCollision(int sourceX, int sourceY, Dictionary<int, HashSet<(int from, int to)>> verticalWalls)
        {
            if (verticalWalls.TryGetValue(sourceX, out HashSet<(int from, int to)> walls))
            {
                var wallsInColumnUnderMe = walls.Where(w => w.to >= sourceY);
                if (wallsInColumnUnderMe.Any())
                {
                    int firstHit = wallsInColumnUnderMe.Min(w => w.from);
                    return firstHit;
                }
            }

            return int.MaxValue;
        }

        private static void SimplifyWalls(ref Dictionary<int, HashSet<(int from, int to)>> walls)
        {
            foreach (var kvp in walls)
            {
                var wallSet = kvp.Value;
                HashSet<(int from, int to)> explodedSet = new HashSet<(int from, int to)>();
                int from = wallSet.Min(w => w.from);
                int to = wallSet.Max(w => w.to);
                for (int i = from; i <= to + 1; i++)
                {
                    if (!wallSet.Any(w => w.from <= i && w.to >= i))
                    {
                        // nothing here
                        if (from < i)
                        {
                            // we haven't moved the goalpost, so to our left there is something
                            explodedSet.Add((from: from, to: i - 1));
                        }
                        from = i + 1; // from is somewhere to the right, but not here
                    } 
                }
                walls[kvp.Key] = explodedSet;
            }
        }

        protected override object PartB(string input)
        {
            Dictionary<int, HashSet<(int from, int to)>> horizontalWalls = new Dictionary<int, HashSet<(int from, int to)>>();
            Dictionary<int, HashSet<(int from, int to)>> verticalWalls = new Dictionary<int, HashSet<(int from, int to)>>();

            // Load walls
            LoadWalls(input, ref horizontalWalls, ref verticalWalls);
            int highestHorizontalY = horizontalWalls.Keys.Max();
            int highestVerticalY = verticalWalls.Max(columnSet => columnSet.Value.Max(verticalWall => verticalWall.to));
            int highestYCoordinate = Math.Max(highestHorizontalY, highestVerticalY);
            int floorY = highestYCoordinate + 2;
            // Create floor, is almost twice to wide, but it doesn't cost us a penalty
            horizontalWalls.Add(floorY, new HashSet<(int from, int to)> { (from: 500-floorY, to: 500+floorY) });

            // Drop sand
            Dictionary<int, HashSet<int>> happyLittleGrainsOfSand = new Dictionary<int, HashSet<int>>(); // Dictionary(x,Set(y))
            while (droppedGrainOfSand(500, 0, horizontalWalls, verticalWalls, ref happyLittleGrainsOfSand))
            {
                // still dropping sand
            }
            int grainsDropped = happyLittleGrainsOfSand.Sum(columnSet => columnSet.Value.Count);
            return grainsDropped;
        }

    }
    
}
