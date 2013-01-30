using System;

using Ionic.Zip;

namespace ZipStrip.Operations
{
	public abstract class ZipOperation
		: IZipOperation
	{
		public OperationResult HandleFile(ZipEntry file, ZipFile zip)
		{
			return Handle(file, zip);
		}

		public OperationResult HandleDirectory(ZipEntry directory, ZipFile zip)
		{
			return Handle(directory, zip);
		}

		protected abstract OperationResult Handle(ZipEntry entry, ZipFile zip);

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, ArgumentsText);
		}

		protected abstract string ArgumentsText { get; }
	}
}