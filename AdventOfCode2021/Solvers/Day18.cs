using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day18 : Solver
    {
        public Day18() : base(2021, 18) { }

        private class SnailNumber : LinkedList<(int depth, int value)>
        {

        }

        protected override object PartA(string input)
        {
            var lines = input.SplitOnNewline();


            List<SnailNumber> snailNumbers = lines.Select(line => GetSnailNumberFromText(line)).ToList();
            SnailNumber sum = snailNumbers.Aggregate((total, toAdd) => AddAndNormalize(total, toAdd));

            int magnitude = Magnitude(sum);

            return magnitude;
        }

        private int Magnitude(SnailNumber sum)
        {
            var item = sum.First;
            while (item != null)
            {
                // find the next number that is at the bottom (no nested number)
                if (item.Value.depth == item.Next?.Value.depth)
                {
                    int magnitude = 3 * item.Value.value + 2 * item.Next.Value.value;
                    int depth = item.Value.depth - 1;
                    item.Value = (depth: depth, value: magnitude);
                    sum.Remove(item.Next);
                    item = sum.First;
                }
                else
                {
                    item = item.Next;
                }
            }

            // We should be left with one node
            int totalMagnitude = sum.First?.Value.value ?? -1;
            return totalMagnitude;
        }

        private SnailNumber AddAndNormalize(SnailNumber total, SnailNumber toAdd)
        {
            SnailNumber result = Add(total, toAdd);
            Normalize(result);
            return result;
        }

        private void Normalize(SnailNumber result)
        {
            bool splitted = false;
            do
            {
                Reduce(result);
                splitted = Split(result);
            } while (splitted);
        }

        private bool Split(SnailNumber result)
        {
            var item = result.First;
            while (item != null)
            {
                if (item.Value.value >= 10)
                {
                    double value = (double)item.Value.value / 2;
                    int depth = item.Value.depth + 1;
                    item.Value = (depth: depth, value: (int)Math.Floor(value));
                    result.AddAfter(item, (depth: depth, value: (int)Math.Ceiling(value)));

                    // The rule says, first rule that applies, so only one split at a time
                    return true;
                }
                else
                {
                    item = item.Next;
                }
            }
            return false;
        }

        private void Reduce(SnailNumber result)
        {
            // Keep reducing
            var item = result.First;
            while (item != null)
            {
                if (item.Value.depth > 4)
                {
                    // lhs and rhs of the snailnumber
                    var LHS = item;
                    var RHS = item.Next;

                    //value to the left
                    if (LHS.Previous != null)
                    {
                        LHS.Previous.Value = (depth: LHS.Previous.Value.depth, value: LHS.Previous.Value.value + LHS.Value.value);
                    }

                    //value to the right
                    if (RHS.Next != null)
                    {
                        RHS.Next.Value = (depth: RHS.Next.Value.depth, value: RHS.Next.Value.value + RHS.Value.value);
                    }

                    // Remove one node, make the other zero
                    result.Remove(RHS);
                    LHS.Value = (depth: LHS.Value.depth - 1, value: 0);

                    // The rule says, first rule that applies, so start again
                    item = result.First;
                }
                else
                {
                    item = item.Next;
                }
            }
        }

        private static SnailNumber Add(SnailNumber LHS, SnailNumber RHS)
        {
            SnailNumber result = new SnailNumber();
            foreach (var item in LHS)
            {
                result.AddLast((depth: item.depth + 1, value: item.value));
            }
            foreach (var item in RHS)
            {
                result.AddLast((depth: item.depth + 1, value: item.value));
            }
            return result;
        }

        private SnailNumber GetSnailNumberFromText(string input)
        {
            SnailNumber result = new SnailNumber();
            string number = "";
            int depth = 0;
            for (int i = 0; i < input.Length; i++)
            {
                depth += input[i] switch
                {
                    '[' => 1,
                    ']' => -1,
                    ',' => 0,
                    _ => ((Func<int>)(() => { result.AddLast((depth: depth, value: input[i] - '0')); return 0; }))(),
                };
            }

            Normalize(result);
            return result;
        }

        protected override object PartB(string input)
        {
            var lines = input.SplitOnNewline();
            int maxnitude = 0;

            foreach (var line in lines)
            {
                foreach (var line2 in lines)
                {
                    if (line == line2)
                    {
                        continue;
                    }

                    int magnitude = Magnitude(AddAndNormalize(GetSnailNumberFromText(line), GetSnailNumberFromText(line2)));
                    maxnitude = Math.Max(maxnitude, magnitude);
                }
            }

            return maxnitude;
        }
    }

}
