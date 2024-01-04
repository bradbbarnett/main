using System;

namespace LadderCompareV3
{
    class DiffTextLine : IComparable
    {
        public string Line;
        public int _hash;

        public DiffTextLine(string str)
        {
            Line = str.Replace("\t", "    ");
            _hash = str.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            return _hash.CompareTo(((DiffTextLine)obj)._hash);
        }
    }
}
