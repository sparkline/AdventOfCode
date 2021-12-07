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
        public void Day05faTest()
        {
            Day05f solution = new Day05f();

            var result = solution.TestPartAResult();

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Day05fbTest()
        {
            Day05f solution = new Day05f();

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
        public void Day07aBriefTest()
        {
            Day07Brief solution = new Day07Brief();

            var result = solution.TestPartAResult();

            Assert.AreEqual(37, result);
        }

        [TestMethod]
        public void Day07bBriefTest()
        {
            Day07Brief solution = new Day07Brief();

            var result = solution.TestPartBResult();

            Assert.AreEqual(168, result);
        }


    }
}