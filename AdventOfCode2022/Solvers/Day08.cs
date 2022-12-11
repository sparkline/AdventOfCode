using AdventOfCode2022.Common;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solvers
{
    public class Day08 : Solver
    {
        public Day08() : base(8) { }


        protected override object PartA(string input)
        {
            var lines = input.SplitOnNewline();
            int width = lines[0].Length;
            int height = lines.Count();

            int[,] forest = new int[width,height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y< height; y++)
                {
                    int size = int.Parse(lines[y][x]+"");
                    forest[x,y] = size;
                }
            }

            bool[,] forestISee = new bool[width, height];
            for (int x= 0; x < width; x++)
            {
                for (int y = 0; y< height; y++)
                {
                    lookThatWay(Enumerable.Range(0, width), Enumerable.Range(y, 1), forest, ref forestISee);
                    lookThatWay(Enumerable.Range(0, width).Reverse(), Enumerable.Range(y, 1), forest, ref forestISee);
                    lookThatWay(Enumerable.Range(x, 1), Enumerable.Range(0, height), forest, ref forestISee);
                    lookThatWay(Enumerable.Range(x, 1), Enumerable.Range(0, height).Reverse(), forest, ref forestISee);
                }
            }

            int thingsISee = Enumerable.Range(0,width).Sum(x => Enumerable.Range(0, height).Count(y => forestISee[x,y]));
            return thingsISee;
        }

        private void lookThatWay(IEnumerable<int> xRange, IEnumerable<int> yRange, int[,] forest, ref bool[,] forestISee)
        {
            int previousMaxSize = -1;
            foreach(int x in xRange)
            {
                foreach (int y in yRange)
                {
                    if (forest[x, y] > previousMaxSize)
                    {
                        forestISee[x, y] = true;
                        previousMaxSize = Math.Max(previousMaxSize,forest[x, y]);
                    }
                }
            }
        }

        private enum direction { left = 0, up = 1, right = 2, down = 3 };
        protected override object PartB(string input)
        {
            
            var lines = input.SplitOnNewline();
            int size = lines.Count(); // width == height

            int[,] forest = new int[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    int treeHeight = int.Parse(lines[y][x] + "");
                    forest[x, y] = treeHeight;
                }
            }

            int[,,] thingsISee = new int[size,size,4];

            for (int outer = 0 ; outer < size; outer++)
            {
                int[,] tallBlockingBoys = new int[10, 4]; // length, direction
                for (int i = 0; i < 10;i++)
                {
                    tallBlockingBoys[i, (int)direction.right] = size - 1;
                    tallBlockingBoys[i, (int)direction.down] = size - 1;
                }
                for (int inner = 0; inner < size; inner++)
                {
                    for (int dir = 0; dir < 4; dir++)
                    {
                        // 0: L2R 1: U2D 2: R2L 3: D2U
                        int pos = (dir >= 2) ? (size-1)-inner : inner; 

                        int x = (dir % 2 == 0) ? pos : outer; 
                        int y = (dir % 2 == 1) ? pos : outer;

                        int treeSize = forest[x, y];

                        thingsISee[x, y, dir] = tallBlockingBoys[treeSize, dir];
                        for (int smallerTreeSize = 0; smallerTreeSize <= treeSize; smallerTreeSize++)
                        {
                            tallBlockingBoys[smallerTreeSize, dir] = pos;
                        }
                    }
                }
            }
            /*
            Console.WriteLine("== Forest ==");
            for (int y =0; y< size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Console.Write(forest[x, y]);
                }

                Console.WriteLine();
            }
            Console.WriteLine("==================");


            foreach (var dir in Enum.GetValues(typeof(direction)))
            {
                Console.WriteLine($"== Things i see - {dir} ==");
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        Console.Write(thingsISee[x, y, (int)dir]);
                    }

                    Console.WriteLine();
                }
                Console.WriteLine("==================");

            }
            */
            long bestScore = 0L;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    int left = x-thingsISee[x, y, (int)direction.left];
                    int right = thingsISee[x, y, (int)direction.right] - x;
                    int up = y-thingsISee[x,y, (int)direction.up];
                    int down = thingsISee[x, y, (int)direction.down] - y;
                    long score = left * right * up * down;
                    bestScore = Math.Max(score, bestScore);
                }
            }

            return bestScore;
        }

    }
    
}
