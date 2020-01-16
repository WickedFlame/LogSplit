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
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [Opacc.ServiceBus.Client.Common.Program.BaseServiceBusClientWinService] [Thr5] Startup delay: 3 sec remaining";
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
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [Opacc.ServiceBus.Client.Common.Program.BaseServiceBusClientWinService] [Thr5] Startup delay: 3 sec remaining";
            
            var pattern = "%{date} %{time} %{level}";

            var parser = new Parser(pattern);
            var result = parser.Parse(str);

            result.Count.Should().Be(3);

            result[0].Should().BeEquivalentTo(new { Key = "date", Value = "2020-01-14" });
            result[1].Should().BeEquivalentTo(new { Key = "time", Value = "21:15:41.4079" });
            result[2].Should().BeEquivalentTo(new { Key = "level", Value = "INFO" });
        }

        [Test]
        public void Parser_Splitter_WithBracketDelimeters()
        {
            var str = @"[2020-01-14 21:15:41.4079] [INFO]  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [Opacc.ServiceBus.Client.Common.Program.BaseServiceBusClientWinService] [Thr5] Startup delay: 3 sec remaining";

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
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [Opacc.ServiceBus.Client.Common.Program.BaseServiceBusClientWinService] [Thr5] Startup delay: 3 sec remaining";

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