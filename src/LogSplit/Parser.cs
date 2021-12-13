using System;
using System.Collections.Generic;

namespace LogSplit
{
    /// <summary>
    /// A parser that separates a string based on the pattern provided
    /// </summary>
    public class Parser
    {
        private readonly string _pattern;

        /// <summary>
        /// Creates a new parser that uses the pattern to split a string
        /// </summary>
        /// <param name="pattern"></param>
        public Parser(string pattern)
        {
            _pattern = pattern;
        }

        /// <summary>
        /// Parse the string based on the pattern
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ParserResult Parse(string value)
        {
            var splits = SplitPattern(_pattern);
            var items = new List<Token>();
            var errors = new List<string>();

            foreach (var scan in splits)
            {
                var nextSplit = value.NextSplitIndex(scan);
                var length = scan.SeparatorLength;

				if (scan.Pattern != null)
                {
                    var key = new TokenKey(scan.Pattern);

                    if (nextSplit < scan.SeparatorLength)
                    {
	                    errors.Add($"Could not split value due to invalid pattern\n - Value: \"{value}\"\n - Invalid delimeter: \"{scan.Separator}\".\n - Preceding pattern: \"{scan.Pattern.Pattern}\"");

						var subscan = scan;
						while (nextSplit > 0)
	                    {
		                    subscan = new Scan(subscan.Pattern.Pattern, subscan.Separator.Substring(0, subscan.Separator.Length - 1));
		                    nextSplit = value.NextSplitIndex(subscan);
							length = subscan.SeparatorLength;
							if (nextSplit >= length)
							{
								break;
							}
	                    }

						if(nextSplit <= 0)
						{
							continue;
						}
                        
                    }

                    items.Add(new Token(key, value.Substring(0, nextSplit == value.Length ? nextSplit : nextSplit - length), value.Substring(0, nextSplit)));
                }


                value = value.Substring(nextSplit);
            }

            if (value.Length > 0)
            {
	            items.Add(new Token(new TokenKey("Remaining"), value));
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
}
