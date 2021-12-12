using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day12_1 : Solver
    {
        public Day12_1() : base(2021, 12, 1, "DFS2") { }

        private Dictionary<string, HashSet<(string name, bool isUpper)>> edges = new Dictionary<string, HashSet<(string name, bool isUpper)>>();

        protected override object PartA(string input)
        {
            edges = LoadEdges(input);
            var solutions = DFS2("start", "start", "end", new List<string> { "start" }, 0);
            return solutions;
        }

        protected override object PartB(string input)
        {
            edges = LoadEdges(input);
            var solutions = DFS2("start", "start", "end", new List<string> { "start" }, 1);
            return solutions;
        }

        /// <summary>
        /// Add edges, make it a bi-directional graph
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private Dictionary<string, HashSet<(string name, bool isUpper)>> LoadEdges(string input)
        {
            Dictionary<string, HashSet<(string name, bool isUpper)>> edges = new Dictionary<string, HashSet<(string name, bool isUpper)>>();
            List<string> lines = input.SplitOnNewline();
            foreach (string line in lines)
            {
                var vertices = line.Split('-');
                var from = vertices[0];
                var to = vertices[1];
                var dest1 = (name: to, to.ToUpper() == to);
                var dest2 = (name: from, from.ToUpper() == from);

                if (edges.TryGetValue(from, out var val1))
                {
                    val1.Add(dest1);
                }
                else
                {
                    edges.Add(from, new HashSet<(string name, bool isUpper)> { dest1 });
                }

                if (edges.TryGetValue(to, out var val2))
                {
                    val2.Add(dest2);
                }
                else
                {
                    edges.Add(to, new HashSet<(string name, bool isUpper)> { dest2 });
                }
            }
            return edges;
        }

        /// <summary>
        /// Depth first algorithm to return amount of paths from start to destination. 
        /// </summary>
        /// <param name="startVertice"></param>
        /// <param name="currentVertice"></param>
        /// <param name="destinationVertice"></param>
        /// <param name="visited">List, shared by reference to save memory</param>
        /// <param name="shortCutsRemaing">Amount of times we are allowed to visit a lowercase multiple times</param>
        /// <returns></returns>
        private int DFS2(string startVertice, string currentVertice, string destinationVertice, List<string> visited, int shortCutsRemaing = 0)
        {
            int solutions = 0;
            var nextSteps = edges[currentVertice];
            foreach (var step in nextSteps)
            {
                string nextVertice = step.name;
                bool isUpper = step.isUpper;

                if (nextVertice == destinationVertice)
                {
                    solutions++;
                    continue;
                }

                int _shortCutsRemaing = shortCutsRemaing;
                if (!isUpper && visited.Contains(nextVertice))
                {
                    _shortCutsRemaing--;
                    if (_shortCutsRemaing < 0 || nextVertice == startVertice)
                    {
                        // Skip, not allowed
                        continue;
                    }
                }

                // Add to our visited array
                visited.Add(nextVertice);
                // Explore
                solutions += DFS2(startVertice, nextVertice, destinationVertice, visited, _shortCutsRemaing);
                // Remove, since we are using the list by reference
                visited.RemoveAt(visited.Count - 1);
            }

            return solutions;
        }

    }
}
