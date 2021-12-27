using AdventOfCode2021.Common;
using System.Text;
using static AdventOfCode2021.Solvers.Day23;

namespace AdventOfCode2021.Solvers
{
    public class Day23_1 : Solver
    {
        private const int CHAMBER_COUNT = 4;
        private const int HALLWAY_SIZE = CHAMBER_COUNT + 3;

        public Day23_1() : base(2021, 23, 1, "Faster") { }

        readonly int[,] stepsFromHallwayToChamber = new int[CHAMBER_COUNT, HALLWAY_SIZE] {
                { 3, 2, 2, 4, 6, 8, 9}, // Room 0
                { 5, 4, 2, 2, 4, 6, 7 }, // Room 1
                { 7, 6, 4, 2, 2, 4, 5 },
                { 9, 8, 6, 4, 2, 2, 3 } };

        protected override object PartA(string input)
        {
            AmphiPod[,] inputChambers = new AmphiPod[CHAMBER_COUNT, 2];
            inputChambers = ReadAmphipods(input);

            return CalculateIt(inputChambers);
        }

        private object CalculateIt(AmphiPod[,] inputChambers)
        {
            StartSW();

            // Constant cost for the give startpositions
            int cost = GetOffset(inputChambers);

            AmphiPod[][] toBeProcessed = RemoveBottom(inputChambers);
            // Queue, breadth first
            PriorityQueue<State, int> movesLeft = new PriorityQueue<State, int>();
            Move signature = new Move(
                new AmphiPod[]{
                AmphiPod.Empty, AmphiPod.Empty, AmphiPod.Empty, AmphiPod.Empty, AmphiPod.Empty, AmphiPod.Empty, AmphiPod.Empty, // hallway
                GetTop(0, toBeProcessed), GetTop(1, toBeProcessed), GetTop(2, toBeProcessed), GetTop(3, toBeProcessed),         // available
            }, cost);
            toBeProcessed = PopTop(toBeProcessed);
            movesLeft.Enqueue(new State(
                toBeProcessed, signature),0);

            Dictionary<ulong, List<Move>> cache = new Dictionary<ulong, List<Move>>();
            Dictionary<ulong, int> cheapestPathToState = new Dictionary<ulong, int> {[GetKey(signature.amphiPods)] = 0 };

            while (movesLeft.TryDequeue(out State state, out int priority))
            {
                List<State> moves;
                if (cache.TryGetValue(GetKey(state.signature.amphiPods), out var cachedMoves))
                {
                    moves = ConvertCachedMoves(state, cachedMoves);
                }
                else
                {
                    var deltaMoves = new List<Move>();

                    // Calculate all the moves allowed for given state
                    for (int chamber = 0; chamber < CHAMBER_COUNT; chamber++)
                    {
                        // ready for receiving?
                        if (state.signature.amphiPods[chamber + HALLWAY_SIZE] != AmphiPod.Empty)
                        {
                            // Let's see where we can reallocate this Amphipod
                            // look to the left
                            for (int positionInHallway = chamber + 1; positionInHallway >= 0; positionInHallway--)
                            {
                                bool stop = TryKickAmphiPodOut(chamber, positionInHallway, state, ref deltaMoves);
                                if (stop)
                                {
                                    break;
                                }
                            }
                            // look to the right
                            for (int positionInHallway = chamber + 2; positionInHallway < HALLWAY_SIZE; positionInHallway++)
                            {
                                bool stop = TryKickAmphiPodOut(chamber, positionInHallway, state, ref deltaMoves);
                                if (stop)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // look to the left
                            for (int positionInHallway = chamber + 1; positionInHallway >= 0; positionInHallway--)
                            {
                                bool stop = TryBringAmphiPodHome(chamber, positionInHallway, state, ref deltaMoves);
                                if (stop)
                                {
                                    break;
                                }
                            }
                            // look to the right
                            for (int positionInHallway = chamber + 2; positionInHallway < HALLWAY_SIZE; positionInHallway++)
                            {
                                bool stop = TryBringAmphiPodHome(chamber, positionInHallway, state, ref deltaMoves);
                                if (stop)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    //               Console.WriteLine();
                    //               Console.WriteLine($"Starting state {iter}");
                    //               Console.WriteLine();
                    //               Console.WriteLine(state);

                    cache.Add(GetKey(state.signature.amphiPods), deltaMoves);
                    moves = ConvertCachedMoves(state, deltaMoves);
                }

                // Moves for next loop
                // Add the moves if they have a score lower than minprice
                foreach (State move in moves)
                {
                    ulong stateKey = GetKey(state);
                    ulong moveKey = GetKey(move);

                    //                   Console.WriteLine();
                    //                   Console.WriteLine();
                    //                   Console.WriteLine("Possible move");
                    //                   Console.WriteLine();
                    //                   Console.WriteLine(move);

                    if (!cheapestPathToState.TryGetValue(moveKey, out int newPrice))
                    {
                        newPrice = int.MaxValue;
                    }

                    if (move.signature.price < newPrice)
                    {
                        // we found a cheaper way to get to this world - keep exploring
                        newPrice = move.signature.price;
                        cheapestPathToState[moveKey] = newPrice;
                        movesLeft.Enqueue(move, newPrice);
                    }

                    /*
                    if (move.signature.price <= minPrice)
                    {
                        if (GameFinished(move))
                        {
                            minPrice = move.signature.price;
                        }
                        else
                        {
                            movesLeft.Push(move);
                        }
                    }
                    */
                }

            }
            AmphiPod[][] emptyChambers = new AmphiPod[4][] { new AmphiPod[0], new AmphiPod[0], new AmphiPod[0], new AmphiPod[0] };
            Move emptyMove = new Move(Enumerable.Repeat(AmphiPod.Empty, HALLWAY_SIZE + CHAMBER_COUNT).ToArray(), 0);
            ulong finishKey = GetKey(new State(emptyChambers, emptyMove));

            int minPrice = cheapestPathToState[finishKey];

            return minPrice;
        }

        private ulong GetKey(State state)
        {
            ulong hash = 23;
            foreach (var amphiPod in state.signature.amphiPods)
            {
                hash = unchecked(31 * hash + (ulong)amphiPod);
            }
            foreach (var chamber in state.toBeProcessed)
            {
                for (int i = 0; i<3; i++) // max 3 deep per chamber
                {
                    if (chamber.Length > i)
                    {
                        hash = unchecked(31 * hash + (ulong)chamber[i]);
                    } 
                    else
                    {
                        hash = unchecked(31 * hash);
                    }
                }
            }
            return hash;
        }  

        private ulong GetKey(AmphiPod[] amphiPods)
        {
            ulong result = 0;
            foreach (var amphiPod in amphiPods)
            {
                result += (ulong)amphiPod;
                result = result << 3;
            }
            return result;
        }

        private List<State> ConvertCachedMoves(State state, List<Move> cachedMoves)
        {
            List<State> result = new List<State>();

            foreach (Move move in cachedMoves)
            {
                var newSignature = QuickClone(move.amphiPods);
                int newPrice = state.signature.price + move.price;
                var newToBeProcessed = state.toBeProcessed;

                for (int chamber = 0; chamber < CHAMBER_COUNT; chamber++)
                {
                    if (state.signature.amphiPods[chamber + HALLWAY_SIZE] != move.amphiPods[chamber + HALLWAY_SIZE])
                    {
                        // assertion, the only way this happens is if a amphipod got kicked out
                        newSignature[chamber + HALLWAY_SIZE] = GetTop(chamber, state.toBeProcessed);
                        newToBeProcessed = PopTop(state.toBeProcessed, chamber);
                        break; // found the single escaping amphipod
                    }
                }

                result.Add(new State(newToBeProcessed, new Move(newSignature, newPrice)));
            }

            return result;
        }

        private bool GameFinished(State state)
        {
            for (int amphipod = 0; amphipod < state.signature.amphiPods.Length; amphipod++)
            {
                if (state.signature.amphiPods[amphipod] != AmphiPod.Empty)
                {
                    return false;
                }
            }
            return true;
        }

        private bool TryKickAmphiPodOut(int chamber, int positionInHallway, State state, ref List<Move> moves)
        {
            AmphiPod amphiPodInChamber = state.signature.amphiPods[chamber + HALLWAY_SIZE];
            bool hallwayIsEmpty = state.signature.amphiPods[positionInHallway] == AmphiPod.Empty;
            if (hallwayIsEmpty)
            {
                // Add move
                AmphiPod[] newAmphipods = QuickClone(state.signature.amphiPods);
                newAmphipods[positionInHallway] = amphiPodInChamber;
                //AmphiPod newAmphipod = GetTop(chamber, state.toBeProcessed);
                // AmphiPod[][] newToBeProcessed = PopTop(state.toBeProcessed, chamber);
                newAmphipods[chamber + HALLWAY_SIZE] = AmphiPod.Empty; // gets picked up by cache
                int steps = stepsFromHallwayToChamber[chamber, positionInHallway];
                int price = steps * Score(amphiPodInChamber);
                //int totalPrice = price + state.signature.price;
                var move = new Move(newAmphipods, price);
                moves.Add(move);

                return false;
            }
            else
            {
                return true;
            }
        }

        private AmphiPod[][] PopTop(AmphiPod[][] toBeProcessed, int chamber = -1)
        {
            AmphiPod[][] result = new AmphiPod[toBeProcessed.Length][];
            for (int i = 0; i < toBeProcessed.Length; i++)
            {
                if (i == chamber || chamber == -1)
                {
                    if (toBeProcessed[i].Length >= 2)
                    {
                        result[i] = toBeProcessed[i][1..];
                    }
                    else
                    {
                        result[i] = Array.Empty<AmphiPod>();
                    }

                }
                else
                {
                    result[i] = toBeProcessed[i];
                }
            }
            return result;
        }

        private bool TryBringAmphiPodHome(int chamber, int positionInHallway, State state, ref List<Move> moves)
        {
            AmphiPod amphiPodBelongingToChamber = AmphiPodForChamber(chamber);
            AmphiPod amphipodInHallway = state.signature.amphiPods[positionInHallway];
            if (amphipodInHallway == amphiPodBelongingToChamber)
            {
                // Add move
                AmphiPod[] newAmphipods = QuickClone(state.signature.amphiPods);
                newAmphipods[positionInHallway] = AmphiPod.Empty;
                newAmphipods[HALLWAY_SIZE + chamber] = AmphiPod.Empty; // Bleep bloop, black hole
                int steps = stepsFromHallwayToChamber[chamber, positionInHallway];
                int price = steps * Score(amphiPodBelongingToChamber);
                // int totalPrice = price + state.signature.price;
                var move = new Move(newAmphipods, price);
                moves.Add(move);
                return true;
            }
            else if (amphipodInHallway != AmphiPod.Empty)
            {
                // Something is blocking
                return true;
            }
            return false;
        }

        private AmphiPod[] QuickClone(AmphiPod[] hallway)
        {
            AmphiPod[] result = new AmphiPod[hallway.Length];
            int size = sizeof(AmphiPod);
            int length = size * hallway.Length;
            System.Buffer.BlockCopy(hallway, 0, result, 0, length);
            return result;
        }

        struct State
        {
            public AmphiPod[][] toBeProcessed;
            public Move signature;

            public State(AmphiPod[][] toBeProcessed, Move signature)
            {
                this.toBeProcessed = toBeProcessed;
                this.signature = signature;
            }

            public override string ToString()
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(signature.ToString());
                sb.Append("  #");
                for (int i = 0; i < toBeProcessed.Length; i++)
                {
                    sb.Append("-#");
                }
                sb.AppendLine();
                int maxDepth = toBeProcessed.Max(x => x.Length);
                for (int depth = 0; depth < maxDepth; depth++)
                {
                    sb.Append("  #");
                    for (int chamber = 0; chamber < toBeProcessed.Length; chamber++)
                    {
                        if (depth < toBeProcessed[chamber].Length)
                        {
                            sb.Append(toBeProcessed[chamber][depth].ShortString());
                        }
                        else
                        {
                            sb.Append(' ');
                        }
                        sb.Append('#');
                    }
                    sb.AppendLine();
                }
                sb.Append("  ");
                sb.Append(new string('#', CHAMBER_COUNT * 2 + 1));
                return sb.ToString();
            }
            /*
            public AmphiPod[] hallway { get { return signature.amphiPods[0..HALLWAY_SIZE]; } }
            public AmphiPod[] chambers { get { return signature.amphiPods[HALLWAY_SIZE..^0]; } }
            */
        }

        struct Signature
        {
            public AmphiPod[] amphiPods;

            public Signature(AmphiPod[] amphiPods)
            {
                this.amphiPods = amphiPods;
            }
            /*
            public override int GetHashCode()
            {
                return string.Join(amphiPods.Select(a => a.ShortString()))
            }
            
            public override bool Equals(object? obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    Signature p = (Signature)obj;
                    if (p.amphiPods.Length != amphiPods.Length)
                    {
                        return false;
                    } else
                    {
                        return Enumerable.SequenceEqual(amphiPods, p.amphiPods);
                    }
                }

            }
            */
        }

        struct Move
        {
            //            public Signature amphiPods;
            public AmphiPod[] amphiPods;

            public int price;

            public Move(AmphiPod[] amphiPods, int price)
            {
                this.amphiPods = amphiPods;
                this.price = price;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Price: {price}");
                sb.AppendLine(new string('#', HALLWAY_SIZE * 2 - 1));
                sb.Append('#');
                for (int i = 0; i < HALLWAY_SIZE; i++)
                {
                    sb.Append(amphiPods[i].ShortString());
                    if (i > 0 && i < HALLWAY_SIZE - 2)
                    {
                        sb.Append('.');
                    }
                }
                sb.AppendLine("#");
                sb.Append("###");
                for (int i = 0; i < CHAMBER_COUNT; i++)
                {
                    sb.Append(amphiPods[HALLWAY_SIZE + i].ShortString());
                    sb.Append('#');
                }
                sb.Append("##");

                return sb.ToString();
            }

        }

        private AmphiPod GetTop(int chamber, AmphiPod[][] toBeProcessed)
        {
            return (toBeProcessed[chamber].Length == 0) ? AmphiPod.Empty : toBeProcessed[chamber][0];
        }

        private AmphiPod[][] RemoveBottom(AmphiPod[,] chambers)
        {
            var result = new AmphiPod[chambers.GetLength(0)][];

            for (int chamber = 0; chamber < chambers.GetLength(0); chamber++)
            {
                List<AmphiPod> amphiPods = new List<AmphiPod>();
                AmphiPod amphiPodForChamber = AmphiPodForChamber(chamber);
                for (int depth = chambers.GetLength(1) - 1; depth >= 0; depth--)
                {
                    AmphiPod amphiPodInChamber = chambers[chamber, depth];
                    if (amphiPodForChamber != amphiPodInChamber)
                    {
                        for (int j = 0; j <= depth; j++)
                        {
                            amphiPods.Add(chambers[chamber, j]);
                        }
                        break;
                    }
                }
                result[chamber] = amphiPods.ToArray();
            }
            return result;
        }

        /// <summary>
        /// All the steps needed except for the first step _in_ the room for Amiphods that belong there.
        /// </summary>
        /// <param name="chambers"></param>
        /// <returns></returns>
        private int GetOffset(AmphiPod[,] chambers)
        {
            int points = 0;
            for (int chamber = 0; chamber < chambers.GetLength(0); chamber++)
            {
                AmphiPod amphiPodForChamber = AmphiPodForChamber(chamber);
                //Console.WriteLine($"Chamber {chamber}, expecting {amphiPodForChamber}");

                // Cost for all Amphipods moving in
                points += (int)(0.5 * (chambers.GetLength(1) - 1) * chambers.GetLength(1)) * Score(amphiPodForChamber);

                // look from the bottom up
                for (int steps = chambers.GetLength(1) - 1; steps >= 0; steps--)
                {
                    AmphiPod amphiPodInChamber = chambers[chamber, steps];
                    if (amphiPodInChamber == amphiPodForChamber)
                    {
                        // Already in the right place, subtract from total cost
                        points -= steps * Score(amphiPodInChamber);
                        //Console.WriteLine($"Ok {amphiPodInChamber} at depth {steps}");
                    }
                    else
                    {
                        // Cost for all the Amphipods in the wrong room to move out
                        for (int step2 = 0; step2 <= steps; step2++)
                        {
                            amphiPodInChamber = chambers[chamber, step2];
                            points += step2 * Score(amphiPodInChamber);
                            //Console.WriteLine($"Get out! {amphiPodInChamber} at depth {step2}");

                        }
                        break;
                    }
                }
            }

            return points;
        }

        private AmphiPod AmphiPodForChamber(int chamber)
        {
            return (AmphiPod)(chamber + 1);
        }

        private int ChamberForAmphiPod(AmphiPod a)
        {
            return (int)a - 1;
        }

        private int Score(AmphiPod a)
        {
            // return (int)Math.Pow(10, (int)a - 1);

            return a switch
            {
                AmphiPod.Amber => 1,
                AmphiPod.Bronze => 10,
                AmphiPod.Copper => 100,
                AmphiPod.Desert => 1000,
                AmphiPod.Empty => 0,
            };
        }

        private AmphiPod[,] ReadAmphipods(string input, bool folded = false)
        {
            AmphiPod[,] extra = new AmphiPod[2, 4] {
                {AmphiPod.Desert,AmphiPod.Copper,AmphiPod.Bronze,AmphiPod.Amber },
                {AmphiPod.Desert,AmphiPod.Bronze,AmphiPod.Amber,AmphiPod.Copper } };

            AmphiPod[,] chambers = new AmphiPod[CHAMBER_COUNT, folded ? 4 : 2];
            var lines = input.SplitOnNewline();
            for (int x = 0; x < chambers.GetLength(0); x++)
            {
                for (int line = 0; line < 2; line++)
                {
                    int y = folded ? line * 3 : line;
                    chambers[x, y] = lines[line + 2][3 + x * 2] switch
                    {
                        'A' => AmphiPod.Amber,
                        'B' => AmphiPod.Bronze,
                        'C' => AmphiPod.Copper,
                        'D' => AmphiPod.Desert,
                        _ => AmphiPod.Empty,
                    };

                    if (folded && line == 0)
                    {
                        // Add additional lines
                        chambers[x, 1] = extra[0, x];
                        chambers[x, 2] = extra[1, x];
                    }
                }
            }

            return chambers;
        }

        protected override object PartB(string input)
        {
            AmphiPod[,] inputChambers = new AmphiPod[CHAMBER_COUNT, 4];
            inputChambers = ReadAmphipods(input, true);

            return CalculateIt(inputChambers);
        }
    }
}
