using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day15_1 : Solver
    {
        public Day15_1() : base(2021, 15, 1, "Thought about it again, wrongly, since we only move right-down") { }

        /// <summary>
        /// The idea, we propagate a front from the top-left to the bottom-right
        /// N = size of input board
        /// Memory usage O(N^2) - N*(N+1)
        /// Iterations O(N) - 2*N - 3
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override object PartA(string input)
        {
            // Load data
            var lines = input.SplitOnNewline();
            int N = lines.Count();
            int[,] grid = new int[N, N];

            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    int value = lines[y][x] - '0';
                    grid[x, y] = value;
                }
            }

            int[] pathLength = Enumerable.Repeat(int.MaxValue, N + 1).ToArray();
            pathLength[1] = 0; // first step is free

            for (int depth = 1; depth < N * 2 - 1; depth++)
            {
                for (int x = 0; x <= depth && x < N; x++)
                {
                    int y = depth - x;
                    if (y < 0 || y >= N)
                    {
                        continue;
                    }

                    // Find cheapest path
                    pathLength[y + 1] = Math.Min(pathLength[y], pathLength[y + 1]) + grid[x, y];
                }
            }

            return pathLength[N];
        }

        /// <summary>
        /// Same as partA, but now with a magical virtual board.
        /// N = size of input board
        /// Memory usage O(N^2) - N*(N+5)
        /// Iterations O(N) - 2*N*5 - 3
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override object PartB(string input)
        {
            // Load data
            var lines = input.SplitOnNewline();
            // Input gridsize
            int N = lines.Count();
            // Virtual gridsize
            int M = 5 * N;

            int[,] grid = new int[N, N];
            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++)
                {
                    int value = lines[y][x] - '0';
                    grid[x, y] = value;
                }
            }

            // This stores the diagonal front as we walk through our grid, position 0 is reserved to prevent OutOfBounds 
            int[] pathLength = Enumerable.Repeat(int.MaxValue, M + 1).ToArray();
            pathLength[1] = 0; // first step is free

            // We can draw M*2 - 1 diagonals on the grid.
            // We skip the first, since it is for free
            for (int depth = 1; depth < M * 2 - 1; depth++)
            {
                // The length of the diagonal is the depth+1
                for (int x = 0; x <= depth && x < M; x++)
                {
                    // Derive y
                    int y = depth - x;

                    // See if we are operating inside the (virtual) grid
                    if (y >= 0 && y < M)
                    {
                        // The grid we are using, steps right and down
                        int gridDistance = (x / N) + (y / N);
                        // The relative location
                        int _x = x % N;
                        int _y = y % N;
                        // The value for this spot in the grid, Only works when the multiplier < 9 (it is 5 in this exercise)
                        int gridValue = ((grid[_x, _y] + gridDistance) % 10) + ((grid[_x, _y] + gridDistance) / 10);
                        // Find shortest path
                        pathLength[y + 1] = Math.Min(pathLength[y], pathLength[y + 1]) + gridValue;
                    }
                }
            }

            // If you want to know why the answer magically propagated to exactly this index; draw it out =)
            return pathLength[M];
        }
    }
}
