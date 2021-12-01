using AdventOfCode2021.Day01;
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


    }
}