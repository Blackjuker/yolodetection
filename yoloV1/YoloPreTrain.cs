// NGA 04/09/2023 : Fonction qui permet de retrouver un objet à partir du nom et renvoyer le centre de cette objet
// NGA 04/09/2023 : Fonction qui permet de capturer l'écran  et l'envoyer à la fonction yolo
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
    class YoloPreTrain
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

        // NGA 04/09/2023 :  Recherche target sur la capture utilisant YOLO pre-train

        public void DetectAndDisplayObjects(string targetClass)
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


        // NGA 04/09/2023 : Capture Ecran

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
    }
}
