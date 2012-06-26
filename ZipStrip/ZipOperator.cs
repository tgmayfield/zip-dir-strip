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

		public void Run(ZipFile zip)
		{
			Run(zip, OutputHelper.DefaultCallback);
		}

		public void Run(ZipFile zip, PercentProgressCallback callback)
		{
			var entries = zip.ToArray();
			var lastProgress = -1;

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
						break;
					}
				}
			}
		}
	}
}