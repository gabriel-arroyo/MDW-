using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MDW_Print.Connectivity
{
    public class ServerTags
    {
        private static byte[] _buffer = new byte[1024];
        private static  List<System.Net.Sockets.Socket> _clientSockets = new List<System.Net.Sockets.Socket>();
        private static System.Net.Sockets.Socket _serverSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static int _port = 501;

        public ServerTags(int port)
        {

            _port = port;
        }

        public void Start()
        {
            var ipAddres = get_IP();
            Console.WriteLine("Configurando servidor en " + ipAddres + "...");
            Logger.WriteLog("Configurando servidor de sockets en " + ipAddres + "...");
            _serverSocket.Bind(new IPEndPoint(ipAddres, _port));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
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
        private void AcceptCallback(IAsyncResult AR)
        {
            System.Net.Sockets.Socket socket = _serverSocket.EndAccept(AR);
            _clientSockets.Add(socket);
            Console.WriteLine("Client connected");
            Logger.WriteLog("Socket cliente conectado");
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


                ParameterizedThreadStart start = new ParameterizedThreadStart(PublishTag);
                Thread threadGpo = new Thread(start);
                threadGpo.Start(text);

                //System.Windows.Forms.MessageBox.Show(text);
                // ------------send signal to GPIO


                string response = "OK";
                byte[] data = Encoding.ASCII.GetBytes(response);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch (Exception)
            {
                Console.WriteLine("Connection lost");
                Logger.WriteLog("Conexión de socket perdida");
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
        }

        protected void NotifyNewTags(List<HTKLibrary.Readers.Tag> tags)
        {
            if (NewTags != null)
                NewTags(this, tags);
        }
        public event NewTagEventHandler NewTags;
        public delegate void NewTagEventHandler(object sender, List<HTKLibrary.Readers.Tag> tags);

        private void PublishTag(object obj)
        {
            //PublishTag
            HTKLibrary.Readers.Tag tag = new HTKLibrary.Readers.Tag();
            try
            {
                string stag = (string)obj;
                stag = stag.Replace("direction", "Direction")
                           .Replace("epc", "EPC")
                           .Replace("ip", "IP")
                           .Replace("rssi", "RSSI")
                           .Replace("timestamp", "TimeStamp");

                XML<HTKLibrary.Readers.Tag> xml = new XML<HTKLibrary.Readers.Tag>();
                tag = xml.DeserializeString(stag);
                tag.EPC = tag.EPC.Trim();
                tag.IP = tag.IP.Trim();
                tag.TimeStamp = tag.TimeStamp.Trim();
                NotifyNewTags(new List<HTKLibrary.Readers.Tag> { tag });
            }
            catch { }
        }

        private void SendCallback(IAsyncResult AR)
        {
            System.Net.Sockets.Socket socket = (System.Net.Sockets.Socket)AR.AsyncState;
            socket.EndSend(AR);
        }

    }
}
