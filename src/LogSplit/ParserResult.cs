using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LogSplit
{
	public sealed class ParserResult : ReadOnlyCollection<Token>
	{
		public ParserResult(IList<Token> items, IEnumerable<string> errors)
			: base(items ?? new List<Token>())
		{
			Errors = errors;
		}

		public IEnumerable<string> Errors { get; set; }

		public object this[string key]
		{
			get { return this.FirstOrDefault(i => i.Key == key)?.Value; }
		}
	}
}
