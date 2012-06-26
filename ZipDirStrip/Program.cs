using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Ionic.Zip;

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

			var directories = new List<ZipEntry>();

			using (var zip = ZipFile.Read(file))
			{
				Console.Write("Renaming");

				var entries = zip.ToArray();
				int lastProgress = 0;

				var usedDirectories = new Dictionary<string, bool>();

				for (int index = 0; index < entries.Length; index++)
				{
					int progress = ((index + 1) * 100) / entries.Length;
					if (progress != lastProgress && progress % 2 == 0)
					{
						Console.Write(".");
						lastProgress = progress;
					}

					var entry = entries[index];
					if (entry.IsDirectory)
					{
						directories.Add(entry);
						continue;
					}

					string orig = entry.FileName;
					var stripped = GetStrippedName(entry.FileName);

					string directory = stripped.Split('/').Reverse().Skip(1).Reverse().StringJoin("/");
					usedDirectories[directory] = true;

					try
					{
						entry.FileName = stripped;
					}
					catch (Exception ex)
					{
						throw new Exception(string.Format("Could not rename '{0}' to '{1}'", orig, stripped), ex);
					}
				}
				Console.WriteLine();

				zip.RemoveEntries(directories);

				Console.Write("Saving  ");
				lastProgress = 0;
				int total = 0;
				zip.SaveProgress += (sender, eventArgs) =>
				{
					if (eventArgs.EntriesTotal != 0 && total == 0)
					{
						total = eventArgs.EntriesTotal;
					}

					if (eventArgs.EntriesSaved == 0)
					{
						return;
					}

					int progress;
					if (total == 0)
					{
						progress = 0;
					}
					else
					{
						progress = (eventArgs.EntriesSaved * 100) / total;
					}
					if (progress != lastProgress && progress % 2 == 0)
					{
						Console.Write(".");
						lastProgress = progress;
					}
				};
				zip.Save();
				Console.WriteLine();
			}
		}

		public static string GetStrippedName(string fileName)
		{
			string[] split = fileName.Split('/');
			if (split.Length > 1)
			{
				split = split.Skip(1).ToArray();
			}

			string result = string.Join("/", split);
			return result;
		}
	}
}
