using System;
using System.Text.RegularExpressions;

using Ionic.Zip;

namespace ZipStrip.Operations
{
	public class RemoveOperation
		: ZipOperation
	{
		private readonly Regex _regex;
		private readonly string _displayPattern;

		public RemoveOperation(Regex regex, string displayPattern)
		{
			_regex = regex;
			_displayPattern = displayPattern;
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

		protected override string ArgumentsText
		{
			get { return _displayPattern; }
		}
	}
}