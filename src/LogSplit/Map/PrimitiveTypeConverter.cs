using System;
using System.Globalization;

namespace LogSplit.Map
{
	public class PrimitiveTypeConverter
	{
		public static ConvertedValue Convert(Type type, string value)
		{
			if (type == typeof(string))
			{
				return new ConvertedValue(value.Trim(), type);
			}

			if (type == typeof(bool) || type == typeof(bool?))
			{
				if (value != null)
				{
					return new ConvertedValue(ParseBoolean(value), type);
				}

				return new ConvertedValue();
			}

			if (type == typeof(long) || type == typeof(long?))
			{
				if (long.TryParse(value, out var l))
				{
					return new ConvertedValue(l, type);
				}

				return new ConvertedValue();
			}

			if (type == typeof(int) || type == typeof(int?))
			{
				if (int.TryParse(value, out var i))
				{
					return new ConvertedValue(i, type);
				}

				return new ConvertedValue();
			}

			if (type == typeof(decimal))
			{
				return new ConvertedValue(decimal.Parse(value), type);
			}

			if (type == typeof(double) || type == typeof(double?))
			{
				if (double.TryParse(value, out var b))
				{
					return new ConvertedValue(b, type);
				}

				return new ConvertedValue();
			}

			if (type == typeof(DateTime) || type == typeof(DateTime?))
			{
				if (DateTime.TryParse(value, out var date))
				{
					return new ConvertedValue(date, type);
				}

				var formats = new string[]
				{
					"yyyy.MM.ddTHH:mm:ss",
					"yyyy-MM-ddTHH:mm:ss",
					"yyyy/MM/ddTHH:mm:ss",
					"yyyyMMdd"
				};

				if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out date))
				{
					return new ConvertedValue(date, type);
				}

				return new ConvertedValue();
			}

			if (type == typeof(Guid))
			{
				if (Guid.TryParse(value, out var guid))
				{
					return new ConvertedValue(guid, type);
				}

				if (Guid.TryParseExact(value, "B", out guid))
				{
					return new ConvertedValue(guid, type);
				}

				return new ConvertedValue();
			}

			if (type == typeof(Type))
			{
				return new ConvertedValue(Type.GetType(value), type);
			}

			if (type.IsEnum)
			{
				try
				{
					return new ConvertedValue(Enum.Parse(type, value, true), type);
				}
				catch (ArgumentException)
				{
					// returns null anyway so do nothing
				}
			}

			return new ConvertedValue();
		}

		public static bool ParseBoolean(object value)
		{
			if (value == null || value == DBNull.Value)
			{
				return false;
			}

			switch (value.ToString().ToLowerInvariant())
			{
				case "1":
				case "y":
				case "yes":
				case "true":
					return true;

				case "0":
				case "n":
				case "no":
				case "false":
				default:
					return false;
			}
		}
	}
}
