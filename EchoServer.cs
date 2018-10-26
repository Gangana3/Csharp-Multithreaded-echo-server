using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    /// <summary>
    /// Represents an echo server
    /// </summary>
    public class EchoServer : IDisposable
    {
        private IPEndPoint address;     // Address of the server
        private Socket serverSocket;          // Server socket
        private int listeners;
        private List<Socket> openClientSockets = new List<Socket>();
        private List<Thread> activeThreads = new List<Thread>();    // All active threads

        public EchoServer(IPEndPoint address, int listeners = 10)
        {
            this.address = address;
            this.serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            this.listeners = listeners;
        }

        /// <summary>
        /// Starts the echo server (Without blocking the executing thread).
        /// </summary>
        public void Start()
        {
            this.serverSocket.Bind(this.address);
            this.serverSocket.Listen(this.listeners);

            var acceptThread = new Thread(new ThreadStart(AcceptClients));
            this.activeThreads.Add(acceptThread);
            acceptThread.Start();    // start accepting clients
        }

        /// <summary>
        /// This method accepts clients and creates threads that connects and responses them
        /// </summary>
        private void AcceptClients()
        {
            while (true)
            {
                Socket clientSock = this.serverSocket.Accept();
                this.openClientSockets.Add(clientSock);
                var connThread = new Thread(() => EchoServer.ReceiveAndResponse(clientSock));
                this.activeThreads.Add(connThread);
                connThread.Start();
            }
        }

        /// <summary>
        /// Disposes the server
        /// </summary>
        public void Dispose()
        {
            foreach (var thread in activeThreads)
                if (thread.IsAlive)
                    thread.Abort();
            this.serverSocket.Close();
            foreach (Socket socket in openClientSockets)
                socket.Close();
        }

        /// <summary>
        /// Closes the server
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// Receives data and responses it.
        /// </summary>
        /// <param name="clientSocket">Connection with the client</param>
        private static void ReceiveAndResponse(Socket clientSocket)
        {
            while (true)
            {
                // Receive data from the client
                byte[] buffer = new byte[1024];
                int bytesReceived = clientSocket.Receive(buffer);
                string data = Encoding.ASCII.GetString(buffer, 0, bytesReceived);

                // Response to the request
                clientSocket.Send(Encoding.ASCII.GetBytes(data));
            }
        }


    }
}