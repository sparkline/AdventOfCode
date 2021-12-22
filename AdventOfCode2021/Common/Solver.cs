using System.Diagnostics;

namespace AdventOfCode2021.Common
{
    public abstract class Solver
    {
        protected Solver(int year, int day, int iteration = 0, string description = "")
        {
            this.Year = year;
            this.Day = day;
            this.iteration = iteration;
            this.description = description;
        }

        private Stopwatch stopwatch = new Stopwatch();
        public int Day { get; protected set; }
        public int Year { get; protected set; }
        public int iteration { get; protected set; }

        public string description { get; protected set; }

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
            string input = GetInput();
            StartSW();
            var result = RunSafeA(input);
            stopwatch.Stop();
            string formattedDescription = string.IsNullOrWhiteSpace(description) ? "" : $" - {description}";
            Console.WriteLine($"[{stopwatch.ElapsedMilliseconds:000000}ms] {Year}-{Day} Part A - Iteration {iteration}{formattedDescription}");
            Console.WriteLine();
            Console.WriteLine(result?.ToString());
            Console.WriteLine();
            return result ?? String.Empty;
        }

        public object TestPartAResult()
        {
            string input = GetTestInput();
            return TestPartAResult(input);
        }
        public object TestPartAResult(string input)
        {
            return RunSafeA(input);
        }

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
            string input = GetInput();
            StartSW();
            var result = RunSafeB(input);
            stopwatch.Stop();
            string formattedDescription = string.IsNullOrWhiteSpace(description) ? "" : $" - {description}";
            Console.WriteLine($"[{stopwatch.ElapsedMilliseconds:000000}ms] {Year}-{Day} Part B - Iteration {iteration}{formattedDescription}");
            Console.WriteLine();
            Console.WriteLine(result?.ToString());
            Console.WriteLine();
            return result ?? String.Empty;
        }

        public object TestPartBResult()
        {
            string input = GetTestInput();
            return TestPartBResult(input);
        }

        public object TestPartBResult(string input)
        {
            return RunSafeB(input);
        }

        #region calling functions

        protected string GetInput()
        {
            string dir = @"C:\Users\jeroenvonk\source\repos\AdventOfCode\AdventOfCode2021\Data\";
            string input = ReadFileContents($"{dir}Input.{Day:00}");
            return input;
        }

        protected string GetTestInput()
        {
            string dir = @"C:\Users\jeroenvonk\source\repos\AdventOfCode\UnitTests\Data\";
            string input = ReadFileContents($"{dir}Test.{Day:00}.in");
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

        protected void StartSW()
        {
            stopwatch.Restart();
        }

    }
}
