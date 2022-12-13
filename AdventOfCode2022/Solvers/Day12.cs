using AdventOfCode2022.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solvers
{
    public class Day12 : Solver
    {
        public Day12() : base(12) { }


        protected override object PartA(string input)
        {
            Graph g = new Graph(input);
            int steps = g.Dijsktra();

            return steps;
        }
        protected override object PartB(string input)
        {
            Graph2 g = new Graph2(input);
            int steps = g.Dijsktra();

            return steps;
        }

    }

    internal class Graph
    {
        private Node[] nodes;
        private int nodeCount { get; }

        public Graph(string input)
        {
            var lines = input.SplitOnNewline();
            int height = lines.Count();
            int width = lines[0].Length;
            this.nodeCount = width * height;
            nodes = new Node[nodeCount];
            // load nodes
            for (int y = 0; y < height; y++)
            {
                for (int x = 0 ; x < width; x++)
                {
                    int uniqueIdentifier = width * y + x;
                    char id = lines[y][x];
                    var node = new Node(id, uniqueIdentifier);
                    nodes[uniqueIdentifier] = node;

                    if (x > 0)
                    {
                        // look left
                        int leftIdentifier = uniqueIdentifier - 1;
                        nodes[uniqueIdentifier].createLinks(nodes[leftIdentifier]);
                    }

                    if (y > 0)
                    {
                        // look up
                        int upIdentifier = uniqueIdentifier - width;
                        nodes[uniqueIdentifier].createLinks(nodes[upIdentifier]);
                    }
                }
            }

        }

        public class Node
        {
            public Node(char id, int uniqueIdentifier)
            {
                this.id = id;
                this.uniqueIdentifier = uniqueIdentifier;
                this.isStart = id == 'S';
                this.isEnd = id == 'E';
                height = ((int)id - (int)'a') + 1;
                height = isStart ? 1 : height;
                height = isEnd ? 26 : height;
            }

            public char id { get; }
            public int uniqueIdentifier { get; }
            public bool isStart { get;  }
            public bool isEnd { get;  }
            public int height { get; }
            public Node[] links { get; protected set; } = new Node[0];

            internal void createLinks(Node that)
            {
                // Note: No check for double adding
                if (this.height - that.height >= -1)
                {
                    this.links = this.links.Concat(new Node[] { that }).ToArray();
                }
                if (this.height - that.height <= 1)
                {
                    that.links = that.links.Concat(new Node[] { this }).ToArray();
                }
            }

            public override bool Equals(object? obj)
            {
                if (obj is Node that)
                {
                    return this.uniqueIdentifier == that.uniqueIdentifier;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return uniqueIdentifier;
            }
        }

        internal int Dijsktra()
        {
            int startNodeId = nodes.Single(n => n.isStart).uniqueIdentifier;
            Node currentNode = nodes[startNodeId];

            int[] distance = Enumerable.Repeat(int.MaxValue, nodeCount).ToArray();
            distance[startNodeId] = 0;
            bool[] visited = Enumerable.Repeat(false, nodeCount).ToArray();

            while (currentNode != null)
            {
                int currentNodeId = currentNode.uniqueIdentifier;

                // Visit links
                int currentDistance = distance[currentNodeId];
                foreach (Node neighbour in currentNode.links)
                {
                    int neighbourId = neighbour.uniqueIdentifier;
                    int neighbourDistance = distance[neighbourId];
                    int weight = 1;
                    int proposedDistance = currentDistance + weight;
                    
                    // Set shortest path for neighbour
                    distance[neighbourId] = Math.Min(neighbourDistance, proposedDistance);
                }

                // Mark visited
                visited[currentNodeId] = true;

                // Select next node
                int minDistance = int.MaxValue;
                currentNode = null;
                for (int i = 0; i < nodeCount; i++)
                {
                    if (!visited[i] && distance[i] < minDistance)
                    {
                        minDistance = distance[i];
                        currentNode = nodes[i];
                    }
                }

                // Check end condition
                if (currentNode != null && currentNode.isEnd) 
                {
                    return distance[currentNode.uniqueIdentifier];
                }
            }
            
            return int.MaxValue;
        }
    }

    // Lazy, copy the code

    internal class Graph2
    {
        private Node[] nodes;
        private int nodeCount { get; }

        public Graph2(string input)
        {
            var lines = input.SplitOnNewline();
            int height = lines.Count();
            int width = lines[0].Length;
            this.nodeCount = width * height;
            nodes = new Node[nodeCount];
            // load nodes
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int uniqueIdentifier = width * y + x;
                    char id = lines[y][x];
                    var node = new Node(id, uniqueIdentifier);
                    nodes[uniqueIdentifier] = node;

                    if (x > 0)
                    {
                        // look left
                        int leftIdentifier = uniqueIdentifier - 1;
                        nodes[uniqueIdentifier].createLinks(nodes[leftIdentifier]);
                    }

                    if (y > 0)
                    {
                        // look up
                        int upIdentifier = uniqueIdentifier - width;
                        nodes[uniqueIdentifier].createLinks(nodes[upIdentifier]);
                    }
                }
            }

        }

        public class Node
        {
            public Node(char id, int uniqueIdentifier)
            {
                this.id = id;
                this.uniqueIdentifier = uniqueIdentifier;
                this.isStart = id == 'E';
                this.isEnd = id == 'a' || id == 'S';
                height = ((int)id - (int)'a') + 1;
                height = id == 'S' ? 1 : height;
                height = id == 'E' ? 26 : height;
            }

            public char id { get; }
            public int uniqueIdentifier { get; }
            public bool isStart { get; }
            public bool isEnd { get; }
            public int height { get; }
            public Node[] links { get; protected set; } = new Node[0];

            internal void createLinks(Node that)
            {
                // Note: No check for double adding
                if (this.height - that.height >= -1)
                {
                    that.links = that.links.Concat(new Node[] { this }).ToArray();
                }
                if (this.height - that.height <= 1)
                {
                    this.links = this.links.Concat(new Node[] { that }).ToArray();
                }
            }

            public override bool Equals(object? obj)
            {
                if (obj is Node that)
                {
                    return this.uniqueIdentifier == that.uniqueIdentifier;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return uniqueIdentifier;
            }
        }

        internal int Dijsktra()
        {
            int startNodeId = nodes.Single(n => n.isStart).uniqueIdentifier;
            Node currentNode = nodes[startNodeId];

            int[] distance = Enumerable.Repeat(int.MaxValue, nodeCount).ToArray();
            distance[startNodeId] = 0;
            bool[] visited = Enumerable.Repeat(false, nodeCount).ToArray();

            while (currentNode != null)
            {
                int currentNodeId = currentNode.uniqueIdentifier;

                // Visit links
                int currentDistance = distance[currentNodeId];
                foreach (Node neighbour in currentNode.links)
                {
                    int neighbourId = neighbour.uniqueIdentifier;
                    int neighbourDistance = distance[neighbourId];
                    int weight = 1;
                    int proposedDistance = currentDistance + weight;

                    // Set shortest path for neighbour
                    distance[neighbourId] = Math.Min(neighbourDistance, proposedDistance);
                }

                // Mark visited
                visited[currentNodeId] = true;

                // Select next node
                int minDistance = int.MaxValue;
                currentNode = null;
                for (int i = 0; i < nodeCount; i++)
                {
                    if (!visited[i] && distance[i] < minDistance)
                    {
                        minDistance = distance[i];
                        currentNode = nodes[i];
                    }
                }

                // Check end condition
                if (currentNode != null && currentNode.isEnd)
                {
                    return distance[currentNode.uniqueIdentifier];
                }
            }

            return int.MaxValue;
        }
    }
}
