using System;

namespace LogSplit
{
	public class TokenKey
	{
		public TokenKey(string key)
		{
			Key = key;
		}

		public TokenKey(ScanPattern pattern)
		{
			if (pattern?.Pattern == null)
			{
				return;
			}

			if(!(pattern.Pattern.StartsWith("%{") && pattern.Pattern.EndsWith("}")))
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
}
