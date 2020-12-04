using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace LogSplit.Tests
{
	public class TokenKeyTests
	{
		[Test]
		public void TokenKey_Ctor()
		{
			Assert.DoesNotThrow(() => new TokenKey("test"));
		}

		[Test]
		public void TokenKey_Ctor_ScanPattern()
		{
			Assert.DoesNotThrow(() => new TokenKey(new ScanPattern("test")));
		}

		[Test]
		public void TokenKey_String_Key()
		{
			var key = new TokenKey("test");
			key.Key.Should().Be("test");
		}

		[Test]
		public void TokenKey_ScanPattern_Key()
		{
			var key = new TokenKey(new ScanPattern("%{test}"));
			key.Key.Should().Be("test");
		}

		[Test]
		public void TokenKey_ScanPattern_Key_NoPattern()
		{
			var key = new TokenKey(new ScanPattern(null));
			key.Key.Should().BeNull();
		}

		[Test]
		public void TokenKey_ScanPattern_Key_InvalidPattern()
		{
			var key = new TokenKey(new ScanPattern("test"));
			key.Key.Should().BeNull();
		}

		[Test]
		public void TokenKey_ScanPattern_KeyWithFunction()
		{
			var key = new TokenKey(new ScanPattern("%{test:len(10)}"));
			key.Key.Should().Be("test");
		}

		[Test]
		public void TokenKey_ScanPattern_ToString()
		{
			var key = new TokenKey("test");
			key.ToString().Should().Be(key.Key);
		}
	}
}
