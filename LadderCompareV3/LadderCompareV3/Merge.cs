using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LadderCompareV3
{
    public class Merge
    {
        public static List<string> Reject(string pathBefore, string pathAfter)
        {
            //Get directory info
            DirectoryInfo dirBefore = new DirectoryInfo(pathBefore);
            DirectoryInfo dirAfter = new DirectoryInfo(pathAfter);

            //Get all ladder files in directory
            FileInfo[] filesBefore = dirBefore.GetFiles("LAD_" + "*.*");
            FileInfo[] filesAfter = dirAfter.GetFiles("LAD_" + "*.*");

            //Initialize lists
            List<string> filesBeforeList = new List<string>();
            List<string> filesAfterList = new List<string>();

            //Convert fileInfo to List<string>
            foreach (var file in filesBefore)
            {
                filesBeforeList.Add(file.Name);
            }
            foreach (var file in filesAfter)
            {
                filesAfterList.Add(file.Name);
            }

            //Find subroutines that exist in one ladder only
            List<string> firstNotSecond = filesBeforeList.Except(filesAfterList).ToList();
            List<string> secondNotFirst = filesAfterList.Except(filesBeforeList).ToList();

            //Rename any subroutines that exist in one ladder only so it won't be added to the merge list
            if (firstNotSecond.Count != 0)
            {
                foreach (var file in firstNotSecond)
                {
                    File.Move(pathBefore + @"\" + file, pathBefore + @"\x" + file);
                }
            }
            if (secondNotFirst.Count != 0)
            {
                foreach (var file in secondNotFirst)
                {
                    File.Move(pathAfter + @"\" + file, pathAfter + @"\x" + file);
                }
            }

            //Return all rejected subroutines to inform the user
            List<string> rejectedRoutines = firstNotSecond.Concat(secondNotFirst).ToList();
            return rejectedRoutines;
        }

        public static List<string> Run(string path)
        {
            List<string> ladder = new List<string>();

            //Get directory info
            DirectoryInfo dir = new DirectoryInfo(path);

            //Get all ladder files in directory
            FileInfo[] files = dir.GetFiles("LAD_" + "*.*");

            //Merge all ladder files
            foreach (var file in files)
            {
                List<string> before = ReadTxt(dir + @"\" + file);
                ladder = AddToList(ladder, before);
            }

            return ladder;
        }

        private static List<string> AddToList(List<string> ladder, List<string> addition)
        {
            for (int i = 0; i < addition.Count; i++)
            {
                ladder.Add(addition[i]);
            }

            return ladder;
        }

        public static List<string> ReadTxt(string fileName)
        {
            List<string> list = new List<string>();

            //Encoding 1252 converts the double byte characters to single byte
            using (StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding(1252)))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }

            return list;
        }
    }
}
