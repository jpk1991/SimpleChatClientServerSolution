using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

/// <summary>
/// Portierung von Creating a simple Chat Client/Server Solution von JAVA zu C#
/// http://pirate.shu.edu/~wachsmut/Teaching/CSAS2214/Virtual/Lectures/chat-client-server.html
/// </summary>
namespace SimpleChatClientServerSolution
{
    /// <summary>
    /// Step 1: Simple, one-time Server
    /// Schaltet ab, nachdem sich ein verbundener Client abgemeldet hat
    /// </summary>
    public class ChatServer
    {
        private Socket socket = null;
        private TcpListener server = null;
        private StreamReader reader = null;

        public ChatServer(int port)
        {
            try
            {
                Console.WriteLine($"Binding to port {port}, please wait ...");
                server = new TcpListener(new IPAddress(0x0100007F), port);
                Console.WriteLine($"Server started: {server}");
                Console.WriteLine("Waiting for a client ...");
                server.Start();
                socket = server.AcceptSocket();
                Console.WriteLine($"Client accepted: {socket}");
                Open();
                Boolean done = false;
                while (!done)
                {
                    try
                    {
                        String line = reader.ReadLine();
                        Console.WriteLine(line);
                        done = line.Equals(".bye");
                    } catch(IOException ioe)
                    {
                        done = true;
                    }
                }
                Close();
            } catch(IOException ioe)
            {
                Console.WriteLine(ioe);
            }
        }

        public void Open()
        {
            reader = new StreamReader(
                new NetworkStream(socket));
        }

        public void Close()
        {
            if(socket != null)
            {
                socket.Close();
            }
            if(reader != null)
            {
                reader.Close();
            }
        }
        

        static void Main(string[] args)
        {
            ChatServer server = null;
            if(args.Length != 1)
            {
                Console.WriteLine("Usage: ChatServer.exe port");
            } else
            {
                server = new ChatServer(int.Parse(args[0]));
            }
        }
    }
}
