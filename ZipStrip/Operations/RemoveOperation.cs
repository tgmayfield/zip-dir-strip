using System;
using System.Text.RegularExpressions;

using Ionic.Zip;

namespace ZipStrip.Operations
{
	public class RemoveOperation
		: ZipOperation
	{
		private readonly Regex _regex;

		public RemoveOperation(Regex regex)
		{
			_regex = regex;
		}

		protected override OperationResult Handle(ZipEntry entry, ZipFile zip)
		{
			if (_regex.IsMatch(entry.FileName))
			{
				zip.RemoveEntry(entry);
				return OperationResult.Removed;
			}

			return OperationResult.NoChange;
		}
	}
}