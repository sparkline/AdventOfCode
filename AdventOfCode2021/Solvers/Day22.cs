using AdventOfCode2021.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Solvers
{
    public class Day22 : Solver
    {
        public Day22() : base(2021, 22) { }
        protected override object PartA(string input)
        {
            List<string> lines = input.SplitOnNewline();
            List<Cuboid> cuboids = lines.Select(line => Cuboid.GetCuboid(line)).Where(c => c.ApplyLimit(50)).ToList();
            List<Cuboid> theWholeShebang = new List<Cuboid>();
            foreach (Cuboid other in cuboids)
            {
                // make some room
                theWholeShebang = theWholeShebang.SelectMany(cuboid => cuboid.Collide(other)).ToList();
                // add it
                theWholeShebang.Add(other);
            }

            return theWholeShebang.Where(c => c.isOn()).Sum(c => c.blocks);
        }

        protected override object PartB(string input)
        {
            List<string> lines = input.SplitOnNewline();
            List<Cuboid> cuboids = lines.Select(line => Cuboid.GetCuboid(line)).Where(c => c != null).ToList();
            LinkedList<Cuboid> theWholeShebang = new LinkedList<Cuboid>();
            foreach (Cuboid other in cuboids)
            {
                var node = theWholeShebang.First;
                while (node != null)
                {
                    bool hit = !node.Value.NoTouchie(other);
                    if (hit)
                    {
                        var exploded = node.Value.Collide(other);
                        exploded.ForEach(c => theWholeShebang.AddFirst(c));
                        theWholeShebang.Remove(node);
                    }
                    node = node.Next;
                }
                theWholeShebang.AddFirst(other);
            }
            return theWholeShebang.Where(c => c.isOn()).Sum(c => c.blocks);
        }

        public class Cuboid
        {
            public enum State { ON, OFF }
            public int x1 { get; private set; }
            public int x2 { get; private set; }
            public int y1 { get; private set; }
            public int y2 { get; private set; }
            public int z1 { get; private set; }
            public int z2 { get; private set; }

            public bool isOn()
            {
                return state == State.ON;
            }
            State state;

            public long blocks { get { return (long)(1 + x2 - x1) * (1 + y2 - y1) * (1 + z2 - z1); } }

            public static Cuboid GetCuboid(string line)
            {
                Regex r = new Regex(@"-?\d+");
                var matches = r.Matches(line);
                int x1 = int.Parse(matches[0].Value);
                int x2 = int.Parse(matches[1].Value);
                int y1 = int.Parse(matches[2].Value);
                int y2 = int.Parse(matches[3].Value);
                int z1 = int.Parse(matches[4].Value);
                int z2 = int.Parse(matches[5].Value);
                State state = line.StartsWith("on") ? State.ON : State.OFF;

                return new Cuboid(x1, x2, y1, y2, z1, z2, state);
            }

            public Cuboid(int x1, int x2, int y1, int y2, int z1, int z2, State state)
            {
                this.x1 = x1;
                this.x2 = x2;
                this.y1 = y1;
                this.y2 = y2;
                this.z1 = z1;
                this.z2 = z2;
                this.state = state;
            }

            public bool ApplyLimit(int limit)
            {
                int l1 = -limit;
                int l2 = limit;

                if (limit <= 0 || (x1 >= l1 && x2 <= l2 && y1 >= l1 && y2 <= l2 && z1 >= l1 && z2 <= l2))
                {
                    // fully in the limits
                    return true;
                }
                else
                if (x2 < l1 || x1 > l2 || y2 < l1 || y1 > l2 || z2 < l1 || z1 > l2)
                {
                    // out of limits
                    return false;
                }
                else
                {
                    x1 = Math.Max(l1, x1);
                    x2 = Math.Min(l2, x2);
                    y1 = Math.Max(l1, y1);
                    y2 = Math.Min(l2, y2);
                    z1 = Math.Max(l1, z1);
                    z2 = Math.Min(l2, z2);
                    return true;
                }
            }

            public List<Cuboid> Collide(Cuboid other)
            {
                List<Cuboid> result = new List<Cuboid>();
                if (NoTouchie(other))
                {
                    // No hit
                    result.Add(this);
                }
                else
                {
                    int _x1, _x2, _y1, _y2, _z1, _z2;
                    // We are going to create max 6 new cubes
                    if (other.x1 > x1)
                    {
                        // space on the left - complete cap
                        _x1 = x1;
                        _x2 = other.x1 - 1;
                        _y1 = y1;
                        _y2 = y2;
                        _z1 = z1;
                        _z2 = z2;
                        result.Add(new Cuboid(_x1, _x2, _y1, _y2, _z1, _z2, state));
                    }
                    if (other.x2 < x2)
                    {
                        _x1 = other.x2 + 1;
                        _x2 = x2;
                        _y1 = y1;
                        _y2 = y2;
                        _z1 = z1;
                        _z2 = z2;
                        result.Add(new Cuboid(_x1, _x2, _y1, _y2, _z1, _z2, state));
                    }
                    if (other.y1 > y1)
                    {
                        // space on the top - only overlap on the z
                        _x1 = Math.Max(x1, other.x1);
                        _x2 = Math.Min(x2, other.x2);
                        _y1 = y1;
                        _y2 = other.y1 - 1;
                        _z1 = z1;
                        _z2 = z2;
                        result.Add(new Cuboid(_x1, _x2, _y1, _y2, _z1, _z2, state));
                    }
                    if (other.y2 < y2)
                    {
                        _x1 = Math.Max(x1, other.x1);
                        _x2 = Math.Min(x2, other.x2);
                        _y1 = other.y2 + 1;
                        _y2 = y2;
                        _z1 = z1;
                        _z2 = z2;
                        result.Add(new Cuboid(_x1, _x2, _y1, _y2, _z1, _z2, state));
                    }
                    if (other.z1 > z1)
                    {
                        // fit it neatly
                        _x1 = Math.Max(x1, other.x1);
                        _x2 = Math.Min(x2, other.x2);
                        _y1 = Math.Max(y1, other.y1);
                        _y2 = Math.Min(y2, other.y2);
                        _z1 = z1;
                        _z2 = other.z1 - 1;
                        result.Add(new Cuboid(_x1, _x2, _y1, _y2, _z1, _z2, state));
                    }
                    if (other.z2 < z2)
                    {
                        _x1 = Math.Max(x1, other.x1);
                        _x2 = Math.Min(x2, other.x2);
                        _y1 = Math.Max(y1, other.y1);
                        _y2 = Math.Min(y2, other.y2);
                        _z1 = other.z2 + 1;
                        _z2 = z2;
                        result.Add(new Cuboid(_x1, _x2, _y1, _y2, _z1, _z2, state));
                    }
                }

                return result;
            }

            public bool NoTouchie(Cuboid other)
            {
                return other.x2 < x1 || other.x1 > x2 || other.y2 < y1 || other.y1 > y2 || other.z2 < z1 || other.z1 > z2;
            }

            public override string ToString()
            {
                const int padding = 0;
                return $"{state} x: {x1,padding}..{x2,padding}, y:{y1,padding}..{y2,padding}, z:{z1,padding}..{z2,padding}";
            }

        }
    }
}
