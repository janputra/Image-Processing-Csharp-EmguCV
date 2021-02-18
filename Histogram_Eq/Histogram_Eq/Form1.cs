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

namespace Histogram_Eq
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         

        }

        Image<Gray, Byte> input = new Image<Gray, byte>(320, 240);
        Image<Gray, Byte> output = new Image<Gray, byte>(320, 240);

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
                string sourcefile = openFileDialog1.FileName;
                

                input = CvInvoke.Imread(sourcefile, Emgu.CV.CvEnum.LoadImageType.AnyColor).ToImage<Gray,Byte>();
                input = input.Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic);
                imageBox1.Image = input;
                histogramBox1.ClearHistogram();
                histogramBox1.GenerateHistograms(input, 256); //--> default way to generate histogram 
                histogramBox1.Refresh();
                
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i, x, y,ncounts,ncdf, temp, np;
            float temp2;
            byte temp3;
            int[] hist = new int[256];
            int[] cdf = new int[256];
            ncdf = 0; np = 0;

            for (i = 0; i < 256; i++)  // generate histogram
            {
                ncounts = 0;
                for (x = 0; x < input.Height; x++)
                {
                    for (y = 0; y < input.Width; y++)
                    {
                        if (i == input.Data[x, y, 0])
                        {
                            ncounts += 1;
                            
                        }
                    }
                }
                hist[i] = ncounts;
                ncdf = ncdf + ncounts;
                cdf[i] = ncdf;
                
            }

            for (x = 0; x < input.Height; x++)
            {
                for (y = 0; y < input.Width; y++)
                {

                    temp = input.Data[x, y, 0];
                    temp2 = (cdf[temp] * 255) / 76800;
                    temp3 = Convert.ToByte(Math.Max(0, Math.Round(temp2)));

                    output.Data[x, y, 0] = temp3;
                    np += 1;
                    
                }

            }

            imageBox2.Image = output;
            histogramBox2.ClearHistogram();
            histogramBox2.GenerateHistograms(output, 256); //--> default way to generate histogram 
            histogramBox2.Refresh();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i, x, y, ncounts, ncdf, temp, np;
            float temp2;
            byte temp3;
            int[] hist = new int[256];
            int[] cdf = new int[256];
            ncdf = 0; np = 0;
            int xtiles, ytiles;

            for (xtiles=0;xtiles<input.Height;xtiles+=8)
                for (ytiles=0; ytiles < input.Width; ytiles += 8)
                {
                    {
                        ncounts = 0;
                        ncdf = 0;
                        for (i = 0; i < 256; i++)  // generate histogram
                        {
                            ncounts = 0;
                            for (x = xtiles; x < xtiles+8; x++)
                            {
                                for (y = ytiles; y < ytiles + 8; y++)
                                {
                                    if (i == input.Data[x, y, 0])
                                    {
                                        ncounts += 1;

                                    }
                                }
                            }
                            hist[i] = ncounts;
                            ncdf = ncdf + ncounts;
                            cdf[i] = ncdf;

                        }

                        for (x = xtiles; x < xtiles + 8; x++)
                        {
                            for (y = ytiles; y < ytiles + 8; y++)
                            {

                                temp = input.Data[x, y, 0];
                                temp2 = (cdf[temp] * 255) / 64;
                                temp3 = Convert.ToByte(Math.Max(0, Math.Round(temp2)));

                                output.Data[x, y, 0] = temp3;
                                np += 1;

                            }

                        }
                    }
                }

            imageBox2.Image = output;

            histogramBox2.ClearHistogram();
            histogramBox2.GenerateHistograms(output, 256); //--> default way to generate histogram 
            histogramBox2.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i, x, y, ncounts, ncdf, temp, np;
            float temp2;
            byte temp3;
            int[] hist = new int[256];
            int[] cdf = new int[256];
            ncdf = 0; np = 0;
            int xtiles, ytiles;

            for (xtiles = 0; xtiles < input.Height; xtiles += 16)
                for (ytiles = 0; ytiles < input.Width; ytiles += 16)
                {
                    {
                        ncounts = 0;
                        ncdf = 0;
                        for (i = 0; i < 256; i++)  // generate histogram
                        {
                            ncounts = 0;
                            for (x = xtiles; x < xtiles + 16; x++)
                            {
                                for (y = ytiles; y < ytiles + 16; y++)
                                {
                                    if (i == input.Data[x, y, 0])
                                    {
                                        ncounts += 1;

                                    }
                                }
                            }
                            hist[i] = ncounts;
                            ncdf = ncdf + ncounts;
                            cdf[i] = ncdf;

                        }

                        for (x = xtiles; x < xtiles + 16; x++)
                        {
                            for (y = ytiles; y < ytiles + 16; y++)
                            {

                                temp = input.Data[x, y, 0];
                                temp2 = (cdf[temp] * 255) /256 ;
                                temp3 = Convert.ToByte(Math.Max(0, Math.Round(temp2)));

                                output.Data[x, y, 0] = temp3;
                                np += 1;

                            }

                        }
                    }
                }
          
            imageBox2.Image = output;
            histogramBox2.ClearHistogram();
            histogramBox2.GenerateHistograms(output, 256); //--> default way to generate histogram 
            histogramBox2.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            int i, x, y, ncounts, ncdf, temp, np;
            int m,n;
            float temp2;
            byte temp3;
            int[] hist = new int[256];
            int[] cdf = new int[256];
            ncdf = 0; np = 0;


            for (m = 3; m < input.Height-2; m++) 
            {
                for (n = 3; n < input.Width-2; n++) 
                {
                                 ncounts = 0;
                                 ncdf = 0;

                                for (i = 0; i < 256; i++)  // generate histogram
                                {
                                    ncounts = 0;
                                    for (x = m - 3; x < m + 3; x++)
                                    {
                                        for (y = n - 3; y < n + 3; y++)
                                        {
                                            if (i == input.Data[x, y, 0])
                                            {
                                                ncounts += 1;

                                            }
                                        }
                                    }
                                    hist[i] = ncounts;
                                    ncdf = ncdf + ncounts;
                                    cdf[i] = ncdf;


                                }

                                for (x = m - 3; x < m + 3; x++)
                                {
                                    for (y = n - 3; y < n + 3; y++)
                                    {

                                        temp = input.Data[x, y, 0];
                                        temp2 = (cdf[temp] * 255) / 49;
                                        temp3 = Convert.ToByte(Math.Max(0, Math.Round(temp2)));

                                        output.Data[x, y, 0] = temp3;
                                        np += 1;

                                    }

                            }
                }
            }

           

            imageBox2.Image = output;
            histogramBox2.ClearHistogram();
            histogramBox2.GenerateHistograms(output, 256); //--> default way to generate histogram 
            histogramBox2.Refresh();




        }

        private void button6_Click(object sender, EventArgs e)
        {
            int i, x, y, ncounts, ncdf, temp, np;
            int m, n;
            float temp2;
            byte temp3;
            int[] hist = new int[256];
            int[] cdf = new int[256];
            ncdf = 0; np = 0;


            for (m = 7; m < input.Height - 6; m++)
            {
                for (n = 7; n < input.Width - 6; n++)
                {
                    ncounts = 0;
                    ncdf = 0;

                    for (i = 0; i < 256; i++)  // generate histogram
                    {
                        ncounts = 0;
                        for (x = m - 7; x < m + 7; x++)
                        {
                            for (y = n - 7; y < n + 7; y++)
                            {
                                if (i == input.Data[x, y, 0])
                                {
                                    ncounts += 1;

                                }
                            }
                        }
                        hist[i] = ncounts;
                        ncdf = ncdf + ncounts;
                        cdf[i] = ncdf;


                    }

                    for (x = m - 7; x < m + 7; x++)
                    {
                        for (y = n - 7; y < n + 7; y++)
                        {

                            temp = input.Data[x, y, 0];
                            temp2 = (cdf[temp] * 255) / 225;
                            temp3 = Convert.ToByte(Math.Max(0, Math.Round(temp2)));

                            output.Data[x, y, 0] = temp3;
                            np += 1;

                        }

                    }
                }
            }



            imageBox2.Image = output;
            histogramBox2.ClearHistogram();
            histogramBox2.GenerateHistograms(output, 256); //--> default way to generate histogram 
            histogramBox2.Refresh();
        }



   
    }
}
