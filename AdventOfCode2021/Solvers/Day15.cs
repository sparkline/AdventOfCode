using AdventOfCode2021.Common;

namespace AdventOfCode2021.Solvers
{
    public class Day15 : Solver
    {
        public Day15() : base(2021, 15, 0, "Dijkstra") { }

        protected override object PartA(string input)
        {
            (int x, int y) lastVisit;
            Dictionary<(int x, int y), (int price, int priceToOrigin, (int x, int y) lastVisit)> visited = new Dictionary<(int x, int y), (int price, int priceToOrigin, (int x, int y) lastVisit)>();
            Dictionary<(int x, int y), (int price, int priceToOrigin, (int x, int y) lastVisit)> toVisit = new Dictionary<(int x, int y), (int price, int priceToOrigin, (int x, int y) lastVisit)>();

            // Load data
            var lines = input.SplitOnNewline();
            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Count(); x++)
                {
                    int value = Int32.Parse("" + lines[y][x]);
                    toVisit.Add((x, y), (price: value, priceToOrigin: int.MaxValue, (x: x, y: y)));
                }
            }
            (int x, int y) origin = (0, 0);
            (int x, int y) destination = (toVisit.Max(k => k.Key.x), toVisit.Max(k => k.Key.y));

            // Set first entry
            toVisit[origin] = (price: 0, priceToOrigin: 0, lastVisit: origin);
            lastVisit = origin;
            visited.Add(lastVisit, toVisit[lastVisit]);
            toVisit.Remove(lastVisit);

            while (toVisit.Count() > 0)
            {
                // exclude last visit?
                var visitNode = visited[lastVisit];
                List<(int x, int y)> neighbours = toVisit.Keys.Where(k => (Math.Abs(k.x - lastVisit.x) + Math.Abs(k.y - lastVisit.y) == 1)).ToList();
                foreach (var neighbourLocation in neighbours)
                {
                    var neighbour = toVisit[neighbourLocation];
                    if (visitNode.priceToOrigin + neighbour.price < neighbour.priceToOrigin)
                    {
                        neighbour.priceToOrigin = visitNode.priceToOrigin + neighbour.price;
                        neighbour.lastVisit = lastVisit;
                        toVisit[(neighbourLocation)] = neighbour;
                    }
                }
                lastVisit = toVisit.OrderBy(v => v.Value.priceToOrigin).First().Key;
                visited.Add(lastVisit, toVisit[lastVisit]);
                toVisit.Remove(lastVisit);

            }

            return visited[destination].priceToOrigin;
        }

        protected override object PartB(string input)
        {
            throw new NotImplementedException();
        }
    }
}
