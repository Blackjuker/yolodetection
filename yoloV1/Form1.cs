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
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public Form1()
            {
                InitializeComponent();
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
            DetectAndDisplayObjects(txtClassName.Text.ToString());
            //DetectAndDisplayObjects("cat");
            }

        //    private void DetectAndDisplayObjects(string targetClass)
        //{
        //    var configurationDetector = new YoloConfigurationDetector();
        //    var config = configurationDetector.Detect(); 
        //    using(var yoloWrapper = new YoloWrapper(config))
        //    {
        //        using(MemoryStream ms = new MemoryStream())
        //        {
        //            pic.Image.Save(ms, ImageFormat.Png);
        //            var items = yoloWrapper.Detect(ms.ToArray());

        //            if (checkBox1.Checked)
        //            {
        //                yoloItemBindingSource.DataSource = items;
        //            }
        //            else
        //            {

        //                //Filtrage des éléments découvert pour afficher uniquement l'élément rechercher
        //                var targetItems = items.Where(item => item.Type == targetClass).ToList();

        //                yoloItemBindingSource.DataSource = targetItems;
        //            }
        //        }
        //    }
        //}

        // detect le centre de l'image trouver 
        //private void DetectAndDisplayObjects(string targetClass)
        //{
        //    var configurationDetector = new YoloConfigurationDetector();
        //    var config = configurationDetector.Detect();
        //    using (var yoloWrapper = new YoloWrapper(config))
        //    {
        //        // Capture a screenshot of the entire desktop
        //        Bitmap screenshot = CaptureScreen();

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            screenshot.Save(ms, ImageFormat.Png);
        //            var items = yoloWrapper.Detect(ms.ToArray());

        //            // Filter the detected items to only include the specified class
        //            var targetItems = items.Where(item => item.Type == targetClass).ToList();

        //            if (targetItems.Count > 0)
        //            {
        //                // Calculate the position of the center of the first detected object
        //                var firstObject = targetItems[0];
        //                var centerX = (firstObject.X + firstObject.Width) / 2;
        //                var centerY = (firstObject.Y + firstObject.Height) / 2;

        //                // Get the dimensions of the desktop
        //                RECT desktopRect;
        //                GetWindowRect(IntPtr.Zero, out desktopRect);
        //                var desktopWidth = desktopRect.Right - desktopRect.Left;
        //                var desktopHeight = desktopRect.Bottom - desktopRect.Top;

        //                // Calculate the position of the center in dimensions of the desktop
        //                var centerXInDesktop = desktopRect.Left + centerX;
        //                var centerYInDesktop = desktopRect.Top + centerY;

        //                // Calculate the position of the center in dimensions of the screen
        //                var screen = Screen.FromPoint(new Point(centerXInDesktop, centerYInDesktop));
        //                var centerXInScreen = centerXInDesktop - screen.Bounds.Left;
        //                var centerYInScreen = centerYInDesktop - screen.Bounds.Top;

        //                // Now you can use centerXInScreen and centerYInScreen for further processing
        //                MessageBox.Show(centerXInScreen.ToString() + " " + centerYInScreen.ToString());
        //            }

        //            yoloItemBindingSource.DataSource = targetItems;
        //        }
        //    }
        //}

        //private void DetectAndDisplayObjects(string targetClass)
        //{
        //    var configurationDetector = new YoloConfigurationDetector();
        //    var config = configurationDetector.Detect();
        //    using (var yoloWrapper = new YoloWrapper(config))
        //    {
        //        // Capture a screenshot of the entire desktop
        //        Bitmap screenshot = CaptureScreen();

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            screenshot.Save(ms, ImageFormat.Png);
        //            var items = yoloWrapper.Detect(ms.ToArray());

        //            // Filter the detected items to only include the specified class
        //            var targetItems = items.Where(item => item.Type == targetClass).ToList();

        //            if (targetItems.Count > 0)
        //            {
        //                // Get the first detected object
        //                var firstObject = targetItems[0];

        //                // Calculate the position of the center of the detected object
        //                var centerX = firstObject.X + firstObject.Width / 2;
        //                var centerY = firstObject.Y + firstObject.Height / 2;

        //                // Draw the center point on the screenshot
        //                using (Graphics g = Graphics.FromImage(screenshot))
        //                {
        //                    g.FillEllipse(Brushes.Red, centerX - 2, centerY - 2, 5, 5);
        //                }

        //                // Display the modified screenshot in the picScreenshot PictureBox
        //                pic.Image = screenshot;
        //            }
        //            yoloItemBindingSource.DataSource = targetItems;
        //        }
        //    }
        //}
        private void DetectAndDisplayObjects(string targetClass)
        {
            var configurationDetector = new YoloConfigurationDetector();
            var config = configurationDetector.Detect();
            using (var yoloWrapper = new YoloWrapper(config))
            {
                // Capture a screenshot of the entire desktop
                Bitmap screenshot = CaptureScreen();

                using (MemoryStream ms = new MemoryStream())
                {
                    screenshot.Save(ms, ImageFormat.Png);
                    var items = yoloWrapper.Detect(ms.ToArray());

                    // Filter the detected items to only include the specified class
                    var targetItems = items.Where(item => item.Type == targetClass).ToList();

                    if (targetItems.Count > 0)
                    {
                        // Get the first detected object
                        var firstObject = targetItems[0];

                        // Calculate the position of the center of the detected object
                        var centerX = firstObject.X + firstObject.Width / 2;
                        var centerY = firstObject.Y + firstObject.Height / 2;

                        // Draw the center point on the screenshot
                        using (Graphics g = Graphics.FromImage(screenshot))
                        {
                            // Draw a larger red circle around the center point
                            int circleRadius = 10;
                            int circleDiameter = circleRadius * 2;
                            g.DrawEllipse(Pens.Red, centerX - circleRadius, centerY - circleRadius, circleDiameter, circleDiameter);

                            // Draw a smaller filled red circle at the center point
                            int pointSize = 5;
                            g.FillEllipse(Brushes.Red, centerX - pointSize, centerY - pointSize, pointSize * 2, pointSize * 2);
                        }

                        // Display the modified screenshot in the picScreenshot PictureBox
                        pic.Image = screenshot;
                    }

                    yoloItemBindingSource.DataSource = targetItems;
                }
            }
        }





        private Bitmap CaptureScreen()
        {
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(screenBounds.Width, screenBounds.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(screenBounds.Left, screenBounds.Top, 0, 0, screenBounds.Size, CopyPixelOperation.SourceCopy);
            }

            return screenshot;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
        }
    }
    }
