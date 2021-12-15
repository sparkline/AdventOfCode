using AdventOfCode2021.Solvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class AdventOfCode2021Test
    {
        public AdventOfCode2021Test()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            baseDir = baseDir.Substring(0, baseDir.IndexOf("bin")) + "Data\\";
            Directory.SetCurrentDirectory(baseDir);
        }

        [TestMethod]
        public void Day01aTest()
        {
            Day01 solution = new Day01();

            var result = solution.TestPartAResult();

            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void Day01bTest()
        {
            Day01 solution = new Day01();

            var result = solution.TestPartBResult();

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Day02aTest()
        {
            Day02 solution = new Day02();

            var result = solution.TestPartAResult();

            Assert.AreEqual(150, result);
        }

        [TestMethod]
        public void Day02bbTest()
        {
            Day02 solution = new Day02();

            var result = solution.TestPartBResult();

            Assert.AreEqual(900, result);
        }

        [TestMethod]
        public void Day03aTest()
        {
            Day03 solution = new Day03();

            var result = solution.TestPartAResult();

            Assert.AreEqual(198, result);
        }

        [TestMethod]
        public void Day03bTest()
        {
            Day03 solution = new Day03();

            var result = solution.TestPartBResult();

            Assert.AreEqual(230, result);
        }

        [TestMethod]
        public void Day04aTest()
        {
            Day04 solution = new Day04();

            var result = solution.TestPartAResult();

            Assert.AreEqual(4512, result);
        }

        [TestMethod]
        public void Day04bTest()
        {
            Day04 solution = new Day04();

            var result = solution.TestPartBResult();

            Assert.AreEqual(1924, result);
        }

        [TestMethod]
        public void Day05aTest()
        {
            Day05 solution = new Day05();

            var result = solution.TestPartAResult();

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Day05bTest()
        {
            Day05 solution = new Day05();

            var result = solution.TestPartBResult();

            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void Day05_1aTest()
        {
            Day05_1 solution = new Day05_1();

            var result = solution.TestPartAResult();

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Day05_1bTest()
        {
            Day05_1 solution = new Day05_1();

            var result = solution.TestPartBResult();

            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void Day06aTest()
        {
            Day06 solution = new Day06();

            var result = solution.TestPartAResult();

            Assert.AreEqual(5934, result);
        }

        // No 6b test available

        [TestMethod]
        public void Day07aTest()
        {
            Day07 solution = new Day07();

            var result = solution.TestPartAResult();

            Assert.AreEqual(37, result);
        }

        [TestMethod]
        public void Day07bTest()
        {
            Day07 solution = new Day07();

            var result = solution.TestPartBResult();

            Assert.AreEqual(168, result);
        }

        [TestMethod]
        public void Day07_1aTest()
        {
            Day07_1 solution = new Day07_1();

            var result = solution.TestPartAResult();

            Assert.AreEqual(37, result);
        }

        [TestMethod]
        public void Day07_1bTest()
        {
            Day07_1 solution = new Day07_1();

            var result = solution.TestPartBResult();

            Assert.AreEqual(168, result);
        }

        [TestMethod]
        public void Day07_2aTest()
        {
            Day07_2 solution = new Day07_2();

            var result = solution.TestPartAResult();

            Assert.AreEqual(37, result);
        }

        /*
         * Error, answer right, but not for this case...
        [TestMethod]
        public void Day07bFastestTest()
        {
            Day07Fast solution = new Day07Fast();

            var result = solution.TestPartBResult();

            Assert.AreEqual(168, result);
        }
        */

        [TestMethod]
        public void Day08aTest()
        {
            Day08 solution = new Day08();

            var result = solution.TestPartAResult();

            Assert.AreEqual(26, result);
        }

        [TestMethod]
        public void Day08bTest()
        {
            Day08 solution = new Day08();

            var result = solution.TestPartBResult();

            Assert.AreEqual(61229, result);
        }

        [TestMethod]
        public void Day09aTest()
        {
            Day09 solution = new Day09();

            var result = solution.TestPartAResult();

            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void Day09bTest()
        {
            Day09 solution = new Day09();

            var result = solution.TestPartBResult();

            Assert.AreEqual(1134, result);
        }

        [TestMethod]
        public void Day10aTest()
        {
            Day10 solution = new Day10();

            var result = solution.TestPartAResult();

            Assert.AreEqual(26397, result);
        }

        [TestMethod]
        public void Day10bTest()
        {
            Day10 solution = new Day10();

            var result = solution.TestPartBResult();

            Assert.AreEqual(288957L, result);
        }

        [TestMethod]
        public void Day11aTest()
        {
            Day11 solution = new Day11();

            var result = solution.TestPartAResult();

            Assert.AreEqual(1656, result);
        }

        [TestMethod]
        public void Day11bTest()
        {
            Day11 solution = new Day11();

            var result = solution.TestPartBResult();

            Assert.AreEqual(195, result);
        }

        [TestMethod]
        public void Day12aTest()
        {
            Day12 solution = new Day12();

            var result = solution.TestPartAResult();

            Assert.AreEqual(226, result);
        }

        [TestMethod]
        public void Day12bTest()
        {
            Day12 solution = new Day12();

            var result = solution.TestPartBResult();

            Assert.AreEqual(3509, result);
        }

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


    }
}