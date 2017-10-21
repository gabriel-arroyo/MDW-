using HTKLibrary.Classes.MDW;
using HTKLibrary.Comunications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDW_Print.Connectivity
{
    public class WebSocket
    {
        public static string Token = "";
        public static SocketClient client;
        private static bool Initialized = false;

        public static void Initialize()
        {
            if (!Pinger.Ping(Program.configManager.SocketIP)) return;
            if (string.IsNullOrEmpty(Program.configManager.SocketIP))
                return;

            client = new SocketClient(Program.configManager.SocketIP, Program.configManager.SocketPort);
            client.Start();
            Initialized = true;
        }
        public static void Write(HTKLibrary.Classes.MDW.Tag tag)
        {
            if (!Initialized) Initialize();
            if (!Initialized) return;
            client.SendMessage(new Tag
            {
                direction = Convert.ToDouble(tag.direction),
                epc = tag.epc,
                erasetime = 0,
                ip = tag.ip,
                rssi = Convert.ToDouble(tag.rssi),
                timestamp = tag.timestamp
            });
        }
    }
}
