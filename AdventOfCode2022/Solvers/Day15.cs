using AdventOfCode2022.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solvers
{
    public class Day15 : Solver
    {
        private readonly int checkLine = 2000000;
        private readonly int searchGrid = 4000000;

        public Day15() : base(15) { }
        public Day15(int checkLine, int searchGrid) : base(15)
        {
            this.checkLine = checkLine;
            this.searchGrid = searchGrid;
        }

        protected override object PartA(string input)
        {
            Dictionary<(int x, int y), int> sensors;
            HashSet<(int x, int y)> beacons;
            loadData(input, out sensors, out beacons);

            long coverageInRow = getCoverageInRow(checkLine, ref sensors, ref beacons);

            return coverageInRow;
        }

        private static void loadData(string input, out Dictionary<(int x, int y), int> sensors, out HashSet<(int x, int y)> beacons)
        {
            var lines = input.SplitOnNewline();
            sensors = new Dictionary<(int x, int y), int>();
            beacons = new HashSet<(int x, int y)>();
            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"Sensor at x=(?<sx>-?\d+), y=(?<sy>-?\d+): closest beacon is at x=(?<bx>-?\d+), y=(?<by>-?\d+)");
                if (match.Success)
                {
                    int bx = match.Groups["bx"].Value.ToInt();
                    int by = match.Groups["by"].Value.ToInt();
                    int sx = match.Groups["sx"].Value.ToInt();
                    int sy = match.Groups["sy"].Value.ToInt();
                    int distance = (bx - sx).Abs() + (by - sy).Abs();
                    beacons.Add((x: bx, y: by));
                    sensors.Add((x: sx, y: sy), distance);
                }
            }
        }

        private long getCoverageInRow(int rowNumber, ref Dictionary<(int x, int y), int> sensors, ref HashSet<(int x, int y)> beacons)
        {
            HashSet<(int from, int to)> coverage = Coverage(rowNumber, ref sensors);

            // Count total
            long coverageCount = coverage.Sum(ft => (ft.to - ft.from) + 1);

            // Remove beacons from count
            var beaconsInRow = beacons.Where(b => b.y == rowNumber).Select(b => b.x).OrderBy(x => x).ToList();
            foreach (var beacon in beaconsInRow)
            {
                if (coverage.Any(ft => ft.from <= beacon && ft.to >= beacon))
                {
                    coverageCount--;
                }
            }

            // Return count
            return coverageCount;
        }

        private HashSet<(int from, int to)> Coverage(int rowNumber, ref Dictionary<(int x, int y), int> sensors)
        {
            HashSet<(int from, int to)> coverage = new HashSet<(int from, int to)>();

            // Make a list of all the coverage
            foreach (var entry in sensors)
            {
                var sensor = entry.Key;
                var sensorRange = entry.Value;

                int shortestDistanceToRow = (sensor.y - rowNumber).Abs();
                if (shortestDistanceToRow <= sensorRange)
                {
                    int distanceLeft = (shortestDistanceToRow - sensorRange).Abs();
                    coverage.Add((from: sensor.x - distanceLeft, to: sensor.x + distanceLeft));
                }
            }
            // Remove overlap and sort list
            coverage = Simplify(coverage);
            return coverage;
        }

        private HashSet<(int from, int to)> Simplify(HashSet<(int from, int to)> coverage)
        {
            HashSet<(int from, int to)> simpleList = new HashSet<(int from, int to)>();
            var sortedList = coverage.OrderBy(a => a.from).GetEnumerator();
            if (sortedList.MoveNext())
            {
                (int from, int to) entry = sortedList.Current;
                while (sortedList.MoveNext())
                {
                    var candidate = sortedList.Current;
                    if (entry.Touches(candidate))
                    {
                        entry = entry.Union(candidate);
                    }
                    else
                    {
                        simpleList.Add(entry);
                        entry = candidate;
                    }
                }
                simpleList.Add(entry);
            }
            return simpleList;
        }

        protected override object PartB(string input)
        {
            Dictionary<(int x, int y), int> sensors;
            HashSet<(int x, int y)> beacons;
            loadData(input, out sensors, out beacons);
            
            for (int y = 0; y <= searchGrid; y++)
            {
                var coverage = Coverage(y, ref sensors).Where(x => x.to >= 0 && x.from <= searchGrid);
                long coverageCount = coverage.Sum(x => 1 + (Math.Min(x.to, searchGrid) - Math.Max(0, x.from)));
                if (coverageCount < searchGrid + 1)
                {
                    // found it
                    long frequency;
                    int x;
                    int rightBound = coverage.Max(c => c.from);
                    if (rightBound > 0)
                    {
                        x = rightBound - 1;
                    } 
                    else
                    {
                        // edge case, only other option
                        x = searchGrid;
                    }

                    frequency = (x * 4000000L) + y;
                    return frequency;
                }
            }

            return -1;
        }

    }
    
}
