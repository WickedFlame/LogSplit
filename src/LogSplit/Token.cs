namespace LogSplit
{
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
