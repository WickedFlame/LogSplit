using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace LogSplit.Tests
{
	public class ParseMapperTests
	{
		[Test]
		public void Parser_Map()
		{
			var parser = new Parser("%{Date} [%{Level}] [%{PC}] %{Message:len(*)}");
			var result = parser.Parse<LogEntry>("01.01.2020 [INFO] [PC-NAME] The log message");
			result.Date.ToString().Should().Be(DateTime.Parse("01.01.2020").ToString());
			result.Level.Should().Be("INFO");
			result.Pc.Should().Be("PC-NAME");
			result.Message.Should().Be("The log message");
		}

		[Test]
		public void ParseMapper_StringExtension()
		{
			var result = "01.01.2020 [INFO] [PC-NAME] The log message".Parse<LogEntry>("%{Date} [%{Level}] [%{PC}] %{Message:len(*)}");
			result.Date.ToString().Should().Be(DateTime.Parse("01.01.2020").ToString());
			result.Level.Should().Be("INFO");
			result.Pc.Should().Be("PC-NAME");
			result.Message.Should().Be("The log message");
		}

		public class LogEntry
		{
			public DateTime Date { get; set; }

			public string Level { get; set; }

			public string Pc { get; set; }

			public string Message { get; set; }
		}
	}
}
