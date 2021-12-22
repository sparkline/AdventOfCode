using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day19 : Solver
    {
        public Day19() : base(2021, 19) { }
        protected override object PartA(string input)
        {
            List<Universe> mismatchedUniverses = readScanners(input);

            var result = FindTheUniverse(mismatchedUniverses.First(), mismatchedUniverses.Skip(1).ToList());
            var beaconList = result.beacons.Select(b => b.position).Distinct().ToList();

            return beaconList.Count();
        }

        protected override object PartB(string input)
        {
            List<Universe> mismatchedUniverses = readScanners(input);

            var result = FindTheUniverse(mismatchedUniverses.First(), mismatchedUniverses.Skip(1).ToList());
            return result.scanners.SelectMany(f => result.scanners.Select(t => Distance(f, t))).Max();
        }


        private Universe FindTheUniverse(Universe referenceUniverse, List<Universe> toBeDiscovered)
        {
            Universe theUniverseFromTheReferencePoint = new Universe();
            theUniverseFromTheReferencePoint.Add(referenceUniverse);
            List<(Instruction instruction, Universe universe)> matchingUniverses = new List<(Instruction, Universe)>();

            foreach (Universe universe in toBeDiscovered)
            {
                Instruction? instruction = Matches(referenceUniverse, universe);
                if (instruction != null)
                {
                    matchingUniverses.Add((instruction.Value, universe));
                }
            }

            List<Universe> universesNotDiscovered = toBeDiscovered.Except(matchingUniverses.Select(u => u.universe)).ToList();
            foreach (var match in matchingUniverses)
            {
                Universe wrongCoordinates = FindTheUniverse(match.universe, universesNotDiscovered);
                Universe rightCoordinates = wrongCoordinates.ApplyInstruction(match.instruction);
                theUniverseFromTheReferencePoint.Add(rightCoordinates);
            }

            return theUniverseFromTheReferencePoint;
        }

        private Instruction? Matches(Universe referenceUniverse, Universe candidate)
        {
            List<(Coord referenceBeacon, Coord candidateBeacon)> beaconPairs = new List<(Coord referenceBeacon, Coord candidateBeacon)>();
            foreach (var referenceBeacon in referenceUniverse.beacons)
            {
                var distancesForReferenceBeacon = referenceUniverse.signatures.Where(s => s.from == referenceBeacon.id || s.to == referenceBeacon.id).Select(s => s.length).ToList();
                foreach (var candidateBeacon in candidate.beacons)
                {
                    var distancesForCandidateBeacon = candidate.signatures.Where(s => s.from == candidateBeacon.id || s.to == candidateBeacon.id).Select(s => s.length).ToList();

                    if (distancesForReferenceBeacon.IntersectWithDuplicates(distancesForCandidateBeacon).Count() >= 11)
                    {
                        beaconPairs.Add((referenceBeacon: referenceBeacon, candidateBeacon: candidateBeacon));
                    }
                }
            }

            if (beaconPairs.Count() >= 12)
            {
                return FindInstruction(beaconPairs);
            }

            return null;
        }

        private Instruction? FindInstruction(List<(Coord referenceBeacon, Coord candidateBeacon)> beaconPairs)
        {
            var anchor = beaconPairs[0];
            List<Coord> reference = beaconPairs.Select(b => b.referenceBeacon).ToList();
            List<Coord> candidates = beaconPairs.Select(b => b.candidateBeacon).ToList();

            foreach (Instruction.Face face in Enum.GetValues(typeof(Instruction.Face)))
            {
                foreach (Instruction.Rotation rotation in Enum.GetValues(typeof(Instruction.Rotation)))
                {
                    var r = anchor.referenceBeacon;
                    var c = anchor.candidateBeacon;
                    Instruction instruction = new Instruction(rotation, face);
                    var cc = TransformAndMove(instruction, c);

                    int dx = r.position.x - cc.position.x;
                    int dy = r.position.y - cc.position.y;
                    int dz = r.position.z - cc.position.z;
                    Instruction fullInstruction = new Instruction(rotation, face, dx, dy, dz);

                    var transformed = candidates.Select(c => TransformAndMove(fullInstruction, c)).ToList();
                    if (reference.IntersectWithDuplicates(transformed).Count() >= 12)
                    {
                        return fullInstruction;
                    }

                }
            }


            return null;


            /* Faster implementation, with some quircks..
            List<int> xRef = beaconPairs.Select(c => c) 
            referenceBeacons.Select(c => c.position.x - referenceBeacons.Min(c => c.position.x)).OrderBy(x => x).ToList();
            List<int> yRef = referenceBeacons.Select(c => c.position.y - referenceBeacons.Min(c => c.position.y)).OrderBy(x => x).ToList();
            List<int> zRef = referenceBeacons.Select(c => c.position.z - referenceBeacons.Min(c => c.position.z)).OrderBy(x => x).ToList();

            List<int> x = extraBeacons.Select(c => c.position.x - extraBeacons.Min(c => c.position.x)).OrderBy(x => x).ToList();
            List<int> y = extraBeacons.Select(c => c.position.y - extraBeacons.Min(c => c.position.y)).OrderBy(x => x).ToList();
            List<int> z = extraBeacons.Select(c => c.position.z - extraBeacons.Min(c => c.position.z)).OrderBy(x => x).ToList();
            List<int> xInverted = extraBeacons.Select(c => Math.Abs(c.position.x - extraBeacons.Max(c => c.position.x))).OrderBy(x => x).ToList();
            List<int> yInverted = extraBeacons.Select(c => Math.Abs(c.position.y - extraBeacons.Max(c => c.position.y))).OrderBy(x => x).ToList();
            List<int> zInverted = extraBeacons.Select(c => Math.Abs(c.position.z - extraBeacons.Max(c => c.position.z))).OrderBy(x => x).ToList();

            Instruction.Face face = Instruction.Face.Forward;
            Instruction.Rotation rotation = Instruction.Rotation._0;

            bool foundFace = false;
            foreach (Instruction.Face tryFace in Enum.GetValues(typeof(Instruction.Face)))
            {
                (List<int> x, List<int> y, List<int> z) result = Transform(new Instruction(rotation, tryFace), x, y, z, xInverted, yInverted, zInverted);
                if (xRef.Intersect(result.x).Count() >= 12)
                {
                    foundFace = true;
                    face = tryFace;
                    break;
                }
            }
            if (!foundFace)
            {
                return FindInstructionSlow(referenceBeacons, extraBeacons);
            }

            bool foundRotation = false;
            foreach (Instruction.Rotation tryRotation in Enum.GetValues(typeof(Instruction.Rotation)))
            {
                (List<int> x, List<int> y, List<int> z) result = Transform(new Instruction(tryRotation, face), x, y, z, xInverted, yInverted, zInverted);
                if (xRef.Intersect(result.x).Count() >= 12 && yRef.Intersect(result.y).Count() >= 12 && zRef.Intersect(result.z).Count() >= 12)
                {
                    foundRotation = true;
                    rotation = tryRotation;
                    break;
                }
            }
            if (!foundRotation)
            {
                return FindInstructionSlow(referenceBeacons, extraBeacons);
            }

            var referenceCorner = referenceBeacons.Select(b => b.position).OrderBy(b => b.x).ThenBy(b => b.y).ThenBy(b => b.z).First();
            var extraCorner = extraBeacons.Select(b => Transform(new Instruction(rotation, face), b)).OrderBy(b => b.x).ThenBy(b => b.y).ThenBy(b => b.z).First();

            int dx = referenceCorner.x - extraCorner.x;
            int dy = referenceCorner.y - extraCorner.y;
            int dz = referenceCorner.z - extraCorner.z;
            return new Instruction(rotation, face, dx, dy, dz);
            */

        }

        internal static int Distance(Coord from, Coord to)
        {
            return Distance(from.position, to.position);
        }

        internal static int Distance((int x, int y, int z) referencePosition, (int x, int y, int z) otherPosition)
        {
            return Math.Abs(referencePosition.x - otherPosition.x)
                + Math.Abs(referencePosition.y - otherPosition.y)
                + Math.Abs(referencePosition.z - otherPosition.z);
        }
        /*
        private Instruction? FindInstruction(IEnumerable<Coord> referenceBeacons, IEnumerable<Coord> extraBeacons)
        {
            List<int> xRef = referenceBeacons.Select(c => c.position.x - referenceBeacons.Min(c => c.position.x)).OrderBy(x => x).ToList();
            List<int> yRef = referenceBeacons.Select(c => c.position.y - referenceBeacons.Min(c => c.position.y)).OrderBy(x => x).ToList();
            List<int> zRef = referenceBeacons.Select(c => c.position.z - referenceBeacons.Min(c => c.position.z)).OrderBy(x => x).ToList();

            List<int> x = extraBeacons.Select(c => c.position.x - extraBeacons.Min(c => c.position.x)).OrderBy(x => x).ToList();
            List<int> y = extraBeacons.Select(c => c.position.y - extraBeacons.Min(c => c.position.y)).OrderBy(x => x).ToList();
            List<int> z = extraBeacons.Select(c => c.position.z - extraBeacons.Min(c => c.position.z)).OrderBy(x => x).ToList();
            List<int> xInverted = extraBeacons.Select(c => Math.Abs(c.position.x - extraBeacons.Max(c => c.position.x))).OrderBy(x => x).ToList();
            List<int> yInverted = extraBeacons.Select(c => Math.Abs(c.position.y - extraBeacons.Max(c => c.position.y))).OrderBy(x => x).ToList();
            List<int> zInverted = extraBeacons.Select(c => Math.Abs(c.position.z - extraBeacons.Max(c => c.position.z))).OrderBy(x => x).ToList();

            Instruction.Face face = Instruction.Face.Forward;
            Instruction.Rotation rotation = Instruction.Rotation._0;

            bool foundFace = false;
            foreach (Instruction.Face tryFace in Enum.GetValues(typeof(Instruction.Face)))
            {
                (List<int> x, List<int> y, List<int> z) result = Transform(new Instruction(rotation, tryFace), x, y, z, xInverted, yInverted, zInverted);
                if (xRef.Intersect(result.x).Count() >= 12)
                {
                    foundFace = true;
                    face = tryFace;
                    break;
                }
            }
            if (!foundFace)
            {
                return FindInstructionSlow(referenceBeacons, extraBeacons);
            }

            bool foundRotation = false;
            foreach (Instruction.Rotation tryRotation in Enum.GetValues(typeof(Instruction.Rotation)))
            {
                (List<int> x, List<int> y, List<int> z) result = Transform(new Instruction(tryRotation, face), x, y, z, xInverted, yInverted, zInverted);
                if (xRef.Intersect(result.x).Count() >= 12 && yRef.Intersect(result.y).Count() >= 12 && zRef.Intersect(result.z).Count() >= 12)
                {
                    foundRotation = true;
                    rotation = tryRotation;
                    break;
                }
            }
            if (!foundRotation)
            {
                return FindInstructionSlow(referenceBeacons, extraBeacons);
            }

            var referenceCorner = referenceBeacons.Select(b => b.position).OrderBy(b => b.x).ThenBy(b => b.y).ThenBy(b => b.z).First();
            var extraCorner = extraBeacons.Select(b => Transform(new Instruction(rotation, face), b)).OrderBy(b => b.x).ThenBy(b => b.y).ThenBy(b => b.z).First();

            int dx = referenceCorner.x - extraCorner.x;
            int dy = referenceCorner.y - extraCorner.y;
            int dz = referenceCorner.z - extraCorner.z;
            return new Instruction(rotation, face, dx, dy, dz);

        }
        */
        private List<Universe> readScanners(string input)
        {
            List<Universe> scanners = new List<Universe>();
            var lines = input.SplitOnNewline();
            Universe scanner = new Universe();
            int uniqueId = 1000;
            foreach (var line in lines)
            {
                if (line.StartsWith("---"))
                {
                    scanner = new Universe();
                    scanners.Add(scanner);
                }
                else
                {
                    var coords = line.Split(',').Select(s => int.Parse(s)).ToArray();
                    scanner.AddBeacon(new Coord(uniqueId++, coords[0], coords[1], coords[2]));
                }
            }

            return scanners;

        }

        internal struct Instruction
        {


            public Instruction(Rotation rotation, Face face)
            {
                this.rotation = rotation;
                this.face = face;
                this.dx = 0;
                this.dy = 0;
                this.dz = 0;
            }

            public Instruction(Rotation rotation, Face face, int dx, int dy, int dz) : this(rotation, face)
            {
                this.dx = dx;
                this.dy = dy;
                this.dz = dz;
            }

            public enum Rotation { _0, _90, _180, _270 }
            public enum Face { Up, Down, Left, Right, Forward, Backward }
            public Rotation rotation { get; }
            public Face face { get; }

            public int dx { get; }
            public int dy { get; }
            public int dz { get; }

            public (int x, int y, int z) position { get { return (x: dx, y: dy, z: dz); } }

            public override string ToString()
            {
                return $"{rotation}-{face}";
            }

        }

        static internal (int x, int y, int z) TransformAndMove(Instruction instruction, (int x, int y, int z) position)
        {
            var result = Transform(instruction, position.x, position.y, position.z, -position.x, -position.y, -position.z);
            result = (x: result.x + instruction.dx, y: result.y + instruction.dy, z: result.z + instruction.dz);
            return result;
        }

        static internal Coord TransformAndMove(Instruction instruction, Coord coord)
        {
            var result = TransformAndMove(instruction, coord.position);
            return new Coord(coord.id, result.x, result.y, result.z);
        }

        static internal (T x, T y, T z) Transform<T>(Instruction instruction, T x, T y, T z, T xi, T yi, T zi)
        {
            (T x, T xi, T y, T yi, T z, T zi) input = (x, xi, y, yi, z, zi);

            (T x, T xi, T y, T yi, T z, T zi) facing = instruction.face switch
            {
                Instruction.Face.Forward => (input.x, input.xi, input.y, input.yi, input.z, input.zi),
                Instruction.Face.Right => (input.z, input.zi, input.y, input.yi, input.xi, input.x),
                Instruction.Face.Backward => (input.xi, input.x, input.y, input.yi, input.zi, input.z),
                Instruction.Face.Left => (input.zi, input.z, input.y, input.yi, input.x, input.xi),
                Instruction.Face.Up => (input.yi, input.y, input.x, input.xi, input.z, input.zi),
                Instruction.Face.Down => (input.y, input.yi, input.xi, input.x, input.z, input.zi),
            };

            (T x, T xi, T y, T yi, T z, T zi) rotated = instruction.rotation switch
            {
                Instruction.Rotation._0 => (facing.x, facing.xi, facing.y, facing.yi, facing.z, facing.zi),
                Instruction.Rotation._90 => (facing.x, facing.xi, facing.zi, facing.z, facing.y, facing.yi),
                Instruction.Rotation._180 => (facing.x, facing.xi, facing.yi, facing.y, facing.zi, facing.z),
                Instruction.Rotation._270 => (facing.x, facing.xi, facing.z, facing.zi, facing.yi, facing.y),
            };


            return (x: rotated.x, y: rotated.y, z: rotated.z);
        }

        internal class Coord
        {
            public (int x, int y, int z) position { get; private set; }
            public int id { get; private set; }
            public Coord(int id, int x, int y, int z)
            {
                this.id = id;
                this.position = (x: x, y: y, z: z);
            }

            public override string ToString()
            {
                return $"[{id}] x: {position.x}, y: {position.y}, z: {position.z}";
            }

            public override bool Equals(object? obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    Coord o = (Coord)obj;
                    return o.position.x == position.x && o.position.y == position.y && o.position.z == position.z;
                }
            }
        }

        internal class Signature
        {
            internal int from { get; private set; }
            internal int to { get; private set; }
            internal int length { get; private set; }

            public Signature(Coord from, Coord to)
            {
                length = Distance(from, to);
                this.from = from.id;
                this.to = to.id;
            }

        }

        internal class Universe
        {
            public List<(int x, int y, int z)> scanners { get; }

            public List<Coord> beacons { get; }
            public List<Signature> signatures { get; private set; }

            public Universe() : this((0, 0, 0))
            {
            }

            public Universe((int x, int y, int z) position)
            {
                signatures = new List<Signature>();
                beacons = new List<Coord>();
                scanners = new List<(int x, int y, int z)>();
                scanners.Add(position);
            }

            internal void CreateSignature()
            {
                signatures.Clear();
                for (int beacon1 = 0; beacon1 < beacons.Count; beacon1++)
                {
                    for (int beacon2 = beacon1 + 1; beacon2 < beacons.Count; beacon2++)
                    {
                        // Manhattan3D
                        signatures.Add(new Signature(beacons[beacon1], beacons[beacon2]));
                    }
                }
            }

            internal void AddBeacon(Coord newBeacon)
            {
                beacons.ForEach(beacon => signatures.Add(new Signature(beacon, newBeacon)));
                beacons.Add(newBeacon);

            }

            internal Coord GetBeacon(int from)
            {
                return beacons.Single(b => b.id == from);
            }

            internal void Add(Universe universe)
            {
                // For now we don't need to keep the signature
                beacons.AddRange(universe.beacons);
                scanners.AddRange(universe.scanners);
            }

            internal Universe ApplyInstruction(Instruction instruction)
            {
                Universe theNewUniverse = new Universe(instruction.position);
                foreach (var beacon in beacons)
                {
                    var transformed = TransformAndMove(instruction, beacon);
                    theNewUniverse.AddBeacon(transformed);
                }
                foreach (var scanner in scanners)
                {
                    var transformed = TransformAndMove(instruction, scanner);
                    theNewUniverse.scanners.Add(transformed);
                }

                return theNewUniverse;
            }


        }
    }
}
