using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day12 : Solver
    {
        public Day12() : base(2021, 12) { }

        private Dictionary<string, HashSet<(string name, bool isUpper)>> edges = new Dictionary<string, HashSet<(string name, bool isUpper)>>();

        protected override object PartA(string input)
        {
            edges = LoadEdges(input);
            var solutions = DFS("start", "end", new string[] { "start" });
            return solutions.Count();
        }

        protected override object PartB(string input)
        {
            edges = LoadEdges(input);
            var solutions = DFS("start", "end", new string[] { "start" }, true);
            return solutions.Count();
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
        /// Depth first, saving ourself some memory usage.
        /// </summary>
        /// <param name="from">Where we are exploring from atm</param>
        /// <param name="destination">Our end destination</param>
        /// <param name="notAllowedToVisitAgain">List of all lowercase vertices we visited</param>
        /// <param name="takeAShortcut">If true, it is allowed to take a shortcut by taking a lowercase-vertice twice</param>
        /// <returns>List of paths possible from <paramref name="from"/> to <paramref name="destination"/> </returns>
        private List<string> DFS(string from, string destination, string[] notAllowedToVisitAgain, bool takeAShortcut = false)
        {
            List<string> solutions = new List<string>();
            var nextSteps = edges[from];
            foreach (var step in nextSteps)
            {
                bool shortCutAllowed = takeAShortcut;
                string name = step.name;
                if (name == destination)
                {
                    // Found end, add to solution
                    solutions.Add(destination);
                    continue;
                }
                else if (notAllowedToVisitAgain.Contains(name))
                {
                    if (name == "start" || !takeAShortcut)
                    {
                        // not allowed, skip
                        continue;
                    }
                    else
                    {
                        // cheat once
                        shortCutAllowed = false;
                    }
                }


                if (step.isUpper)
                {
                    // Search deeper, don't grow the visited array, since we don't use the uppercase values
                    solutions.AddRange(DFS(name, destination, notAllowedToVisitAgain, shortCutAllowed));
                }
                else
                {
                    // Add to our visited array
                    string[] newVisited = new string[notAllowedToVisitAgain.Length + 1];
                    if (notAllowedToVisitAgain.Length > 0)
                    {
                        notAllowedToVisitAgain.CopyTo(newVisited, 1);
                    }
                    newVisited[0] = name;
                    solutions.AddRange(DFS(name, destination, newVisited, shortCutAllowed));
                }
            }

            // Create the solution strings. Alternative could have been to use an inverted join of a visited array.
            return solutions.Select(s => $"{from}, {s}").ToList();
        }

    }
}
