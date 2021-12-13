using LogSplit.Map;

namespace LogSplit
{
	/// <summary>
	/// Extensions for <see cref="Parser"/>
	/// </summary>
	public static class ParserExtensions
	{
		/// <summary>
		/// Parse a logstring and extract the desired properties
		/// </summary>
		/// <param name="log">the logmessage</param>
		/// <param name="pattern">the parserpattern</param>
		/// <returns></returns>
		public static ParserResult Parse(this string log, string pattern)
		{
			var parser = new Parser(pattern);
			return parser.Parse(log);
		}

		/// <summary>
		/// Parse a value to a object
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parser"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T Parse<T>(this Parser parser, string value)
		{
			var result = parser.Parse(value);
			return Mapper.Map<T>(result);
		}

		/// <summary>
		/// Parse a string to a object based on the pattern
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="log"></param>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public static T Parse<T>(this string log, string pattern)
		{
			var result = log.Parse(pattern);

			return Mapper.Map<T>(result);
		}
	}
}
