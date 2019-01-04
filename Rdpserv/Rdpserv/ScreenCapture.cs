using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

namespace Rdpserv
{
    public class ScreenCapture : System.MarshalByRefObject
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern int SetCursorPos(int x, int y);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(
            IntPtr hdcDest, // handle to destination DC
            int nXDest, // x-coord of destination upper-left corner
            int nYDest, // y-coord of destination upper-left corner
            int nWidth, // width of destination rectangle
            int nHeight, // height of destination rectangle
            IntPtr hdcSrc, // handle to source DC
            int nXSrc, // x-coordinate of source upper-left corner
            int nYSrc, // y-coordinate of source upper-left corner
            System.Int32 dwRop // raster operation code
            );
        private const Int32 SRCCOPY = 0xCC0020;
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;

        public void SetMousePosition(int ptx, int pty)
        {
            SetCursorPos(ptx, pty);
        }

        public Size GetDesktopBitmapSize()
        {
            return new Size(GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN));
        }
        public byte[] GetDesktopBitmapBytes()
        {
            Size DesktopBitmapSize = GetDesktopBitmapSize();
            Graphics Graphic = Graphics.FromHwnd(GetDesktopWindow());
            Bitmap MemImage = new Bitmap(DesktopBitmapSize.Width, DesktopBitmapSize.Height, Graphic);

            Graphics MemGraphic = Graphics.FromImage(MemImage);
            IntPtr dc1 = Graphic.GetHdc();
            IntPtr dc2 = MemGraphic.GetHdc();
            BitBlt(dc2, 0, 0, DesktopBitmapSize.Width, DesktopBitmapSize.Height, dc1, 0, 0, SRCCOPY);
            Graphic.ReleaseHdc(dc1);
            MemGraphic.ReleaseHdc(dc2);
            Graphic.Dispose();
            MemGraphic.Dispose();

            Graphics g = System.Drawing.Graphics.FromImage(MemImage);
            System.Windows.Forms.Cursor cur = System.Windows.Forms.Cursors.Arrow;
            //cur.Draw(g, new Rectangle(System.Windows.Forms.Cursor.Position.X - 10, System.Windows.Forms.Cursor.Position.Y - 10, cur.Size.Width, cur.Size.Height));
            cur.Draw(g, new Rectangle(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y, cur.Size.Width, cur.Size.Height));

            MemoryStream ms = new MemoryStream();
            MemImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.GetBuffer();
        }


        public Image Get_Resized_Image(int w, int h, byte[] image)
        {
            MemoryStream ms = new MemoryStream(image);

            Image bt = Image.FromStream(ms);
            try
            {
                Size sizing = new Size(w, h);
                bt = new System.Drawing.Bitmap(bt, sizing);

            }
            catch (Exception) { }
            return bt;

        }

        public float difference(Image OrginalImage, Image SecoundImage)
        {
            float percent = 0;
            try
            {
                float counter = 0;

                Bitmap bt1 = new Bitmap(OrginalImage);
                Bitmap bt2 = new Bitmap(SecoundImage);
                int size_H = bt1.Size.Height;
                int size_W = bt1.Size.Width;

                float total = size_H * size_W;

                Color pixel_image1;
                Color pixel_image2;

                for (int x = 0; x != size_W; x++)
                {

                    for (int y = 0; y != size_H; y++)
                    {
                        pixel_image1 = bt1.GetPixel(x, y);
                        pixel_image2 = bt2.GetPixel(x, y);

                        if (pixel_image1 != pixel_image2)
                        {
                            counter++;
                        }

                    }

                }
                percent = (counter / total) * 100;

            }
            catch (Exception) { percent = 0; }

            return percent;
        }
    }
}
