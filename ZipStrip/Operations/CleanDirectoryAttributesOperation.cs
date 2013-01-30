using System;
using System.IO;

using Ionic.Zip;

namespace ZipStrip.Operations
{
	public class CleanDirectoryAttributesOperation
		: ZipOperation
	{
		protected override OperationResult Handle(ZipEntry entry, ZipFile zip)
		{
			if (entry.IsDirectory && (entry.Attributes & FileAttributes.Directory) != FileAttributes.Directory)
			{
				entry.Attributes |= FileAttributes.Directory;
			}

			// This just helps if a file is resaved. It shouldn't be treated as modified if this cleans something up.
			return OperationResult.NoChange;
		}

		protected override string ArgumentsText
		{
			get { return null; }
		}
	}
}