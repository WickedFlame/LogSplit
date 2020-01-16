using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LogSplit
{
    public class Parser
    {
        private readonly string _pattern;

        public Parser(string pattern)
        {
            _pattern = pattern;
        }

        public ParserResult Parse(string value)
        {
            var splits = SplitPattern(_pattern);
            var items = new List<Token>();

            foreach (var scan in splits)
            {
                var separatorLength = scan.Separator != null ? scan.Separator.Length : 0;
                var index = scan.Separator != null ? value.IndexOf(scan.Separator, StringComparison.OrdinalIgnoreCase) + separatorLength : -1;
                if (index < 0)
                {
                    index = value.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
                }

                if (scan.Placeholder != null)
                {

                    var key = scan.Placeholder.Substring(2, scan.Placeholder.Length - 3);

                    items.Add(new Token(key, value.Substring(0, index - separatorLength).Trim()));
                }


                value = value.Substring(index);
            }

            return new ParserResult(items);
        }

        private IEnumerable<Scan> SplitPattern(string pattern)
        {
            var splits = new List<Scan>();
            var split = ScanNext(pattern);
            splits.Add(split);

            while (split.Separator != null)
            {
                var length = pattern.StartIndex(split);

                var splitlenth = split.Separator != null ? pattern.IndexOf(split.Separator, length, StringComparison.OrdinalIgnoreCase) + split.Separator.Length : 0;
                pattern = pattern.Substring(splitlenth);

                split = ScanNext(pattern);
                if (!string.IsNullOrEmpty(split.Placeholder))
                {
                    splits.Add(split);
                }
            }

            return splits;
        }

        private Scan ScanNext(string pattern)
        {
            var start = pattern.IndexOf("%{", 0, StringComparison.OrdinalIgnoreCase);
            if (start > 0)
            {
                return new Scan(null, pattern.Substring(0, start));
            }

            var index = pattern.IndexOf("}", StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                return new Scan(pattern, null);
            }


            var value = pattern.Substring(0, index + 1);
            var nextStart = pattern.IndexOf("%{", index, StringComparison.OrdinalIgnoreCase);
            if (nextStart < 0)
            {
                nextStart = pattern.Length;
            }

            var next = value.Length < nextStart ? pattern.Substring(value.Length, nextStart - value.Length) : null;
            return new Scan(value, next);
        }
    }

    internal static class StringExtensions
    {
        public static int StartIndex(this string pattern, Scan scan)
        {
            var placeholder = scan.Placeholder ?? string.Empty;
            var start = pattern.IndexOf(placeholder, StringComparison.OrdinalIgnoreCase);
            return start + placeholder.Length;
        }
    }

    public class Scan
    {
        public Scan(string placeholder, string separator)
        {
            Placeholder = placeholder;
            Separator = separator;
        }

        public string Separator { get; set; }

        public string Placeholder { get; set; }
    }

    public sealed class ParserResult : ReadOnlyCollection<Token>
    {
        public ParserResult(IList<Token> items)
            : base(items ?? new List<Token>())
        {
        }
    }

    public sealed class Token
    {
        public Token(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public object Value { get; }
    }
}
