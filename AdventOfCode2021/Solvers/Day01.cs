using AdventOfCode2021.Common;

namespace AdventOfCode2021.Day01
{
    public class Day01 : Solver
    {
        public Day01() : base(2021, 1, CodeType.Original) { }

        protected override object PartA(string input)
        {
            var valueList = input.ToIntList();
            int prevValue = int.MaxValue;
            int count = 0;
            foreach (var item in valueList)
            {
                if (item > prevValue) count++;
                prevValue = item;
            }
            return count;
        }

        protected override object PartB(string input)
        {
            var valueList = input.ToIntList();
            int prevValue = int.MaxValue;
            int count = 0;
            for (int i = 0; i < valueList.Count - 2; i++)
            {
                int newValue = valueList[i] + valueList[i + 1] + valueList[i + 2];
                if (newValue > prevValue) count++;
                prevValue = newValue;
            }
            return count;
        }
    }
}
