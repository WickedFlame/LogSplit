using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace LogSplit.Tests
{
    public class ParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Parser_1()
        {
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [SomeClient.exe] [Thr5] Startup delay: 3 sec remaining";
            //str = @"2020-01-15 10:05:50.1545 ERROR [PC-NAME] [PC-NAME\iis.service] [6128:management.tool.agent.exe] [management.tool.Agent.CacheManagerCore] [Thr11] Maintain failed: ReloadOOWebPortalConfigAndInitiateActivities failed: Creation of ServiceBus-Connection";

            //var pattern = "%{TIMESTAMP:timestamp} %{WORD:time} %{WORD:level}  [%{WORD:client}] [%{WORD:user}] [%{WORD:agent}] [%{WORD:source}] [%{WORD:thread}] %{GREEDYDATA:message}";
            //var pattern = "%{TIMESTAMP:timestamp} %{GREEDYDATA:message}";
            //var pattern = "%{YEAR:year}-%{MONTHNUM:month}-%{MONTHDAY:day}";
            var pattern = "%{year}-%{month}-%{day}";

            var parser = new Parser(pattern);
            var result = parser.Parse(str);

            result[0].Should().BeEquivalentTo(new {Key = "year", Value = "2020"});
            result[1].Should().BeEquivalentTo(new { Key = "month", Value = "01" });
            result[2].Should().BeEquivalentTo(new { Key = "day", Value = "14" });
        }

        [Test]
        public void Parser_2()
        {
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [SomeClient.exe] [Thr5] Startup delay: 3 sec remaining";
            
            var pattern = "%{date} %{time} %{level}";

            var parser = new Parser(pattern);
            var result = parser.Parse(str);

            result.Count.Should().Be(3);

            result[0].Should().BeEquivalentTo(new { Key = "date", Value = "2020-01-14" });
            result[1].Should().BeEquivalentTo(new { Key = "time", Value = "21:15:41.4079" });
            result[2].Should().BeEquivalentTo(new { Key = "level", Value = "INFO" });
        }

        [Test]
        public void Parser_Large()
        {
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [SomeClient.exe] [Thr5] Startup delay: 3 sec remaining";

            var pattern = "%{date:len(24)} %{level} [%{pc}] [%{user}] [%{service}] [%{client}] [%{thread}] %{message:len(*)}";

            var parser = new Parser(pattern);
            var result = parser.Parse(str);

            result.Count.Should().Be(8);

            result[0].Should().BeEquivalentTo(new { Key = "date", Value = "2020-01-14 21:15:41.4079" });
            result[1].Should().BeEquivalentTo(new { Key = "level", Value = "INFO" });
            result[2].Should().BeEquivalentTo(new { Key = "pc", Value = "PC-NAME" });
            result[3].Should().BeEquivalentTo(new { Key = "user", Value = @"PC-NAME\iis.service" });
            result[4].Should().BeEquivalentTo(new { Key = "service", Value = "5640:management.tool.agent.exe" });
            result[5].Should().BeEquivalentTo(new { Key = "client", Value = "SomeClient.exe" });
            result[6].Should().BeEquivalentTo(new { Key = "thread", Value = "Thr5" });
            result[7].Should().BeEquivalentTo(new { Key = "message", Value = "Startup delay: 3 sec remaining" });
        }

        [Test]
        public void Parser_Invalid()
        {
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [SomeClient.exe] [Thr5] Startup delay: 3 sec remaining";

            var pattern = "%{date:len(24)} %{level} [%{pc}] [%{user}] [%{service}] [%{client}] {%{thread}] {message:len(*)}";

            var parser = new Parser(pattern);
            var result = parser.Parse(str);

            result.Count.Should().Be(6);

            result.Errors.Count().Should().Be(1);

            result[0].Should().BeEquivalentTo(new { Key = "date", Value = "2020-01-14 21:15:41.4079" });
            result[1].Should().BeEquivalentTo(new { Key = "level", Value = "INFO" });
            result[2].Should().BeEquivalentTo(new { Key = "pc", Value = "PC-NAME" });
            result[3].Should().BeEquivalentTo(new { Key = "user", Value = @"PC-NAME\iis.service" });
            result[4].Should().BeEquivalentTo(new { Key = "service", Value = "5640:management.tool.agent.exe" });
            result[5].Should().BeEquivalentTo(new { Key = "Remaining", Value = "SomeClient.exe] [Thr5] Startup delay: 3 sec remaining" });
        }

        [Test]
        public void Parser_ErrorMessage()
        {
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [SomeClient.exe] [Thr5] Startup delay: 3 sec remaining";

            var pattern = "%{date:len(24)} %{level} [%{pc}] [%{user}] [%{service}] [%{client}] {%{thread}] {message:len(*)}";

            var parser = new Parser(pattern);
            var result = parser.Parse(str);

            result.Errors.Count().Should().Be(1);
            result.Errors.First().Should().Be("Could not split value due to invalid pattern\n - Value: \"SomeClient.exe] [Thr5] Startup delay: 3 sec remaining\"\n - Invalid delimeter: \"] {\".\n - Preceding pattern: \"%{client}\"");
        }
    }
}