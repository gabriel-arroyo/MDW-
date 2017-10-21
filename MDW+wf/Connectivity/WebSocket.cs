using HTKLibrary.Classes.MDW;
using HTKLibrary.Comunications;
using MDW_wf.Controller;
using MDW_wf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDW_wf.Connectivity
{
    public class WebSocket
    {
        public static string Token = "";
        public static SocketClient client;
        private static bool Initialized = false;

        public static void Initialize()
        {
            if (!Pinger.Ping(ConfigManager.SocketIP)) return;
            if (string.IsNullOrEmpty(ConfigManager.SocketIP))
                return;

            client = new SocketClient(ConfigManager.SocketIP, ConfigManager.SocketPort);
            client.Start();
            Initialized = true;
        }
        public static void Write(ListViewTagsData tag)
        {
            if (!Initialized) Initialize();
            if (!Initialized) return;
            client.SendMessage(new Tag
            {
                direction = Convert.ToDouble(tag.Direction),
                epc = tag.EPC,
                erasetime = 0,
                ip = tag.IP,
                rssi = Convert.ToDouble(tag.RSSI),
                timestamp = tag.Timestamp
            });
        }
    }
}
