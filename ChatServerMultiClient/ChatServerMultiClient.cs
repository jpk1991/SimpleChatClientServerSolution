using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace ChatServerMultiClient
{
    class ChatServerMultiClient
    {
        private Socket clientSocket = null;
        private TcpListener server = null;
        private Thread thread = null;
        private StreamReader reader = null;
        private ChatServerThread client = null;

        public ChatServerMultiClient(int port)
        {
            try
            {
                Console.WriteLine($"Binding to port {port}, please wait ...");
                server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                Console.WriteLine($"Server started: {server}");
                Start();
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe);
            }
        }

        public void Run()
        {
            while (thread != null)
            {
                try
                {
                    Console.WriteLine("Waiting for a client ...");
                    server.Start();
                    AddThread(server.AcceptSocket());
                   // Console.WriteLine($"Client accepted: {clientSocket}");
                   // Open();
                   // Boolean done = false;
                   //while (!done)
                   //{
                   //    try
                   //    {
                   //        String line = reader.ReadLine();
                   //        Console.WriteLine(line);
                   //        done = line.Equals(".bye");
                   //    }
                   //    catch (IOException ioe)
                   //    {
                   //        done = true;
                   //    }
                   //}
                   //Close();
                }
                catch (IOException ioe)
                {
                    Console.WriteLine($"Accceptance Error: {ioe}");
                }
            }
        }

        public void AddThread(Socket socket)
        {
            Console.WriteLine($"Client accepted: {socket}");
            client = new ChatServerThread(this, socket);
            try
            {
                client.Open();
                client.Start();
            } catch(IOException ioe)
            {
                Console.WriteLine($"Error opening thread: {ioe}");
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


        public void Stop()
        {
            if (thread != null)
            {
                thread.Abort();

            }
        }
        static void Main(string[] args)
        {
            ChatServerMultiClient server = null;
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ChatServerMultiClient.exe port");
            }
            else
            {
                server = new ChatServerMultiClient(int.Parse(args[0]));
            }
        }
    }
}
