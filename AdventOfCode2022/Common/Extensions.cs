﻿using System.Runtime.CompilerServices;

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

        public static (int x, int y) Sum(this (int x, int y) own, (int x, int y) other)
        {
            return (own.x + other.x, own.y + other.y);
        }
        public static (int x, int y) Subtract(this (int x, int y) own, (int x, int y) other)
        {
            return (own.x - other.x, own.y - other.y);
        }

        public static (int x, int y) NormalVector(this (int x, int y) own)
        {
            return (Math.Sign(own.x), Math.Sign(own.y));
        }

        public static int Abs(this int x) { return Math.Abs(x); }

        public static int ToInt(this string s) { return int.Parse(s); }

        public static bool Touches(this (int from, int to) own, (int from, int to) other)
        {
            return (own.from <= other.to) && (other.from <= own.to);
        }
        public static (int from, int to) Union(this (int from, int to) own, (int from, int to) other)
        {
            return (from: Math.Min(own.from, other.from), to: Math.Max(own.to, other.to));
        }

    }

}
