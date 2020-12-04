namespace LogSplit
{
	public static class ParserExtensions
	{
		/// <summary>
		/// Parse a logstring and extract the desired properties
		/// </summary>
		/// <param name="log">the logmessage</param>
		/// <param name="pattern">the parserpattern</param>
		/// <returns></returns>
		public static ParserResult ParseLog(this string log, string pattern)
		{
			var parser = new Parser(pattern);
			return parser.Parse(log);
		}
	}
}
