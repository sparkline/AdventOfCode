using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day15_2 : Solver
    {
        public Day15_2() : base(2021, 15, 1, "Okay, okay, A* it is") { }

        protected override object PartA(string input)
        {
            // Load data
            var lines = input.SplitOnNewline();
            int N = lines.Count();

            int[] vertices = new int[N * N];

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = lines[i / N][i % N] - '0';
            }

            return AstarSolver(vertices, N);
        }

        private int AstarSolver(int[] cost, int n, int repeatFactor = 1)
        {
            int N = n * repeatFactor;

            int[] costFromStart = Enumerable.Repeat(int.MaxValue, N * N).ToArray();
            int[] costToEnd = Enumerable.Repeat(int.MaxValue, N * N).ToArray();
            const int START = 0;
            int END = N * N - 1;
            costFromStart[START] = 0;
            costToEnd[START] = manhattan(START, N);

            PriorityQueue<int, int> toExplore = new PriorityQueue<int, int>();
            HashSet<int> inQueue = new HashSet<int> { START };
            toExplore.Enqueue(START, costToEnd[START]);
            while (toExplore.TryDequeue(out int vertice, out int costToEndForVertice))
            {
                // Select the current best candidate to reach the end
                // Remove ourself from the to-do list
                inQueue.Remove(vertice);

                if (vertice == END)
                {
                    // Found the end
                    return costFromStart[END];
                }

                // Fetch neighbours
                HashSet<int> neighbours = getNeighbours(vertice, N);
                foreach (int neighbour in neighbours)
                {
                    // Calculate what it will cost to go from start to neighbour from vertice
                    int minimumCost = costFromStart[vertice] + Cost(cost, neighbour, N, repeatFactor);
                    if (minimumCost < costFromStart[neighbour])
                    {
                        costFromStart[neighbour] = minimumCost;
                        costToEnd[neighbour] = minimumCost + manhattan(neighbour, N);
                        if (!inQueue.Contains(neighbour))
                        {
                            toExplore.Enqueue(neighbour, costToEnd[neighbour]);
                        }
                    }
                }
            }

            return -1;
        }

        private int Cost(int[] cost, int neighbour, int N, int repeatFactor)
        {
            // Yeah, the compiler will optimize this...

            // Dimension of the cost-grid
            int costWidth = N / repeatFactor;
            // Position within the big grid
            int x = neighbour % N;
            int y = neighbour / N;
            // The grid we are using, steps right and down
            int gridDistance = (x / costWidth) + (y / costWidth);
            // The relative location
            int _x = x % costWidth;
            int _y = y % costWidth;
            // Location in cost grid
            int _neighbour = _x + (_y * costWidth);
            // The value for this spot in the grid, Only works when the repeatFactor < 9 (it is 5 in this exercise)
            int gridValue = ((cost[_neighbour] + gridDistance) % 10) + ((cost[_neighbour] + gridDistance) / 10);

            return gridValue;
        }

        private HashSet<int> getNeighbours(int vertice, int N)
        {
            HashSet<int> result = new HashSet<int>();

            int topLeft = 0;
            int bottomRight = N * N - 1;
            int left = vertice - 1;
            int right = vertice + 1;
            int top = vertice - N;
            int bottom = vertice + N;

            if (left / N == vertice / N && left >= topLeft)
            {
                result.Add(left);
            }
            if (right / N == vertice / N && right <= bottomRight)
            {
                result.Add(right);
            }
            if (top % N == vertice % N && top >= topLeft)
            {
                result.Add(top);
            }
            if (bottom % N == vertice % N && bottom <= bottomRight)
            {
                result.Add(bottom);
            }

            return result;
        }

        private int manhattan(int index, int N)
        {
            // Manhattan distance to end. Can't be less than this.
            return (N - (index % N)) + (N - (index / N));
        }


        protected override object PartB(string input)
        {
            // Load data
            var lines = input.SplitOnNewline();
            int N = lines.Count();

            int[] vertices = new int[N * N];

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = lines[i / N][i % N] - '0';
            }

            return AstarSolver(vertices, N, 5);
        }
    }
}
