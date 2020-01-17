using NUnit.Framework;
using FluentAssertions;

namespace LogSplit.Tests
{
    public class SplitterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Parser_Splitter_WithBracketDelimeters()
        {
            var str = @"[2020-01-14 21:15:41.4079] [INFO]  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [SomeClient.exe] [Thr5] Startup delay: 3 sec remaining";

            var pattern = "[%{date}] [%{level}]";

            var parser = new Parser(pattern);
            var result = parser.Parse(str);

            result.Count.Should().Be(2);

            result[0].Should().BeEquivalentTo(new { Key = "date", Value = "2020-01-14 21:15:41.4079" });
            result[1].Should().BeEquivalentTo(new { Key = "level", Value = "INFO" });
        }

        [Test]
        public void Parser_Splitter_NotAtEnd()
        {
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [SomeClient.exe] [Thr5] Startup delay: 3 sec remaining";

            var pattern = "%{date} %{time} %{level} [%{pc}]";

            var parser = new Parser(pattern);
            var result = parser.Parse(str);

            result.Count.Should().Be(4);

            result[0].Should().BeEquivalentTo(new { Key = "date", Value = "2020-01-14" });
            result[1].Should().BeEquivalentTo(new { Key = "time", Value = "21:15:41.4079" });
            result[2].Should().BeEquivalentTo(new { Key = "level", Value = "INFO" });
            result[3].Should().BeEquivalentTo(new { Key = "pc", Value = "PC-NAME" });
        }
    }
}