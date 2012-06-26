using System;
using System.Collections.Generic;

using NDesk.Options;

namespace ZipStrip.Operations
{
	[ZipOperationArgument]
	public class DirStripOperationArgument
		: IZipOperationArgument
	{
		public void Configure(OptionSet set, List<IZipOperation> operations)
		{
			set.Add("strip", proto => operations.Add(new DirStripOperation()));
		}
	}
}