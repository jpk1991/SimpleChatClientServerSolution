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
/// The Simple Client corresponding to the previous server (and to step 2 and step 3 servers as well)
/// </summary>
namespace SimpleChatClientServerSolution
{
    /// <summary>
    /// Portierung von Simple Chat Client zu C#
    /// </summary>
    class ChatClient
    {
        private Socket socket = null;
        private TextReader console = null;
        private StreamWriter streamOut = null;

        /// <summary>
        /// Konstruktor vom Chatclient
        /// </summary>
        /// <param name="serverIP">IP des Servers</param>
        /// <param name="serverPort">Serverport zum Verbinden</param>
        public ChatClient(IPAddress serverIP, int serverPort)
        {
            Console.WriteLine("Establishing connection. Please wait ...");
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(serverIP, serverPort);
                if (socket.Connected)
                {
                    Console.WriteLine($"Connected: {socket}");
                } else
                {
                    Console.WriteLine("Connection error");
                }
                Start();
            }catch (SocketException se) {
                Console.WriteLine($"Socket exception: {se.Message}");
            } catch (IOException ioe)
            {
                Console.WriteLine($"Unknown exception: {ioe.Message}");
            }
            String line = "";
            while (!line.Equals(".bye"))
            {
                try
                {
                    Console.WriteLine("Please insert text: ");
                    line = console.ReadLine();
                    streamOut.WriteLine(line);
                    streamOut.Flush();
                } catch (IOException ioe)
                {
                    Console.WriteLine($"Sending error: {ioe.Message}");
                }
            }
        }

        /// <summary>
        /// Initialisierung des Textwriters und deStreamwriters
        /// </summary>
        public void Start()
        {
            console = Console.In;
            streamOut = new StreamWriter(new NetworkStream(socket));
        }

        /// <summary>
        /// Schließen der Verbindungen
        /// </summary>
        public void Stop()
        {
            try
            {
                if (console != null)
                {
                    console.Close();
                }
                if (streamOut != null)
                {
                    streamOut.Close();
                }
                if (socket != null)
                {
                    socket.Close();
                }
            } catch (IOException ioe)
            {
                Console.WriteLine($"Error closing: {ioe.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(String[] args)
        {
            ChatClient client = null;
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: ChatClient.exe hostIP port");
            } else
            {
                client = new ChatClient(IPAddress.Parse(args[0]), int.Parse(args[1]));
            }
        }
    }
}
