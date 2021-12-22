using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day21 : Solver
    {
        public Day21() : base(2021, 21) { }

        /*
         * Given deterministic dice with d rolls per turn
         * With p players
         * The score at iteration i is:
         * d(ipd+pd+0.5(d+1))
         * 
         * 
         * 
         */

        protected override object PartA(string input)
        {
            var lines = input.SplitOnNewline();
            int playerOnePosition = int.Parse(lines[0].Split(":")[1]);
            int playerTwoPosition = int.Parse(lines[1].Split(":")[1]);

            int d = 3;
            int p = 2;
            int i = 0;
            int p1Score = 0;
            int p2Score = 0;
            int rolls = 0;
            while (p1Score < 1000 && p2Score < 1000)
            {
                int p1roll = 3 * (i * 6 + 2);
                rolls += 3;
                playerOnePosition = (playerOnePosition + p1roll - 1) % 10 + 1;
                p1Score += playerOnePosition;

                if (p1Score >= 1000)
                {
                    break;
                }

                rolls += 3;
                int p2roll = 3 * (i * 6 + 5);
                playerTwoPosition = (playerTwoPosition + p2roll - 1) % 10 + 1;
                p2Score += playerTwoPosition;

                i++;
            }

            int loser = Math.Min(p1Score, p2Score);
            return loser * rolls;
        }

        protected override object PartB(string input)
        {
            var lines = input.SplitOnNewline();
            int playerOnePosition = int.Parse(lines[0].Split(":")[1]);
            int playerTwoPosition = int.Parse(lines[1].Split(":")[1]);

            Dictionary<int, int> rollFreqs = Enumerable.Range(1, 3)
                .SelectMany(firstRoll => Enumerable.Range(1, 3), (firstDie, secondDie) => firstDie + secondDie)
                .SelectMany(firstTwoRolls => Enumerable.Range(1, 3), (firstTwoDie, thirdDie) => firstTwoDie + thirdDie)
                .GroupBy(totalRoll => totalRoll)
                .ToDictionary(x => x.Key, x => x.Count());

            long win1, win2;
            win1 = win2 = 0;

            Stack<(int p1Score, int p1Position, long p1Universes, int p2Score, int p2Position, long p2Universes, bool evenTurn)> playsLeft = new Stack<(int p1Score, int p1Position, long p1Universes, int p2Score, int p2Position, long p2Universes, bool evenTurn)>();
            playsLeft.Push((p1Score: 0, p1Position: playerOnePosition, p1Universes: 1l, p2Score: 0, p2Position: playerTwoPosition, p2Universes: 1l, evenTurn: true));

            while (playsLeft.Count > 0)
            {
                var play = playsLeft.Pop();
                foreach (var roll in rollFreqs)
                {
                    int dieValue = roll.Key;
                    int dieFreq = roll.Value;
                    int newScore, score, position, newPosition;
                    position = play.evenTurn ? play.p1Position : play.p2Position;
                    newPosition = (position + dieValue - 1) % 10 + 1;
                    score = play.evenTurn ? play.p1Score : play.p2Score;
                    newScore = score + newPosition;

                    if (newScore >= 21)
                    {
                        win1 += play.evenTurn ? play.p1Universes * dieFreq : 0;
                        win2 += play.evenTurn ? 0 : play.p2Universes * dieFreq;
                    }
                    else
                    {
                        if (play.evenTurn)
                        {
                            playsLeft.Push((p1Score: newScore, p1Position: newPosition, p1Universes: play.p1Universes * dieFreq,
                                p2Score: play.p2Score, p2Position: play.p2Position, p2Universes: play.p2Universes * dieFreq,
                                evenTurn: !play.evenTurn));
                        }
                        else
                        {
                            playsLeft.Push((p1Score: play.p1Score, p1Position: play.p1Position, p1Universes: play.p1Universes * dieFreq,
                                p2Score: newScore, p2Position: newPosition, p2Universes: play.p2Universes * dieFreq,
                                evenTurn: !play.evenTurn));
                        }

                    }
                }

            }

            return Math.Max(win1, win2);
        }
    }
}
