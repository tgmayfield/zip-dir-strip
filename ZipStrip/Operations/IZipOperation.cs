using System;

using Ionic.Zip;

namespace ZipStrip.Operations
{
	public interface IZipOperation
	{
		OperationResult HandleFile(ZipEntry file, ZipFile zip);
		OperationResult HandleDirectory(ZipEntry directory, ZipFile zip);
	}
}