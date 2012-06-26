using System;
using System.Linq;

using Ionic.Zip;

namespace ZipStrip.Operations
{
	public class DirStripOperation
		: ZipOperation
	{
		protected virtual string GetStripped(string name)
		{
			string[] split = name.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (split.Length == 1)
			{
				return null;
			}

			return string.Join("/", split.Skip(1));
		}

		protected override OperationResult Handle(ZipEntry entry, ZipFile zip)
		{
			string orig = entry.FileName;
			string stripped = GetStripped(entry.FileName);
			if (stripped == null)
			{
				zip.RemoveEntry(entry);
				return OperationResult.Removed;
			}

			try
			{
				entry.FileName = stripped;
				return OperationResult.Changed;
			}
			catch (Exception ex)
			{
				string type = entry.IsDirectory ? "directory" : "file";
				throw new Exception(string.Format("Could not rename {0} '{1}' to '{2}'", type, orig, stripped), ex);
			}
		}
	}
}