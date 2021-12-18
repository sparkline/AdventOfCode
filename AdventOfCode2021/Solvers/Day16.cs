using AdventOfCode2021.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Solvers
{
    public class Day16 : Solver
    {
        public Day16() : base(2021, 16) { }

        protected override object PartA(string input)
        {
            var bitString = input.SplitOnNewline().First().Hex2Bin();

            var packet = new Packet(ref bitString);

            return packet.VersionSum();
        }

        protected override object PartB(string input)
        {
            var bitString = input.SplitOnNewline().First().Hex2Bin();

            var packet = new Packet(ref bitString);

            return packet.GetValue();
        }

        /*
         * Description in EBF
         * 
         * Packet                   = Version,TypedPackage
         * Version                  = bit,bit,bit
         * bit                      = '0','1'
         * TypedPacket              = LiteralPacket | OperatorPacket
         * 
         * LiteralPacket            = 4,LiteralValue
         * LiteralValue             = {Value}*,TerminatingValue,Padding
         * Value                    = '1',Number
         * TerminatingValue         = '0',Number
         * 
         * OperatorPacket           = OperatorIndicator, OperatorLength, Packet*
         * OperatorIndicator        = 0,1,2,3,5,6,7,8,9,A,B,C,D,E,F
         * OperatorLength           = PacketLength | PacketCount
         * PacketLength             = '0', bit,bit,bit,bit,bit,bit,bit,bit,bit,bit,bit,bit,bit,bit,bit (* 15 bits *) 
         * PacketCount              = '1', bit,bit,bit,bit,bit,bit,bit,bit,bit,bit,bit (* 11 bits *)
         * 
         * Padding                  = ['0']['0']['0']
         * 
         * Number                   = 0 | ... | 4 | ... | F
         * 0                        = '0','0','0','0'
         * ...
         * 4                        = '0','1','0','0'
         * ...
         * F                        = '1','1','1','1'
         * 
         */
        class Packet
        {
            private int version { get; }
            private PacketType packetType { get; }

            private List<Packet> children = new List<Packet>();
            private long value = 0;

            public Packet(ref string binaryString)
            {
                // Yep we are gonna do everything in the constructor
                this.version = ReadNumber(ref binaryString, 3);
                this.packetType = ReadPacketType(ref binaryString);
                switch (packetType)
                {
                    case PacketType.Literal:
                        ReadLiteral(ref binaryString);
                        break;
                    default:
                        ReadOperator(ref binaryString);
                        break;
                }
            }

            public int VersionSum()
            {
                return version + children.Sum(child => child.VersionSum());
            }

            public long GetValue()
            {
                return value;
            }


            private void ReadOperator(ref string binaryString)
            {
                // Read parameters
                int lengthType = ReadNumber(ref binaryString, 1);
                switch (lengthType)
                {
                    case 0:
                        int packetLength = ReadNumber(ref binaryString, 15);
                        string packetString = binaryString[..packetLength];
                        binaryString = binaryString[packetLength..];
                        while (packetString.Length > 0)
                        {
                            Packet child = new Packet(ref packetString);
                            children.Add(child);
                        }
                        break;
                    case 1:
                        int packetCount = ReadNumber(ref binaryString, 11);
                        for(int i = 0; i < packetCount; i++)
                        {
                            Packet child = new Packet(ref binaryString);
                            children.Add(child);
                        }
                        break;
                }

                // Calculate value
                var childValues = children.Select(child => child.value);
                this.value = this.packetType switch
                {
                    PacketType.Sum => childValues.Sum(),
                    PacketType.Product => childValues.Aggregate((product, childValue) => product * childValue),
                    PacketType.Minimum => childValues.Min(),
                    PacketType.Maximum => childValues.Max(),
                    PacketType.GT => childValues.First() > childValues.Last() ? 1 : 0,
                    PacketType.LT => childValues.First() < childValues.Last() ? 1 : 0,
                    PacketType.EQ => childValues.First() == childValues.Last() ? 1 : 0,
                    _ => throw new NotSupportedException()
                };
            }

            private void ReadLiteral(ref string binaryString)
            {
                StringBuilder numberString = new StringBuilder();
                bool lastPacket;
                do
                {
                    lastPacket = (binaryString[0] == '0');
                    numberString.Append(binaryString[1..5]);
                    binaryString = binaryString[5..];
                } while (!lastPacket);

                long number = Convert.ToInt64(numberString.ToString(), 2);
                this.value = number;
            }

            private PacketType ReadPacketType(ref string binaryString)
            {
                int number = ReadNumber(ref binaryString, 3);
                return Enum.Parse<PacketType>(number.ToString());
            }

            private int ReadNumber(ref string binaryString, int bits)
            {
                string numberString = binaryString[..bits];
                int result = Convert.ToInt32(numberString, 2);
                binaryString = binaryString[bits..];
                return result;
            }

            enum PacketType
            {
                Sum = 0,
                Product = 1,
                Minimum = 2,
                Maximum = 3,
                Literal = 4,
                GT = 5,
                LT = 6,
                EQ = 7,
            }
        }
    }
}
