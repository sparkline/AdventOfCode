using System.Diagnostics;

namespace AdventOfCode2021.Common
{
    public enum CodeType
    {
        Original,
        Fastest,
        Brief,
    }
    public abstract class Solver
    {
        protected Solver(int year, int day, CodeType codeType)
        {
            this.Year = year;
            this.Day = day;
            this.CodeType = codeType;
        }

        public int Day { get; protected set; }
        public int Year { get; protected set; }
        public CodeType CodeType { get; protected set; }

        #region Part A

        protected abstract object PartA(string input);
        private object RunSafeA(string input)
        {
            try
            {
                return PartA(input);
            }
            catch (NotImplementedException)
            {
                return "[Part A is not implemented yet.]";
            }
        }
        public object PartA()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            string input = GetInput();
            var result = RunSafeA(input);
            stopWatch.Stop();
            Console.WriteLine($"{Year}-{Day} Part A ({CodeType}) [{stopWatch.ElapsedMilliseconds:000000}ms]:    {result?.ToString()}");
            return result ?? String.Empty;
        }

        public object TestPartAResult()
        {
            string input = GetTestInput();
            return RunSafeA(input);
        }

        #region calling functions

        private string FetchInputFile()
        {
            string inputFile = $"Input.{Day:00}";
            return ReadFileContents(inputFile);
        }

        #endregion
        #endregion


        #region Part B

        protected abstract object PartB(string input);
        private object RunSafeB(string input)
        {
            try
            {
                return PartB(input);
            }
            catch (NotImplementedException)
            {
                return "[Part B is not implemented yet.]";
            }
        }

        public object PartB()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            string input = GetInput();
            var result = RunSafeB(input);
            stopWatch.Stop();
            Console.WriteLine($"{Year}-{Day} Part B ({CodeType}) [{stopWatch.ElapsedMilliseconds:000000}ms]:    {result?.ToString()}");
            return result ?? String.Empty;
        }

        public object TestPartBResult()
        {
            string input = GetTestInput();
            return RunSafeB(input);
        }

        #region calling functions

        protected string GetInput()
        {
            string input = ReadFileContents($"Input.{Day:00}");
            return input;
        }

        protected string GetTestInput()
        {
            string input = ReadFileContents($"Test.{Day:00}.in");
            return input;
        }

        #endregion
        #endregion

        private string ReadFileContents(string fileName)
        {
            try
            {
                string baseDir = "";
                //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                //baseDir = baseDir.Substring(0, baseDir.IndexOf("bin"))+"Data\\";
                return File.ReadAllText(baseDir + fileName);
            }
            catch (FileNotFoundException fnfEx)
            {
                Console.WriteLine($"Could not find file '{fileName}'");
                throw fnfEx;
            }
        }

    }
}
