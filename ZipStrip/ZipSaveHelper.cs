using System;

using Ionic.Zip;

namespace ZipStrip
{
	public static class ZipSaveHelper
	{
		public static void RegisterIntegerSaveProgress(this ZipFile zip, ProgressCallback callback)
		{
			int total = 0;
			zip.SaveProgress += (sender, eventArgs) =>
			{
				if (eventArgs.EntriesTotal != 0
					&& total == 0)
				{
					total = eventArgs.EntriesTotal;
				}

				if (eventArgs.EntriesSaved == 0)
				{
					return;
				}
				
				int progress;
				if (total == 0)
				{
					progress = 0;
				}
				else
				{
					progress = eventArgs.EntriesSaved;
				}

				callback(zip, progress, total);
			};
		}
	}
}