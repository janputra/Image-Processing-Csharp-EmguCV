using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Sharpening
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Capture capture =  new Capture();
        Image<Gray, byte> gray = new Image<Gray, byte>(320, 240);
        Image<Gray, byte> sharp = new Image<Gray, byte>(320, 240);

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    //    Image<Gray, byte> conv(Image<Gray,byte> input, int[][] mask) 
     //   {




    //    }

        private void timer1_Tick(object sender, EventArgs e)
        {
            gray = capture.QueryFrame().ToImage<Gray, byte>();
            gray = gray.Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic).Flip(Emgu.CV.CvEnum.FlipType.Horizontal);


            for (int x = 0; x < gray.Height; x++)
            {
                for (int y = 0; y < gray.Width; y++)
                {
                    if (x > 0)
                    {
                        if (y > 0)
                        {
                            if (x < gray.Height-1)
                            {
                                if (y < gray.Width-1)
                                {
                                    byte z4 = gray.Data[x, y, 0];
                                    byte z1 = gray.Data[x - 1, y, 0];
                                    byte z3 = gray.Data[x, y - 1, 0];
                                    byte z5 = gray.Data[x, y + 1, 0];
                                    byte z7 = gray.Data[x + 1, y, 0];
                                    int sharpData = (5 * z4) - z1 - z3 - z5 - z7;
                                    if (sharpData > 255) { sharpData = 255; }
                                    if (sharpData < 0) { sharpData = 0; }

                                    sharp.Data[x, y, 0] = Convert.ToByte(sharpData);
                                }
                                else { sharp.Data[x, y, 0] = gray.Data[x, y, 0]; }
                            }
                            else { sharp.Data[x, y, 0] = gray.Data[x, y, 0]; }
                        }
                        else { sharp.Data[x, y, 0] = gray.Data[x, y, 0]; }
                    }
                    else { sharp.Data[x, y, 0] = gray.Data[x, y, 0]; }
                   


                }
            }

            imageBox1.Image = gray;
            imageBox2.Image = sharp;
        }
    }
}
