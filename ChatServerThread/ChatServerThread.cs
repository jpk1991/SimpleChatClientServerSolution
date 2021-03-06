﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace ChatServerThread
{
    /// <summary>
    /// Many Time Server, One Client (Client is as before)
    /// Der Server arbeitet weiter, nachdem sich ein verbundener Client abgemeldet hat.
    /// Verbindung nur von einem Client gleichzeitig möglich.
    /// </summary>
    class ChatServerThread
    {
        private Socket clientSocket = null;
        private TcpListener server = null;
        private Thread thread = null;
        private StreamReader reader = null;

        public ChatServerThread(int port)
        {
            try
            {
                Console.WriteLine($"Binding to port {port}, please wait ...");
                server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                Console.WriteLine($"Server started: {server}");
                Start();
            } catch(IOException ioe)
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
                    clientSocket = server.AcceptSocket();
                    Console.WriteLine($"Client accepted: {clientSocket}");
                    Open();
                    Boolean done = false;
                    while (!done)
                    {
                        try
                        {
                            String line = reader.ReadLine();
                            Console.WriteLine(line);
                            done = line.Equals(".bye");
                        }
                        catch (IOException ioe)
                        {
                            done = true;
                        }
                    }
                    Close();
                }
                catch (IOException ioe)
                {
                    Console.WriteLine(ioe);
                }
            }
        }

        public void Open()
        {
            reader = new StreamReader(
                new NetworkStream(clientSocket));
        }

        public void Close()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
            if (reader != null)
            {
                reader.Close();
            }
        }

        public void Start()
        {
            if(thread == null)
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
            ChatServerThread server = null;
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ChatServerThread.exe port");
            } else
            {
                server = new ChatServerThread(int.Parse(args[0]));
            }
        }
    }
}
