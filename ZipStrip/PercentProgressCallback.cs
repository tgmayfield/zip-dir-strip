using System;

using Ionic.Zip;

namespace ZipStrip
{
	public delegate void PercentProgressCallback(ZipFile file, int progress);
}