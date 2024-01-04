using System;

namespace LadderCompareV3
{
    public class DiffResultSpan : IComparable
    {
        private const int BAD_INDEX = -1;

        public int DestIndex { get; }
        public int SourceIndex { get; }
        public int Length { get; private set; }
        public DiffResultSpanStatus Status { get; }

        protected DiffResultSpan(
            DiffResultSpanStatus status,
            int destIndex,
            int sourceIndex,
            int length)
        {
            Status = status;
            DestIndex = destIndex;
            SourceIndex = sourceIndex;
            Length = length;
        }

        public static DiffResultSpan CreateNoChange(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.NoChange, destIndex, sourceIndex, length);
        }

        public static DiffResultSpan CreateReplace(int destIndex, int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.Replace, destIndex, sourceIndex, length);
        }

        public static DiffResultSpan CreateDeleteSource(int sourceIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.DeleteSource, BAD_INDEX, sourceIndex, length);
        }

        public static DiffResultSpan CreateAddDestination(int destIndex, int length)
        {
            return new DiffResultSpan(DiffResultSpanStatus.AddDestination, destIndex, BAD_INDEX, length);
        }

        public void AddLength(int i)
        {
            Length += i;
        }

        public override string ToString()
        {
            return string.Format("{0} (Dest: {1},Source: {2}) {3}",
                Status.ToString(),
                DestIndex.ToString(),
                SourceIndex.ToString(),
                Length.ToString());
        }

        public int CompareTo(object obj)
        {
            return DestIndex.CompareTo(((DiffResultSpan)obj).DestIndex);
        }
    }
}
