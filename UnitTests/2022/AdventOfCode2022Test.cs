using AdventOfCode2022.Solvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class AdventOfCode2022Test
    {
        public AdventOfCode2022Test()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            baseDir = baseDir.Substring(0, baseDir.IndexOf("bin")) + "2022\\Data\\";
            Directory.SetCurrentDirectory(baseDir);
        }
        
        [TestMethod]
        public void Day01aTest()
        {
            Day01 solution = new Day01();

            var result = solution.TestPartAResult();

            Assert.AreEqual(24000, result);
        }
        
        [TestMethod]
        public void Day01bTest()
        {
            Day01 solution = new Day01();

            var result = solution.TestPartBResult();

            Assert.AreEqual(45000, result);
        }

        [TestMethod]
        public void Day02rpcTest()
        {
            Day02 solution = new Day02();
            Assert.AreEqual(0, solution.CompareRPC(Day02.RoPaSc.Rock, Day02.RoPaSc.Rock));
            Assert.AreEqual(-1, solution.CompareRPC(Day02.RoPaSc.Rock, Day02.RoPaSc.Paper));
            Assert.AreEqual(1, solution.CompareRPC(Day02.RoPaSc.Rock, Day02.RoPaSc.Scissors));
            Assert.AreEqual(1, solution.CompareRPC(Day02.RoPaSc.Paper, Day02.RoPaSc.Rock));
            Assert.AreEqual(0, solution.CompareRPC(Day02.RoPaSc.Paper, Day02.RoPaSc.Paper));
            Assert.AreEqual(-1, solution.CompareRPC(Day02.RoPaSc.Paper, Day02.RoPaSc.Scissors));
            Assert.AreEqual(-1, solution.CompareRPC(Day02.RoPaSc.Scissors, Day02.RoPaSc.Rock));
            Assert.AreEqual(1, solution.CompareRPC(Day02.RoPaSc.Scissors, Day02.RoPaSc.Paper));
            Assert.AreEqual(0, solution.CompareRPC(Day02.RoPaSc.Scissors, Day02.RoPaSc.Scissors));
        }


        [TestMethod]
        public void Day02aTest()
        {
            Day02 solution = new Day02();

            var result = solution.TestPartAResult();

            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void Day02bbTest()
        {
            Day02 solution = new Day02();

            var result = solution.TestPartBResult();

            Assert.AreEqual(12, result);
        }
        
        [TestMethod]
        public void Day03aTest()
        {
            Day03 solution = new Day03();

            var result = solution.TestPartAResult();

            Assert.AreEqual(157, result);
        }
        
        [TestMethod]
        public void Day03bTest()
        {
            Day03 solution = new Day03();

            var result = solution.TestPartBResult();

            Assert.AreEqual(70, result);
        }
        
        [TestMethod]
        public void Day04aTest()
        {
            Day04 solution = new Day04();

            var result = solution.TestPartAResult();

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Day04bTest()
        {
            Day04 solution = new Day04();

            var result = solution.TestPartBResult();

            Assert.AreEqual(4, result);
        }
        
        [TestMethod]
        public void Day05aTest()
        {
            Day05 solution = new Day05();

            var result = solution.TestPartAResult();

            Assert.AreEqual("CMZ", result);
        }

        [TestMethod]
        public void Day05bTest()
        {
            Day05 solution = new Day05();

            var result = solution.TestPartBResult();

            Assert.AreEqual("MCD", result);
        }

        [TestMethod]
        public void Day06aTest()
        {
            Day06 solution = new Day06();

            Assert.AreEqual(7, solution.TestPartAResult("mjqjpqmgbljsphdztnvjfqwrcgsmlb"));
            Assert.AreEqual(5, solution.TestPartAResult("bvwbjplbgvbhsrlpgdmjqwftvncz"));
            Assert.AreEqual(6, solution.TestPartAResult("nppdvjthqldpwncqszvftbrmjlhg"));
            Assert.AreEqual(10, solution.TestPartAResult("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg"));
            Assert.AreEqual(11, solution.TestPartAResult("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"));
        }

        [TestMethod]
        public void Day06bTest()
        {
            Day06 solution = new Day06();

            Assert.AreEqual(19, solution.TestPartBResult("mjqjpqmgbljsphdztnvjfqwrcgsmlb"));
            Assert.AreEqual(23, solution.TestPartBResult("bvwbjplbgvbhsrlpgdmjqwftvncz"));
            Assert.AreEqual(23, solution.TestPartBResult( "nppdvjthqldpwncqszvftbrmjlhg"));
            Assert.AreEqual(29, solution.TestPartBResult( "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg"));
            Assert.AreEqual(26, solution.TestPartBResult("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"));
        }


        [TestMethod]
        public void Day07aTest()
        {
            Day07 solution = new Day07();

            var result = solution.TestPartAResult();

            Assert.AreEqual(95437L, result);
        }

        [TestMethod]
        public void Day07bTest()
        {
            Day07 solution = new Day07();

            var result = solution.TestPartBResult();

            Assert.AreEqual(24933642L, result);
        }


        [TestMethod]
        public void Day08aTest()
        {
            Day08 solution = new Day08();

            var result = solution.TestPartAResult();

            Assert.AreEqual(21, result);
        }

        [TestMethod]
        public void Day08bTest()
        {
            Day08 solution = new Day08();

            var result = solution.TestPartBResult();

            Assert.AreEqual(8L, result);
        }

        [TestMethod]
        public void Day09aTest()
        {
            Day09 solution = new Day09();

            var result = solution.TestPartAResult();

            Assert.AreEqual(13, result);
        }

        [TestMethod]
        public void Day09bTest()
        {
            Day09 solution = new Day09();
            string input = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";

            var result = solution.TestPartBResult(input);

            Assert.AreEqual(36, result);
        }

        [TestMethod]
        public void Day10aTest()
        {
            Day10 solution = new Day10();

            var result = solution.TestPartAResult();

            Assert.AreEqual(13140L, result);
        }

        [TestMethod]
        public void Day10bTest()
        {
            Day10 solution = new Day10();

            string expectedResult = @"##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....";

            var result = solution.TestPartBResult();

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Day11aTest()
        {
            Day11 solution = new Day11();

            var result = solution.TestPartAResult();

            Assert.AreEqual(10605, result);
        }

        [TestMethod]
        public void Day11bTest()
        {
            Day11 solution = new Day11();

            var result = solution.TestPartBResult();

            Assert.AreEqual(2713310158L, result);
        }

        [TestMethod]
        public void Day12aTest()
        {
            Day12 solution = new Day12();

            var result = solution.TestPartAResult();

            Assert.AreEqual(31, result);
        }

        [TestMethod]
        public void Day12bTest()
        {
            Day12 solution = new Day12();

            var result = solution.TestPartBResult();

            Assert.AreEqual(29, result);
        }
/*
        [TestMethod]
        public void Day12_1aTest()
        {
            Day12_1 solution = new Day12_1();

            var result = solution.TestPartAResult();

            Assert.AreEqual(226, result);
        }

        [TestMethod]
        public void Day12_1bTest()
        {
            Day12_1 solution = new Day12_1();

            var result = solution.TestPartBResult();

            Assert.AreEqual(3509, result);
        }

        [TestMethod]
        public void Day12_2aTest()
        {
            Day12_2 solution = new Day12_2();

            var result = solution.TestPartAResult();

            Assert.AreEqual(226, result);
        }

        [TestMethod]
        public void Day12_2bTest()
        {
            Day12_2 solution = new Day12_2();

            var result = solution.TestPartBResult();

            Assert.AreEqual(3509, result);
        }

        [TestMethod]
        public void Day13aTest()
        {
            Day13 solution = new Day13();

            var result = solution.TestPartAResult();

            Assert.AreEqual(17, result);
        }

        // No test for 13b

        [TestMethod]
        public void Day14aTest()
        {
            Day14 solution = new Day14();

            var result = solution.TestPartAResult();

            Assert.AreEqual(1588L, result);
        }

        [TestMethod]
        public void Day14bTest()
        {
            Day14 solution = new Day14();

            var result = solution.TestPartBResult();

            Assert.AreEqual(2188189693529L, result);
        }

        [TestMethod]
        public void Day15aTest()
        {
            Day15 solution = new Day15();

            var result = solution.TestPartAResult();

            Assert.AreEqual(40, result);
        }

        [TestMethod]
        public void Day15_1aTest()
        {
            Day15_1 solution = new Day15_1();

            var result = solution.TestPartAResult();

            Assert.AreEqual(40, result);
        }

        [TestMethod]
        public void Day15_1bTest()
        {
            Day15_1 solution = new Day15_1();

            var result = solution.TestPartBResult();

            Assert.AreEqual(315, result);
        }

        [TestMethod]
        public void Day15_2aTest()
        {
            Day15_2 solution = new Day15_2();

            var result = solution.TestPartAResult();

            Assert.AreEqual(40, result);
        }

        [TestMethod]
        public void Day15_2bTest()
        {
            Day15_2 solution = new Day15_2();

            var result = solution.TestPartBResult();

            Assert.AreEqual(315, result);
        }

        [TestMethod]
        public void Day16a1Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartAResult(@"8A004A801A8002F478");

            Assert.AreEqual(16, result);
        }

        [TestMethod]
        public void Day16a2Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartAResult(@"620080001611562C8802118E34");

            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void Day16a3Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartAResult(@"C0015000016115A2E0802F182340");

            Assert.AreEqual(23, result);
        }

        [TestMethod]
        public void Day16a4Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartAResult(@"A0016C880162017C3686B18A3D4780");

            Assert.AreEqual(31, result);
        }

        [TestMethod]
        public void Day16b1Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartBResult(@"C200B40A82");

            Assert.AreEqual(3L, result);
        }

        [TestMethod]
        public void Day16b2Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartBResult(@"04005AC33890");

            Assert.AreEqual(54L, result);
        }

        [TestMethod]
        public void Day16b3Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartBResult(@"880086C3E88112");

            Assert.AreEqual(7L, result);
        }

        [TestMethod]
        public void Day16b4Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartBResult(@"CE00C43D881120");

            Assert.AreEqual(9L, result);
        }

        [TestMethod]
        public void Day16b5Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartBResult(@"D8005AC2A8F0");

            Assert.AreEqual(1L, result);
        }

        [TestMethod]
        public void Day16b6Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartBResult(@"F600BC2D8F");

            Assert.AreEqual(0L, result);
        }

        [TestMethod]
        public void Day16b7Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartBResult(@"9C005AC2F8F0");

            Assert.AreEqual(0L, result);
        }

        [TestMethod]
        public void Day16b8Test()
        {
            Day16 solution = new Day16();

            var result = solution.TestPartBResult(@"9C0141080250320F1802104A08");

            Assert.AreEqual(1L, result);
        }

        [TestMethod]
        public void Day17aTest()
        {
            Day17 solution = new Day17();

            var result = solution.TestPartAResult();

            Assert.AreEqual(45, result);
        }

        [TestMethod]
        public void Day17bTest()
        {
            Day17 solution = new Day17();

            var result = solution.TestPartBResult();

            Assert.AreEqual(112, result);
        }

        [TestMethod]
        public void Day18aTest()
        {
            Day18 solution = new Day18();

            var result = solution.TestPartAResult();

            Assert.AreEqual(4140, result);
        }

        [TestMethod]
        public void Day18bTest()
        {
            Day18 solution = new Day18();

            var result = solution.TestPartBResult();

            Assert.AreEqual(3993, result);
        }

        [TestMethod]
        public void Day19aTest()
        {
            Day19 solution = new Day19();

            var result = solution.TestPartAResult();

            Assert.AreEqual(79, result);
        }

        [TestMethod]
        public void Day19bTest()
        {
            Day19 solution = new Day19();

            var result = solution.TestPartBResult();

            Assert.AreEqual(3621, result);
        }

        [TestMethod]
        public void Day20aTest()
        {
            Day20 solution = new Day20();

            var result = solution.TestPartAResult();

            Assert.AreEqual(35, result);
        }

        [TestMethod]
        public void Day20bTest()
        {
            Day20 solution = new Day20();

            var result = solution.TestPartBResult();

            Assert.AreEqual(3351, result);
        }

        [TestMethod]
        public void Day21aTest()
        {
            Day21 solution = new Day21();

            var result = solution.TestPartAResult();

            Assert.AreEqual(739785, result);
        }

        [TestMethod]
        public void Day21bTest()
        {
            Day21 solution = new Day21();

            var result = solution.TestPartBResult();

            Assert.AreEqual(444356092776315l, result);
        }

        [TestMethod]
        public void Day22aTest1()
        {
            Day22 solution = new Day22();

            Day22.Cuboid cuboid1 = new Day22.Cuboid(0, 10, 0, 20, 0, 30, Day22.Cuboid.State.ON);
            Day22.Cuboid cuboid2 = new Day22.Cuboid(0, 10, 0, 20, 0, 30, Day22.Cuboid.State.ON);
            Day22.Cuboid col = new Day22.Cuboid(3, 7, 3, 17, 3, 27, Day22.Cuboid.State.ON);

            var collided = cuboid2.Collide(col);
            var collided2 = collided.SelectMany(a => a.Collide(col));

            long c4 = col.blocks;
            long c1 = cuboid1.blocks;
            long c2 = collided.Sum(c => c.blocks);
            long c3 = collided2.Sum(c => c.blocks);
            Assert.AreEqual(11 * 21 * 31L, c1);
            Assert.AreEqual(c1, c2 + c4);
            Assert.AreEqual(c2, c3);
        }

        [TestMethod]
        public void Day22aTest()
        {
            Day22 solution = new Day22();

            var result = solution.TestPartAResult();
            //590784
            //474140
            Assert.AreEqual(474140L, result);
        }

        [TestMethod]
        public void Day22bTest()
        {
            Day22 solution = new Day22();

            var result = solution.TestPartBResult();

            Assert.AreEqual(2758514936282235L, result);
        }

        [TestMethod]
        public void Day23aTest()
        {
            Day23 solution = new Day23();

            var result = solution.TestPartAResult();

            Assert.AreEqual(12521, result);
        }

        [TestMethod]
        public void Day23bTest()
        {
            Day23 solution = new Day23();

            var result = solution.TestPartBResult();

            Assert.AreEqual(44169, result);
        }

        [TestMethod]
        public void Day25aTest()
        {
            Day25 solution = new Day25();

            var result = solution.TestPartAResult();

            Assert.AreEqual(58, result);
        }
        */

    }
}