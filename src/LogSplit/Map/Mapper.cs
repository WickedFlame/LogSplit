using System.Linq;

namespace LogSplit.Map
{
	public class Mapper
	{
		private Mapper(){}

		public static T Map<T>(ParserResult result)
		{
			var instance = typeof(T).CreateInstance();

			foreach (var item in result)
			{
				var properties = typeof(T).GetProperties();
				var propertyInfo = properties.FirstOrDefault(p => p.Name.ToLower() == item.Key.ToLower());
				if (propertyInfo == null)
				{
					continue;
				}

				var converted = PrimitiveTypeConverter.Convert(propertyInfo.PropertyType, item.Value.ToString());
				if (converted == null)
				{
					continue;
				}

				propertyInfo.SetValue(instance, converted, null);
			}

			return (T)instance;
		}
	}
}
