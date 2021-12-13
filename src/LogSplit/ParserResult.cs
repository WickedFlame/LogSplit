using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LogSplit
{
	/// <summary>
	/// A result that is created by the parser
	/// </summary>
	public sealed class ParserResult : ReadOnlyCollection<Token>
	{
		public ParserResult(IList<Token> items, IEnumerable<string> errors)
			: base(items ?? new List<Token>())
		{
			Errors = errors;
		}

		/// <summary>
		/// Errors that occured while parsing
		/// </summary>
		public IEnumerable<string> Errors { get; set; }

		/// <summary>
		/// Gets the parsed items
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object this[string key]
		{
			get { return this.FirstOrDefault(i => i.Key == key)?.Value; }
		}
	}
}
