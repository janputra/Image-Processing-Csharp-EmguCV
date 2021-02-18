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

namespace HSVSegmentation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       // Capture capture= new Capture();
        Image<Bgr,byte> frame= new Image<Bgr,byte>(320,240);
        Image<Hsv, byte> hsv = new Image<Hsv, byte>(320, 240);
        Image<Gray, byte> result = new Image<Gray, byte>(320, 240);
        string sourcefile;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            frame = CvInvoke.Imread(sourcefile, Emgu.CV.CvEnum.LoadImageType.AnyColor).ToImage<Bgr, byte>();
            frame = frame.Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic);
            int thrs = trackBar1.Value;

            textBox1.Text = Convert.ToString(thrs - 10);
            textBox2.Text = Convert.ToString(thrs + 10);


            hsv = frame.Convert<Hsv, byte>();

            for (int i = 0; i < frame.Height; i++)
            {
                for (int j = 0; j < frame.Width; j++)
                {
                    int hue = hsv.Data[i, j, 0];

                    if (hue > thrs -10 && hue < thrs + 10)
                    {
                        result.Data[i, j, 0] = 255;
                    }
                    else
                    {

                        result.Data[i, j, 0] = 0;

                    }


                }

            
                imageBox2.Image = result;
            }
            
            }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            
        }

        Image<Gray, byte> Erosion(Image<Gray,byte> src) 
        {
            int height= src.Height;
            int width= src.Width;
            Image<Gray, byte> res = new Image<Gray, byte>(width, height);

           
            for (int i = 1; i < height-1; i++)
            {
                for (int j = 1; j < width-1; j++)
                {
                    byte a00 = src.Data[i - 1, j - 1, 0];
                    byte a01 = src.Data[i - 1, j , 0];//
                    byte a02 = src.Data[i - 1, j+1, 0];

                    byte a10 = src.Data[i , j - 1, 0];//
                    byte a11 = src.Data[i , j, 0];//
                    byte a12 = src.Data[i , j + 1, 0];//

                    byte a20 = src.Data[i + 1, j - 1, 0];
                    byte a21 = src.Data[i + 1, j, 0];//
                    byte a22 = src.Data[i + 1, j + 1, 0];



                    if (a01 == 255 && a10 == 255 && a11 == 255 && a12 == 255 && a21 == 255)
                    {
                        res.Data[i, j, 0] = 255;

                    }
                    else 
                    {
                        res.Data[i, j, 0] = 0;
                    }
                }
            }
            return res;
        
        }

        Image<Gray, byte> Dilation(Image<Gray, byte> src)
        {
            int height = src.Height;
            int width = src.Width;
            Image<Gray, byte> res = new Image<Gray, byte>(width, height);


            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    byte a00 = src.Data[i - 1, j - 1, 0];
                    byte a01 = src.Data[i - 1, j, 0];
                    byte a02 = src.Data[i - 1, j + 1, 0];

                    byte a10 = src.Data[i, j - 1, 0];
                    byte a11 = src.Data[i, j, 0];
                    byte a12 = src.Data[i, j + 1, 0];

                    byte a20 = src.Data[i + 1, j - 1, 0];
                    byte a21 = src.Data[i + 1, j, 0];
                    byte a22 = src.Data[i + 1, j + 1, 0];



                    if (a01 == 255 || a10 == 255 || a11 == 255 || a12 == 255 || a21 == 255)
                    {
                        res.Data[i, j, 0] = 255;

                    }
                    else
                    {
                        res.Data[i, j, 0] = 0;
                    }



                }
            }
            return res;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select an Image.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.Title = "Select an Image";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a an Image file was selected, open it.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // take the path of image file
                sourcefile = openFileDialog1.FileName;


                frame = CvInvoke.Imread(sourcefile, Emgu.CV.CvEnum.LoadImageType.AnyColor).ToImage<Bgr,  byte>();
                frame =frame.Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic);
                imageBox1.Image = frame;
             

            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            result = Erosion(result);
            imageBox3.Image = result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            result = Dilation(result);
            imageBox3.Image = result;
        }
    }
}
