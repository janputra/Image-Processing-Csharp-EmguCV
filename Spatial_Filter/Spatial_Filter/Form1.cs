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


namespace Spatial_Filter
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

        Image<Gray, Byte> source = new Image<Gray, byte>(320, 240);

        Image<Gray, Byte> output = new Image<Gray, byte>(320, 240);
        Image<Gray, Byte> output1 = new Image<Gray, byte>(320, 240);
        string sourcefile;
        bool smoothing, sharpening, edge;

        #region Filter Mask Declaration

        double[,] SF3x3 = new double[3, 3]{{0.111111,0.111111,0.111111},
                                            {0.111111,0.111111,0.111111},
                                            {0.111111,0.111111,0.111111}};

        double[,] SF5x5 = new double[5, 5]{{0.04,0.04,0.04,0.04,0.04},
                                         {0.04,0.04,0.04,0.04,0.04},
                                         {0.04,0.04,0.04,0.04,0.04},
                                         {0.04,0.04,0.04,0.04,0.04},
                                         {0.04,0.04,0.04,0.04,0.04}};


        double[,] SF7x7 = new double[7, 7]{{0.020408,0.020408,0.020408,0.020408,0.020408,0.020408,0.020408},
                                        {0.020408,0.020408,0.020408,0.020408,0.020408,0.020408,0.020408},
                                        {0.020408,0.020408,0.020408,0.020408,0.020408,0.020408,0.020408},
                                        {0.020408,0.020408,0.020408,0.020408,0.020408,0.020408,0.020408},
                                        {0.020408,0.020408,0.020408,0.020408,0.020408,0.020408,0.020408},
                                        {0.020408,0.020408,0.020408,0.020408,0.020408,0.020408,0.020408},
                                        {0.020408,0.020408,0.020408,0.020408,0.020408,0.020408,0.020408}};

        double[,] WSF3x3 = new double[3, 3]{{0.062500,0.125000,0.062500},
                                            {0.125000,0.250000,0.125000},
                                            {0.062500,0.125000,0.062500}};


        double[,] WSF5x5 = new double[5, 5]{{0.003906,0.015625,0.023438,0.015625,0.003906},
                                            {0.015625,0.062500,0.093750,0.062500,0.015625},
                                            {0.023438,0.093750,0.140625,0.093750,0.023438},
                                            {0.015625,0.062500,0.093750,0.062500,0.015625},
                                            {0.003906,0.015625,0.023438,0.015625,0.003906}};

        double[,] WSF7x7 = new double[7, 7]{{0.000239,0.001433,0.003583,0.004778,0.003583,0.001433,0.000239},
                                            {0.001433,0.008600,0.021500,0.028667,0.021500,0.008600,0.001433},
                                            {0.003583,0.021500,0.053751,0.071667,0.060917,0.021500,0.003583},
                                            {0.004778,0.028667,0.071667,0.095557,0.071667,0.028667,0.004778},
                                            {0.003583,0.021500,0.060917,0.071667,0.060917,0.021500,0.003583},
                                            {0.001433,0.008600,0.021500,0.028667,0.021500,0.008600,0.001433},
                                            {0.000239,0.001433,0.003583,0.004778,0.003583,0.001433,0.000239}};

        double[,] SLaplacian = new double[3, 3]{{0,1,0},
                                            {1,-4,1},
                                            {0,1,0}};

        double[,] eSLaplacian = new double[3, 3]{{0,-1,0},
                                            {-1,5,-1},
                                            {0,-1,0}};

        double[,] VLaplacian = new double[3, 3]{{1,1,1},
                                            {1,-8,1},
                                            {1,1,1}};

        double[,] eVLaplacian = new double[3, 3]{{-1,-1,-1},
                                            {-1,9,-1},
                                            {-1,-1,-1}};


        double[,] hRobert = new double[3, 3]{{1,0,0},
                                            {0,-1,0},
                                            {0,0,0}};

        double[,] vRobert = new double[3, 3]{{0,1,0},
                                            {-1,0,0},
                                            {0,0,0}};


        double[,] hPrewitt = new double[3, 3]{{-1,-1,-1},
                                            {0,0,0},
                                            {1,1,1}};


        double[,] vPrewitt = new double[3, 3]{{-1,0,1},
                                            {-1,0,1},
                                            {-1,0,1}};


        double[,] hSobel = new double[3, 3]{{-1,-2,-1},
                                            {0,0,0},
                                            {1,2,1}};


        double[,] vSobel = new double[3, 3]{{-1,0,1},
                                            {-2,0,2},
                                            {-1,0,1}};



        #endregion


        #region Image Operator
        Image<Gray, Byte> Spatial_filter(Image<Gray, Byte> src, int winSize, double[,] mask)
        {

            int x, y, i, j;

            double dummy;

            int height = src.Height;
            int width = src.Width;
            Image<Gray, Byte> res = new Image<Gray, Byte>(width, height);

            int a = (winSize - 1) / 2;

            for (x = a; x < height - a; x++)
            {
                for (y = a; y < width - a; y++)
                {
                    dummy = 0;
                    for (i = -a; i <= a; i++)
                    {
                        for (j = -a; j <= a; j++)
                        {

                            dummy = dummy + (src.Data[x + i, y + j, 0] * mask[a + i, a + j]);

                        }
                    }

                    if (dummy < 0) { dummy = 0; }
                    else if (dummy > 255) { dummy = 255; }
                    res.Data[x, y, 0] = Convert.ToByte(dummy);
                }
            }

            return res;
        }

        Image<Gray, byte> EdgeDet(Image<Gray, Byte> src, double[,] hMask, double[,] vMask) 
        {
            int x, y, i, j;

            double hDummy,vDummy,rDummy;

            int height = src.Height;
            int width = src.Width;
            Image<Gray, Byte> res = new Image<Gray, Byte>(width, height);

           
            for (x = 1; x < height - 1; x++)
            {
                for (y = 1; y < width - 1; y++)
                {
                    hDummy = 0;
                    vDummy = 0;
                    for (i = -1; i <= 1; i++)
                    {
                        for (j = -1; j <= 1; j++)
                        {

                            hDummy = hDummy + (src.Data[x + i, y + j, 0] * hMask[1 + i, 1 + j]);
                            vDummy = vDummy + (src.Data[x + i, y + j, 0] * vMask[1 + i, 1 + j]);

                        }
                    }

                    rDummy = Math.Sqrt(Math.Pow(hDummy, 2) + Math.Pow(vDummy, 2));
                    if (rDummy < 0) { rDummy = 0; }
                    else if (rDummy > 255) { rDummy = 255; }
                    res.Data[x, y, 0] = Convert.ToByte(rDummy);
                }
            }

            return res;
        
        }
        #endregion



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


                source = CvInvoke.Imread(sourcefile, Emgu.CV.CvEnum.LoadImageType.AnyColor).ToImage<Gray, Byte>();
                source = source.Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic);
                imageBox1.Image = source;


            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            source = CvInvoke.Imread(sourcefile, Emgu.CV.CvEnum.LoadImageType.AnyColor).ToImage<Gray, Byte>();
            source = source.Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic);

            if (smoothing)
            {
                if (comboBox1.Text == "3x3 Smoothing Filter")
                {
                    output = Spatial_filter(source, 3, SF3x3);
                }
                else if (comboBox1.Text == "5x5 Smoothing Filter")
                {
                    output = Spatial_filter(source, 5, SF5x5);
                }
                else if (comboBox1.Text == "7x7 Smoothing Filter")
                {
                    output = Spatial_filter(source, 7, SF7x7);
                }
                else if (comboBox1.Text == "3x3 Weighted Smoothing Filter")
                {
                    output = Spatial_filter(source, 3, WSF3x3);
                }
                else if (comboBox1.Text == "5x5 Weighted Smoothing Filter")
                {
                    output = Spatial_filter(source, 5, WSF5x5);
                }
                else if (comboBox1.Text == "7x7 Weighted Smoothing Filter")
                {
                    output = Spatial_filter(source, 7, WSF7x7);
                }

                imageBox2.Image = output;
            }

            if (sharpening)
            {
                if (comboBox1.Text == "Simple Laplacian")
                {
                    output = Spatial_filter(source, 3, SLaplacian);
                    output1 = source - output;
                    // output1 = Spatial_filter(source, 3, eSLaplacian);
                }
                else if (comboBox1.Text == "Variant of Laplacian")
                {

                    output = Spatial_filter(source, 3, VLaplacian);
                    //output1 = source - output;
                    output1 = Spatial_filter(source, 3, eVLaplacian);
                }
                imageBox2.Image = output;
                imageBox3.Image = output1;
            }

            if (edge)
            {
                if (comboBox1.Text == "Robert") 
                {
                    output = EdgeDet(source, hRobert, vRobert);
                }
                else if (comboBox1.Text == "Prewitt")
                {
                    output=EdgeDet(source,hPrewitt,vPrewitt);
                }
                else if (comboBox1.Text == "Sobel")
                {
                    output=EdgeDet(source,hSobel,vSobel);
                }
                imageBox2.Image = output;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            source = CvInvoke.Imread(sourcefile, Emgu.CV.CvEnum.LoadImageType.AnyColor).ToImage<Gray, Byte>();
            source = source.Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic);

            if (comboBox2.Text == "3x3 Smoothing Filter")
            {
                output = Spatial_filter(source, 3, SF3x3);
            }
            else if (comboBox2.Text == "5x5 Smoothing Filter")
            {
                output = Spatial_filter(source, 5, SF5x5);
            }
            else if (comboBox2.Text == "7x7 Smoothing Filter")
            {
                output = Spatial_filter(source, 7, SF7x7);
            }
            else if (comboBox2.Text == "3x3 Weighted Smoothing Filter")
            {
                output = Spatial_filter(source, 3, WSF3x3);
            }
            else if (comboBox2.Text == "5x5 Weighted Smoothing Filter")
            {
                output = Spatial_filter(source, 5, WSF5x5);
            }
            else if (comboBox2.Text == "7x7 Weighted Smoothing Filter")
            {
                output = Spatial_filter(source, 7, WSF7x7);
            }

            imageBox3.Image = output;

            if (edge)
            {
                if (comboBox2.Text == "Robert") 
                {
                    output = EdgeDet(source, hRobert, vRobert);
                }
                else if (comboBox2.Text == "Prewitt")
                {
                    output=EdgeDet(source,hPrewitt,vPrewitt);
                }
                else if (comboBox2.Text == "Sobel")
                {
                    output=EdgeDet(source,hSobel,vSobel);
                }
                imageBox3.Image = output;
            }
        }


        private void smoothingToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (smoothingToolStripMenuItem.Checked)
            {
                sharpeningToolStripMenuItem.Checked = false;
                edgeDetectionToolStripMenuItem.Checked = false;

            }
            comboBox1.Items.Clear();
            comboBox1.Items.Add("3x3 Smoothing Filter");
            comboBox1.Items.Add("5x5 Smoothing Filter");
            comboBox1.Items.Add("7x7 Smoothing Filter");
            comboBox1.Items.Add("3x3 Weighted Smoothing Filter");
            comboBox1.Items.Add("5x5 Weighted Smoothing Filter");
            comboBox1.Items.Add("7x7 Weighted Smoothing Filter");

            comboBox2.Items.Clear();
            comboBox2.Items.Add("3x3 Smoothing Filter");
            comboBox2.Items.Add("5x5 Smoothing Filter");
            comboBox2.Items.Add("7x7 Smoothing Filter");
            comboBox2.Items.Add("3x3 Weighted Smoothing Filter");
            comboBox2.Items.Add("5x5 Weighted Smoothing Filter");
            comboBox2.Items.Add("7x7 Weighted Smoothing Filter");

            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            smoothing = true;
            sharpening = false;
            edge = false;

        }

        private void sharpeningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sharpeningToolStripMenuItem.Checked)
            {
                smoothingToolStripMenuItem.Checked = false;
                edgeDetectionToolStripMenuItem.Checked = false;

                comboBox1.Enabled = true;
                comboBox2.Enabled = false;
                comboBox1.Items.Clear();
                comboBox1.Items.Add("Simple Laplacian");
                comboBox1.Items.Add("Variant of Laplacian");

                smoothing = false;
                sharpening = true;
                edge = false;

            }

        }

        private void edgeDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (edgeDetectionToolStripMenuItem.Checked)
            {
                smoothingToolStripMenuItem.Checked = false;
                sharpeningToolStripMenuItem.Checked = false;

                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox1.Items.Clear();
                comboBox1.Items.Add("Robert");
                comboBox1.Items.Add("Prewitt");
                comboBox1.Items.Add("Sobel");
                

                comboBox2.Items.Clear();
                comboBox2.Items.Add("Robert");
                comboBox2.Items.Add("Prewitt");
                comboBox2.Items.Add("Sobel");
                

                smoothing = false;
                sharpening = false;
                edge = true;

            }

        }
    }
}
