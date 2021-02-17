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
using System.Windows.Forms.DataVisualization.Charting;

namespace _1221018_Jan_Putra_Bahtra_Agung_Citra1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Capture capture = new Capture();

        int x, y; // pixel coordinat

        int red, green, blue; // store image data according the colours

        Image<Bgr, byte> DefaultImage = new Image<Bgr, byte>(320, 240);
        Image<Gray, int> GrayImage = new Image<Gray, int>(320, 240);
        Image<Bgr, Single> ConvertedImage = new Image<Bgr, Single>(320, 240);
        Image<Bgr, Single> DefaultImageH = new Image<Bgr, Single>(320, 240);// Image variable for RGB histogram


        // the constructor for converting RGB to Grayscale 
        void RGBtoGray(Image<Bgr, byte> input, Image<Gray, int> output) // the input is variable that store original image & output is variable that store grayscale image 
        {
            int gray;
            //input = input.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);



            for (x = 0; x < input.Height; x++)
            {
                for (y = 0; y < input.Width; y++)
                {
                    blue = input.Data[x, y, 0];
                    green = input.Data[x, y, 1];
                    red = input.Data[x, y, 2];

                    gray = (blue + green + red) / 3;
                    output.Data[x, y, 0] = gray;
                }

            }
        }

        // the constructor for converting RGB to Binary Image
        void RGBtoBinary(Image<Bgr, byte> input, Image<Gray, int> output, int th) // the input is variable that store original image & output is variable that store binary image// th is  threshold value
        {
            int gray;

           //input = input.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);



            for (x = 0; x < input.Height; x++)
            {
                for (y = 0; y < input.Width; y++)
                {
                    blue = input.Data[x, y, 0];
                    green = input.Data[x, y, 1];
                    red = input.Data[x, y, 2];

                    gray = (blue + green + red) / 3;
                    if (gray >= th)
                    {
                        gray = 255;
                    }
                    else
                    {
                        gray = 0;
                    }
                    output.Data[x, y, 0] = gray;
                }

            }
        }


        // the constructor for converting RGB to YUV
        void RGBtoYUV(Image<Bgr, byte> input, Image<Bgr, Single> output)
        {
            float Yy, Yu, Yv;
            //input = input.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);



            for (x = 0; x < input.Height; x++)
            {
                for (y = 0; y < input.Width; y++)
                {
                    blue = input.Data[x, y, 0];
                    green = input.Data[x, y, 1];
                    red = input.Data[x, y, 2];
                    Yy = Convert.ToSingle((0.299 * red) + (0.587 * green) + (0.114 * blue));
                    Yu = Convert.ToSingle((-0.1470 * red) + (-0.288886 * green) + (0.436 * blue));
                    Yv = Convert.ToSingle((0.615 * red) + (-0.51499 * green) + (-0.10001 * blue));

                    output.Data[x, y, 0] = Convert.ToSingle(Yv);
                    output.Data[x, y, 1] = Convert.ToSingle(Yu);
                    output.Data[x, y, 2] = Convert.ToSingle(Yy);

                }

            }
        }

        // the constructor for converting RGB to Inverse
        void RGBtoInverse(Image<Bgr, byte> input, Image<Gray, int> output)
        {
            int gray, Inv;

            //input = input.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);



            for (x = 0; x < input.Height; x++)
            {
                for (y = 0; y < input.Width; y++)
                {
                    blue = input.Data[x, y, 0];
                    green = input.Data[x, y, 1];
                    red = input.Data[x, y, 2];

                    gray = (blue + green + red) / 3;
                    Inv = 255 - gray;
                    output.Data[x, y, 0] = Inv;
                }

            }
        }
        // the constructor for converting RGB to CIE XYZ
        void RGBtoXYZ(Image<Bgr, byte> input, Image<Bgr, Single> output)
        {
            float Xx, Xy, Xz;
            //input = input.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);



            for (x = 0; x < input.Height; x++)
            {
                for (y = 0; y < input.Width; y++)
                {
                    blue = input.Data[x, y, 0];
                    green = input.Data[x, y, 1];
                    red = input.Data[x, y, 2];
                    Xx = Convert.ToSingle((0.49 * red) + (0.31 * green) + (0.2 * blue));
                    Xy = Convert.ToSingle((0.177 * red) + (0.831 * green) + (0.01 * blue));
                    Xz = Convert.ToSingle((0.0 * red) + (0.01 * green) + (0.99 * blue));

                    output.Data[x, y, 0] = Convert.ToSingle(Xz);
                    output.Data[x, y, 1] = Convert.ToSingle(Xy);
                    output.Data[x, y, 2] = Convert.ToSingle(Xx);


                }

            }
        }
        // the constructor for converting RGB to YIQ
        void RGBtoYIQ(Image<Bgr, byte> input, Image<Bgr, Single> output)
        {
            float Yy, Yi, Yq;
            //input = input.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);

            for (x = 0; x < input.Height; x++)
            {
                for (y = 0; y < input.Width; y++)
                {
                    blue = input.Data[x, y, 0];
                    green = input.Data[x, y, 1];
                    red = input.Data[x, y, 2];
                    Yy = Convert.ToSingle((0.299 * red) + (0.587 * green) + (0.114 * blue));
                    Yi = Convert.ToSingle((0.596 * red) + (-0.274 * green) + (-0.322 * blue));
                    Yq = Convert.ToSingle((0.211 * red) + (-0.523 * green) + (0.312 * blue));
                    output.Data[x, y, 0] = Convert.ToSingle(Yq);
                    output.Data[x, y, 1] = Convert.ToSingle(Yi);
                    output.Data[x, y, 2] = Convert.ToSingle(Yy);
                }

            }
        }


        // the constructor for Brightness Operation
        void BrightnessOP(Image<Bgr, byte> input, Image<Bgr, Single> output, int k) // k as brightness value
        {
            int Bred, Bblue, Bgreen; // Store image data after brightness operation

           // input = input.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);



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

                    output.Data[x, y, 0] = Convert.ToSingle(Bblue);
                    output.Data[x, y, 1] = Convert.ToSingle(Bgreen);
                    output.Data[x, y, 2] = Convert.ToSingle(Bred);
                }

            }
        }
        // the constructor for Contrast Operation
        void ContrastOP(Image<Bgr, byte> input, Image<Bgr, Single> output, int G, int P)
        {
            int Cred, Cblue, Cgreen; // store Image data after contrast operation

            //input = input.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);



            for (x = 0; x < input.Height; x++)
            {
                for (y = 0; y < input.Width; y++)
                {
                    blue = input.Data[x, y, 0];
                    green = input.Data[x, y, 1];
                    red = input.Data[x, y, 2];

                    Cred = (G * (red - P)) + P;
                    Cgreen = (G * (green - P)) + P;
                    Cblue = (G * (blue - P)) + P;

                    if (Cblue > 255)
                    {
                        Cblue = 255;
                    }
                    else if (Cblue < 0)
                    {
                        Cblue = 0;
                    }

                    if (Cgreen > 255)
                    {
                        Cgreen = 255;
                    }
                    else if (Cgreen < 0)
                    {
                        Cgreen = 0;
                    }

                    if (Cred > 255)
                    {
                        Cred = 255;
                    }
                    else if (Cred < 0)
                    {
                        Cred = 0;
                    }
                    if (P == 0 )
                    {
                        output.Data[x, y, 0] = input.Data[x, y, 0];
                        output.Data[x, y, 1] = input.Data[x, y, 1];
                        output.Data[x, y, 2] = input.Data[x, y, 2];

                    }
                    else
                    {
                        output.Data[x, y, 0] = Convert.ToSingle(Cblue);
                        output.Data[x, y, 1] = Convert.ToSingle(Cgreen);
                        output.Data[x, y, 2] = Convert.ToSingle(Cred);
                    }
                }

            }
        }
        void Hist_1Matrix(Image<Gray, int> img, Chart chart, ListBox listbox) //img as GrayImage, chart as chart for showing grayscale histogram, listbox as listbox for storing image data 
        {
            int ncounts, h;
            listbox.Items.Clear();
            chart1.Series.Clear();
            chart.Series.Add("ImgData");
            chart.Series["ImgData"].Points.Clear();
            chart.Series["ImgData"].Color = Color.Gray;
            int[] Hist = new int[256];
            for (h = 0; h < 256; h++)
            {
                ncounts = 0;
                for (x = 0; x < img.Height; x++)
                {
                    for (y = 0; y < img.Width; y++)
                    {
                        if (h == img.Data[x, y, 0])
                        {
                            ncounts += 1;
                        }
                    }
                }
                Hist[h] = ncounts;

                chart.Series["ImgData"].Points.AddXY(h, Hist[h]);
                listbox.Items.Add("(" + h + ") = " + Hist[h]);
            }
        }
        void Hist_3Matrix(Image<Bgr, Single> img, Chart chart1, Chart chart2, Chart chart3, ListBox listbox1, ListBox listbox2, ListBox listbox3)
        {
            int nred, nblue, ngreen;
            listbox1.Items.Clear();
            chart1.Series.Clear();
            chart1.Series.Add("ImgDataRed");
            chart1.Series["ImgDataRed"].Points.Clear();
            chart1.Series["ImgDataRed"].Color = Color.Red;
            listbox2.Items.Clear();
            chart2.Series.Clear();
            chart2.Series.Add("ImgDataGreen");
            chart2.Series["ImgDataGreen"].Points.Clear();
            chart2.Series["ImgDataGreen"].Color = Color.Green;
            listbox3.Items.Clear();
            chart3.Series.Clear();
            chart3.Series.Add("ImgDataBlue");
            chart3.Series["ImgDataBlue"].Points.Clear();
            chart3.Series["ImgDataBlue"].Color = Color.Blue;
            int[] HistRed = new int[256];
            int[] HistBlue = new int[256];
            int[] HistGreen = new int[256];
            for (red = 0; red < 256; red++)
            {
                nred = 0;
                for (x = 0; x < img.Height; x++)
                {
                    for (y = 0; y < img.Width; y++)
                    {
                        if (red == img.Data[x, y, 2])
                        {
                            nred += 1;
                        }
                    }
                }
                HistRed[red] = nred;
                chart1.Series["ImgDataRed"].Points.AddXY(red, HistRed[red]);
                listbox1.Items.Add("(" + red + ") = " + HistRed[red]);
            }

            for (blue = 0; blue < 256; blue++)
            {
                nblue = 0;
                for (x = 0; x < img.Height; x++)
                {
                    for (y = 0; y < img.Width; y++)
                    {
                        if (blue == img.Data[x, y, 0])
                        {
                            nblue += 1;
                        }
                    }
                }
                HistBlue[blue] = nblue;
                chart3.Series["ImgDataBlue"].Points.AddXY(blue, HistBlue[blue]);
                listbox3.Items.Add("(" + blue + ") = " + HistBlue[blue]);
            }

            for (green = 0; green < 256; green++)
            {
                ngreen = 0;
                for (x = 0; x < img.Height; x++)
                {
                    for (y = 0; y < img.Width; y++)
                    {
                        if (green == img.Data[x, y, 1])
                        {
                            ngreen += 1;
                        }
                    }
                }
                HistGreen[green] = ngreen;
                chart2.Series["ImgDataGreen"].Points.AddXY(green, HistGreen[green]);
                listbox2.Items.Add("(" + green + ") = " + HistGreen[green]);
            }




        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            timer1.Enabled = true;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart2.ChartAreas[0].AxisX.Minimum = 0;
            chart3.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 3000;
            chart2.ChartAreas[0].AxisY.Maximum = 3000;
            chart3.ChartAreas[0].AxisY.Maximum = 3000;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DefaultImage = capture.QueryFrame().ToImage<Bgr, Byte>(); ;
            
            imageBox1.Image = DefaultImage;
            DefaultImageH = DefaultImage.Convert<Bgr, Single>();


        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                RGBtoBinary(DefaultImage, GrayImage, trackBar1.Value);

                imageBox1.Image = GrayImage;
            }
            textBox1.Text = Convert.ToString(trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = Convert.ToString(trackBar2.Value);
            if (radioButton8.Checked)
            {
                BrightnessOP(DefaultImage, ConvertedImage, trackBar2.Value);
                imageBox1.Image = ConvertedImage;
            }
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox3.Text = Convert.ToString(trackBar3.Value);
            if (radioButton9.Checked)
            {

                ContrastOP(DefaultImage, ConvertedImage,4, trackBar3.Value);
                imageBox1.Image = ConvertedImage;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            timer1.Enabled = false;
            chart2.Visible = true;
            listBox2.Visible = true;
            chart3.Visible = true;
            listBox3.Visible = true;
            chart1.Visible = true;
            listBox1.Visible = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            trackBar1.Value = 0;
            trackBar2.Value = 0;
            trackBar3.Value = 0;
            chart2.Visible = false;
            listBox2.Visible = false;
            chart3.Visible = false;
            listBox3.Visible = false;
            chart1.Visible = false;
            listBox1.Visible = false;
            this.Width = 372;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            imageBox1.Image = DefaultImageH;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            RGBtoGray(DefaultImage, GrayImage);
            imageBox1.Image = GrayImage;

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

            RGBtoBinary(DefaultImage, GrayImage, trackBar1.Value);
            imageBox1.Image = GrayImage;

        }


        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            RGBtoInverse(DefaultImage, GrayImage);
            imageBox1.Image = GrayImage;

        }


        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

            RGBtoXYZ(DefaultImage, ConvertedImage);
            imageBox1.Image = ConvertedImage;

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

            RGBtoYIQ(DefaultImage, ConvertedImage);
            imageBox1.Image = ConvertedImage;

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

            RGBtoYUV(DefaultImage, ConvertedImage);
            imageBox1.Image = ConvertedImage;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton5.Checked || radioButton6.Checked || radioButton7.Checked || radioButton8.Checked || radioButton9.Checked)
            {


                Hist_3Matrix(ConvertedImage, chart1, chart2, chart3, listBox1, listBox2, listBox3);
                chart2.Visible = true;
                listBox2.Visible = true;
                chart3.Visible = true;
                listBox3.Visible = true;
                chart1.Visible = true;
                listBox1.Visible = true;
                this.Width = 1028;

            }
            else if (radioButton2.Checked || radioButton3.Checked || radioButton4.Checked)
            {
                Hist_1Matrix(GrayImage, chart1, listBox1);
                chart2.Visible = false;
                listBox2.Visible = false;
                chart3.Visible = false;
                listBox3.Visible = false;
                chart1.Visible = true;
                listBox1.Visible = true;
                this.Width = 700;
            }
            else
            {

                Hist_3Matrix(DefaultImageH, chart1, chart2, chart3, listBox1, listBox2, listBox3);
                chart2.Visible = true;
                listBox2.Visible = true;
                chart3.Visible = true;
                listBox3.Visible = true;
                chart1.Visible = true;
                listBox1.Visible = true;
                this.Width = 1028;

            }


        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            imageBox1.Image = DefaultImage;
            trackBar2.Value = 0;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            imageBox1.Image = DefaultImage;
            trackBar3.Value = 0;
        }
    }
}
