using System;
using System.ComponentModel.Composition;

namespace ZipStrip.Operations
{
	public class ZipOperationArgumentAttribute
		: ExportAttribute
	{
		public ZipOperationArgumentAttribute()
			: base(typeof(IZipOperationArgument))
		{
		}
	}
}