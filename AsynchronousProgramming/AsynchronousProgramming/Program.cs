using AsynchronousProgramming.DanielSWolf;
using CustomConsole;
using GenericParse;

namespace AsynchronousProgramming
{
	internal class Program
	{
		private static event Action OnBeginloadingOperation;

		private static string[] menu = { "\"Download\" large data file", "Exit program" };

		private static bool _loopMain = true;

		static void Main(string[] args)
		{
			// subscribing to events before entering main loop
			OnBeginloadingOperation += BeginLoadingOperation;

			while (_loopMain)
			{
				PrintMenu();
				SelectMenuOption();
			}

			// unsubscribing from events after exiting main loop
			OnBeginloadingOperation -= BeginLoadingOperation;

			ConsoleHelper.HaltProgram();
		}

		/// <summary>
		/// Displays all menu options in the console.
		/// </summary>
		static void PrintMenu()
		{
			Console.WriteLine("Welcome to the smallest async loader");
			ConsoleHelper.PrintBlank();
			ConsoleHelper.PrintStrings(menu);
		}

		/// <summary>
		/// Waits for user input and calls SwitchOnMenuSelection(), passing the user's input as a parameter.
		/// </summary>
		private static void SelectMenuOption()
		{
			// looping until a valid option is selected
			while (true)
			{
				ConsoleHelper.PrintBlank();
				Console.Write("Select option: ");
				int tempSelect = GenericReadLine.TryReadLine<int>();

				if (!SwitchOnMenuSelection(tempSelect))
				{
					break;
				}
			}
		}

		/// <summary>
		/// Uses a switch statement to call the appropriate method based on the user's menu selection.
		/// </summary>
		/// <param name="selection">The user's menu selection</param>
		/// <returns>The desired loop state</returns>
		private static bool SwitchOnMenuSelection(int selection)
		{
			bool tempReturnValue = true;

			// clearing console and printing menu again to prevent clutter
			Console.Clear();
			PrintMenu();
			ConsoleHelper.PrintBlank();

			switch (selection)
			{
				case 1: // begin loading operation
					OnBeginloadingOperation?.Invoke();
					break;
				case 2: // exit program
					tempReturnValue = false;
					_loopMain = false;
					break;
				default:
					break;
			}

			return tempReturnValue;
		}

		/// <summary>
		/// Calls the async PerformLoadingOperation() method and waits for it to complete.
		/// </summary>
		private static void BeginLoadingOperation() => PerformLoadingOperation().Wait();

		private static async Task PerformLoadingOperation(int duration = 300, int stepDelay = 20)
		{
			Console.CursorVisible = false;

			Console.Write("Downloading data file... ");
			using (var progress = new AsyncProgressBar())
			{
				for (int i = 0; i <= duration; i++)
				{
					progress.Report((double)i / duration);
					await Task.Delay(stepDelay);
				}
			}
			Console.WriteLine("Download complete.");

			Console.CursorVisible = true;
		}
	}
}