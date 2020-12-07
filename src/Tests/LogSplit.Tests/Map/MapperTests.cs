using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using LogSplit.Map;
using NUnit.Framework;

namespace LogSplit.Tests.Map
{
	public class MapperTests
	{
		[Test]
		public void Mapper_Map()
		{
			var result = "01.01.2020 [INFO] [PC-NAME] The log message".Parse("%{Date} [%{Level}] [%{PC}] %{Message:len(*)}");

			var mapped = Mapper.Map<MapperEntry>(result);

			mapped.Date.ToString().Should().Be(DateTime.Parse("01.01.2020").ToString());
			mapped.Level.Should().Be("INFO");
			mapped.Pc.Should().Be("PC-NAME");
			mapped.Message.Should().Be("The log message");
		}

		[Test]
		public void Mapper_InvalidProperties()
		{
			var result = "01.01.2020 [INFO] [PC-NAME] The log message".Parse("%{Prop1} [%{Prop2}] [%{Prop3}] %{Prop4:len(*)}");

			var mapped = Mapper.Map<MapperEntry>(result);

			mapped.Date.Should().Be(DateTime.MinValue);
			mapped.Level.Should().BeNull();
			mapped.Pc.Should().BeNull();
			mapped.Message.Should().BeNull();
		}

		[Test]
		public void Mapper_SingleProperties()
		{
			// only map the level...
			var result = "01.01.2020 [INFO] [PC-NAME] The log message".Parse("%{Prop1} [%{Level}] [%{Prop3}] %{Prop4:len(*)}");

			var mapped = Mapper.Map<MapperEntry>(result);

			mapped.Date.Should().Be(DateTime.MinValue);
			mapped.Level.Should().Be("INFO");
			mapped.Pc.Should().BeNull();
			mapped.Message.Should().BeNull();
		}

		[Test]
		public void Mapper_IncompleteSetup()
		{
			var result = "01.01.2020 [INFO] [PC-NAME] The log message".Parse("[%{Level}] [%{PC}] %{Message:len(*)}");

			var mapped = Mapper.Map<MapperEntry>(result);

			mapped.Date.Should().Be(DateTime.MinValue);
			mapped.Level.Should().Be("INFO");
			mapped.Pc.Should().Be("PC-NAME");
			mapped.Message.Should().Be("The log message");
		}

		[Test]
		public void Mapper_CaseInsensitive()
		{
			var result = "01.01.2020 [INFO] [PC-NAME] The log message".Parse("%{date} [%{level}] [%{pC}] %{message:len(*)}");

			var mapped = Mapper.Map<MapperEntry>(result);

			mapped.Date.ToString().Should().Be(DateTime.Parse("01.01.2020").ToString());
			mapped.Level.Should().Be("INFO");
			mapped.Pc.Should().Be("PC-NAME");
			mapped.Message.Should().Be("The log message");
		}

		public class MapperEntry
		{
			public DateTime Date { get; set; }

			public string Level { get; set; }

			public string Pc { get; set; }

			public string Message { get; set; }
		}
	}
}
