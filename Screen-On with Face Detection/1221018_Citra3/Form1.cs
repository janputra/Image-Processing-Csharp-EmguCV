using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.UI;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Runtime.InteropServices;
namespace _1221018_Citra3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static class Monitor
        {
            // Signatures for unmanaged calls
            [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
            
            const int HWND_BROADCAST = 0xffff;
            //the message is sent to all 
            //top-level windows in the system

            const int HWND_TOPMOST = -1;
            //the message is sent to one 
            //top-level window in the system

            const int SC_MONITORPOWER = 0xF170;
            const int WM_SYSCOMMAND = 0x0112;
            const int MONITOR_ON = -1;
            const int MONITOR_OFF = 2;
            const int MONITOR_STANDBY = 1;
           

            public static void ON()
            {
                SendMessage((IntPtr)HWND_BROADCAST, WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)MONITOR_ON);
            }

            public static void OFF()
            {
                SendMessage((IntPtr)HWND_BROADCAST, WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)MONITOR_OFF);
            }

           
         
        }
        Capture capture = new Capture();
        Image<Bgr, Byte> ColoredIMG = new Image<Bgr, byte>(320, 240);
        Image<Bgr, Byte> ColoredIMG2 = new Image<Bgr, byte>(320, 240);
        Image<Gray, Byte> GrayIMG = new Image<Gray, byte>(320, 240);
        HaarCascade face_det = new HaarCascade("haarcascade_frontalface_alt.xml");
        int a,b,c,d, Dcount, Scount;
        Boolean ActiveD, ActiveS;
        Boolean Wajah;
        string monitor_state;

        private void timer1_Tick(object sender, EventArgs e)
        {
            ColoredIMG = capture.QueryFrame().Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
            ColoredIMG = ColoredIMG.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);
            GrayIMG = ColoredIMG.Convert<Gray, Byte>();

            var detectedFaces = GrayIMG.DetectHaarCascade(face_det, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
 new Size(20, 20))[0];
            Wajah = false;
           foreach (var face in detectedFaces) 
           {
               ColoredIMG.Draw(face.rect, new Bgr(Color.Pink), 2);
               Wajah = true;            
           }
            
    
            imageBox1.Image = ColoredIMG;

           if (Wajah == true)
            {
                a = 0 ;
            }
            if (Wajah == false) 
            {
                a = a + 1;
            }
            if (comboBox3.Text == "Second(s)") 
            {
                Dcount = 10* Convert.ToInt32(comboBox1.Text);
            
            }
            else if (comboBox3.Text == "Minute(s)") 
            {
                Dcount = 600 * Convert.ToInt32(comboBox1.Text);
            }
           
            Scount = 600 * Convert.ToInt32(comboBox2.Text);
            if (ActiveD == true)
            {
                if (a == Dcount)
                {
                    if (monitor_state == "ON")
                    {

                        Monitor.OFF();
                        monitor_state = "OFF";
                    }

                }

                if (a == 0)
                {
                    if (monitor_state == "OFF")
                    {
                        Monitor.ON();
                        monitor_state = "ON";
                    }
                }
            }
            if (ActiveS == true)
            {
                if (a == Scount)
                {
                    a = 0;
                    if (radioButton1.Checked)
                    {
                        Application.SetSuspendState(PowerState.Suspend, false, false);
                    }
                    if (radioButton2.Checked) 
                    {
                        Application.SetSuspendState(PowerState.Hibernate, false, false);
                    }
                }
            }
            b = a / 600;
            c = a / 10;
            if (a > 599)
            {
                if (d > 599)
                {
                    d = 0;
                }
                c = d / 10;
                d = d + 1;
                
                
              
            }
           textBox1.Text = Convert.ToString(b) + " " + "Minute(s)";
           textBox2.Text = Convert.ToString(c) + " " + "Second(s)";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start") 
            {
                timer1.Enabled = true;
                button1.Text = "Stop";
            }
            else if (button1.Text == "Stop") 
            {
                timer1.Enabled = false;
                imageBox1.Image = ColoredIMG2;
                button1.Text = "Start";
            }

        }

    

        private void Form1_Load(object sender, EventArgs e)
        {
            a = 0;
            d = 0;
            monitor_state = "ON";

          
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Enable")
            {
                button2.Text = "Disable";
                ActiveD = true;
            }
            else if (button2.Text == "Disable")
            {
                button2.Text = "Enable";
                ActiveD = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "Enable")
            {
                button4.Text = "Disable";
                ActiveS = true;
            }
            else if (button4.Text == "Disable")
            {
                button4.Text = "Enable";
                ActiveS = false;
            }
        }

      

       

    }
}
