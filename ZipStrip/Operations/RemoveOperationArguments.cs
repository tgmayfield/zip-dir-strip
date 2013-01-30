using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using NDesk.Options;

namespace ZipStrip.Operations
{
	[ZipOperationArgument]
	public class RemoveOperationArguments
		: IZipOperationArgument
	{
		public void Configure(OptionSet set, List<IZipOperation> operations)
		{
			set.Add("remove=", value =>
				{
					var regex = new Regex(value);
					operations.Add(new RemoveOperation(regex, value));
				});
			set.Add("iremove=", "Ignore case remove regex", value =>
				{
					var regex = new Regex(value, RegexOptions.IgnoreCase);
					operations.Add(new RemoveOperation(regex, "i/" + value));
				});
		}
	}
}