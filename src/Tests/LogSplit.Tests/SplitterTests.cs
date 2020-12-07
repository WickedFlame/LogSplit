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
            var result = @"[2020-01-14 21:15:41.4079] [INFO]  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [SomeClient.exe] [Thr5] Startup delay: 3 sec remaining"
	            .Parse("[%{date}] [%{level}]");

            result.Count.Should().Be(3);

            result[0].Should().BeEquivalentTo(new { Key = "date", Value = "2020-01-14 21:15:41.4079" });
            result[1].Should().BeEquivalentTo(new { Key = "level", Value = "INFO" });
        }

        [Test]
        public void Parser_Splitter_NotAtEnd()
        {
            var result = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [SomeClient.exe] [Thr5] Startup delay: 3 sec remaining"
	            .Parse("%{date} %{time} %{level} [%{pc}]");

            result.Count.Should().Be(5);

            result[0].Should().BeEquivalentTo(new { Key = "date", Value = "2020-01-14" });
            result[1].Should().BeEquivalentTo(new { Key = "time", Value = "21:15:41.4079" });
            result[2].Should().BeEquivalentTo(new { Key = "level", Value = "INFO" });
            result[3].Should().BeEquivalentTo(new { Key = "pc", Value = "PC-NAME" });
        }
    }
}