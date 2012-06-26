using System;
using System.Collections.Generic;

using NDesk.Options;

namespace ZipStrip.Operations
{
	public interface IZipOperationArgument
	{
		void Configure(OptionSet set, List<IZipOperation> operations);
	}
}