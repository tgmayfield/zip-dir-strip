using System;

using Ionic.Zip;

namespace ZipStrip
{
	public static class OutputHelper
	{
		private static void WriteDot(ZipFile zip, int progress)
		{
			Console.Write(".");
			if (progress == 100)
			{
				Console.WriteLine();
			}
		}

		public static readonly PercentProgressCallback DefaultCallback = WriteDot;
	}
}