using MDW_wf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using HTKLibrary.Readers;
using System.Threading;

namespace MDW_wf.Connectivity
{
    public class ServerGPIO
    {
        private byte[] _buffer = new byte[1024];
        private List<System.Net.Sockets.Socket> _clientSockets = new List<System.Net.Sockets.Socket>();
        private System.Net.Sockets.Socket _serverSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private int _port = 100;
        public delegate void GPIOEventHandler(GPIO gpio);
        public event GPIOEventHandler GPIOSignal;
        protected virtual void OnGPIOSignal(GPIO gpio)
        {
            if (GPIOSignal != null)
                GPIOSignal(gpio);
        }

        public ServerGPIO(int port)
        {
            
            _port = port;
        }

        public void Start()
        {
            Console.WriteLine("Setting up server...");
            Logger.WriteLog("Iniciando servidor socket GPIO");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }
        
        private void AcceptCallback(IAsyncResult AR)
        {
            System.Net.Sockets.Socket socket = _serverSocket.EndAccept(AR);
            _clientSockets.Add(socket);
            Console.WriteLine("Client connected");
            Logger.WriteLog("Cliente socket GPIO connectado");
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

                
                ParameterizedThreadStart start = new ParameterizedThreadStart(GPO);
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
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
        }

        private void GPO(object obj)
        {
            try
            {
                string sgpio = (string)obj;
                GPIO gpio = new GPIO(sgpio);
                OnGPIOSignal(gpio);
                CS203.SetGPO0(gpio.ip, gpio.gpio0);
                CS203.SetGPO1(gpio.ip, gpio.gpio1);
                Thread.Sleep(1000);
                CS203.SetGPO0(gpio.ip, false);
                CS203.SetGPO1(gpio.ip, false);
                gpio.gpio0 = false;
                gpio.gpio1 = false;
                OnGPIOSignal(gpio);
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
