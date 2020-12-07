using System;
using System.Globalization;
using System.Linq;
using LogSplit.Map;

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
		public static ParserResult Parse(this string log, string pattern)
		{
			var parser = new Parser(pattern);
			return parser.Parse(log);
		}

		public static T Parse<T>(this Parser parser, string pattern)
		{
			var result = parser.Parse(pattern);
			return Mapper.Map<T>(result);
		}

		public static T Parse<T>(this string log, string pattern)
		{
			var result = log.Parse(pattern);

			return Mapper.Map<T>(result);
		}
	}
}
