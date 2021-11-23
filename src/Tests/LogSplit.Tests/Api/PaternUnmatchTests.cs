using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace LogSplit.Tests.Api
{
    public class PaternUnmatchTests
    {
        [Test]
        public void Api_PatternUnmatch()
        {
            var log = $"[{DateTime.Now:o}] [Information] The message";
            var pattern = "[%{Date}] [%{Level}] [%{Message:len(*)}]";

            var parser = new Parser(pattern);
            var result = parser.Parse(log);

            result.Should().Contain(r => r.Key == "Date").And.Contain(r => r.Key == "Level").And.Contain(r => r.Key == "Message");
        }
    }
}
