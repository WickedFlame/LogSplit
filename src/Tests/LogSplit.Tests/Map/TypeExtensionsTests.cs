using System;
using System.Collections.Generic;
using System.Text;
using LogSplit.Map;
using NUnit.Framework;

namespace LogSplit.Tests.Map
{
	public class TypeExtensionsTests
	{
		[Test]
		public void TypeExtensions_CreateInstance()
		{
			Assert.IsNotNull(typeof(TypeExtensionsTests).CreateInstance());
		}

		[Test]
		public void TypeExtensions_CreateInstance_CheckType()
		{
			Assert.IsInstanceOf<TypeExtensionsTests>(typeof(TypeExtensionsTests).CreateInstance());
		}
	}
}
