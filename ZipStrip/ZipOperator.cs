using System;
using System.Collections.Generic;
using System.Linq;

using Ionic.Zip;

using ZipStrip.Operations;

namespace ZipStrip
{
	public class ZipOperator
	{
		private readonly List<IZipOperation> _operations;

		public ZipOperator(params IZipOperation[] operations)
			: this(operations.AsEnumerable())
		{
		}

		public ZipOperator(IEnumerable<IZipOperation> operations)
		{
			_operations = operations.ToList();
		}

		public void Run(ZipFile zip, out bool wasChanged)
		{
			Run(zip, OutputHelper.DefaultCallback, out wasChanged);
		}

		public void Run(ZipFile zip, PercentProgressCallback callback, out bool wasChanged)
		{
			var entries = zip.ToArray();
			var lastProgress = -1;

			wasChanged = false;

			for (var index = 0; index < entries.Length; index++)
			{
				var progress = ((index + 1) * 100) / entries.Length;
				if (progress != lastProgress && progress % 2 == 0)
				{
					callback(zip, progress);
					lastProgress = progress;
				}

				var entry = entries[index];

				foreach (var op in _operations)
				{
					OperationResult result;
					if (entry.IsDirectory)
					{
						result = op.HandleDirectory(entry, zip);
					}
					else
					{
						result = op.HandleFile(entry, zip);
					}

					if (result == OperationResult.Removed)
					{
						Console.Write("\nRemoved: {0}", entry.FileName);
						wasChanged = true;
						break;
					}
					if (result == OperationResult.Changed)
					{
						wasChanged = true;
					}
				}
			}
		}
	}
}