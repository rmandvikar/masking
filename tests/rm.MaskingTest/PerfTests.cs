using System.Diagnostics;
using NUnit.Framework;
using rm.Masking;

namespace rm.MaskingTest
{
	[TestFixture]
	internal class PerfTests
	{
		private const int iterations = 1_000_000;

		[Explicit]
		[Test]
		public void Perf_OpinionatedMask()
		{
			Perf_Mask(new OpinionatedMask());
		}

		[Explicit]
		[Test]
		public void Perf_PreserveLengthMask()
		{
			Perf_Mask(new PreserveLengthMask());
		}

		[Explicit]
		[Test]
		public void Perf_CompressLengthMask()
		{
			Perf_Mask(new CompressLengthMask());
		}

		[Explicit]
		[Test]
		public void Perf_FixedMask()
		{
			Perf_Mask(new FixedMask());
		}

		[Explicit]
		[Test]
		public void Perf_NoopMask()
		{
			Perf_Mask(new NoopMask());
		}

		private void Perf_Mask(IMaskValue mask)
		{
			var s = "13_coders_ftw_37_13_coders_ftw_37";
			var stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < iterations; i++)
			{
				mask.Mask(s);
			}
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);
		}
	}
}
