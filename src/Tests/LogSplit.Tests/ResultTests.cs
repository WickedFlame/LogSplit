using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace LogSplit.Tests
{
    public class ResultTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Parser_Result_KeyIndexer()
        {
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [SomeClient.exe] [Thr5] Startup delay: 3 sec remaining";

            var pattern = "%{date:len(24)} %{level} [%{pc}] [%{user}] [%{service}] [%{client}] [%{thread}] %{message:len(*)}";

            var parser = new Parser(pattern);
            var result = parser.Parse(str);

            result["date"].Should().Be("2020-01-14 21:15:41.4079");
            result["level"].Should().Be("INFO");
            result["pc"].Should().BeEquivalentTo("PC-NAME");
            result["user"].Should().BeEquivalentTo(@"PC-NAME\iis.service");
            result["service"].Should().BeEquivalentTo("5640:management.tool.agent.exe");
            result["client"].Should().BeEquivalentTo("SomeClient.exe");
            result["thread"].Should().BeEquivalentTo("Thr5");
            result["message"].Should().BeEquivalentTo("Startup delay: 3 sec remaining");
        }

        [Test]
        public void Parser_Result_Invalid_KeyIndexer()
        {
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [SomeClient.exe] [Thr5] Startup delay: 3 sec remaining";

            var pattern = "%{date:len(24)} %{level} [%{pc}] [%{user}] [%{service}] [%{client}] [%{thread}] %{message:len(*)}";

            var parser = new Parser(pattern);
            var result = parser.Parse(str);

            result["invalid"].Should().BeNull();
        }
    }
}