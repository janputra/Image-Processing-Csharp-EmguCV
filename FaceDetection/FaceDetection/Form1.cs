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

namespace FaceDetection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Image<Bgr, byte> src = new Image<Bgr, byte>(480, 360);
        Image<Gray, byte> gray = new Image<Gray, byte>(480, 360);     
        string sourcefile;

        private void Form1_Load(object sender, EventArgs e)
        {

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


               src = CvInvoke.Imread(sourcefile, Emgu.CV.CvEnum.LoadImageType.AnyColor).ToImage<Bgr, byte>();
                src = src.Resize(480, 360, Emgu.CV.CvEnum.Inter.Cubic);
                imageBox1.Image = src;


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
         //   src = CvInvoke.Imread(sourcefile, Emgu.CV.CvEnum.LoadImageType.AnyColor).ToImage<Bgr, byte>();
          //  src = src.Resize(640, 480, Emgu.CV.CvEnum.Inter.Cubic);

            gray = src.Convert<Gray, byte>();

            CascadeClassifier face = new CascadeClassifier("haarcascade_frontalface_default.xml");

            Rectangle[] facesDetected = face.DetectMultiScale(
                    gray,
                    1.1,
                    10,
                    new Size(20, 20));

            foreach (Rectangle f in facesDetected) 
            {
                src.Draw(f, new Bgr(Color.Red), 1);

            }


            imageBox1.Image = src;
        }
    }
}
