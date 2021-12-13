using System;

namespace LogSplit.Map
{
	/// <summary>
	/// Extensions for <see cref="Type"/>
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Create a instance of the given type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
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
