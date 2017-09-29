using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ChatServerMultiClient
{
    class ChatServerThread
    {
        private Socket socket = null;
        private ChatServerMultiClient server = null;
        private int ID = -1;
        private StreamReader reader = null;
        private Thread thread = null;

        public ChatServerThread(ChatServerMultiClient server, Socket socket)
        {
            this.socket = socket;
            this.server = server;
            ID = ((IPEndPoint)socket.LocalEndPoint).Port; //get socket port

        }

        public void Run()
        {
            Console.WriteLine($"Server Thread {ID} running.");
            while(true)
            {
                String line = reader.ReadLine();
                Console.WriteLine(line);
            }
        }

        public void Open()
        {
            reader = new StreamReader(
                new NetworkStream(socket));
        }

        public void Close()
        {
            if (socket != null)
            {
                socket.Close();
            }
            if (reader != null)
            {
                reader.Close();
            }
        }

        public void Start()
        {
            if (thread == null)
            {
                thread = new Thread(new ThreadStart(Run));
                thread.Start();
            }
        }
    }
}
