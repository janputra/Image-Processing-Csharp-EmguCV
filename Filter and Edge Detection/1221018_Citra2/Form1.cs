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
using System.Windows.Forms.DataVisualization.Charting;

namespace _1221018_Citra2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Capture capture = new Capture(0);
        int x, y;
        int k, l;

        Image<Bgr, byte> Default = new Image<Bgr, byte>(320, 240);
        Image<Gray, byte> Gray = new Image<Gray, byte>(320, 240);
        Image<Gray, byte> Result = new Image<Gray, byte>(320, 240);
       
        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart2.ChartAreas[0].AxisX.Minimum = 0;
           
            chart1.ChartAreas[0].AxisY.Maximum = 3000;
            chart2.ChartAreas[0].AxisY.Maximum = 3000;
           

        }
        // constructor for smoothing (mean filter)  
        void Smoothing(Image<Gray,byte> input, Image<Gray,byte> output, int m , int n)
        {
            int a, b, avg;
           
            a = (m - 1) / 2;
            b = (n - 1) / 2;
            
            for (y = a; y < input.Height - a ; y++)
            {
                for (x = b; x < input.Width - b; x++)
                {
                    avg = 0;
                    for (k = -a; k <= a; k++)
                    {
                        for (l = -b; l <= b; l++)
                        {
                            avg = avg + (input.Data[y + k, x + l, 0]);
                            
                        }
                    }
                    avg = avg / (m * n);
                    if (avg > 255)
                    {
                        avg = 255;
                    }
                    else if (avg < 0)
                    {
                        avg  = 0;
                    }
                    output.Data[y, x, 0] = Convert.ToByte(avg);
                }
            }
        }
        

        //Constructor for Maximum filter
        void MaxFilter(Image<Gray, byte> input, Image<Gray, byte> output, int m, int n)
        {
            int a, b, max;
           
            a = (m - 1) / 2;
            b = (n - 1) / 2;

          
            for (y = a; y < input.Height - a; y++)
            {
                for (x = b; x < input.Width - b; x++)
                {
                   max = input.Data[y-1,x-1,0];
                    for (k = -a; k <= a; k++)
                    {
                        for (l = -b; l <= b; l++)
                        {
                           max = Math.Max(max, input.Data[y + k, x + l, 0]);
                        }
                    }
                   
                    output.Data[y, x, 0] = Convert.ToByte(max);
                }
            }
        }

        //Constructor for Minimum filter
        void MinFilter(Image<Gray, byte> input, Image<Gray, byte> output, int m, int n)
        {
            int a, b, min;
           
            a = (m - 1) / 2;
            b = (n - 1) / 2;

          
            for (y = a; y < input.Height - a; y++)
            {
                for (x = b; x < input.Width - b; x++)
                {
                    min = input.Data[y - 1, x - 1, 0];
                    for (k = -a; k <= a; k++)
                    {
                        for (l = -b; l <= b; l++)
                        {
                            min = Math.Min(min, input.Data[y+ k,x+ l, 0]);
                        }
                    }
                    output.Data[y, x, 0] = Convert.ToByte(min);
                }
            }
        }
        

        //Contructor for edge detection with image gradient metode
        void EdgeIG(Image<Gray, byte> input, Image<Gray, byte> output) 
        {
            int[] a = new int[3];
            double gx, gy,M;
            for (y = 0; y < input.Height-1; y++)
            {
                for (x = 0; x < input.Width-1; x++)
                {
                   a[0] = input.Data[y,x,0];
                   a[1] = input.Data[y,x+1,0];
                   a[2] = input.Data[y+1,x,0];
                   gx = a[1] - a[0];
                   gy = a[2] - a[0];
                   M = Math.Pow(gx, 2) + Math.Pow(gy, 2);
                   M = Math.Sqrt(M);

                   if (M > 255) 
                   {
                       M = 255;
                   }
                   else if (M < 0)
                   {
                       M = 0;
                   }

                   output.Data[y, x, 0] = Convert.ToByte(M);
                    
                
                }

                    
                }
            
        }

        void EdgeRobert(Image<Gray, byte> input, Image<Gray, byte> output)
        {
            int[] a = new int[4];
            double gx, gy, M;
            for (y = 0; y < input.Height-1 ; y++)
            {
                for (x = 0; x < input.Width-1; x++)
                {
                    a[0] = input.Data[y, x, 0];
                    a[1] = input.Data[y, x + 1, 0];
                    a[2] = input.Data[y + 1, x, 0];
                    a[3] = input.Data[y + 1, x + 1, 0];
                    gx = a[3] - a[0];
                    gy = a[2] - a[1];
                    M = Math.Pow(gx, 2) + Math.Pow(gy, 2);
                    M = Math.Sqrt(M);

                    if (M > 255)
                    {
                        M = 255;
                    }
                    else if (M < 0)
                    {
                        M = 0;
                    }

                    output.Data[y, x, 0] = Convert.ToByte(M);


                }


            }
        }

       void EdgePrewitt(Image<Gray, byte> input, Image<Gray, byte> output)
        {
            int[] a = new int[9];
            double gx, gy, M;
            for (y = 1; y < input.Height-1; y++)
            {
                for (x = 1; x < input.Width-1; x++)
                {
                    a[0] = input.Data[y - 1, x - 1, 0];
                    a[1] = input.Data[y - 1, x, 0];
                    a[2] = input.Data[y - 1, x + 1, 0];
                    a[3] = input.Data[y, x - 1, 0];
                    a[4] = input.Data[y, x, 0];
                    a[5] = input.Data[y, x + 1, 0];
                    a[6] = input.Data[y + 1, x - 1, 0];
                    a[7] = input.Data[y + 1, x, 0];
                    a[8] = input.Data[y + 1, x + 1, 0];
                    gx = ((a[6]+a[7]+a[8]) - (a[0]+a[1]+a[2]));
                    gy = ((a[2] + a[5] + a[8]) - (a[0] + a[3] + a[6]));
                    M = Math.Pow(gx, 2) + Math.Pow(gy, 2);
                    M = Math.Sqrt(M);

                    if (M > 255)
                    {
                        M = 255;
                    }
                    else if (M  < 0)
                    {
                        M = 0;
                    }

                    output.Data[y, x, 0] = Convert.ToByte(M);


                }


            }

        }


       void EdgeSobel(Image<Gray, byte> input, Image<Gray, byte> output)
       {
           int[] a = new int[9];
           double gx, gy, M;
           for (y = 1; y < input.Height-1 ; y++)
           {
               for (x = 1; x < input.Width-1 ; x++)
               {
                   a[0] = input.Data[y - 1, x - 1, 0];
                   a[1] = input.Data[y - 1, x, 0];
                   a[2] = input.Data[y - 1, x + 1, 0];
                   a[3] = input.Data[y, x - 1, 0];
                   a[4] = input.Data[y, x, 0];
                   a[5] = input.Data[y, x + 1, 0];
                   a[6] = input.Data[y + 1, x - 1, 0];
                   a[7] = input.Data[y + 1, x, 0];
                   a[8] = input.Data[y + 1, x + 1, 0];

                   gx = ((a[6] + (2*a[7]) + a[8]) - (a[0] + (2*a[1]) + a[2]));
                   gy = ((a[2] + (2*a[5]) + a[8]) - (a[0] + (2*a[3]) + a[6]));
                   M = Math.Pow(gx, 2) + Math.Pow(gy, 2);
                   M = Math.Sqrt(M);

                   if (M > 255)
                   {
                       M = 255;
                   }
                   else if (M < 0)
                   {
                       M = 0;
                   }

                   output.Data[y, x, 0] = Convert.ToByte(M);


               }


           }

       }



       

        void Hist_1Matrix(Image<Gray, byte> img, Chart chart, ListBox listbox) //img as GrayImage, chart as chart for showing grayscale histogram, listbox as listbox for storing image data 
        {
            int ncounts, h;
            listbox.Items.Clear();
            chart.Series.Clear();
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
   
        private void timer1_Tick(object sender, EventArgs e)
        {
            Default = capture.QueryFrame().Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
            Default = Default.Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, true);
            Gray = Default.Convert<Gray, byte>();
            if (comboBox1.Text == "Filter") 
            {
                if (radioButton5.Checked)
                {
                    if (radioButton1.Checked)
                    {
                        Smoothing(Gray, Result, 3, 3);
                    }
                    else if (radioButton2.Checked)
                    {
                        MaxFilter(Gray, Result, 3, 3);
                    }
                    else if (radioButton3.Checked)
                    {
                        MinFilter(Gray, Result, 3, 3);
                    }
                }
                else if (radioButton6.Checked)
                {
                    if (radioButton1.Checked)
                    {
                        Smoothing(Gray, Result, 5, 5);
                    }
                    else if (radioButton2.Checked)
                    {
                        MaxFilter(Gray, Result, 5, 5);
                    }
                    else if (radioButton3.Checked)
                    {
                        MinFilter(Gray, Result, 5, 5);
                    }
                }
                else if (radioButton7.Checked) 
                {
                    if (radioButton1.Checked)
                    {
                        Smoothing(Gray, Result, 7, 7);
                    }
                    else if (radioButton2.Checked)
                    {
                        MaxFilter(Gray, Result, 7, 7);
                    }
                    else if (radioButton3.Checked)
                    {
                        MinFilter(Gray, Result, 7, 7);
                    }

                }
            }
            else if (comboBox1.Text == "Edge Detection") 
            {
                if (radioButton1.Checked) 
                {
                    EdgeIG(Gray, Result);
                }
                if (radioButton2.Checked)
                {
                    EdgeRobert(Gray, Result);
                }
                if (radioButton3.Checked)
                {
                    EdgePrewitt(Gray, Result);

                }
                if (radioButton4.Checked)
                {
                    EdgeSobel(Gray, Result);
                    
                }
            }
           
            imageBox1.Image = Gray;
            imageBox2.Image = Result;


        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)// matrix 3 x 3
        {
            if (radioButton1.Checked)
            {
                Smoothing(Gray, Result, 3, 3);
            }
            else if (radioButton2.Checked)
            {
                MaxFilter(Gray, Result, 3, 3);
            }
            else if (radioButton3.Checked)
            {
                MinFilter(Gray, Result, 3, 3);
            }
            if (chart1.Visible == true)
            {
            
                Hist_1Matrix(Result, chart2, listBox2);
            }
            imageBox2.Image = Result;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)// matrix 5 x 5
        {
            if (radioButton1.Checked)
            {
                Smoothing(Gray, Result, 5, 5);
            }
            else if (radioButton2.Checked)
            {
                MaxFilter(Gray, Result, 5, 5);
            }
            else if (radioButton3.Checked)
            {
                MinFilter(Gray, Result, 5, 5);
            }
            if (chart1.Visible == true)
            {
                
                Hist_1Matrix(Result, chart2, listBox2);
            }
            imageBox2.Image = Result;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e) // matrix 7 x 7
        {
            if (radioButton1.Checked)
            {
                Smoothing(Gray, Result, 7, 7);
            }
            else if (radioButton2.Checked)
            {
                MaxFilter(Gray, Result, 7, 7);
            }
            else if (radioButton3.Checked)
            {
                MinFilter(Gray, Result, 7, 7);
            }
            if (chart1.Visible == true)
            {
                Hist_1Matrix(Result, chart2, listBox2);
            }
            imageBox2.Image = Result;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Edge Detection") 
            {
                EdgeIG(Gray, Result);
               
            }
            if (comboBox1.Text == "Filter")
            {
                if (radioButton5.Checked) 
                {
                    Smoothing(Gray, Result, 3, 3);
                }
                else if (radioButton6.Checked)
                {
                    Smoothing(Gray, Result, 5, 5);
                }
                else if (radioButton7.Checked)
                {
                    Smoothing(Gray, Result, 7, 7);
                }
            
            }
            if (chart1.Visible == true)
            {

                Hist_1Matrix(Result, chart2, listBox2);
            }
            imageBox2.Image = Result;
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Edge Detection")
            {
                EdgeRobert(Gray, Result);
                
            }
            if (comboBox1.Text == "Filter")
            {
                if (radioButton5.Checked)
                {
                    MaxFilter(Gray, Result, 3, 3);
                }
                else if (radioButton6.Checked)
                {
                    MaxFilter(Gray, Result, 5, 5);
                }
                else if (radioButton7.Checked)
                {
                    MaxFilter(Gray, Result, 7, 7);
                }

            }
            if (chart1.Visible == true)
            {

                Hist_1Matrix(Result, chart2, listBox2);
            }
            imageBox2.Image = Result;

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Edge Detection")
            {
                EdgePrewitt(Gray, Result);
                
            }
            if (comboBox1.Text == "Filter")
            {
                if (radioButton5.Checked)
                {
                    MinFilter(Gray, Result, 3, 3);
                }
                else if (radioButton6.Checked)
                {
                    MinFilter(Gray, Result, 5, 5);
                }
                else if (radioButton7.Checked)
                {
                    MinFilter(Gray, Result, 7, 7);
                }

            }
            if (chart1.Visible == true)
            {

                Hist_1Matrix(Result, chart2, listBox2);
            }
            imageBox2.Image = Result;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Edge Detection")
            {
                EdgeSobel(Gray, Result);
                if (chart1.Visible == true)
                {
                    
                    Hist_1Matrix(Result, chart2, listBox2);
                }
                imageBox2.Image = Result;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = true;
            groupBox5.Visible = true;
            Hist_1Matrix(Gray, chart1, listBox1);
            Hist_1Matrix(Result, chart2, listBox2);
            button3.Enabled = false;
            groupBox3.Top = button1.Top;
            groupBox5.Top = groupBox3.Top;
            button1.Top = groupBox3.Top + groupBox3.Height + 21;
            button2.Top = button1.Top;
            button3.Top = button1.Top;
            button4.Top = button1.Top;
            comboBox1.Top = button1.Top;
            groupBox1.Top = comboBox1.Top + 34;
            groupBox2.Top = groupBox1.Top;
            this.Height = 720;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            button3.Enabled = false;
            groupBox3.Visible = false;
            groupBox5.Visible = false;
            groupBox3.Top = 439;
            groupBox5.Top = groupBox3.Top;
            button1.Top = 285;
            button2.Top = button1.Top;
            button3.Top = button1.Top;
            button4.Top = button1.Top;
            comboBox1.Top = button1.Top;
            groupBox1.Top = comboBox1.Top + 34;
            groupBox2.Top = groupBox1.Top;
            this.Height = 474;
        }

      
        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Filter")
            {
                groupBox1.Visible = true;
                groupBox1.Height = groupBox2.Height;
                groupBox2.Visible = true;
                radioButton1.Text = "Smoothing (Average Filter)";
                radioButton2.Text = "Max Filter";
                radioButton3.Text = "Min Filter";
                groupBox1.Text = "Filter";
                label2.Text = "Hasil Filtering";
                groupBox3.Text = "Histogram Hasil Filtering";
                
            }
            else if (comboBox1.Text == "Edge Detection") 
            {
                groupBox1.Visible = true;
                groupBox1.Height = 114;
                groupBox2.Visible = false;
                radioButton1.Text = "Gradient Operator";
                radioButton2.Text = "Robert Operator";
                radioButton3.Text = "Prewitt operator";
                radioButton4.Text = "Sobel operator";
                groupBox1.Text = "Edge Detection";
                label2.Text = "Hasil Edge Detection";
                groupBox5.Text = "Histogram Hasil Edge Detection";
            }
        }

        

    }
}
