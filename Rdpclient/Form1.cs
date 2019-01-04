using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Rdpclient
{
    public partial class Form1 : Form
    {
        private Thread screenThread;
        ScreenCapture screen = new ScreenCapture();
        private static int prevSize = 0;

        IPEndPoint ipserv;
        Socket srvSocket;

        double ratioX;
        double ratioY;
        double ratio;

        public delegate void deleg(String data);

        public Form1()
        {
            InitializeComponent();
        }

        private void connectMenuItem_Click(object sender, EventArgs e)
        {
            //ipserv = new IPEndPoint(IPAddress.Parse("10.200.15.237"), 17820);           
            //ipserv = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 17820);
            ipserv = new IPEndPoint(IPAddress.Parse("10.1.23.230"), 17820);
            srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            srvSocket.Connect(ipserv);

            screenThread = new Thread(new ThreadStart(GetScreen));
            screenThread.Start();

            imgBox.MouseMove += new MouseEventHandler(this.imgBox_MouseMove);
            /*
            axRDPViewer.OnConnectionEstablished += axRDPViewer_OnConnectionEstablished;
            String rpdCon = File.ReadAllText(@"D:\rdpcon.txt");
            MessageBox.Show(rpdCon);
            axRDPViewer.Connect(rpdCon, "Test", "Parola");
            */
        }

        private void imgBox_MouseMove(object sender, EventArgs e)
        {
            Point point = imgBox.PointToClient(Cursor.Position);
            SendMouse(point);
            this.BeginInvoke(new deleg(Stats), Convert.ToString(point.X) + " - " + Convert.ToString(point.Y));
        }

        private void SendMouse(Point points)
        {
            List<Byte> pointList = new List<Byte>();
            Byte[] coordsX = new Byte[2];
            Byte[] coordsY = new Byte[2];
            coordsX = BitConverter.GetBytes(Convert.ToInt16(Math.Round(points.X * ratioX, MidpointRounding.AwayFromZero)));
            coordsY = BitConverter.GetBytes(Convert.ToInt16(Math.Round(points.Y * ratioY, MidpointRounding.AwayFromZero)));
            pointList.Add(coordsX[0]);
            pointList.Add(coordsX[1]);
            pointList.Add(coordsY[0]);
            pointList.Add(coordsY[1]);
                        
            srvSocket.Send(pointList.ToArray());            
        }

        private void GetScreen()
        {
            Byte[] data = new Byte[1024];
                        
            //NetworkStream nets = new NetworkStream(srvSocket);
            //BinaryReader binReader = new BinaryReader(nets);
            while (true)     
            {                
                //int sizeBytes = binReader.ReadInt32();
                //data = binReader.ReadBytes(sizeBytes);
                                
                data = ReceiveDataV2(srvSocket);
                MemoryStream mem = new MemoryStream(data);
                Bitmap bmp = new Bitmap(Image.FromStream(mem));
                ratioX = Convert.ToDouble(bmp.Size.Width) / Convert.ToDouble(imgBox.Width);
                ratioY = Convert.ToDouble(bmp.Size.Height) / Convert.ToDouble(imgBox.Height);
                //ratio = Convert.ToDouble(bmp.Size.Width) / Convert.ToDouble(bmp.Size.Height);
                Size sizing = new Size(imgBox.Width, imgBox.Height);
                bmp = new System.Drawing.Bitmap(bmp, sizing);

                imgBox.Image = bmp;
                Application.DoEvents();
                //this.BeginInvoke(new deleg(Stats), Convert.ToString(data.Length));
            }
        }

        private Byte[] ReceiveDataV2(Socket s)
        {
            //NetworkStream nets = new NetworkStream(s);

            int remaining = 0;
            int received = 0;
            Byte[] dataSize = new Byte[4];

            received = s.Receive(dataSize, 0, 4, SocketFlags.None);
            int size = BitConverter.ToInt32(dataSize, 0);
            int dataleft = size;

            Byte[] data = new Byte[size];
            //nets.Read(data, 0, size);
            
            while (remaining < size)
            {
                received = s.Receive(data, remaining, dataleft, SocketFlags.None);
                if (received == 0)
                {
                    break;
                }
                remaining += received;
                dataleft -= received;
            }
            

            return data;
        }

        private void ConnectV2()
        {
            IPEndPoint ipserv = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 17820);
            Socket srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            srvSocket.Connect(ipserv);

            NetworkStream nets = new NetworkStream(srvSocket);

            //Byte[] dataSize = new Byte[4];
            int size = -1;
            int lastGood = 0;
            

            while (true)
            {
                lastGood = size;
                Byte[] dataSize = new Byte[4];
                srvSocket.Receive(dataSize, 0, 4, SocketFlags.None);
                //nets.Read(dataSize, 0, 4);
                while (size < 0)
                {
                    size = BitConverter.ToInt32(dataSize, 0);
                }
                //size = BitConverter.ToInt32(dataSize, 0);
                /*
                size = dataSize[0];
                size += 256 * dataSize[1];
                size += 65536 * dataSize[2];
                size += 16777216 * dataSize[3];
                */
                //BinaryFormatter bifo = new BinaryFormatter();                
                
                //Byte[] data = new Byte[size];
                Byte[] data = new Byte[1024];
                //nets.Read(data, 0, size);
                File.AppendAllText(@"D:\sizes.txt", Convert.ToString(size) + "\r\n");                
                //MemoryStream mem = new MemoryStream(data, 0, data.Length);
                //Bitmap bmp = new Bitmap(mem);
                
                //mem.Write(data, 0, data.Length);
                //mem.Seek(0, SeekOrigin.Begin);
                //bifo.Serialize(mem, data);
                //ImageConverter imco = new ImageConverter();
                //imgBox.Image = (Image)imco.ConvertFrom(bifo.Deserialize(mem));

                int remaining = 0;
                int received = 0;
                int dataleft = size;

                while (remaining < size)
                {
                    received = srvSocket.Receive(data, remaining, dataleft, SocketFlags.None);
                    if (received == 0)
                    {
                        break;
                    }
                    remaining += received;
                    dataleft -= received;
                }

                MemoryStream mem = new MemoryStream(data);
                try
                {
                    imgBox.Image = Image.FromStream(mem);
                }
                catch (Exception ex)
                {
                }
                //imgBox.Image = bmp;
                //mem.Flush();                
                this.BeginInvoke(new deleg(Stats), "Proceeding");
            }

        }
        
        private void Connect()
        {
            try
            {
                //Byte[] data = new Byte[1024];

                //IPEndPoint ipserv = new IPEndPoint(IPAddress.Parse("10.5.2.14"), 17820);
                IPEndPoint ipserv = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 17820);
                Socket srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                srvSocket.Connect(ipserv);
                //MemoryStream mem = new MemoryStream();
                while (true)
                {
                    //imgBox.Image = ReceiveData(srvSocket);
                    Byte[] data = new Byte[1024];
                    data = ReceiveData(srvSocket);
                    
                    if (data.Length > 0)
                    {
                        try
                        {
                            MemoryStream mem = new MemoryStream(data);
                            
                            //mem.Write(data, 0, data.Length);
                            //mem.Seek(0, SeekOrigin.Begin);
                            //Bitmap bmp = new Bitmap(Image.FromStream(mem));
                            //imgBox.Image = bmp;

                            this.BeginInvoke(new deleg(Stats), Convert.ToString(mem.Length));
                            if (mem.Length > 0)
                            {
                                mem.Seek(0, SeekOrigin.Begin);
                                //this.BeginInvoke(new deleg(Stats), "Almost " + DateTime.Now.Ticks);
                                imgBox.Image = Image.FromStream(mem, false, false);
                            }
                            mem.Flush();
                            //bmp.Save(@"D:\" + DateTime.Now.Ticks + ".bmp");
                            //MessageBox.Show("la noli");
                        }
                        catch (Exception ex)
                        {
                            this.BeginInvoke(new deleg(Stats), "hata var " + DateTime.Now.Ticks);
                        }
                    }
                    Application.DoEvents();
                }                
                //MessageBox.Show("wtf");
                //srvSocket.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(Convert.ToString(count));
                MessageBox.Show(ex.Message);
            }
        }

        private void Stats(String statsData)
        {
            //lblStats.Text += logData + " | " + string.Format("{0:dd-MM-yy HH:mm:ss}", DateTime.Now.ToString()) + "\r\n";
            lblStats.Text = statsData;
        }

        private Byte[] ReceiveData(Socket s)
        {
            NetworkStream nets = new NetworkStream(s);
            BinaryFormatter formatter = new BinaryFormatter();
            
            /*
            int remaining = 0;
            int received = 0;
            Byte[] dataSize = new Byte[4];

            received = s.Receive(dataSize, 0, 4, SocketFlags.None);
            int size = BitConverter.ToInt32(dataSize, 0);
            int dataleft = size;
            Byte[] data = new Byte[size];
            */

            Byte[] junk = new Byte[0];
            Byte[] dataSize_ = new Byte[4];
            nets.Read(dataSize_, 0, 4);
            int size_ = BitConverter.ToInt32(dataSize_, 0);
            
            if ((size_ > 0) && size_ < Int32.MaxValue)
            {
                if (prevSize == 0) prevSize = size_;
                int abs = Math.Abs(prevSize - size_);
                int diff = (abs / prevSize) * 100;

                if (diff < 20)
                {
                    Byte[] data_ = new Byte[size_];
                    
                    nets.Read(data_, 0, size_);
                    
                    /*
                    while (remaining < size)
                    {
                        received = s.Receive(data, remaining, dataleft, SocketFlags.None);
                        if (received == 0)
                        {
                            break;
                        }
                        remaining += received;
                        dataleft -= received;
                    }
                    */

                    prevSize = size_;
                    nets.Flush();
                    return data_;
                }
                else
                {
                    nets.Flush();
                    return junk;
                }                            
            }
            else
            {
                nets.Flush();
                return junk;
            }            
        }

        private void disconnectMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Size screenSize = screen.GetDesktopBitmapSize();
            this.Height = screenSize.Height;
            this.Width = screenSize.Width;
            this.Top = 0;
            this.Left = 0;
            this.WindowState = FormWindowState.Maximized;

            grupScreen.Height = this.Height - 70;
            grupScreen.Width = this.Width - 25;
            grupScreen.Top = 30;
            grupScreen.Left = 5;

            imgBox.Height = grupScreen.Height - 60;
            imgBox.Width = grupScreen.Width - 60;
            imgBox.Top = 30;
            imgBox.Left = 30;
        }      
    }

    public class ScreenCapture : System.MarshalByRefObject
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

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



        public Size GetDesktopBitmapSize()
        {
            return new Size(GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN));
        }
    }
}
