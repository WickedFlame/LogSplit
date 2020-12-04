using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace LogSplit.Tests
{
	public class ParserExtensionsTests
	{
		[Test]
		public void ParserExtensions_ParseLog()
		{
			var result = "01.01.2020 [INFO] [PC-NAME] The log message".ParseLog("%{date} [%{level}] [%{pc}] %{message:len(*)}");
			result[0].Should().BeEquivalentTo(new { Key = "date", Value = "01.01.2020" });
			result["date"].Should().Be("01.01.2020");
		}
	}
}
