using AdventOfCode2022.Common;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Solvers
{
    public class Day07 : Solver
    {
        public Day07() : base(7) { }


        protected override object PartA(string input)
        {
            Item root = getRoot(input);

            var dirsUnderSize = root.getAll().Where(r => r.isDir && r.getSize() <= 100000);
            long totalSize = dirsUnderSize.Sum(dir => dir.getSize());
            return totalSize;
        }

        private static Item getRoot(string input)
        {
            Item root = Item.Root();
            Item currentItem = root;
            var commandosWithResponses = input.Split("$", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
            foreach (var commandWithResponse in commandosWithResponses)
            {
                var commando = commandWithResponse.SplitOnNewline()[0].Split()[0];
                switch (commando)
                {
                    case "cd":
                        var commandoParam = commandWithResponse.SplitOnNewline()[0].Split()[1];
                        switch (commandoParam)
                        {
                            case "..":
                                currentItem = currentItem.parent;
                                break;

                            case "/":
                                currentItem = root;
                                break;
                            default:
                                currentItem = currentItem.children.First(c => c.name == commandoParam);
                                break;
                        }
                        break;

                    case "ls":
                        var response = commandWithResponse.SplitOnNewline().Skip(1);
                        foreach (var entry in response)
                        {
                            var contents = entry.Split();
                            if (contents[0] == "dir")
                            {
                                currentItem.children.Add(Item.Dir(contents[1], currentItem));
                            }
                            else
                            {
                                currentItem.children.Add(Item.File(contents[1], long.Parse(contents[0]), currentItem));
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }

            }

            return root;
        }

        protected override object PartB(string input)
        {
            Item root = getRoot(input);
            long usedSpace = root.getSize();
            long extraSpaceNeeded = usedSpace - (70000000 - 30000000);

            var dirsUnderSize = root.getAll().Where(r => r.isDir && r.getSize() >= extraSpaceNeeded);
            var smallestFit = dirsUnderSize.Min(dir => dir.getSize());
            return smallestFit;
        }


        private class Item
        {
            public long size { get; private set; }
            public string name { get; private set; }
            public List<Item> children { get; private set; }
            public Item parent { get; private set; }

            public bool isDir { get; private set; }



            public static Item File(string name, long size, Item parent)
            {
                return new Item
                {
                    name = name,
                    size = size,
                    children = new List<Item>(),
                    parent = parent,
                    isDir = false,
                };
            }

            public static Item Dir(string name, Item parent)
            {
                return new Item
                {
                    name = name,
                    size = 0,
                    children = new List<Item>(),
                    parent = parent,
                    isDir = true,
                };
            }

            public static Item Root()
            {
                return new Item
                {
                    name = "/",
                    size = 0,
                    children = new List<Item>(),
                    parent = null,
                    isDir = true,
                };
            }

            public long getSize()
            {
                long totalSize = size;
                totalSize += children.Sum(child => child.getSize());
                return totalSize;
            }

            public IEnumerable<Item> getAll()
            {
                List<Item> result = new List<Item> { this };
                result.AddRange(children.SelectMany(child => child.getAll()));
                return result;
            }
        }
    }
    
    
}
