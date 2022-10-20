namespace LogSplit
{
	/// <summary>
	/// A part of the pattern that is used to scan the string
	/// </summary>
	public class ScanPattern
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pattern"></param>
		public ScanPattern(string pattern)
		{
			Pattern = pattern;
		}

		/// <summary>
		/// Gets the part of the pattern that is scanned
		/// </summary>
		public string Pattern { get; set; }
	}
}
