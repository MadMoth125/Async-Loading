using System;

namespace GenericParse
{
	public class GenericReadLine
	{
		public static T TryReadLine<T>()
		{
			while (true)
			{
				try
				{
					if (AttemptParse(Console.ReadLine() ?? " ", out T value))
					{
						return value;
					}
				}
				catch (FormatException)
				{
					Console.WriteLine("Invalid input format. Please try again.");
				}

				Console.WriteLine("Invalid input. Please try again.");
			}
		}

		// meant for internal use only
		private static bool AttemptParse<T>(string input, out T result)
		{
			try
			{
				result = (T)Convert.ChangeType(input, typeof(T));
				return true;
			}
			catch (Exception)
			{
				result = default;
				return false;
			}
		}
	}
}
