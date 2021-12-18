using AdventOfCode2021.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solvers
{
    public class Day17 : Solver
    {
        public Day17() : base(2021, 17) { }
        protected override object PartA(string input)
        {
            string pattern = @"target area: x=(?<x1>-?\d+)\.\.(?<x2>-?\d+), y=(?<y1>-?\d+)\.\.(?<y2>-?\d+)";
            Regex r = new Regex(pattern);
            var match = r.Match(input);
            int minX = Int32.Parse(match.Groups["x1"].Value);
            int minY = Int32.Parse(match.Groups["y1"].Value);
            int maxX = Int32.Parse(match.Groups["x2"].Value);
            int maxY = Int32.Parse(match.Groups["y2"].Value);

            /*
             * We need to shoot up. And hit it on the way back.
             * The velocity when we come down after the arc is our initial velocity, but inverted.
             * We just want to hit the box on the way down. So the maximum velocity is the difference between the origin and the bottom of the box.
             * Maximum height is therefore InitialVy*(InitialVy+1)/ 2
             * 
             */

            int maxVy = Math.Abs(minY) - 1;
            int stylishY = (maxVy * (maxVy + 1)) / 2;

            return stylishY;
        }

        protected override object PartB(string input)
        {
            string pattern = @"target area: x=(?<x1>-?\d+)\.\.(?<x2>-?\d+), y=(?<y1>-?\d+)\.\.(?<y2>-?\d+)";
            Regex r = new Regex(pattern);
            var match = r.Match(input);
            int minX = Int32.Parse(match.Groups["x1"].Value);
            int minY = Int32.Parse(match.Groups["y1"].Value);
            int maxX = Int32.Parse(match.Groups["x2"].Value);
            int maxY = Int32.Parse(match.Groups["y2"].Value);

            // See part A
            int maxVy = Math.Abs(minY) - 1;
            // Shooting down, first hit
            int minVy = -Math.Abs(minY);
            // first hit in the box
            int maxVx = Math.Abs(maxX);
            // dead in the box - minX = minVx * (minVx+1) * 0.5
            int minVx = (int)Math.Floor(Math.Sqrt((double)minX*2 - 0.25) - 0.5);

            int hits = 0;
            for (int vx = minVx; vx <= maxVx; vx++)
            {
                for (int vy = minVy; vy <= maxVy; vy++)
                {
                    int x = 0;
                    int y = 0;
                    int _vx = vx;
                    int _vy = vy;
                    while (x <= maxX && y >= minY)
                    {
                        x += _vx;y += _vy;

                        if (x <= maxX && y >= minY && x >= minX && y <= maxY)
                        {
                            hits++;
                            break;
                        }

                        if (_vx > 0)
                        {
                            _vx--;
                        }
                        _vy--;
                    }
                }
            }

            return hits;


            /*
             * Atempt 1, the hard way... 
             * 
             * 
            Dictionary<int, List<int>> flyBy = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> dead = new Dictionary<int, List<int>>();
            for (int initialVx = 1; initialVx <= maxX; initialVx++)
            {
                // Let's see which x values will intersect with the targetarea
                int x = 0;
                int vx = initialVx;
                for (int step = 1; true; step++)
                {
                    if (vx <= 0 || x >= maxX)
                    {
                        // No hit, we are dead in the water, or we have overshot
                        break;
                    }

                    // increase x-position, and decrease speed
                    x += vx--;
                    if (x >= minX && x <= maxX)
                    {
                        // Hit! - try to add, might already found a value
                        if (vx == 0)
                        {
                            if (!dead.ContainsKey(step))
                            {
                                dead.Add(step, new List<int>());
                            }
                            dead[step].Add(initialVx);
                        } 
                        else
                        {
                            if (!flyBy.ContainsKey(step))
                            {
                                flyBy.Add(step, new List<int>());
                            }
                            flyBy[step].Add(initialVx);
                        }

                        // Might, hit a second time, don't break.
                    }
                }
            }
            
            Dictionary<int, List<int>> yHit = new Dictionary<int, List<int>>();
            int maxVy = Math.Abs(minY) - 1;
            // All options where we have a chance
            for (int initialVy = minY; initialVy <= maxVy; initialVy++)
            {
                var curveSteps = 0;
                if (initialVy > 0)
                {
                    curveSteps += initialVy * 2 + 1;
                }
                int absVy = Math.Abs(initialVy);

                decimal dYmin = Math.Min(Math.Abs(minY), Math.Abs(maxY));
                decimal dYmax = Math.Max(Math.Abs(minY), Math.Abs(maxY));
                // This sum is wrong
                decimal stepsMin = (dYmin + 1) / (absVy + 1);
                decimal stepsMax = (dYmax + 1) / (absVy + 1);
                for (int stepsToAdd = (int)Math.Ceiling(stepsMin); stepsToAdd <= (int)Math.Floor(stepsMax); stepsToAdd++)
                {
                    int steps = curveSteps + stepsToAdd;
                    if (!yHit.ContainsKey(steps))
                    {
                        yHit.Add(steps, new List<int>());
                    }
                    yHit[steps].Add(initialVy);
                }
            }

            // Create some combinations
            HashSet<(int x, int y)> hits = new HashSet<(int x,int y)> ();
            foreach (var yvalue in yHit)
            {
                int step = yvalue.Key;
                if (flyBy.TryGetValue(step, out var xValues))
                {
                    foreach (var x in xValues)
                    {
                        foreach (var y in yvalue.Value)
                        {
                            hits.Add((x: x, y: y));
                        }
                    }
                }

                foreach (var x2Values in dead.Where(k => k.Key <= step).Select(k => k.Value))
                {
                    foreach (var x in x2Values)
                    {
                        foreach (var y in yvalue.Value)
                        {
                            hits.Add((x: x, y: y));
                        }
                    }
                }
            }

            return hits.Count();
            */
        }
    }
}
