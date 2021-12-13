using System;
using System.Collections.Generic;
using System.Text;
using LogSplit.Map;
using NUnit.Framework;

namespace LogSplit.Tests.Map
{
	public class PrimitiveTypeConverterTests
	{
		private T Map<T>(string value)
		{
			return (T)PrimitiveTypeConverter.Convert(typeof(T), value).Value;
		}

		[Test]
		public void PrimitiveTypeConverter_String()
		{
			Assert.AreEqual(Map<string>("String"), "String");
		}

		[Test]
		public void PrimitiveTypeConverter_Bool()
		{
			Assert.AreEqual(Map<bool>("0"), false);
			Assert.AreEqual(Map<bool>("1"), true);
			Assert.AreEqual(Map<bool>("false"), false);
			Assert.AreEqual(Map<bool>("true"), true);
		}

		[Test]
		public void PrimitiveTypeConverter_Long()
		{
			Assert.AreEqual(Map<long>("123456789"), 123456789);
		}

		[Test]
		public void PrimitiveTypeConverter_Int()
		{
			Assert.AreEqual(Map<int>("2"), 2);
		}

		[Test]
		public void PrimitiveTypeConverter_Decimal()
		{
			Assert.AreEqual(Map<decimal>("2.2"), 2.2M);
		}

		[Test]
		public void PrimitiveTypeConverter_Double()
		{
			Assert.AreEqual(Map<double>("2.2"), 2.2);
		}

		[Test]
		public void PrimitiveTypeConverter_DateTime()
		{
			Assert.AreEqual(Map<DateTime>("2020.01.01 12:12:12"), DateTime.Parse("2020.01.01 12:12:12"));
		}

		[Test]
		public void PrimitiveTypeConverter_Guid()
		{
			Assert.AreEqual(Map<Guid>("1FB53019-AD8C-4FBD-B528-BCD07F6EB6BA"), Guid.Parse("1FB53019-AD8C-4FBD-B528-BCD07F6EB6BA"));
		}

		[Test]
		public void PrimitiveTypeConverter_Type()
		{
			Assert.AreEqual(
				Map<Type>("LogSplit.Tests.Map.PrimitiveTypeConverterTests, LogSplit.Tests"),
				this.GetType());
		}
	}
}
