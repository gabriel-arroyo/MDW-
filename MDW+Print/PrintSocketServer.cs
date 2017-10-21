using HTKLibrary.Classes.MDW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MDW_Print
{
    public class PrintSocketServer
    {
        private byte[] _buffer = new byte[1024];
        private List<System.Net.Sockets.Socket> _clientSockets = new List<System.Net.Sockets.Socket>();
        private System.Net.Sockets.Socket _serverSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private int _port = 100;

        public delegate void SocketPrintEventHandler(PrintTag tag);
        public event SocketPrintEventHandler PrintFromSocket;
        protected virtual void OnPrintFromSocket(PrintTag tag)
        {
            if (PrintFromSocket != null)
                PrintFromSocket(tag);
        }

        public PrintSocketServer(int port)
        {

            _port = port;
        }
        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

        private IPAddress get_IP()
        {
            IPAddress ip = null;
            foreach (IPAddress a in localIPs)
            {
                if (a.AddressFamily == AddressFamily.InterNetwork)
                    ip = a;
            }


            return ip;
        }
        public void Start()
        {
            var ip = get_IP();
            Console.WriteLine("Setting up server...");
            _serverSocket.Bind(new IPEndPoint(ip, _port));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void AcceptCallback(IAsyncResult AR)
        {
            System.Net.Sockets.Socket socket = _serverSocket.EndAccept(AR);
            _clientSockets.Add(socket);
            Console.WriteLine("Client connected");
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            System.Net.Sockets.Socket socket = (System.Net.Sockets.Socket)AR.AsyncState;
            try
            {
                int reveived = socket.EndReceive(AR);
                byte[] dataBuf = new byte[reveived];
                Array.Copy(_buffer, dataBuf, reveived);

                string text = Encoding.ASCII.GetString(dataBuf);
                if (text == "") return;
                
                ParameterizedThreadStart start = new ParameterizedThreadStart(PrintTag);
                Thread threadGpo = new Thread(start);
                threadGpo.Start(text);



                string response = "200";
                byte[] data = Encoding.ASCII.GetBytes(response);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch (Exception)
            {
                Console.WriteLine("Connection lost");
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
        }

        private void PrintTag(object obj)
        {
            string stag = (string)obj;
            PrintTag tag = XMLToTag(stag);
            OnPrintFromSocket(tag);
        }

        private void SendCallback(IAsyncResult AR)
        {
            System.Net.Sockets.Socket socket = (System.Net.Sockets.Socket)AR.AsyncState;
            socket.EndSend(AR);
        }
        public PrintTag XMLToTag(string xmlText)
        {
            var stringReader = new System.IO.StringReader(xmlText);
            var serializer = new XmlSerializer(typeof(SocketTag));
            SocketTag tag = (SocketTag)serializer.Deserialize(stringReader);
            PrintTag pTag = new HTKLibrary.Classes.MDW.PrintTag();
            pTag.epc = tag.epc;
            for(int i = 0; i < tag.fields.Length; i++)
            {
                pTag.fields.Add(tag.fields[i], tag.values[i]);
            }
            return pTag;
        }
        public string TagToXML(PrintTag tag)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(PrintTag));
            serializer.Serialize(stringwriter, tag);
            return stringwriter.ToString();
        }

    }
    public class SocketTag
    {
        public string epc { get; set; }
        public string[] fields { get; set; }
        public string[] values { get; set; }

        public SocketTag()
        {

        }
    }
}
