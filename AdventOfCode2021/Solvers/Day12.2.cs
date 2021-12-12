using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day12_2 : Solver
    {
        public Day12_2() : base(2021, 12, 1, "DFS3, without recursion") { }

        private Dictionary<string, HashSet<(string name, bool isUpper)>> edges = new Dictionary<string, HashSet<(string name, bool isUpper)>>();

        protected override object PartA(string input)
        {
            edges = LoadEdges(input);
            var solutions = DFS3("start", "end", 0);
            return solutions;
        }

        protected override object PartB(string input)
        {
            edges = LoadEdges(input);
            var solutions = DFS3("start", "end", 1);
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

                if (to != "start" && from != "end")
                {
                    if (edges.TryGetValue(from, out var val1))
                    {
                        val1.Add(dest1);
                    }
                    else
                    {
                        edges.Add(from, new HashSet<(string name, bool isUpper)> { dest1 });
                    }
                }

                if (from != "start" && to != "end")
                {
                    if (edges.TryGetValue(to, out var val2))
                    {
                        val2.Add(dest2);
                    }
                    else
                    {
                        edges.Add(to, new HashSet<(string name, bool isUpper)> { dest2 });
                    }
                }
            }
            return edges;
        }

        /// <summary>
        /// Depth first algorithm to return amount of paths from start to destination. 
        /// </summary>
        /// <param name="startVertice"></param>
        /// <param name="destinationVertice"></param>
        /// <param name="shortCutsRemaing">Amount of times we are allowed to visit a lowercase multiple times</param>
        /// 
        /// 
        /// <returns></returns>
        private int DFS3(string startVertice, string destinationVertice, int shortcutsAllowed = 0)
        {
            int solutions = 0;
            Stack<(string vertice, int remainingShortcuts, string[] visited)> toVisit = new Stack<(string vertice, int remainingShortcuts, string[])>();
            toVisit.Push((startVertice, shortcutsAllowed, new string[] { startVertice }));

            while (toVisit.Count > 0)
            {
                var visit = toVisit.Pop();
                if (edges.TryGetValue(visit.vertice, out HashSet<(string name, bool isUpper)>? newConnections))
                {
                    foreach (var conn in newConnections)
                    {
                        string targetVertice = conn.name;

                        // Skip if end
                        if (targetVertice == destinationVertice)
                        {
                            solutions++;
                        }

                        // Skip if double
                        int _shortCutsAllowed = visit.remainingShortcuts;
                        if (!conn.isUpper && visit.visited.Contains(targetVertice))
                        {
                            if (--_shortCutsAllowed < 0)
                            {
                                continue;
                            }
                        }

                        toVisit.Push((targetVertice, _shortCutsAllowed, visit.visited.Append(targetVertice).ToArray()));
                    }
                }
            }

            return solutions;
        }

    }
}
