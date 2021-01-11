using System;
using System.Threading;

namespace BzKovSoft.ObjectSlicer
{
	class SliceTry
	{
		public SliceTryItem[] items;
		bool _finished;
		public BzSliceTryData sliceData;
		public Action<BzSliceTryResult> callBack;
		public bool sliced;

		public bool Finished
		{
			get
			{
				Thread.MemoryBarrier();
				return _finished;
			}
			set
			{
				_finished = value;
				Thread.MemoryBarrier();
			}
		}
	}
}