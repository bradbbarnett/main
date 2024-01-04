using System;

namespace LadderCompareV3
{
    public interface DiffList
    {
        int Count();
        IComparable GetByIndex(int index);
    }
}
