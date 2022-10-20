using System;
using FluentAssertions;
using NUnit.Framework;

namespace LogSplit.Tests.Api
{
    public class PaternUnmatchTests
    {
        [Test]
        public void Api_PatternUnmatch_Last()
        {
            var log = $"[{DateTime.Now:o}] [Information] The message";
            var pattern = "[%{Date}] [%{Level}] [%{Message:len(*)}]";

            var parser = new Parser(pattern);
            var result = parser.Parse(log);

            result.Should().Contain(r => r.Key == "Date").And.Contain(r => r.Key == "Level").And.Contain(r => r.Key == "Message");
        }

        [TestCase("Date", "0001-01-01T00:00:00.0000000")]
        [TestCase("Level", "Information")]
        [TestCase("Message", "The message")]
        public void Api_PatternUnmatch_Last_(string key, string value)
        {
            var log = $"[{DateTime.MinValue:o}] [Information] The message";
            var pattern = "[%{Date}] [%{Level}] [%{Message:len(*)}]";

            var parser = new Parser(pattern);
            var result = parser.Parse(log);

            result.Should().Contain(r => r.Key == key && r.Value == value);
        }

        [Test]
        public void Api_PatternUnmatch_AllToLast()
        {
            var log = $"The message";
            var pattern = "[%{Date}] [%{Level}] [%{Message:len(*)}]";

            var parser = new Parser(pattern);
            var result = parser.Parse(log);

            result.Should().ContainSingle(r => r.Key == "Message" && r.Value == "The message");
        }
    }
}
