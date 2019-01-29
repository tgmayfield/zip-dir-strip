using System;

using Ionic.Zip;

namespace ZipStrip
{
	public static class OutputHelper
	{
		private static ZipFile _last;
		private static int _lastProgress;
		private static int _lastTotal;
		private static int _lastPercent;

		/// <summary>
		/// Inspiration: http://www.nullify.net/Article/269
		/// </summary>
		private static void DrawProgressBar(ZipFile zip, int progress, int total)
		{
			if (total == 0)
			{
				total = 1;
			}
			if (progress > total)
			{
				progress = total;
			}

			if (_last == zip)
			{
				if (_lastPercent == progress / total)
				{
					if (progress - _lastProgress < 50)
					{
						return;
					}
				}
			}
			_last = zip;
			_lastProgress = progress;
			_lastTotal = total;
			_lastPercent = progress / total;

			//draw empty progress bar
			Console.CursorLeft = 0;
			Console.Write("["); //start
			Console.CursorLeft = 32;
			Console.Write("]"); //end
			Console.CursorLeft = 1;
			float onechunk = 30.0f / total;

			var originalBackground = Console.BackgroundColor;

			//draw filled part
			int position = 1;
			for (int i = 0; i < onechunk * progress; i++)
			{
				Console.BackgroundColor = ConsoleColor.Gray;
				Console.CursorLeft = position++;
				Console.Write(" ");
			}

			//draw unfilled part
			for (int i = position; i <= 31; i++)
			{
				Console.BackgroundColor = originalBackground;
				Console.CursorLeft = position++;
				Console.Write(" ");
			}

			//draw totals
			Console.CursorLeft = 35;
			Console.BackgroundColor = originalBackground;

			string totalString = total.ToString("N0");
			string progressString = progress.ToString("N0").PadLeft(totalString.Length, ' ');
			Console.Write("{0} of {1}   ", progressString, totalString); // Extra spaces to provide room for other console output

			if (progress == total)
			{
				Console.WriteLine();
			}
		}

		public static readonly ProgressCallback ProgressBarCallback = DrawProgressBar;
	}
}