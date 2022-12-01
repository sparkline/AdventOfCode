namespace AdventOfCode2022.Common
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

        private static Dictionary<char, string> hex2binLookupTable = new Dictionary<char, string>
        {
             {'0', "0000"}
            ,{'1', "0001"}
            ,{'2', "0010"}
            ,{'3', "0011"}
            ,{'4', "0100"}
            ,{'5', "0101"}
            ,{'6', "0110"}
            ,{'7', "0111"}
            ,{'8', "1000"}
            ,{'9', "1001"}
            ,{'A', "1010"}
            ,{'B', "1011"}
            ,{'C', "1100"}
            ,{'D', "1101"}
            ,{'E', "1110"}
            ,{'F', "1111"}
        };

        public static string Hex2Bin(this char input)
        {
            return hex2binLookupTable[input];
        }

        public static string Hex2Bin(this string input)
        {
            return string.Concat(input.SelectMany(c => c.Hex2Bin()));
        }

        public static IEnumerable<T> IntersectWithDuplicates<T>(this IEnumerable<T> input, IEnumerable<T> other)
        {
            // use as tokens
            List<T> available = new List<T>(other);
            // Add if we have a token
            return input.Where(c => available.Remove(c));
        }

        public static IEnumerable<TKey> GetNonNull<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey?> keySelector)
            where TKey : struct
        {
            return source.Select(keySelector)
                .Where(x => x.HasValue)
                .Select(x => x.Value);
        }


    }

}
