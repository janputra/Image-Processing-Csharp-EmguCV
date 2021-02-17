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

namespace _1221018_CitraUTS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Capture capture = new Capture();
        int x, y,r;
        int xpos0,ypos0, temp11, temp12;
        int blue, red, green;
        Image<Bgr, Byte> ColoredIMG = new Image<Bgr, byte>(320, 240);
        Image<Hsv, Byte> HSVIMG = new Image<Hsv, byte>(320, 240);
        Image<Gray, Byte> GrayIMG = new Image<Gray, byte>(320, 240);


      void Detect(Image<Hsv, Byte> input, Image<Gray, Byte> output)
        {
            int DHue;
            int VHue = trackBar1.Value;
            int Gray;
            
            for (y = 0; y < input.Height; y++)
            {
                for (x = 0; x < input.Width; x++)
                {
                    DHue = input.Data[y, x, 0];
                    if (DHue > VHue-5 &&  DHue < VHue+5 )
                    {
                        Gray = 255;

                    }
                    else
                    {
                        Gray = 0;
                    }

                    output.Data[y, x, 0] = Convert.ToByte(Gray);
                    

                }
            }
            output._Erode(3);
            output._Dilate(1);
        }

      void Tracker(Image<Gray, Byte> input, Image<Bgr, Byte> input1)
      {
         
          int gray, i;
          int[] xpos = new int[200000];
          int[] ypos = new int[200000];
          i = 0;

          PointF center = new PointF();
          for (y = 0; y < input.Height; y++)
          {
              for (x = 0; x < input.Width; x++)
              {
                  gray = input.Data[y, x, 0];
                  if (gray == 255)
                  {
                      xpos[i] = x;
                      ypos[i] = y;

                      i = i + 1;
                  }
              }
          }
              if (i != 0)
              {
                  i = i - 1;
              }
              else
              {
                  i = 1;
              }


             temp11 = (xpos[i] - xpos[0])/2;
              temp12 =  ((ypos[i] - ypos[0]))/2;

              xpos0 = xpos[0] + temp11;
              ypos0 = ypos[0]+ temp12;
             

              center.X = xpos0;
              center.Y = ypos0;
              r = temp12;
              if (r != 0)
              {
                  input.Draw(new CircleF(center, r), new Gray(255), 2);
                  input1.Draw(new CircleF(center, r), new Bgr(Color.Red), 2);

              }

          
      }

        double distance(double r)
        {
            r = r / 2;
            double c2,c1,c0;
            c2 = 0.00105;
            c1 =-1.18260;
            c0 = 98.8725;
            double hasil;
            hasil = c2 * (r * r);
            hasil = hasil + (c1 * r);
            hasil = hasil + c0;
            return hasil;
         }

        void BrightnessOP(Image<Bgr, byte> input, Image<Bgr, byte> output,  int k) // k as brightness value
        {
            int Bred, Bblue, Bgreen; // Store image data after brightness operator


            for (x = 0; x < input.Height; x++)
            {
                for (y = 0; y < input.Width; y++)
                {
                    blue = input.Data[x, y, 0];
                    green = input.Data[x, y, 1];
                    red = input.Data[x, y, 2];

                    Bblue = blue + k;
                    Bgreen = green + k;
                    Bred = red + k;


                    if (Bblue > 255)
                    {
                        Bblue = 255;
                    }
                    else if (Bblue < 0)
                    {
                        Bblue = 0;
                    }

                    if (Bgreen > 255)
                    {
                        Bgreen = 255;
                    }
                    else if (Bgreen < 0)
                    {
                        Bgreen = 0;
                    }

                    if (Bred > 255)
                    {
                        Bred = 255;
                    }
                    else if (Bred < 0)
                    {
                        Bred = 0;
                    }

                    output.Data[x, y, 0] = Convert.ToByte(Bblue);
                    output.Data[x, y, 1] = Convert.ToByte(Bgreen);
                    output.Data[x, y, 2] = Convert.ToByte(Bred);
                }

            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            ColoredIMG = capture.QueryFrame().Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
            ColoredIMG = ColoredIMG.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);
           
            BrightnessOP(ColoredIMG, ColoredIMG, trackBar2.Value);

            HSVIMG = ColoredIMG.Convert<Hsv, Byte>();
            imageBox1.Image = ColoredIMG;
            Detect(HSVIMG, GrayIMG);
            Tracker(GrayIMG, ColoredIMG);
            imageBox2.Image = GrayIMG;
            if (r != 0)
            {
                label3.Text = "Estimsi Jarak (cm) :" + " " + Convert.ToString(distance(r)) + " " + "cm";
            }
            label4.Text = "Radius (pixel) :" + " " + Convert.ToString(r);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = Convert.ToString(trackBar1.Value);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            temp11 = 0;
            temp12 = 0;
            trackBar1.Value = 19;
            label1.Text = Convert.ToString(trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label2.Text = Convert.ToString(trackBar2.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



    }

}