using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace LogSplit.Tests
{
	public class ParserResultTests
	{
		[Test]
		public void ParserResult_Ctor()
		{
			Assert.DoesNotThrow(() => new ParserResult(new List<Token>(), new List<string>()));
		}

		[Test]
		public void ParserResult_Ctor_NullErrors()
		{
			Assert.DoesNotThrow(() => new ParserResult(new List<Token>(), null));
		}

		[Test]
		public void ParserResult_Ctor_NullItems()
		{
			Assert.DoesNotThrow(() => new ParserResult(null, null));
		}

		[Test]
		public void ParserResult_NullItems()
		{
			var result = new ParserResult(null, null);
			result.Count.Should().Be(0);
		}

		[Test]
		public void ParserResult_NullErrors()
		{
			var result = new ParserResult(null, null);
			result.Errors.Should().BeNull();
		}

		[Test]
		public void ParserResult_Errors()
		{
			var errors = new List<string>();
			var result = new ParserResult(null, errors);
			result.Errors.Should().BeSameAs(errors);
		}

		[Test]
		public void ParserResult_GetKey()
		{
			var items = new List<Token>
			{
				new Token(new TokenKey("key"), "test")
			};
			var result = new ParserResult(items, null);
			result["key"].Should().BeSameAs(items[0].Value);
		}
	}
}
