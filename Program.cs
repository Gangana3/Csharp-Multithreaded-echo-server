using System.Net;


namespace Server
{
    class Program
    {

        static void Main(string[] args)
        {

            //Usage
            IPEndPoint address = new IPEndPoint(IPAddress.Loopback, 8000);  // Server's Address
            using (EchoServer server = new EchoServer(address))             // Create an instance of EchoServer
            {
                server.Start();                                             // Start the echo server
                System.Threading.Thread.Sleep(1000 * 800);                  // Make the server active for 800 seconds
            }
        }
    }
}
