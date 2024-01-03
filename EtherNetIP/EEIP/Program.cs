using System;

namespace EEIP
{
    class Program
    {
        static void Main(string[] args)
        {
            EEIPClient eipClient = new EEIPClient();

            //UInt32 sessionHandle = eipClient.RegisterSession("192.168.178.66", 0xAF12);

            UInt32 sessionHandle = eipClient.RegisterSession("192.168.178.107", 0xAF12);
            //            eipClient.O_T_ConnectionType = Sres.Net.EEIP.ConnectionType.Null;
            //            eipClient.O_T_Length = 0;


            eipClient.O_T_InstanceID = 101;
            eipClient.O_T_Length = 2;//7;
            eipClient.O_T_RealTimeFormat = RealTimeFormat.Header32Bit;
            eipClient.O_T_OwnerRedundant = false;
            eipClient.O_T_Priority = EEIP.Priority.High;
            eipClient.O_T_VariableLength = false;
            eipClient.O_T_ConnectionType = EEIP.ConnectionType.Point_to_Point;

            eipClient.T_O_InstanceID = 104;
            eipClient.T_O_Length = 3;
            eipClient.T_O_RealTimeFormat = EEIP.RealTimeFormat.Modeless;
            eipClient.T_O_OwnerRedundant = true;
            eipClient.T_O_Priority = EEIP.Priority.High;
            eipClient.T_O_VariableLength = false;
            eipClient.T_O_ConnectionType = EEIP.ConnectionType.Multicast;
            eipClient.ForwardOpen();

            for (int i = 0; i < 3; i++)
            {
                eipClient.O_T_IOData[0] = 1;
                System.Threading.Thread.Sleep(1000);
                eipClient.O_T_IOData[0] = 2;
                System.Threading.Thread.Sleep(1000);
                eipClient.O_T_IOData[0] = 3;
                System.Threading.Thread.Sleep(1000);
                eipClient.O_T_IOData[0] = 3;
                System.Threading.Thread.Sleep(1000);


            }

            Console.ReadKey();
            while (true)
            {
                Console.WriteLine("Value of First Byte 1: " + eipClient.T_O_IOData[0]);
                Console.WriteLine("Value of First Byte 1: " + eipClient.T_O_IOData[1]);
                Console.WriteLine("Value of First Byte 1: " + eipClient.T_O_IOData[2]);
                Console.WriteLine("Value of First Byte 1: " + eipClient.T_O_IOData[3]);
                Console.WriteLine("Value of First Byte 1: " + eipClient.T_O_IOData[4]);
                Console.WriteLine("Value of First Byte 1: " + eipClient.T_O_IOData[5]);
                System.Threading.Thread.Sleep(1000);
            }
            eipClient.ForwardClose();
            System.Threading.Thread.Sleep(1000);



            Console.ReadKey();
        }
    }
}
