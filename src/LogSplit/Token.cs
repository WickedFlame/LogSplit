namespace LogSplit
{
	/// <summary>
	/// A parsed token
	/// </summary>
	public sealed class Token
	{
		private readonly TokenKey _key;

		/// <summary>
		/// Creates a token
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public Token(TokenKey key, string value) : this(key, value, value)
		{
		}

		/// <summary>
		/// Creates a token additionally containing the raw value
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="raw"></param>
		public Token(TokenKey key, string value, string raw)
		{
			_key = key;
			Value = value.Trim();
			Raw = raw;
		}

		/// <summary>
		/// Gets the token key
		/// </summary>
		public string Key => _key.Key;

		/// <summary>
		/// Gets the token value
		/// </summary>
		public string Value { get; }

		/// <summary>
		/// Gets the raw value
		/// </summary>
		public string Raw { get; }
	}
}
