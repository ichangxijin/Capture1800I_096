using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ImageCapturing
{
    public partial class TriggerSettingForm : Form
    {

        TriggerCOM TrigCOM = null;


        public TriggerSettingForm(TriggerCOM trigger)
        {
            
            InitializeComponent();
            TrigCOM = trigger;
            comboBox1.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Console.Clear();
            byte[] triggerdata = new byte[14];
            triggerdata[0] = 0x55;
            triggerdata[1] = 0x66;
            triggerdata[2] = 0x77;
            triggerdata[3] = 0x88;
            triggerdata[4] = 0x01;
            triggerdata[5] = (byte)(comboBox1.SelectedIndex);
            triggerdata[6] = byte.Parse(textBoxDelaytime.Text.Trim());
            triggerdata[7] = byte.Parse(textBoxFrametime1.Text.Trim());
            triggerdata[8] = byte.Parse(textBoxFrametime2.Text.Trim());
            int framnum = byte.Parse(textBoxFramenumber.Text.Trim());
            byte[] bframe = BitConverter.GetBytes(framnum);
            triggerdata[9] = bframe[0];
            triggerdata[10] = bframe[1];
            triggerdata[11] = bframe[2];
            triggerdata[12] = bframe[3];

            //byte Xor = 0x00;
            //for (int i = 0; i < 13;i++ )
            //{
            //    Xor = (byte)(Xor ^ triggerdata[i;])
            //}
            //triggerdata[13] = Xor;
            triggerdata[13] = (byte)(0xFF - triggerdata[12]);

            TrigCOM.SendCMD(triggerdata);
        }


        private void button3_Click(object sender, EventArgs e)
        {

            Console.Clear();
            byte[] triggerdata = new byte[14];
            triggerdata[0] = 0x55;
            triggerdata[1] = 0x66;
            triggerdata[2] = 0x77;
            triggerdata[3] = 0x88;
            triggerdata[4] = 0x02;
            triggerdata[5] = 0x00;
            triggerdata[6] = 0x00;
            triggerdata[7] = 0x00;
            triggerdata[8] = 0x00;
            triggerdata[9] = 0x00;
            triggerdata[10] = 0x00;
            triggerdata[11] = 0x00;
            triggerdata[12] = 0x00;

            //byte Xor = 0x00;
            //for (int i = 0; i < 13; i++)
            //{
            //    Xor = (byte)(Xor ^ triggerdata[i]);
            //}
            //triggerdata[13] = Xor;
            triggerdata[13] = (byte)(0xFF - triggerdata[12]);

            TrigCOM.SendCMD(triggerdata);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.Clear();
            byte[] triggerdata = new byte[14];
            triggerdata[0] = 0x55;
            triggerdata[1] = 0x66;
            triggerdata[2] = 0x77;
            triggerdata[3] = 0x88;
            triggerdata[4] = 0x02;
            triggerdata[5] = 0x00;
            triggerdata[6] = 0x00;
            triggerdata[7] = 0x00;
            triggerdata[8] = 0x00;
            triggerdata[9] = 0x00;
            triggerdata[10] = 0x00;
            triggerdata[11] = 0x00;
            triggerdata[12] = 0x00;

            //byte Xor = 0x00;
            //for (int i = 0; i < 13; i++)
            //{
            //    Xor = (byte)(Xor ^ triggerdata[i]);
            //}
            //triggerdata[13] = Xor;
            triggerdata[13] = (byte)(0xFF - triggerdata[12]);

            int softFnumber = int.Parse(textBoxSoftFrameNumber.Text);
            int softFrameTime = int.Parse(textBoxSoftFrameTime.Text);
            for (int i = 0; i < softFnumber; i++)
            {
                Console.WriteLine("Frame...................................................." + (i + 1));

                TrigCOM.SendCMD(triggerdata);

                Thread.Sleep(softFrameTime);
            }
        }


        //bool opentimefeedback= false;
        //private void button4_Click(object sender, EventArgs e)
        //{

        //    Console.Clear();
        //    if (opentimefeedback)
        //    {
        //        byte[] triggerdata = new byte[14];
        //        triggerdata[0] = 0x55;
        //        triggerdata[1] = 0x66;
        //        triggerdata[2] = 0x77;
        //        triggerdata[3] = 0x88;
        //        triggerdata[4] = 0x81;
        //        triggerdata[5] = 0x02;
        //        triggerdata[6] = 0x00;
        //        triggerdata[7] = 0x00;
        //        triggerdata[8] = 0x00;
        //        triggerdata[9] = 0x00;
        //        triggerdata[10] = 0x00;
        //        triggerdata[11] = 0x00;
        //        triggerdata[12] = 0x00;

        //        byte Xor = 0x00;
        //        for (int i = 0; i < 13; i++)
        //        {
        //            Xor = (byte)(Xor ^ triggerdata[i]);
        //        }
        //        triggerdata[13] = Xor;

        //        trigger.SendBeamInfo(triggerdata);

        //        opentimefeedback = false;


        //        button4.Text = "Open time feedback";
        //    }
        //    else
        //    {
        //        byte[] triggerdata = new byte[14];
        //        triggerdata[0] = 0x55;
        //        triggerdata[1] = 0x66;
        //        triggerdata[2] = 0x77;
        //        triggerdata[3] = 0x88;
        //        triggerdata[4] = 0x81;
        //        triggerdata[5] = 0x00;
        //        triggerdata[6] = 0x00;
        //        triggerdata[7] = 0x00;
        //        triggerdata[8] = 0x00;
        //        triggerdata[9] = 0x00;
        //        triggerdata[10] = 0x00;
        //        triggerdata[11] = 0x00;
        //        triggerdata[12] = 0x00;

        //        byte Xor = 0x00;
        //        for (int i = 0; i < 13; i++)
        //        {
        //            Xor = (byte)(Xor ^ triggerdata[i]);
        //        }
        //        triggerdata[13] = Xor;

        //        trigger.SendBeamInfo(triggerdata);

        //        opentimefeedback = true;

        //        button4.Text = "Close time feedback";
        //    }
        //}
    }
}
