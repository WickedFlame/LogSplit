namespace LogSplit
{
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
}
