﻿using System.Text;
using System.Threading.Tasks;

namespace AsynchronousProgramming.DanielSWolf
{

	/// <summary>
	/// An ASCII progress bar
	/// Credit to DanielSWolf: https://gist.github.com/DanielSWolf/0ab6a96899cc5377bf54
	/// Modified to be used with Task & async/await instead of Thread.Sleep()
	/// </summary>
	public class AsyncProgressBar : IDisposable, IProgress<double>
	{
		private const int blockCount = 10;
		private readonly TimeSpan animationInterval = TimeSpan.FromSeconds(1.0 / 8);
		private const string animation = @"|/-\";

		private readonly Timer timer;

		private double currentProgress = 0;
		private string currentText = string.Empty;
		private bool disposed = false;
		private int animationIndex = 0;

		public AsyncProgressBar()
		{
			timer = new Timer(TimerHandler);

			// A progress bar is only for temporary display in a console window.
			// If the console output is redirected to a file, draw nothing.
			// Otherwise, we'll end up with a lot of garbage in the target file.
			if (!Console.IsOutputRedirected)
			{
				ResetTimer();
			}
		}

		public void Report(double value)
		{
			// Make sure value is in [0..1] range
			value = Math.Max(0, Math.Min(1, value));
			Interlocked.Exchange(ref currentProgress, value);
		}

		private async void TimerHandler(object state)
		{
			if (disposed) return;

			await Task.Run(() => {
				lock (timer)
				{
					int progressBlockCount = (int)(currentProgress * blockCount);
					int percent = (int)(currentProgress * 100);
					string text = string.Format("[{0}{1}] {2,3}% {3}",
						new string('#', progressBlockCount), new string('-', blockCount - progressBlockCount),
						percent,
						animation[animationIndex++ % animation.Length]);
					UpdateText(text);

					ResetTimer();
				}
			});
		}

		private void UpdateText(string text)
		{
			// Get length of common portion
			int commonPrefixLength = 0;
			int commonLength = Math.Min(currentText.Length, text.Length);
			while (commonPrefixLength < commonLength && text[commonPrefixLength] == currentText[commonPrefixLength])
			{
				commonPrefixLength++;
			}

			// Backtrack to the first differing character
			StringBuilder outputBuilder = new StringBuilder();
			outputBuilder.Append('\b', currentText.Length - commonPrefixLength);

			// Output new suffix
			outputBuilder.Append(text.Substring(commonPrefixLength));

			// If the new text is shorter than the old one: delete overlapping characters
			int overlapCount = currentText.Length - text.Length;
			if (overlapCount > 0)
			{
				outputBuilder.Append(' ', overlapCount);
				outputBuilder.Append('\b', overlapCount);
			}

			Console.Write(outputBuilder);
			currentText = text;
		}

		private void ResetTimer()
		{
			timer.Change(animationInterval, Timeout.InfiniteTimeSpan);
		}

		public void Dispose()
		{
			lock (timer)
			{
				disposed = true;
				UpdateText(string.Empty);
			}
		}
	}
}