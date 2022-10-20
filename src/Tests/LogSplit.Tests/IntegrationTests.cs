using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace LogSplit.Tests
{
	public class IntegrationTests
	{
		[Test]
		public void IntegrationTakeRest()
		{
			var nolen = "2020-12-02T14:57:55.4885443+01:00 [Update] [Critical]  - [general.txt] This is generally a longer message but is kept short for testing"
				.Parse("%{Date} [%{Type}] [%{Severity}] %{Message:len(*)}");
			var len = "2020-12-02T14:57:55.4885443+01:00 [Update] [Critical]  - [general.txt] This is generally a longer message but is kept short for testing"
				.Parse("%{Date} [%{Type}] [%{Severity}] %{Message:len(*)}");

			for (var i = 0; i < nolen.Count; i++)
			{
				Assert.AreEqual(nolen[i].Key, len[i].Key);
				Assert.AreEqual(nolen[i].Value, len[i].Value);
			}
		}

		[Test]
		public void Integration_SplitInBrackets()
		{
			var split = "2020-12-07T16:11:29.2087116+01:00 [Info] [Minor] [general.txt] [Line: 7,0] This is generally a longer message but is kept short for testing"
				.Parse("%{Date} [%{Type}] [%{Severity}] [%{File}] [Line: %{Line},%{Index}] %{Message:len(*)}");

			Assert.AreEqual("2020-12-07T16:11:29.2087116+01:00", split["Date"]);
			Assert.AreEqual("Info", split["Type"]);
			Assert.AreEqual("Minor", split["Severity"]);
			Assert.AreEqual("general.txt", split["File"]);
			Assert.AreEqual("7", split["Line"]);
			Assert.AreEqual("0", split["Index"]);
			Assert.AreEqual("This is generally a longer message but is kept short for testing", split["Message"]);
		}

		[Test]
		public void Integration_FillLastIfNotAllCriteriasAreMet()
		{
			var result = "2020-12-07T16:11:29.2087116+01:00 [Info] [Minor] This is generally a longer message but is kept short for testing"
				.Parse("%{Date} [%{Type}] [%{Severity}] [%{File}] [Line: %{Line},%{Index}] %{Message:len(*)}");

			Assert.AreEqual("2020-12-07T16:11:29.2087116+01:00", result["Date"]);
			Assert.AreEqual("Info", result["Type"]);
			Assert.AreEqual("Minor", result["Severity"]);
			Assert.AreEqual(null, result["File"]);
			Assert.AreEqual(null, result["Line"]);
			Assert.AreEqual(null, result["Index"]);
			Assert.AreEqual("This is generally a longer message but is kept short for testing", result["Message"]);

			Assert.AreEqual(4, result.Errors.Count());
		}

		[Test]
		public void Integration_ParseToType_DatePattern_NotMet()
		{
			Assert.DoesNotThrow(() => "This is the message".Parse<ParseToType>("%{Date} [%{Type}] [%{Severity}] %{Message:len(*)}"));
		}

		[Test]
		public void Integration_ParseToType_MoveDataToNext()
		{
			"This is the message".Parse<ParseToType>("%{Date} [%{Type}] [%{Severity}] %{Message:len(*)}")
				.Message.Should().Be("This is the message");
		}

		public class ParseToType
        {
            public DateTime Date { get; set; }

            public string Type { get; set; }

            public string Severity { get; set; }

            public string Message { get; set; }
        }
	}
}
