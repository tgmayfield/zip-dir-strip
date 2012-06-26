using System;

using Ionic.Zip;

namespace ZipStrip
{
	public static class ZipSaveHelper
	{
		public static void RegisterIntegerSaveProgress(this ZipFile zip)
		{
			RegisterIntegerSaveProgress(zip, OutputHelper.DefaultCallback);
		}

		public static void RegisterIntegerSaveProgress(this ZipFile zip, PercentProgressCallback callback)
		{
			int total = 0;
			int lastProgress = -1;
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
					progress = (eventArgs.EntriesSaved * 100) / total;
				}

				if (progress != lastProgress
					&& progress % 2 == 0)
				{
					callback(zip, progress);
					lastProgress = progress;
				}
			};
		}
	}
}