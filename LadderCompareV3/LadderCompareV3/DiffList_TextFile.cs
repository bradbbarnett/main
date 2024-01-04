using System;
using System.Collections;
using System.Collections.Generic;

namespace LadderCompareV3
{
    public class DiffList_TextFile : DiffList
    {
        private ArrayList _lines;

        public DiffList_TextFile(List<string> fileName)
        {
            _lines = new ArrayList();

            foreach (string line in fileName)
            {
                _lines.Add(new DiffTextLine(line));
            }
        }

        public int Count()
        {
            return _lines.Count;
        }

        public IComparable GetByIndex(int index)
        {
            return (DiffTextLine)_lines[index];
        }
    }
}
