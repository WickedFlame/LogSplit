using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace LogSplit.Tests
{
	public class ScanPatternTests
	{
		[Test]
		public void ScanPattern_Ctor()
		{
			var pattern = new ScanPattern("test");
		}

		[Test]
		public void ScanPattern_Pattern()
		{
			var pattern = new ScanPattern("test");
			pattern.Pattern.Should().Be("test");
		}
	}
}
