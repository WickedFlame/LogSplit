using System.Collections.Generic;
using System.Linq;

namespace LogSplit.Map
{
	public class Mapper
	{
		private Mapper(){}

		public static T Map<T>(ParserResult result)
		{
			var instance = typeof(T).CreateInstance();

			var unconverted = new List<string>();

			foreach (var item in result)
			{
				var properties = typeof(T).GetProperties();
				var propertyInfo = properties.FirstOrDefault(p => p.Name.ToLower() == item.Key.ToLower());
				if (propertyInfo == null)
				{
					continue;
				}

				var value = unconverted.Any() ? $"{string.Join(string.Empty, unconverted.Select(v => v))}{item.Raw}" : item.Value;
				var converted = PrimitiveTypeConverter.Convert(propertyInfo.PropertyType, value);
				if (!converted.Converted || converted.Value == null)
				{
					//try without unconverted values
					converted = PrimitiveTypeConverter.Convert(propertyInfo.PropertyType, item.Value);
					if (!converted.Converted || converted.Value == null)
					{
						unconverted.Add(item.Raw);
						continue;
					}

					// conversion was successful so delete the list with unconverted
					unconverted.Clear();
				}

				propertyInfo.SetValue(instance, converted.Value, null);
			}

			return (T)instance;
		}
	}
}
