using System;
using System.Configuration;

namespace AutomationAPI
{
    public class CNC
    {
        //Define
        public short[] savedAlarmKey, currentAlarmKey;
        public string[] savedToolKey, currentToolKey;
        public ushort hndl;
        public string tag;
        public int magSize;
        public string ip;
        private bool alreadyUpdatedConnect;
        private bool ping;
        private bool previousPingResult;

        //Constructor
        /// <summary>
        /// All input parameters come from the App.config file
        /// </summary>
        public CNC(string tagName, string ipAddress, string magazineSize)
        {
            ip = ConfigurationManager.ConnectionStrings[ipAddress].ConnectionString;
            tag = ConfigurationManager.ConnectionStrings[tagName].ConnectionString;
            magSize = Convert.ToInt32(ConfigurationManager.ConnectionStrings[magazineSize].ConnectionString);
            currentAlarmKey = new short[4];
            savedAlarmKey = new short[4];
            currentToolKey = new string[magSize];
            savedToolKey = new string[magSize];
            alreadyUpdatedConnect = false;
            ping = false;
            previousPingResult = false;
        }

        //Methods
        public ushort Connect()
        {
            int returnVal = MazakLibrary.MazConnect(out hndl, ip, 50100, 1);

            //logging
            if (returnVal != 0)
            {
                if (alreadyUpdatedConnect == false)
                {
                    Log.Update(String.Format("{0} was unable to connect", tag));
                    alreadyUpdatedConnect = true;
                }
            }
            else
            {
                Log.Update(String.Format("{0} connection successful!", tag));
                alreadyUpdatedConnect = false;
            }
            return hndl;
        }

        public void Disconnect()
        {
            int returnVal = MazakLibrary.MazDisconnect(hndl);

            //logging
            if (returnVal != 0)
            {
                Log.Update(String.Format("{0} was unable to disconnect", tag));
            }
            else
            {
                Log.Update(String.Format("{0} disconnected successfully!", tag));
            }
        }

        public MAZ_ALARMALL ReadAlarms()
        {
            int returnVal = MazakLibrary.MazGetAlarm(hndl, out MAZ_ALARMALL alarmData);

            for (int i = 0; i < 4; i++)
            {
                //Unique identifier for easier comparison between current data and previously known data
                currentAlarmKey[i] = alarmData.alarm[i].eno;
            }
            return alarmData;
        }

        public MAZ_TLIFE[] ReadToolData()
        {
            MAZ_TLIFE[] toolData = new MAZ_TLIFE[magSize];

            for (int i = 0; i < magSize; i++)
            {
                int returnVal = MazakLibrary.MazGetToolLife(hndl, 0, i + 1, out MAZ_TLIFEALL data);
                toolData[i] = data.tLife[0];

                if (returnVal == 0)
                {
                    //Unique identifier for easier comparison between current data and previously known data
                    string clif = toolData[i].clif.ToString();
                    string cuse = toolData[i].cuse.ToString();
                    string lif = toolData[i].lif.ToString();
                    string use = toolData[i].use.ToString();
                    currentToolKey[i] = clif + cuse + lif + use;
                }
            }
            return toolData;
        }

        public void Ping()
        {
            ping = Log.PingHost(ip);

            if (ping == false && previousPingResult == true)
            {
                Log.Update(String.Format("{0} lost connection", tag));
                previousPingResult = false;
            }
            else if (ping == true && previousPingResult == false)
            {
                Log.Update(String.Format("{0} connection established", tag));
                previousPingResult = true;
            }
        }
    }
}