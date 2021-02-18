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
using ZedGraph;
namespace DIP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Capture capture = new Capture();
        Image<Bgr, byte> frame= new Image<Bgr,byte>(320,240);
        Image<Bgr, byte> frameR = new Image<Bgr, byte>(320, 240);
        Image<Bgr, byte> frameG = new Image<Bgr, byte>(320, 240);
        Image<Bgr, byte> frameB = new Image<Bgr, byte>(320, 240);
        Image<Gray, byte> gray = new Image<Gray, byte>(320, 240);
        Image<Gray, byte> inverse = new Image<Gray, byte>(320, 240);
        Image<Gray, byte> binary = new Image<Gray, byte>(320, 240);


        public void Form1_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
 
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            frame = capture.QueryFrame().ToImage<Bgr, byte>();
            frame = frame.Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic).Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
            gray = frame.Convert<Gray, byte>();
            if (radioButton1.Checked)
            {
               
                imageBox1.Image = frame;
            }


            if (radioButton2.Checked) 
            {
                for (int x = 0; x < frame.Height; x++)
                {
                    for (int y = 0; y < frame.Width; y++)
                    {
                        frameR.Data[x, y, 2] = frame.Data[x, y, 2];
                        frameR.Data[x, y, 0] =0;
                        frameR.Data[x, y, 1] = 0;

                        frameG.Data[x, y, 2] = 0;
                        frameG.Data[x, y, 0] = 0;
                        frameG.Data[x, y, 1] = frame.Data[x, y, 1];

                        frameB.Data[x, y, 2] = 0;
                        frameB.Data[x, y, 0] = frame.Data[x, y, 0];
                        frameB.Data[x, y, 1] = 0;

                        

                    }
                    imageBox1.Image = frameR;
                    imageBox2.Image = frameG;
                    imageBox3.Image = frameB;
                }
               
            }
            if (radioButton3.Checked) 
            {
                imageBox1.Image = frame;
                for (int x = 0; x < frame.Height; x++)
                {
                    for (int y = 0; y < frame.Width; y++)
                    {
                        byte R= frame.Data[x,y,2];
                        byte G= frame.Data[x,y,1];
                        byte B= frame.Data[x,y,0];
                        gray.Data[x,y,0] = Convert.ToByte((R + B + G) / 3);



                    }
                }
              
                /*
                //Generate histogram with densehistogram function --> color can be modified for display and comparing diferent histogram
              
                DenseHistogram hist = new DenseHistogram(256, new RangeF(0, 255));
                hist.Calculate<byte>(new Image<Gray, byte>[] { gray }, true, null);
                histogramBox1.AddHistogram("hist", Color.Red, hist, 256,  new  float [] {0.0f, 255.0f});
                */
                
                histogramBox1.GenerateHistograms(gray, 256); //--> default way to generate histogram 
                histogramBox1.Refresh();
                histogramBox1.ClearHistogram();
                imageBox2.Image = gray;

                //updateHist(gray);
           
            }
            if (radioButton4.Checked) 
            {

                for (int x = 0; x < frame.Height; x++)
                {
                    for (int y = 0; y < frame.Width; y++)
                    {
                        if (gray.Data[x, y, 0] < trackBar1.Value)
                        {
                            binary.Data[x, y, 0] = 0;
                        }
                        else
                        {
                            binary.Data[x, y, 0] = 255;

                        }
                    }

                    imageBox1.Image=frame;
                    imageBox2.Image=gray;
                    imageBox3.Image = binary;
                }
                
            
             if (radioButton5.Checked) 
            {

                for (int x = 0; x < frame.Height; x++)
                {
                    for (int y = 0; y < frame.Width; y++)
                    {
                        
                    }

                    imageBox1.Image = frame;
                    imageBox2.Image = gray;
                    
                }
                }
                
            }

        }

        void updateHist(Image<Gray,byte> img) 
        {
            //zedGraphControl1.Refresh();
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = "Histogram";
            myPane.XAxis.Title.Text = "Gray Value";
            myPane.YAxis.Title.Text = "pixels";

            int ncounts, i,x,y;
            int[] hist = new int[256];
            PointPairList imgData = new PointPairList();

            /*
            chart1.Series.Clear();
            chart1.Series.Add("ImgData");
            chart1.Series["ImgData"].Points.Clear();
            chart1.Series["ImgData"].Color = Color.Gray;
            */ 
             for (i = 0; i < 256; i++)
            {
                ncounts = 0;
                for (x = 0; x < img.Height; x++)
                {
                    for (y = 0; y < img.Width; y++)
                    {
                        if (i == img.Data[x, y, 0])
                        {
                            ncounts += 1;
                        }
                    }
                }
                hist[i] = ncounts;
                imgData.Add(i, hist[i]);

              //  chart1.Series["ImgData"].Points.AddXY(i, hist[i]);
            }
           
              BarItem histo=  myPane.AddBar("Imgdata",imgData,Color.Gray) ;

             zedGraphControl1.AxisChange();
             zedGraphControl1.Invalidate();
             zedGraphControl1.Refresh();
             myPane.CurveList.Remove(histo);   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                trackBar1.Enabled = true;

            }
            else { trackBar1.Enabled = false; }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

       
    }
}
