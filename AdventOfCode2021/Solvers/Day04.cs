using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day04 : Solver
    {
        public Day04() : base(2021, 4) { }

        protected override object PartA(string input)
        {
            List<string> lines = input.SplitOnNewline();
            List<int> bingoNumbers = readBingoNumbers(lines.First());
            var bingoBoards = readBingoBoards(lines.Skip(1).ToList());

            foreach (var number in bingoNumbers)
            {
                foreach (var board in bingoBoards)
                {
                    if (board.markNumber(number))
                    {
                        int lastNumber = number;
                        int sumOfUnmarked = board.SumOfUnmarked();
                        return lastNumber * sumOfUnmarked;
                    }
                }
            }
            return 0;
        }

        protected override object PartB(string input)
        {
            List<string> lines = input.SplitOnNewline();
            List<int> bingoNumbers = readBingoNumbers(lines.First());
            var bingoBoards = readBingoBoards(lines.Skip(1).ToList());

            foreach (var number in bingoNumbers)
            {
                foreach (var board in bingoBoards)
                {
                    if (board.markNumber(number))
                    {
                        bool allBingos = bingoBoards.Count(b => b.bingo) == bingoBoards.Count();
                        if (allBingos)
                        {
                            int lastNumber = number;
                            int sumOfUnmarked = board.SumOfUnmarked();
                            return lastNumber * sumOfUnmarked;
                        }
                    }
                }
            }
            return 0;
        }

        private List<BingoBoard> readBingoBoards(List<string> lines)
        {
            List<BingoBoard> boards = lines.Select((line, index) => new { Index = index, Line = line })
                .GroupBy(c => c.Index / BingoBoard.BOARD_SIZE)
                .Select(c => c.Select(a => a.Line).ToList())
                .Select(l => new BingoBoard(l))
                .ToList();

            return boards;
        }

        private List<int> readBingoNumbers(string input)
        {
            List<int> bingoNumbers = input.ToIntList(Extensions.SeparatorOption.Comma);
            return bingoNumbers;
        }

        private class BingoBoard
        {
            public const int BOARD_SIZE = 5;
            private BingoNumber[,] BingoNumbers = new BingoNumber[BOARD_SIZE, BOARD_SIZE];
            public bool bingo { get; private set; } = false;

            public BingoBoard(List<string> nextBoard)
            {
                var numbers = nextBoard.Select(x => x.ToIntList(Extensions.SeparatorOption.Spaces)).ToList();

                for (int y = 0; y < BOARD_SIZE; y++)
                {
                    for (int x = 0; x < BOARD_SIZE; x++)
                    {
                        BingoNumbers[y, x] = new BingoNumber(numbers[y][x]);
                    }
                }
            }

            internal bool markNumber(int number)
            {
                bool bingo = false;
                for (int y = 0; y < BOARD_SIZE; y++)
                {
                    for (int x = 0; x < BOARD_SIZE; x++)
                    {
                        BingoNumber bingoNumber = BingoNumbers[y, x];
                        if (bingoNumber.value == number)
                        {
                            BingoNumbers[y, x].marked = true;
                            bingo |= checkForBingo(x, y);
                        }
                    }
                }
                return bingo;
            }

            internal int SumOfUnmarked()
            {
                int sum = 0;
                for (int y = 0; y < BOARD_SIZE; y++)
                {
                    for (int x = 0; x < BOARD_SIZE; x++)
                    {
                        BingoNumber bingoNumber = BingoNumbers[y, x];
                        if (!bingoNumber.marked)
                        {
                            sum += bingoNumber.value;
                        }
                    }
                }
                return sum;
            }

            private bool checkForBingo(int x, int y)
            {
                bool verticalBingo = true;
                bool horizontalBingo = true;
                for (int y2 = 0; y2 < BOARD_SIZE; y2++)
                {
                    verticalBingo &= BingoNumbers[y2, x].marked;
                }
                for (int x2 = 0; x2 < BOARD_SIZE; x2++)
                {
                    horizontalBingo &= BingoNumbers[y, x2].marked;
                }
                bool bingo = verticalBingo || horizontalBingo;
                this.bingo |= bingo;
                return bingo;
            }

            public class BingoNumber
            {
                public int value;
                public bool marked;

                public BingoNumber(int value)
                {
                    this.marked = false;
                    this.value = value;
                }

            }
        }
    }
}
