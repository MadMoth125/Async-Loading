namespace CustomConsole
{
	static class ConsoleHelper
	{
		#region Parsing
		public static ConsoleKeyInfo HaltProgram(string msgPrompt = "Input any key to close program...")
		{
			Console.WriteLine(msgPrompt);
			return Console.ReadKey();
		}
		#endregion

		#region Menu Printing
		public static void PrintStrings(string[] strings)
		{
			for (int i = 0; i < strings.Length; i++)
			{
				Console.WriteLine($"{i + 1}. {strings[i]}");
			}
		}

		public static void PrintStrings(params string[][] strings)
		{
			int tempIndex = 0;
			foreach (var menu in strings)
			{
				for (int i = 0; i < menu.Length; i++)
				{
					tempIndex++;
					Console.WriteLine($"{tempIndex}. {menu[i]}");
				}
			}
		}
		#endregion

		#region Basic Printing
		public static void PrintBlank()
		{
			Console.WriteLine();
		}

		public static void PrintValue<T>(T val)
		{
			Console.WriteLine($"{nameof(val)} is: {val}");
		}

		public static void PrintInvalidSelection()
		{
			Console.WriteLine("Invalid entry, please try again.");
		}
		#endregion
		
		public static bool ListEmpty<T>(List<T> list)
		{
			return !(list.Count > 0);
		}
	}
}