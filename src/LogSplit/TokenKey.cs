using System;

namespace LogSplit
{
	/// <summary>
	/// The TokenKey
	/// </summary>
	public class TokenKey
	{
		/// <summary>
		/// creates a new token key
		/// </summary>
		/// <param name="key"></param>
		public TokenKey(string key)
		{
			Key = key;
		}

		/// <summary>
		/// Creates a new token key based on the <see cref="ScanPattern"/>
		/// </summary>
		/// <param name="pattern"></param>
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

		/// <summary>
		/// The key of the token
		/// </summary>
		public string Key { get; }

		/// <summary>
		/// Gets the <see cref="Key"/>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Key;
		}
	}
}
