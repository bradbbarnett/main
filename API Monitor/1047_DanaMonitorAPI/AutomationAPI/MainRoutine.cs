using System;
using LibplctagWrapper;
using System.Threading;

namespace AutomationAPI
{
    static class MainRoutine
    {
        private static Controller AB;
        private static CNC cnc1, cnc2, cnc3, cnc4;
        private static ushort hndlCnc1 = 0, hndlCnc2 = 0, hndlCnc3 = 0, hndlCnc4 = 0;

        public static void Ping()
        {
            while (true)
            {
                if (cnc1 != null)
                {
                    cnc1.Ping();
                }
                if (cnc2 != null)
                {
                    cnc2.Ping();
                }
                if (cnc3 != null)
                {
                    cnc3.Ping();
                }
                if (cnc4 != null)
                {
                    cnc4.Ping();
                }

                Thread.Sleep(1000);
            }
        }

        public static void MainMethod()
        {
            AB = new Controller();
            cnc1 = new CNC("tagCNC1", "ipAddressCNC1", "magSize1");
            cnc2 = new CNC("tagCNC2", "ipAddressCNC2", "magSize2");
            cnc3 = new CNC("tagCNC3", "ipAddressCNC3", "magSize3");
            cnc4 = new CNC("tagCNC4", "ipAddressCNC4", "magSize4");

            while (true)
            {
                //CNC1
                hndlCnc1 = cnc1.Connect();
                UpdateAlarms(cnc1);
                UpdateToolData(cnc1);
                cnc1.Disconnect();

                //CNC2
                hndlCnc2 = cnc2.Connect();
                UpdateAlarms(cnc2);
                UpdateToolData(cnc2);
                cnc2.Disconnect();

                //CNC3
                hndlCnc3 = cnc3.Connect();
                UpdateAlarms(cnc3);
                UpdateToolData(cnc3);
                cnc3.Disconnect();

                //CNC4
                hndlCnc4 = cnc4.Connect();
                UpdateAlarms(cnc4);
                UpdateToolData(cnc4);
                cnc4.Disconnect();

                //Wait 1 second before looping
                Thread.Sleep(1000);
            }
        }

        public static void Stop()
        {
            //Disconnects from the CNCs
            if (cnc1.hndl != 0)
            {
                cnc1.Disconnect();
            }
            if (cnc2.hndl != 0)
            {
                cnc2.Disconnect();
            }
            if (cnc3.hndl != 0)
            {
                cnc3.Disconnect();
            }
            if (cnc4.hndl != 0)
            {
                cnc4.Disconnect();
            }

            //Disconnects from AB PLC
            if (AB != null)
            {
                AB.Dispose();
            }
        }

        private static void UpdateAlarms(CNC cnc)
        {
            //Define
            Tag[] alarm = new Tag[4];

            //Read data
            MAZ_ALARMALL currentAlarmData = cnc.ReadAlarms();

            //Write data
            for (int i = 0; i < 4; i++)
            {
                //Only write a tag if it has changed
                if (cnc.currentAlarmKey[i] != cnc.savedAlarmKey[i])
                {
                    //Update values for next scan
                    cnc.savedAlarmKey[i] = cnc.currentAlarmKey[i];

                    //No alarm - Clear tag
                    if (cnc.currentAlarmKey[i] == 0)
                    {
                        WriteTag(alarm[i], cnc.tag + ".Alarm[" + i + "]", DataType.String, Convert.ToByte(0));
                    }
                    //Write CNC message to tag
                    else
                    {
                        WriteTag(alarm[i], cnc.tag + ".Alarm[" + i + "]", DataType.String, currentAlarmData.alarm[i].msg);
                    }
                }
            }
        }

        public static void UpdateToolData(CNC cnc)
        {
            //Define
            Tag[] currentLife = new Tag[cnc.magSize];
            Tag[] totalLife = new Tag[cnc.magSize];

            //Read data
            MAZ_TLIFE[] currentToolData = cnc.ReadToolData();

            //Write data
            for (int i = 0; i < cnc.magSize; i++)
            {
                //Only write a tag if it has changed
                if (cnc.currentToolKey[i] != cnc.savedToolKey[i])
                {
                    //Update values for next scan
                    cnc.savedToolKey[i] = cnc.currentToolKey[i];

                    //Write tags
                    WriteTag(currentLife[i], cnc.tag + ".ToolData[" + i + "].CurrentLife", DataType.Int32, currentToolData[i].use);
                    WriteTag(totalLife[i], cnc.tag + ".ToolData[" + i + "].TotalLife", DataType.Int32, currentToolData[i].lif);
                }
            }
        }

        private static void WriteTag(Tag tag, string tagName, int dataType, int valueInt32)
        {
            tag = new Tag(tagName, dataType, 1);
            AB.AddTag(tag);
            AB.SetInt32Value(tag, 0, valueInt32);
            AB.WriteTag(tag, 100);
            AB.RemoveTag(tag);
        }

        private static void WriteTag(Tag tag, string tagName, int dataType, string valueString)
        {
            tag = new Tag(tagName, dataType, 1);
            AB.AddTag(tag);
            AB.SetStringValue(tag, valueString);
            AB.RemoveTag(tag);
        }

        private static void WriteTag(Tag tag, string tagName, int dataType, byte valueUint8)
        {
            tag = new Tag(tagName, dataType, 1);
            AB.AddTag(tag);
            AB.SetUint8Value(tag, 0, valueUint8);
            AB.WriteTag(tag, 100);
            AB.RemoveTag(tag);
        }
    }
}
