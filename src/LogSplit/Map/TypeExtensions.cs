using System;

namespace LogSplit.Map
{
	public static class TypeExtensions
	{
		public static object CreateInstance(this Type type)
		{
			try
			{
				return Activator.CreateInstance(type);
			}
			catch (Exception e)
			{
				throw new ArgumentException($"Could not create an instance of Type {type.FullName}", e);
			}
		}
	}
}
