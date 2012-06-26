using System;

using Ionic.Zip;

namespace ZipStrip
{
	public delegate void ProgressCallback(ZipFile file, int progress, int total);
}