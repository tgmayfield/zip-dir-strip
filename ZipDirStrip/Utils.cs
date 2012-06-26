using System;
using System.Collections.Generic;

namespace ZipDirStrip
{
	public static class Utils
	{
		public static string StringJoin(this IEnumerable<string> values, string separator)
		{
			return string.Join(separator, values);
		}
	}
}