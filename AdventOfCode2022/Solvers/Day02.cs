using AdventOfCode2022.Common;
using System;

namespace AdventOfCode2022.Solvers
{
    public class Day02 : Solver
    {
        public Day02() : base(2) { }

        private RoPaSc opponentOracle(string input)
        {
            return input switch
            {
                "A" => RoPaSc.Rock,
                "B" => RoPaSc.Paper,
                "C" => RoPaSc.Scissors,
                _ => throw new NotImplementedException()
            };
        }

        private RoPaSc myOracle(string input)
        {
            return input switch
            {
                "X" => RoPaSc.Rock,
                "Y" => RoPaSc.Paper,
                "Z" => RoPaSc.Scissors,
                _ => throw new NotImplementedException()
            };
        }

        private RoPaSc outcomeOracle(RoPaSc opponent, string input)
        {
            int opponentValue = (int)opponent;
            int outcome = input switch
            {
                "X" => 2,
                "Y" => 3,
                "Z" => 4,
                _ => throw new NotImplementedException()
            };

            int me = (opponentValue + outcome) % 3;
            return (RoPaSc)me;
        }

        private int gameScore(RoPaSc me, RoPaSc opponent)
        {
            int winningBonus = CompareRPC(me, opponent) switch
            {
                -1 => 0,
                0 => 3,
                1 => 6,
                _ => throw new NotImplementedException()
            };
            int pieceBonus = me switch
            {
                RoPaSc.Rock => 1,
                RoPaSc.Paper => 2,
                RoPaSc.Scissors => 3,
            };

            return winningBonus + pieceBonus;
        }

        protected override object PartA(string input)
        {
            var pairs = input.SplitOnNewline();
            List<(RoPaSc opponent, RoPaSc me)> games = new List<(RoPaSc left, RoPaSc right)>();
            foreach (var pair in pairs)
            {
                var opponent = pair.Split(' ')[0];
                var me = pair.Split(' ')[1];
                games.Add((opponentOracle(opponent), myOracle(me)));
            }
            int totalScore = games.Sum(p => gameScore(p.me, p.opponent));

            return totalScore;
        }

        protected override object PartB(string input)
        {
            var pairs = input.SplitOnNewline();
            List<(RoPaSc opponent, RoPaSc me)> games = new List<(RoPaSc left, RoPaSc right)>();
            foreach (var pair in pairs)
            {
                var opponent = opponentOracle(pair.Split(' ')[0]);
                var outcome = pair.Split(' ')[1];
                games.Add((opponent, outcomeOracle(opponent, outcome)));
            }

            int totalScore = games.Sum(p => gameScore(p.me, p.opponent));

            return totalScore;
        }

        public enum RoPaSc { Rock = 0, Paper = 1, Scissors = 2 };
        public int CompareRPC(RoPaSc left, RoPaSc right)
        {
            int diff = left - right;
            if (Math.Abs(diff) > 1)
            {
                diff = -Math.Sign(diff);
            }
            return diff;
        }
    }


    
    
}
