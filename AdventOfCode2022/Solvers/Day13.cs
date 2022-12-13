using AdventOfCode2022.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solvers
{
    public class Day13 : Solver
    {
        public Day13() : base(13) { }


        protected override object PartA(string input)
        {
            var pairs = input.Split("\n\n");
            int index = 1;
            int score = 0;
            foreach (var pair in pairs)
            {
                string lhs = pair.SplitOnNewline()[0];
                string rhs = pair.SplitOnNewline()[1];

                var lhsPacket = Packet.List(ref lhs, 1, out int lhs_end);
                var rhsPacket = Packet.List(ref rhs, 1, out int rhs_end);

                if (lhsPacket.CompareTo(rhsPacket) < 0)
                {
                    score += index;
                }

                index++;
            }

            return score;
        }
        protected override object PartB(string input)
        {
            var pair = input.SplitOnNewline();
            List<Packet> packets = pair.Select(input => Packet.List(ref input, 1, out int i)).ToList();
            string[] dividers = new string[] { "[[2]]", "[[6]]" };

            foreach (string divider in dividers)
            {
                packets.Add(Packet.Divider(divider));
            }

            packets.Sort();

            int score = 1;
            foreach (string divider in dividers)
            {
                score *= (packets.FindIndex(p => divider.Equals(p.identifier)) + 1);
            }

            return score;
        }

        private class Packet : IComparable
        {
            private int number;
            private bool isNumber;
            private List<Packet> children = new List<Packet>();
            public string identifier { get; private set; }

            public Packet(int number)
            {
                this.number = number;
                this.isNumber = true;
            }

            public Packet()
            {
                this.isNumber = false;
            }

            public static Packet Number(ref string input, int from, out int to)
            {
                int pointer = from;
                while (Char.IsDigit(input,pointer) && pointer < input.Length) { 
                    pointer++; 
                }
                to = pointer - 1; // pointing to last value that is a int
                return new Packet(int.Parse(input[from..(to+1)]));
            }

            public static Packet List(ref string input, int from, out int to)
            {
                Packet packet = new Packet();
                int pointer = from;
                while (pointer < input.Length)
                {
                    switch (input[pointer])
                    {
                        case '[':
                            packet.children.Add(Packet.List(ref input, pointer + 1, out pointer));
                            break;
                        case ']':
                            to = pointer;
                            return packet;
                        case ',':
                            break;
                        case char c when c >= '0' && c <= '9':
                            packet.children.Add(Packet.Number(ref input, pointer, out pointer));
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    pointer++;
                }

                throw new Exception("No closing parenthesis");
            }

            /// <summary>
            /// Negative = this < that
            /// </summary>
            /// <param name="that"></param>
            /// <returns></returns>
            public int CompareTo(object? obj)
            {
                if (obj is Packet that)
                {

                    if (this.isNumber && that.isNumber)
                    {
                        return this.number - that.number;
                    }
                    if (this.isNumber)
                    {
                        this.promoteToList();
                    }
                    if (that.isNumber)
                    {
                        that.promoteToList();
                    }
                    for (int i = 0; i < this.children.Count || i < that.children.Count; i++)
                    {
                        if (i == this.children.Count) {
                            return -1;
                        }
                        if (i == that.children.Count)
                        {
                            return 1;
                        }
                        int compare = this.children[i].CompareTo(that.children[i]);
                        if (compare != 0)
                        {
                            return compare;
                        }
                    }

                    return 0;
                }

                return 0;
            }

            private void promoteToList()
            {
                children.Add(new Packet(number));
                this.isNumber = false;
            }

            internal static Packet Divider(string input)
            {
                Packet packet = Packet.List(ref input, 1, out int i);
                packet.identifier = input;
                return packet;
            }
        }

    }
    
}
