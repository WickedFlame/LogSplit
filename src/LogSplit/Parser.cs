namespace LogSplit
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

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
            var errors = new List<string>();

            foreach (var scan in splits)
            {
                var nextSplit = value.NextSplitIndex(scan);

                if (scan.Pattern != null)
                {
                    var key = new TokenKey(scan.Pattern);

                    if (nextSplit < scan.SeparatorLength)
                    {
                        errors.Add($"Could not split value due to invalid pattern\n - Value: \"{value}\"\n - Invalid delimeter: \"{scan.Separator}\".\n - Preceding pattern: \"{scan.Pattern.Pattern}\"");
                        items.Add(new Token(new TokenKey("Remaining"), value));
                        break;
                    }

                    items.Add(new Token(key, value.Substring(0, nextSplit - scan.SeparatorLength).Trim()));
                }


                value = value.Substring(nextSplit);
            }

            return new ParserResult(items, errors);
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
                if (split.Pattern != null)
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
            var key = new TokenKey(scan.Pattern);
            var placeholder = key.Key ?? string.Empty;
            var start = pattern.IndexOf(placeholder, StringComparison.OrdinalIgnoreCase);
            return start + placeholder.Length;
        }

        public static int NextSplitIndex(this string value, Scan scan)
        {
            var len = 0;
            var function = new ScanFunction(scan.Pattern);
            if (!string.IsNullOrEmpty(function.Name))
            {
                switch (function.Name)
                {
                    case "len":
                        len = (int) function.Evaluate();
                        break;
                }
            }

            if (len > value.Length)
            {
                return value.Length;
            }

            var nextSplit = scan.Separator != null ? value.IndexOf(scan.Separator, len, StringComparison.OrdinalIgnoreCase) + scan.SeparatorLength : -1;
            if (nextSplit < 0)
            {
                nextSplit = value.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
            }

            return nextSplit;
        }
    }

    internal class Scan
    {
        public Scan(string pattern, string separator)
        {
            Separator = separator;

            if (!string.IsNullOrEmpty(pattern))
            {
                Pattern = new ScanPattern(pattern);
            }
        }

        public string Separator { get; }

        public ScanPattern Pattern { get; }

        public int SeparatorLength => Separator?.Length ?? 0;
    }

    public class ScanPattern
    {
        public ScanPattern(string pattern)
        {
            Pattern = pattern;
        }

        public string Pattern { get; set; }
    }

    internal class ScanFunction
    {
        public ScanFunction(ScanPattern pattern)
        {
            if (pattern?.Pattern == null || !pattern.Pattern.Contains(":"))
            {
                return;
            }

            var function = pattern.Pattern.Substring(2, pattern.Pattern.Length - 3).Split(':')[1];

            Name = function.Split('(')[0];

            if (!function.Contains("("))
            {
                Parameters = new string[] {};
                return;
            }

            var parameterString = function.Substring(Name.Length + 1, function.Length - (Name.Length + 2));
            Parameters = parameterString.Split(',');

        }

        public string Name { get; }

        public string[] Parameters { get; }

        public object Evaluate()
        {
            switch (Name)
            {
                case "len":
                    if (Parameters.Length < 1)
                    {
                        throw new InvalidOperationException("There are no parameters defined in the function len. Please define the length of the string to parse");
                    }

                    if (int.TryParse(Parameters[0], out int len))
                    {
                        return len;
                    }

                    if (Parameters[0] == "*")
                    {
                        return int.MaxValue;
                    }

                    throw new InvalidOperationException("Invalid parameters defined in the function len. Valid parameters are integers or *");
            }

            return null;
        }
    }

    public class TokenKey
    {
        public TokenKey(string key)
        {
            Key = key;
        }

        public TokenKey(ScanPattern pattern)
        {
            if (pattern == null)
            {
                return;
            }

            Key = pattern.Pattern.Substring(2, pattern.Pattern.Length - 3);

            if (Key.IndexOf(":", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Key = Key.Split(':')[0];
            }
        }

        public string Key { get; }

        public override string ToString()
        {
            return Key;
        }
    }

    public sealed class ParserResult : ReadOnlyCollection<Token>
    {
        public ParserResult(IList<Token> items, IEnumerable<string> errors)
            : base(items ?? new List<Token>())
        {
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; set; }
    }

    public sealed class Token
    {
        private readonly TokenKey _key;

        public Token(TokenKey key, object value)
        {
            _key = key;
            Value = value;
        }

        public string Key => _key.Key;

        public object Value { get; }
    }
}
