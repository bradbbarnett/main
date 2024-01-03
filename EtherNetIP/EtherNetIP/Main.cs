using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using EEIP;

namespace EtherNetIP
{
    public partial class Form_Main : Form
    {
        EEIPClient eeipClient = new EEIPClient();
        bool[] bit = new bool[16];
        bool connected = false;

        public Form_Main()
        {
            InitializeComponent();

            textBox_IPAddress.Text = "192.168.1.100";

            textBox_Input.Text = "101";
            textBox_Input.MaxLength = 3;

            textBox_Output.Text = "151";
            textBox_Output.MaxLength = 3;

            panel_Connection.BackColor = Color.Red;
        }

        private void textBox_IPAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void textBox_Input_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox_Output_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(textBox_Input.Text) > 255)
            {
                MessageBox.Show("Input assembly instance max = 255");
                return;
            }

            if (Convert.ToInt32(textBox_Output.Text) > 255)
            {
                MessageBox.Show("Output assembly instance max = 255");
                return;
            }

            if (connected == false)
            {
                eeipClient = new EEIPClient();

                //A Session has to be registered before any communication can be established
                eeipClient.RegisterSession(textBox_IPAddress.Text);

                //Parameters from Target -> Originator
                eeipClient.T_O_InstanceID = Convert.ToByte(textBox_Input.Text);
                eeipClient.T_O_Length = 20;
                eeipClient.T_O_RealTimeFormat = RealTimeFormat.Modeless;
                eeipClient.T_O_OwnerRedundant = false;
                eeipClient.T_O_Priority = Priority.Scheduled;
                eeipClient.T_O_VariableLength = false;
                eeipClient.T_O_ConnectionType = ConnectionType.Point_to_Point;
                eeipClient.RequestedPacketRate_T_O = 500000;    //RPI in  500ms is the Standard value

                //Parameters from Originator -> Target
                eeipClient.O_T_InstanceID = Convert.ToByte(textBox_Output.Text);              //Instance ID of the Output Assembly
                eeipClient.O_T_Length = 20;                     //The Method "Detect_O_T_Length" detect the Length using an UCMM Message
                eeipClient.O_T_RealTimeFormat = RealTimeFormat.Header32Bit;   //Header Format
                eeipClient.O_T_OwnerRedundant = false;
                eeipClient.O_T_Priority = Priority.Scheduled;
                eeipClient.O_T_VariableLength = false;
                eeipClient.O_T_ConnectionType = ConnectionType.Point_to_Point;
                eeipClient.RequestedPacketRate_O_T = 500000;        //500ms is the Standard value

                //Forward open initiates the Implicit Messaging
                eeipClient.ForwardOpen();

                Thread thread = new Thread(new ThreadStart(Background));
                thread.Start();

                connected = true;
                panel_Connection.BackColor = Color.Green;
            }
        }

        private void button_Disconnect_Click(object sender, EventArgs e)
        {
            long lastReceivedTick = eeipClient.LastReceivedImplicitMessage.Ticks;
            eeipClient.ForwardClose();
            eeipClient.UnRegisterSession();
            connected = false;
            panel_Connection.BackColor = Color.Red;

            


        }

        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            eeipClient.ForwardClose();
            eeipClient.UnRegisterSession();
            connected = false;
            panel_Connection.BackColor = Color.Red;
        }

        private void button_Bit0_Click(object sender, EventArgs e)
        {
            if (bit[0] == false)
            {
                bit[0] = true;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_0001 | eeipClient.O_T_IOData[0]);
                label_OutBit0.Text = "1";
            }
            else
            {
                bit[0] = false;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[0]);
                label_OutBit0.Text = "0";
            }
        }

        private void button_Bit1_Click(object sender, EventArgs e)
        {
            if (bit[1] == false)
            {
                bit[1] = true;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_0010 | eeipClient.O_T_IOData[0]);
                label_OutBit1.Text = "1";
            }
            else
            {
                bit[1] = false;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[0]);
                label_OutBit1.Text = "0";
            }
        }

        private void button_Bit2_Click(object sender, EventArgs e)
        {
            if (bit[2] == false)
            {
                bit[2] = true;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_0100 | eeipClient.O_T_IOData[0]);
                label_OutBit2.Text = "1";
            }
            else
            {
                bit[2] = false;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[0]);
                label_OutBit2.Text = "0";
            }
        }

        private void button_Bit3_Click(object sender, EventArgs e)
        {
            if (bit[3] == false)
            {
                bit[3] = true;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_1000 | eeipClient.O_T_IOData[0]);
                label_OutBit3.Text = "1";
            }
            else
            {
                bit[3] = false;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[0]);
                label_OutBit3.Text = "0";
            }
        }

        private void button_Bit4_Click(object sender, EventArgs e)
        {
            if (bit[4] == false)
            {
                bit[4] = true;
                eeipClient.O_T_IOData[0] = (byte)(0b_0001_0000 | eeipClient.O_T_IOData[0]);
                label_OutBit4.Text = "1";
            }
            else
            {
                bit[4] = false;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[0]);
                label_OutBit4.Text = "0";
            }
        }

        private void button_Bit5_Click(object sender, EventArgs e)
        {
            if (bit[5] == false)
            {
                bit[5] = true;
                eeipClient.O_T_IOData[0] = (byte)(0b_0010_0000 | eeipClient.O_T_IOData[0]);
                label_OutBit5.Text = "1";
            }
            else
            {
                bit[5] = false;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[0]);
                label_OutBit5.Text = "0";
            }
        }

        private void button_Bit6_Click(object sender, EventArgs e)
        {
            if (bit[6] == false)
            {
                bit[6] = true;
                eeipClient.O_T_IOData[0] = (byte)(0b_0100_0000 | eeipClient.O_T_IOData[0]);
                label_OutBit6.Text = "1";
            }
            else
            {
                bit[6] = false;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[0]);
                label_OutBit6.Text = "0";
            }
        }

        private void button_Bit7_Click(object sender, EventArgs e)
        {
            if (bit[7] == false)
            {
                bit[7] = true;
                eeipClient.O_T_IOData[0] = (byte)(0b_1000_0000 | eeipClient.O_T_IOData[0]);
                label_OutBit7.Text = "1";
            }
            else
            {
                bit[7] = false;
                eeipClient.O_T_IOData[0] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[0]);
                label_OutBit7.Text = "0";
            }
        }

        private void button_Bit8_Click(object sender, EventArgs e)
        {
            if (bit[8] == false)
            {
                bit[8] = true;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_0001 | eeipClient.O_T_IOData[1]);
                label_OutBit8.Text = "1";
            }
            else
            {
                bit[8] = false;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[1]);
                label_OutBit8.Text = "0";
            }
        }

        private void button_Bit9_Click(object sender, EventArgs e)
        {
            if (bit[9] == false)
            {
                bit[9] = true;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_0010 | eeipClient.O_T_IOData[1]);
                label_OutBit9.Text = "1";
            }
            else
            {
                bit[9] = false;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[1]);
                label_OutBit9.Text = "0";
            }
        }

        private void button_BitA_Click(object sender, EventArgs e)
        {
            if (bit[10] == false)
            {
                bit[10] = true;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_0100 | eeipClient.O_T_IOData[1]);
                label_OutBitA.Text = "1";
            }
            else
            {
                bit[10] = false;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[1]);
                label_OutBitA.Text = "0";
            }
        }

        private void button_BitB_Click(object sender, EventArgs e)
        {
            if (bit[11] == false)
            {
                bit[11] = true;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_1000 | eeipClient.O_T_IOData[1]);
                label_OutBitB.Text = "1";
            }
            else
            {
                bit[11] = false;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[1]);
                label_OutBitB.Text = "0";
            }
        }

        private void button_BitC_Click(object sender, EventArgs e)
        {
            if (bit[12] == false)
            {
                bit[12] = true;
                eeipClient.O_T_IOData[1] = (byte)(0b_0001_0000 | eeipClient.O_T_IOData[1]);
                label_OutBitC.Text = "1";
            }
            else
            {
                bit[12] = false;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[1]);
                label_OutBitC.Text = "0";
            }
        }

        private void button_BitD_Click(object sender, EventArgs e)
        {
            if (bit[13] == false)
            {
                bit[13] = true;
                eeipClient.O_T_IOData[1] = (byte)(0b_0010_0000 | eeipClient.O_T_IOData[1]);
                label_OutBitD.Text = "1";
            }
            else
            {
                bit[13] = false;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[1]);
                label_OutBitD.Text = "0";
            }
        }

        private void button_BitE_Click(object sender, EventArgs e)
        {
            if (bit[14] == false)
            {
                bit[14] = true;
                eeipClient.O_T_IOData[1] = (byte)(0b_0100_0000 | eeipClient.O_T_IOData[1]);
                label_OutBitE.Text = "1";
            }
            else
            {
                bit[14] = false;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[1]);
                label_OutBitE.Text = "0";
            }
        }

        private void button_BitF_Click(object sender, EventArgs e)
        {
            if (bit[15] == false)
            {
                bit[15] = true;
                eeipClient.O_T_IOData[1] = (byte)(0b_1000_0000 | eeipClient.O_T_IOData[1]);
                label_OutBitF.Text = "1";
            }
            else
            {
                bit[15] = false;
                eeipClient.O_T_IOData[1] = (byte)(0b_0000_0000 & eeipClient.O_T_IOData[1]);
                label_OutBitF.Text = "0";
            }
        }

        private void UpdateInputs()
        {
            label_InBit0.Text = (eeipClient.T_O_IOData[0] & 0b_0000_0001) == 0b_0000_0001 ? "1" : "0";
            label_InBit1.Text = (eeipClient.T_O_IOData[0] & 0b_0000_0010) == 0b_0000_0010 ? "1" : "0";
            label_InBit2.Text = (eeipClient.T_O_IOData[0] & 0b_0000_0100) == 0b_0000_0100 ? "1" : "0";
            label_InBit3.Text = (eeipClient.T_O_IOData[0] & 0b_0000_1000) == 0b_0000_1000 ? "1" : "0";
            label_InBit4.Text = (eeipClient.T_O_IOData[0] & 0b_0001_0000) == 0b_0001_0000 ? "1" : "0";
            label_InBit5.Text = (eeipClient.T_O_IOData[0] & 0b_0010_0000) == 0b_0010_0000 ? "1" : "0";
            label_InBit6.Text = (eeipClient.T_O_IOData[0] & 0b_0100_0000) == 0b_0100_0000 ? "1" : "0";
            label_InBit7.Text = (eeipClient.T_O_IOData[0] & 0b_1000_0000) == 0b_1000_0000 ? "1" : "0";
            label_InBit8.Text = (eeipClient.T_O_IOData[1] & 0b_0000_0001) == 0b_0000_0001 ? "1" : "0";
            label_InBit9.Text = (eeipClient.T_O_IOData[1] & 0b_0000_0010) == 0b_0000_0010 ? "1" : "0";
            label_InBitA.Text = (eeipClient.T_O_IOData[1] & 0b_0000_0100) == 0b_0000_0100 ? "1" : "0";
            label_InBitB.Text = (eeipClient.T_O_IOData[1] & 0b_0000_1000) == 0b_0000_1000 ? "1" : "0";
            label_InBitC.Text = (eeipClient.T_O_IOData[1] & 0b_0001_0000) == 0b_0001_0000 ? "1" : "0";
            label_InBitD.Text = (eeipClient.T_O_IOData[1] & 0b_0010_0000) == 0b_0010_0000 ? "1" : "0";
            label_InBitE.Text = (eeipClient.T_O_IOData[1] & 0b_0100_0000) == 0b_0100_0000 ? "1" : "0";
            label_InBitF.Text = (eeipClient.T_O_IOData[1] & 0b_1000_0000) == 0b_1000_0000 ? "1" : "0";
        }

        private void Background()
        {
            while (connected == true)
            {
                this.Invoke(new Action(() => UpdateInputs()));

                //Detect Timeout (Read last Received Message Property)
                if (eeipClient.LastReceivedImplicitMessage.Ticks != 0)
                {
                    if (DateTime.Now.Ticks > eeipClient.LastReceivedImplicitMessage.Ticks + (3000 * 10000))
                    {
                        eeipClient.ForwardClose();
                        eeipClient.UnRegisterSession();
                        connected = false;
                        Invoke(new Action(() => panel_Connection.BackColor = Color.Red));
                    }
                }

                Thread.Sleep(50);
            }
        }
    }
}
