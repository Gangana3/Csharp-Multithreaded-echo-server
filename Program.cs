using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Server
{
    class Program
    {

        static void Main(string[] args)
        {
            using (var server = new EchoServer(new IPEndPoint(IPAddress.Loopback, 8000)))
            {
                server.Start();
                System.Threading.Thread.Sleep(1000 * 800);
            }
        }
    }
}
