using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Rdpserv
{
    public class Server
    {
        private Thread commandThread;
        private Thread listenThread;
        private Thread screenThread;

        IPEndPoint ipEnd;
        Socket server;
        Socket client;

        ScreenCapture screen = new ScreenCapture();

        public void run()
        {
            runMe();
        }

        private void runMe()
        {
            listenThread = new Thread(new ThreadStart(Listen));
            listenThread.Start();
        }

        private void GetCommand()
        {
            while (true)
            {
                try
                {
                    Byte[] commands = new Byte[4];
                    client.Receive(commands, 0, 4, SocketFlags.None);

                    List<Byte> commandList = new List<Byte>(commands);
                    Byte[] coordsX = new Byte[4];
                    Byte[] coordsY = new Byte[4];
                    coordsX[0] = commandList[0];
                    coordsX[1] = commandList[1];
                    coordsY[0] = commandList[2];
                    coordsY[1] = commandList[3];
                    int pointX = BitConverter.ToInt32(coordsX, 0);
                    int pointY = BitConverter.ToInt32(coordsY, 0);
                    Console.WriteLine(pointX + " - " + pointY);
                    screen.SetMousePosition(pointX, pointY);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }

        private void Listen()
        {
            ipEnd = new IPEndPoint(IPAddress.Any, 17820);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            server.Bind(ipEnd);
            server.Listen(10);

            Console.WriteLine("Waiting for a client");

            while (true)
            {                
                client = server.Accept();
                IPEndPoint clientEnd = (IPEndPoint)client.RemoteEndPoint;
                Console.WriteLine("Connected with {0} at port {1}", clientEnd.Address, clientEnd.Port);

                screenThread = new Thread(new ThreadStart(SendScreen));
                screenThread.Start();
            }
        }

        private void SendScreen()
        {
            try
            {
                commandThread = new Thread(new ThreadStart(GetCommand));
                commandThread.Start();

                while (true)
                {
                    Image oldImage = screen.Get_Resized_Image(100, 100, screen.GetDesktopBitmapBytes());

                    //NetworkStream nets = new NetworkStream(client);
                    //BinaryWriter binWriter = new BinaryWriter(nets);
                    while (client.Connected)
                    {
                        Byte[] imageByte = screen.GetDesktopBitmapBytes();
                        Image newImage = screen.Get_Resized_Image(100, 100, imageByte);
                        float diff = screen.difference(newImage, oldImage);

                        //Console.WriteLine("Image difference : %" + diff.ToString());

                        if (diff >= 0.1f)
                        {
                            //Console.WriteLine("Sending image data...");                       

                            /*
                            Image imageSend = screen.Get_Resized_Image(800, 600, imageByte);
                            Bitmap bmp = new Bitmap(imageSend);
                            MemoryStream mem = new MemoryStream();
                            bmp.Save(mem, ImageFormat.Jpeg);
                            Byte[] bmpBytes = mem.GetBuffer();                        
                            */
                            //MemoryStream mem = new MemoryStream(imageByte);
                            //Image img = Image.FromStream(mem);

                            int remaining = 0;
                            int size = imageByte.Length;
                            //int size = bmpBytes.Length;
                            int total = size;
                            int sent;

                            Byte[] dataSize = new Byte[4];
                            dataSize = BitConverter.GetBytes(size);

                            //Console.WriteLine(client.Connected);
                            sent = client.Send(dataSize);
                            //NetworkStream nets = new NetworkStream(client);
                            //nets.Write(dataSize, 0, 4);
                            //Image imageSend = screen.Get_Resized_Image(screen.GetDesktopBitmapSize().Width, screen.GetDesktopBitmapSize().Height, imageByte);
                            //MemoryStream mem = new MemoryStream();
                            //imageSend.Save(mem, ImageFormat.Bmp);
                            //Byte[] buffer = new Byte[mem.Length];
                            //mem.Seek(0, SeekOrigin.Begin);
                            //mem.Read(buffer, 0, buffer.Length);

                            //binWriter.Write(buffer.Length);
                            //binWriter.Write(buffer);
                            //nets.Flush();
                            //nets.Write(imageByte, 0, imageByte.Length);
                            //img.Save(nets, ImageFormat.Bmp);

                            //imageSend.Save(nets, ImageFormat.Bmp);
                            //File.AppendAllText(@"D:\sizec.txt", Convert.ToString(size) + "\r\n");
                            int count = 0;
                            while (remaining < size)
                            {
                                sent = client.Send(imageByte, remaining, total, SocketFlags.None);
                                remaining += sent;
                                total -= sent;
                                //Console.WriteLine(sent + " -- " + count++);
                            }

                            //nets.Flush();
                            //nets.Close();                        
                        }
                        else
                        {
                            //Console.WriteLine("No image difference");
                        }
                        //nets.Flush();
                        oldImage = newImage;
                    }
                }
                Console.ReadLine();
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                runMe();
            }
        }
              

        /*
        private RDPSession rdpSession = new RDPSession();
        private Thread tred;
        private Boolean donelimGuzelleselim;

        private void OpenSession(Object client)
        {
            IRDPSRAPIAttendee Client = (IRDPSRAPIAttendee)client;
            Client.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_VIEW;
        }

        public void SetSession()
        {
            rdpSession.OnAttendeeConnected += OpenSession;
            rdpSession.OnAttendeeDisconnected += EndSession;
            rdpSession.OnControlLevelChangeRequest += ControlLevelChange;
            rdpSession.Open();
            donelimGuzelleselim = true;
            tred = new Thread(new ThreadStart(Loop));
            tred.Start();
        }

        public void InviteClient()
        {
            IRDPSRAPIInvitation Invitation;
            Invitation = rdpSession.Invitations.CreateInvitation("Test", "Grup", "parola", 1);            
            File.WriteAllText(@"C:\rdpcon.txt", Invitation.ConnectionString);
            Console.WriteLine("Davetiye hazirlandi");
        }

        private void EndSession(Object client)
        {
            rdpSession.Close();
            Marshal.ReleaseComObject(rdpSession);
            rdpSession = null;
            donelimGuzelleselim = false;
        }

        private void ControlLevelChange(Object client, CTRL_LEVEL RequestedLevel)
        {
            IRDPSRAPIAttendee Client = (IRDPSRAPIAttendee)client;
            Client.ControlLevel = RequestedLevel;
        }

        private void Loop()
        {
            while (donelimGuzelleselim)
            {
                //dön baba donelim
            }
        }
         */
    }
}
