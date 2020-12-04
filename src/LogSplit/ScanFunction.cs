using System;

namespace LogSplit
{
	internal class ScanFunction
	{
		public ScanFunction(ScanPattern pattern)
		{
			if (pattern?.Pattern == null || !pattern.Pattern.Contains(":"))
			{
				return;
			}

			var function = pattern.Pattern.Substring(2, pattern.Pattern.Length - 3).Split(':')[1];

			Name = function.Split('(')[0];

			if (!function.Contains("("))
			{
				Parameters = new string[] { };
				return;
			}

			var parameterString = function.Substring(Name.Length + 1, function.Length - (Name.Length + 2));
			Parameters = parameterString.Split(',');

		}

		public string Name { get; }

		public string[] Parameters { get; }

		public object Evaluate()
		{
			switch (Name)
			{
				case "len":
					if (Parameters.Length < 1)
					{
						throw new InvalidOperationException("There are no parameters defined in the function len. Please define the length of the string to parse");
					}

					if (int.TryParse(Parameters[0], out int len))
					{
						return len;
					}

					if (Parameters[0] == "*")
					{
						return int.MaxValue;
					}

					throw new InvalidOperationException("Invalid parameters defined in the function len. Valid parameters are integers or *");
			}

			return null;
		}
	}
}
