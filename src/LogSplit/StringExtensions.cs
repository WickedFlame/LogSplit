using System;

namespace LogSplit
{
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
						len = (int)function.Evaluate();
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
}
