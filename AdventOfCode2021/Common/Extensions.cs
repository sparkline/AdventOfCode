namespace AdventOfCode2021.Common
{
    public static class Extensions
    {
        public enum SeparatorOption
        {
            NewLines,
            Spaces,
            Comma
        }

        public static List<int> ToIntList(this string input, SeparatorOption separatorOption = SeparatorOption.NewLines)
        {
            return SplitOnNewline(input, true, separatorOption).Select(s => int.Parse(s)).ToList();
        }

        public static List<string> SplitOnNewline(this string input, bool removeEmptyLines = true, SeparatorOption separatorOption = SeparatorOption.NewLines)
        {
            StringSplitOptions options = removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            string[] separator = getSeparator(separatorOption);
            return input.Split(separator, options).ToList();
        }

        private static string[] getSeparator(SeparatorOption separatorOption) => separatorOption switch
        {
            SeparatorOption.NewLines => new string[] { "\r\n", "\r", "\n" },
            SeparatorOption.Spaces => new string[] { " ", "\t" },
            SeparatorOption.Comma => new string[] { "," },
            _ => throw new ArgumentException(),
        };
    }

}
