﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Ionic.Zip;

using NDesk.Options;

using ZipStrip.Operations;

namespace ZipStrip
{
	class Program
	{
		private readonly static string AssemblyDirectory = Path.GetDirectoryName(typeof(Program).Assembly.Location);
		
		static int Main(string[] args)
		{
			int result;
			try
			{
				result = MainInside(args);
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex);
				result = 1;
			}

			if (Debugger.IsAttached)
			{
				Console.WriteLine();
				Console.Write("- Press any key to exit -");
				Console.ReadKey();
			}

			return result;
		}

		static int MainInside(string[] args)
		{
			var dllCatalog = new DirectoryCatalog(AssemblyDirectory, "*.dll");
			var exeCatalog = new DirectoryCatalog(AssemblyDirectory, "*.exe");
			var catalog = new AggregateCatalog(dllCatalog, exeCatalog);
			var mef = new CompositionContainer(catalog);

			var operationArguments = mef.GetExportedValues<IZipOperationArgument>();
			var operations = new List<IZipOperation>();

			string file = null;
			var options = new OptionSet()
			{
				{ "file=", val => file = val },
			};
			
			foreach (var arg in operationArguments)
			{
				arg.Configure(options, operations);
			}

			var unparsedArguments = options.Parse(args);
			if (unparsedArguments.Count == 0 && file == null)
			{
				Console.Error.WriteLine("No argument found for file name");
				return 1;
			}
			if (file == null)
			{
				file = unparsedArguments.FirstOrDefault(arg => !(arg.StartsWith("-") | arg.StartsWith("/")));
				if (file != null)
				{
					unparsedArguments.Remove(file);
				}
			}
			if (file != null)
			{
				try
				{
					file = Path.GetFullPath(file);
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine("Could not use '{0}' as a file name: {1}", file, ex.Message);
					throw;
				}

				Console.WriteLine("File: {0}", file);
			}
			if (unparsedArguments.Count > 0)
			{
				foreach (var arg in unparsedArguments)
				{
					Console.Error.WriteLine("Unknown option: {0}", arg);
				}
				return 1;
			}

			if (!File.Exists(file))
			{
				throw new FileNotFoundException("Could not find ZIP file", file);
			}

			using (var zip = new ZipFile(file))
			{
				var op = new ZipOperator(operations);

				bool changed;

				Console.Write("Analyzing");
				op.Run(zip, out changed);

				if (!changed)
				{
					Console.WriteLine("No changes");
					return 0;
				}

				Console.Write("Saving   ");
				zip.RegisterIntegerSaveProgress();
				zip.Save();
				return 0;
			}
		}
	}
}