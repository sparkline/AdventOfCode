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

    }
}