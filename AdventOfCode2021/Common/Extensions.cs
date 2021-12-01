namespace AdventOfCode2021.Common
{
    public static class Extensions
    {
        public static List<int> ToIntList(this string input)
        {
            return input.Split(new string[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();
        }
    }
}
