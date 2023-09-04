// NGA 04/09/2023 : appel de la fonction de detection     
using Alturos.Yolo;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data; 
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    namespace yoloV1
    {
        public partial class Form1 : Form
        {

        YoloPreTrain yoloPreTrain;
       

        public Form1()
            {
                InitializeComponent();

            yoloPreTrain = new YoloPreTrain();

            }

            private void btnOpen_Click(object sender, EventArgs e)
            {
                using(OpenFileDialog ofd = new OpenFileDialog() { Filter = "All Files|*.*|PNG|*.png|JPEG|*.jpeg|JPG|*.jpg" })
                {
                    if(ofd.ShowDialog() == DialogResult.OK)
                    {
                        pic.Image = Image.FromFile(ofd.FileName);
                    }
                }
            }

            private void btnDetect_Click(object sender, EventArgs e)
            {

            // NGA 04/09/2023 : appel de la fonction de detection 

            yoloPreTrain.DetectAndDisplayObjects(txtClassName.Text.ToString());
          
            }

       






      
    }
    }
