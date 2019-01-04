using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rdpserv
{
    class Program
    {
        static void Main(string[] args)
        {
            Server rdpServer = new Server();
            rdpServer.run();
            //rdpServer.SetSession();
            //rdpServer.InviteClient();
        }
    }
}
