using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace LogSplit.Tests
{
    public class Functions
    {
        [Test]
        public void Parser_Functions_Length()
        {
            var str = @"2020-01-14 21:15:41.4079 INFO  [PC-NAME] [PC-NAME\iis.service] [5640:management.tool.agent.exe] [Opacc.ServiceBus.Client.Common.Program.BaseServiceBusClientWinService] [Thr5] Startup delay: 3 sec remaining";

            var pattern = "%{date:len(24)} %{level}";

            var parser = new Parser(pattern);
            var result = parser.Parse(str);

            result.Count.Should().Be(2);

            result[0].Should().BeEquivalentTo(new { Key = "date", Value = "2020-01-14 21:15:41.4079" });
            result[1].Should().BeEquivalentTo(new { Key = "level", Value = "INFO" });
        }
    }
}
