using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Office.Interop.Word;

namespace LadderCompareV3
{
    class Compare
    {
        public static void Run(string beforeDir, string afterDir, List<string> beforeLadder, List<string> afterLadder)
        {
            //Convert the Japanese symbols
            beforeLadder = ConvertLadder(beforeLadder);
            afterLadder = ConvertLadder(afterLadder);

            //Print converted ladder
            WriteTxt(beforeDir + @"\" + Path.GetFileName(beforeDir) + "Ladder.txt", beforeLadder);
            WriteTxt(afterDir + @"\" + Path.GetFileName(afterDir) + "Ladder.txt", afterLadder);

            //Find rungs and insert a marker
            beforeLadder = FindRungs(beforeLadder);
            afterLadder = FindRungs(afterLadder);

            //Duplicate the list so the current state (stored in ladderPrep) can be fetched later 
            List<string> ladderBeforePrep = new List<string>(beforeLadder);
            List<string> ladderAfterPrep = new List<string>(afterLadder);

            //Remove rung numbers so they are not compared
            beforeLadder = RemoveRungNums(beforeLadder);
            afterLadder = RemoveRungNums(afterLadder);

            //Compare ladders
            ArrayList diffArray = TextDiff(beforeLadder, afterLadder);

            //Convert comparison array to list of strings
            List<string> diffList = new List<string>();
            diffList = DiffToList(diffArray);

            //Add rung numbers back
            beforeLadder = AddRungNums(ladderBeforePrep, beforeLadder);
            afterLadder = AddRungNums(ladderAfterPrep, afterLadder);

            //Compile a list that shows ladders side by side
            List<string> ladderSideBySide = new List<string>();
            ladderSideBySide = CreateCompareText(diffList, beforeLadder, afterLadder);

            //Remove RUNG
            ladderSideBySide = RemoveRUNG(ladderSideBySide);

            //Print debug ladder if selected
            if (Main.debug == true)
            {
                WriteTxt(afterDir + @"\debug.txt", ladderSideBySide);
            }

            //Find all differences, character to character for red color
            List<string> changes = new List<string>();
            changes = FindDifferences(ladderSideBySide);

            //Create RTF document for red text and store in temp file
            RichTextBox box = CreateRTF(ladderSideBySide, changes);
            box.SaveFile(afterDir + @"\temp.rtf");

            //Create Word document
            CreateWordDoc(beforeDir, afterDir, @"\temp.rtf");

            //Delete temp file
            File.Delete(afterDir + @"\temp.rtf");
        }

        private static List<string> ConvertLadder(List<string> ladder)
        {
            ladder = Replace("„Ÿ", "--", ladder);
            ladder = Replace("¥", "-", ladder);
            ladder = Replace(@"\u005E", "X ", ladder);
            ladder = Replace("„§  „", @"-| |-", ladder);
            ladder = Replace("„§X „", @"-|/|-", ladder);
            ladder = Replace("„¦", "*-", ladder);
            ladder = Replace("„©", "*-", ladder);
            ladder = Replace("„¤", "*-", ladder);
            ladder = Replace("„£", "* ", ladder);
            ladder = Replace("„§", "* ", ladder);
            ladder = Replace("„-", "*-", ladder);
            ladder = Replace("„¨", "*-", ladder);
            ladder = Replace("   ¨", "---> ", ladder);
            ladder = Replace("„", "|", ladder);
            ladder = Replace("X ", @"/-", ladder);
            ladder = Replace("-ª", " ↑ ", ladder);
            ladder = Replace("-«", " ↓ ", ladder);
            ladder = Replace(@"\* ª\*", @"-|↑|-", ladder);
            ladder = Replace(@"\* «\*", @"-|↓|-", ladder);

            return ladder;
        }

        private static List<string> Replace(string originalString, string replacementString, List<string> file)
        {
            List<string> newFile = new List<string>();

            foreach (string line in file)
            {
                string x = Regex.Replace(line, originalString, replacementString);
                newFile.Add(x);
            }

            file = newFile;
            return file;
        }

        private static void WriteTxt(string fileName, List<string> text)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding(1252)))
            {
                foreach (string line in text)
                {
                    sw.WriteLine(line);
                }
            }
        }

        private static List<string> FindRungs(List<string> fileName)
        {
            for (int i = 0; i < fileName.Count; i++)
            {
                if (fileName[i].ElementAt(9).ToString() != " ")
                {
                    string x = fileName[i - 1];
                    fileName.RemoveAt(i - 1);
                    fileName.Insert(i - 1, "RUNG" + x);
                    i++;
                }
            }

            return fileName;
        }

        private static List<string> RemoveRungNums(List<string> fileName)
        {
            for (int i = 0; i < fileName.Count; i++)
            {
                if (fileName[i].StartsWith("RUNG"))
                {
                    string x = fileName[i + 1];
                    x = x.Remove(0, 10);
                    string y = "          ";
                    x = x.Insert(0, y);
                    fileName[i + 1] = x;
                }
            }

            return fileName;
        }

        private static ArrayList TextDiff(List<string> before, List<string> after)
        {
            DiffList_TextFile bLF = new DiffList_TextFile(before);
            DiffList_TextFile aLF = new DiffList_TextFile(after);

            DiffEngine de = new DiffEngine();
            de.ProcessDiff(bLF, aLF);

            ArrayList rep = de.DiffReport();

            return rep;
        }

        private static List<string> DiffToList(ArrayList diffArray)
        {
            List<string> list = new List<string>();

            foreach (DiffResultSpan drs in diffArray)
            {
                list.Add(drs.ToString());
            }

            return list;
        }

        private static List<string> AddRungNums(List<string> prevLadder, List<string> fileName)
        {
            for (int i = 0; i < prevLadder.Count; i++)
            {
                if (prevLadder[i].StartsWith("RUNG"))
                {
                    string x = prevLadder[i + 1];
                    x = x.Remove(10, x.Length - 10);
                    string y = fileName[i + 1];
                    y = y.Remove(0, 10);
                    y = y.Insert(0, x);
                    fileName[i + 1] = y;
                }
            }

            return fileName;
        }

        private static List<string> CreateCompareText(List<string> changes, List<string> beforeLadder, List<string> afterLadder)
        {
            List<string> ladders = new List<string>();
            int rungPreviousBeforeStart = -2;
            int rungPreviousBeforeEnd = -2;
            int rungPreviousAfterStart = -2;
            int rungPreviousAfterEnd = -2;

            //Grab useful info about changes and conver to List of strings
            foreach (string line in changes)
            {
                //startIndex
                int b = line.IndexOf("t:");
                int c = line.IndexOf("e:");
                int d = line.IndexOf(") ");

                //length
                int e = c - b - 9;
                int f = d - c - 3;

                //info about changes.
                string xEvent = line.Substring(0, b - 5);
                int afterIndex = Convert.ToInt32(line.Substring(b + 3, e));
                int beforeIndex = Convert.ToInt32(line.Substring(c + 3, f));
                int lengthIndex = Convert.ToInt32(line.Substring(d + 2));

                //needed so that rung writing doesn't duplicate
                int rungBeforeStart = 0;
                int rungBeforeEnd = 0;
                int rungBeforeLength = 0;
                int rungAfterStart = 0;
                int rungAfterEnd = 0;
                int rungAfterLength = 0;

                if (xEvent == "Replace")
                {
                    //Finds beginning of rung
                    for (int i = beforeIndex; i >= 0; i--)
                    {
                        if (beforeLadder[i].StartsWith("RUNG"))
                        {
                            rungBeforeStart = i;
                            break;
                        }
                    }

                    //Finds end of last rung modified
                    for (int i = beforeIndex + lengthIndex; i < beforeLadder.Count; i++)
                    {
                        if (beforeLadder[i].StartsWith("RUNG"))
                        {
                            rungBeforeEnd = i;
                            break;
                        }
                    }

                    rungBeforeLength = rungBeforeEnd - rungBeforeStart;

                    //Finds beginning of rung
                    for (int i = afterIndex; i >= 0; i--)
                    {
                        if (afterLadder[i].StartsWith("RUNG"))
                        {
                            rungAfterStart = i;
                            break;
                        }
                    }

                    //Finds end of last rung modified
                    for (int i = afterIndex + lengthIndex; i < afterLadder.Count; i++)
                    {
                        if (afterLadder[i].StartsWith("RUNG"))
                        {
                            rungAfterEnd = i;
                            break;
                        }
                    }

                    rungAfterLength = rungAfterEnd - rungAfterStart;

                    //Checks if current rungs have already been added to the list
                    if ((rungPreviousBeforeEnd <= rungBeforeStart) && (rungPreviousAfterEnd <= rungAfterStart))
                    {
                        if (rungBeforeLength == rungAfterLength)
                        {
                            int i = rungBeforeStart;
                            int j = rungAfterStart;

                            for (; i < rungBeforeEnd; i++)
                            {
                                ladders.Add(beforeLadder[i] + afterLadder[j]);
                                rungPreviousBeforeStart = rungBeforeStart;
                                rungPreviousBeforeEnd = rungBeforeEnd;
                                rungPreviousAfterStart = rungAfterStart;
                                rungPreviousAfterEnd = rungAfterEnd;
                                j++;
                            }
                        }

                        if (rungBeforeLength < rungAfterLength)
                        {
                            int i = rungBeforeStart;
                            int j = rungAfterStart;

                            for (; i < rungBeforeEnd; i++)
                            {
                                ladders.Add(beforeLadder[i] + afterLadder[j]);
                                rungPreviousBeforeStart = rungBeforeStart;
                                rungPreviousBeforeEnd = rungBeforeEnd;
                                j++;
                            }

                            for (; j < rungAfterEnd; j++)
                            {
                                ladders.Add("".PadLeft(157) + afterLadder[j]);
                                rungPreviousAfterStart = rungAfterStart;
                                rungPreviousAfterEnd = rungAfterEnd;
                            }
                        }

                        if (rungBeforeLength > rungAfterLength)
                        {
                            int i = rungBeforeStart;
                            int j = rungAfterStart;

                            for (; j < rungAfterEnd; j++)
                            {
                                ladders.Add(beforeLadder[i] + afterLadder[j]);
                                rungPreviousAfterStart = rungAfterStart;
                                rungPreviousAfterEnd = rungAfterEnd;
                                i++;
                            }

                            for (; i < rungBeforeEnd; i++)
                            {
                                ladders.Add(beforeLadder[i] + "".PadRight(157));
                                rungPreviousBeforeStart = rungBeforeStart;
                                rungPreviousBeforeEnd = rungBeforeEnd;
                            }
                        }
                    }
                }

                if (xEvent == "AddDestination")
                {
                    for (int i = afterIndex; i >= 0; i--)
                    {
                        if (afterLadder[i].StartsWith("RUNG"))
                        {
                            rungAfterStart = i;
                            break;
                        }
                    }

                    for (int i = afterIndex + lengthIndex; i < afterLadder.Count; i++)
                    {
                        if (afterLadder[i].StartsWith("RUNG"))
                        {
                            rungAfterEnd = i;
                            break;
                        }
                    }

                    if (rungPreviousAfterEnd <= rungAfterStart)
                    {
                        int j = rungAfterStart;

                        for (; j < rungAfterEnd; j++)
                        {
                            ladders.Add("".PadLeft(157) + afterLadder[j]);
                            rungPreviousAfterStart = rungAfterStart;
                            rungPreviousAfterEnd = rungAfterEnd;
                        }
                    }
                    else if (rungPreviousAfterEnd != rungAfterEnd)
                    {
                        rungAfterStart = rungPreviousAfterEnd;
                        int j = rungAfterStart;

                        for (; j < rungAfterEnd; j++)
                        {
                            ladders.Add("".PadLeft(157) + afterLadder[j]);
                            rungPreviousAfterStart = rungAfterStart;
                            rungPreviousAfterEnd = rungAfterEnd;
                        }
                    }
                }

                if (xEvent == "DeleteSource")
                {
                    for (int i = beforeIndex; i >= 0; i--)
                    {
                        if (beforeLadder[i].StartsWith("RUNG"))
                        {
                            rungBeforeStart = i;
                            break;
                        }
                    }

                    for (int i = beforeIndex + lengthIndex; i < beforeLadder.Count; i++)
                    {
                        if (beforeLadder[i].StartsWith("RUNG"))
                        {
                            rungBeforeEnd = i;
                            break;
                        }
                    }

                    if (rungPreviousBeforeEnd <= rungBeforeStart)
                    {
                        int i = rungBeforeStart;

                        for (; i < rungBeforeEnd; i++)
                        {
                            ladders.Add(beforeLadder[i] + "".PadRight(157));
                            rungPreviousBeforeStart = rungBeforeStart;
                            rungPreviousBeforeEnd = rungBeforeEnd;
                        }
                    }
                    else if (rungPreviousBeforeEnd != rungBeforeEnd)
                    {
                        rungBeforeStart = rungPreviousBeforeEnd;
                        int i = rungBeforeStart;

                        for (; i < rungBeforeEnd; i++)
                        {
                            ladders.Add(beforeLadder[i] + "".PadRight(157));
                            rungPreviousBeforeStart = rungBeforeStart;
                            rungPreviousBeforeEnd = rungBeforeEnd;
                        }
                    }
                }
            }

            return ladders;
        }

        private static List<string> RemoveRUNG(List<string> fileName)
        {
            for (int i = 0; i < fileName.Count; i++)
            {
                if (fileName[i].StartsWith("RUNG"))
                {
                    string x = fileName[i];
                    x = x.Remove(0, 4);
                    fileName[i] = x;
                }
            }

            for (int i = 0; i < fileName.Count; i++)
            {
                if (fileName[i].Substring(157, 4) == "RUNG")
                {
                    string x = fileName[i];
                    x = x.Remove(157, 4);
                    fileName[i] = x;
                }
            }

            return fileName;
        }

        private static List<string> FindDifferences(List<string> ladder)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < ladder.Count; i++)
            {
                if (ladder[i].Length == 314)
                {
                    for (int j = 0; j < 157; j++)
                    {
                        if (ladder[i][j] != ladder[i][j + 157])
                        {
                            list.Add(i.ToString() + " " + j.ToString());
                        }
                    }
                }
            }

            return list;
        }

        private static RichTextBox CreateRTF(List<string> file, List<string> listOfChanges)
        {
            RichTextBox box = new RichTextBox();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < file.Count; i++)
            {
                sb.AppendLine(file[i]);
            }

            box.AppendText(sb.ToString());



            for (int i = 0; i < listOfChanges.Count; i++)
            {
                string change = listOfChanges[i].ToString();
                int changeIndex = change.IndexOf(" ");
                int line = Convert.ToInt32(change.Substring(0, changeIndex));
                int character = Convert.ToInt32(change.Substring(changeIndex));

                box.Select(line * 315 + character, 1);
                box.SelectionColor = Color.Red;
                box.SelectionFont = new System.Drawing.Font(box.Font, FontStyle.Bold);
                box.DeselectAll();
                box.Select((line * 315) + (character + 157), 1);
                box.SelectionColor = Color.Red;
                box.SelectionFont = new System.Drawing.Font(box.Font, FontStyle.Bold);
                box.DeselectAll();
            }

            return box;
        }

        private static void CreateWordDoc(string beforeDir, string afterDir, string name)
        {
            string beforeName = Path.GetFileName(beforeDir);
            string afterName = Path.GetFileName(afterDir);

            var wordApp = new Microsoft.Office.Interop.Word.Application();
            var doc = wordApp.Documents.Open(afterDir + name);

            //settings
            doc.Application.Options.DisableFeaturesbyDefault = false;
            doc.Compatibility[WdCompatibility.wdDontBalanceSingleByteDoubleByteWidth] = true;
            doc.Paragraphs.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceExactly;
            doc.Paragraphs.Format.SpaceBefore = 0;
            doc.Paragraphs.Format.SpaceAfter = 0;
            doc.Content.Font.Name = "Lucida Sans Typewriter";
            doc.Content.Font.Scaling = 47;

            if (Main.orientation == "Portrait")
            {
                doc.PageSetup.TopMargin = wordApp.InchesToPoints(0.5f);
                doc.PageSetup.BottomMargin = wordApp.InchesToPoints(0.5f);
                doc.Paragraphs.Format.LineSpacing = 6;
                doc.Content.Font.Size = 6;
                doc.PageSetup.HeaderDistance = wordApp.InchesToPoints(0.5f);
                doc.PageSetup.FooterDistance = wordApp.InchesToPoints(0.5f);
            }
            else
            {
                doc.PageSetup.TogglePortrait();
                doc.PageSetup.TopMargin = wordApp.InchesToPoints(0.4f);
                doc.PageSetup.BottomMargin = wordApp.InchesToPoints(0.4f);
                doc.Paragraphs.Format.LineSpacing = 8;
                doc.Content.Font.Size = 8;
                doc.PageSetup.HeaderDistance = wordApp.InchesToPoints(0.4f);
                doc.PageSetup.FooterDistance = wordApp.InchesToPoints(0.4f);
            }

            doc.PageSetup.LeftMargin = wordApp.InchesToPoints(0.5f);
            doc.PageSetup.RightMargin = wordApp.InchesToPoints(0.5f);

            int length = beforeName.Length + 10;
            int middleLength = 157 - length;
            string middleSpace = new string(' ', middleLength);
            string leftSpace = new string(' ', 10);

            foreach (Section section in doc.Sections)
            {
                Range headerRange = section.Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage);
                headerRange.Font.Scaling = 47;
                headerRange.Font.Name = "Lucida Sans Typewriter";

                if (Main.orientation == "Portrait")
                {
                    headerRange.Font.Size = 6;
                }
                else
                {
                    headerRange.Font.Size = 8;
                }

                headerRange.Text = leftSpace + beforeName + middleSpace + leftSpace + afterName;
            }

            object fileName = afterDir + @"\" + beforeName + " -- " + afterName;
            doc.SaveAs2(ref fileName, WdSaveFormat.wdFormatDocumentDefault);
            doc.SaveAs2(ref fileName, WdSaveFormat.wdFormatPDF);

            doc.Close();
            wordApp.Quit();
        }
    }
}
