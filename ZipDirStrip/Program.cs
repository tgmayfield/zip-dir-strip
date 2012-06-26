using System;
using System.IO;

using Ionic.Zip;

using ZipStrip;
using ZipStrip.Operations;

namespace ZipDirStrip
{
	class Program
	{
		static int Main(string[] args)
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
		static void MainInside(string[] args)
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

			using (var zip = ZipFile.Read(file))
			{
				var op = new DirStripOperation();
				var runner = new ZipOperator(op);

				Console.Write("Renaming");
				runner.Run(zip);

				Console.Write("Saving  ");
				zip.RegisterIntegerSaveProgress();
				zip.Save();
			}
		}
	}
}
