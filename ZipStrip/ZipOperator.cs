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

		public void Run(ZipFile zip, ProgressCallback callback, out bool wasChanged)
		{
			var entries = zip.ToArray();

			wasChanged = false;

			for (var index = 0; index < entries.Length; index++)
			{
				callback(zip, (index + 1), entries.Length);
				
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
						Console.WriteLine("Removed: {0}", entry.FileName);
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