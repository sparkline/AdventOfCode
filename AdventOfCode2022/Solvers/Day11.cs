using AdventOfCode2022.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solvers
{
    public class Day11 : Solver
    {
        public Day11() : base(11) { }


        protected override object PartA(string input)
        {
            var monkeyInput = input.Split("\n\n");
            List<Monkey> monkeys = monkeyInput.Select(input => new Monkey(input, true)).ToList();
            foreach (Monkey monkey in monkeys)
            {
                monkey.falseMonkey = monkeys[monkey.falseMonkeyIndex];
                monkey.trueMonkey = monkeys[monkey.trueMonkeyIndex];
            }

            for (int round = 1; round <= 20; round ++)
            {
                foreach (Monkey monkey in monkeys)
                {
                    monkey.act();
                }
            }

            int monkeyBusiness = monkeys.Select(monkey => monkey.inspections).OrderByDescending(i => i).Take(2).Aggregate(1, (total, next) => total * next);

            return monkeyBusiness;
        }
        protected override object PartB(string input)
        {
            var monkeyInput = input.Split("\n\n");
            List<Monkey> monkeys = monkeyInput.Select(input => new Monkey(input, false)).ToList();
            int commonDenominator = monkeys.Aggregate(1, (total, monkey) => total * monkey.divisibleTest);
            foreach (Monkey monkey in monkeys)
            {
                monkey.falseMonkey = monkeys[monkey.falseMonkeyIndex];
                monkey.trueMonkey = monkeys[monkey.trueMonkeyIndex];
                monkey.commonDenominator = commonDenominator;
            }

            for (int round = 1; round <= 10000; round++)
            {
                foreach (Monkey monkey in monkeys)
                {
                    monkey.act();
                }
            }

            long monkeyBusiness = monkeys.Select(monkey => monkey.inspections).OrderByDescending(i => i).Take(2).Aggregate(1L, (total, next) => total * next);

            return monkeyBusiness;
        }

    }

    internal class Operation
    {
        private Operator _operator;
        private Value lhs;
        private Value rhs;
        private int lhsNum;
        private int rhsNum;

        private enum Operator { MULTIPLY, ADD};
        private enum Value { NUM, OLD};

        public Operation(string input) 
        {
            var parts = input.Split();
            this._operator = parts[1] switch
            {
                "*" => Operator.MULTIPLY,
                "+" => Operator.ADD,
                _ => throw new NotImplementedException(),
            };
            switch (parts[0])
            {
                case "old":
                    this.lhs = Value.OLD;
                    break;
                default:
                    this.lhs = Value.NUM;
                    this.lhsNum = int.Parse(parts[0]);
                    break;
            }
            switch (parts[2])
            {
                case "old":
                    this.rhs = Value.OLD;
                    break;
                default:
                    this.rhs = Value.NUM;
                    this.rhsNum = int.Parse(parts[2]);
                    break;
            }
        }

        public long calc(long oldValue)
        {
            long _lhs = lhs switch
            {
                Value.OLD => oldValue,
                Value.NUM => lhsNum,
            };
            long _rhs = rhs switch
            {
                Value.OLD => oldValue,
                Value.NUM => rhsNum,
            };
            return _operator switch
            {
                Operator.ADD => _lhs + _rhs,
                Operator.MULTIPLY => _lhs * _rhs,
            };
        }
    }

    internal class Monkey
    {
        public Monkey(string input, bool worry)
        {
            var lines = input.SplitOnNewline();
            int.Parse(lines[0].Split()[1][..^1]);

            this.number = int.Parse(lines[0].Split()[1][..^1]);
            this.worries = new LinkedList<long>(lines[1].Split(":")[1].ToIntList(Extensions.SeparatorOption.Comma).Select(i => (long)i));
            this.operation = new Operation(lines[2].Split("=")[1].Trim());
            this.divisibleTest = int.Parse(lines[3].Split()[^1]);
            this.trueMonkeyIndex = int.Parse(lines[4].Split()[^1]);
            this.falseMonkeyIndex = int.Parse(lines[5].Split()[^1]);
            this.worry = worry;
        }

        private bool worry { get; }
        public int number { get; private set; }
        public LinkedList<long> worries { get; private set; }
        public Operation operation { get; private set; }
        public int divisibleTest { get; private set; }
        public int trueMonkeyIndex { get; private set; }
        public int falseMonkeyIndex { get; private set; }
        public Monkey trueMonkey { get; set; }
        public Monkey falseMonkey { get; set; }
        public int commonDenominator { get; set; }
        public int inspections { get; private set; }

        internal void act()
        {
            var currentWorry = worries.First;
            while (currentWorry != null)
            {
                var nextWorry = currentWorry.Next;
                inspections++;

                currentWorry.Value = operation.calc(currentWorry.Value);
                if (worry)
                {
                    currentWorry.Value = currentWorry.Value / 3;
                } else
                {
                    currentWorry.Value = currentWorry.Value % commonDenominator;
                }
                throwItem(currentWorry.Value);
                worries.Remove(currentWorry);

                currentWorry = nextWorry;
            }
        }

        private void throwItem(long value)
        {
            bool isDivisible = (value % divisibleTest) == 0;
            if (isDivisible)
            {
                trueMonkey.worries.AddLast(value);
            }
            else
            {
                falseMonkey.worries.AddLast(value);
            }
        }
    }

}
