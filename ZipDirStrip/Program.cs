using System;
using System.IO;

namespace ZipDirStrip
{
	internal class Program
	{
		private static int Main(string[] args)
		{
			try
			{
				MainInside(args);
				return 0;
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex);
				return 1;
			}
		}

		private static void MainInside(string[] args)
		{
			if (args.Length != 1)
			{
				throw new InvalidOperationException("Expected one argument for ZIP file name");
			}

			string file = args[0];
			if (!File.Exists(file))
			{
				throw new FileNotFoundException("Could not find ZIP file", file);
			}

			var delegateArgs = new[]
			{
				"--strip",
				file,
			};

			ZipStrip.Program.Main(delegateArgs);
		}
	}
}