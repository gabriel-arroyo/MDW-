using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDW_wf.Model
{
    public class LegacyTag
    {
        public string EPC { get; set; }
        public string rssi { get; set; }
        public string ip { get; set; }
        public string client { get; set; }
        public int erasetime { get; set; }
        public string direction { get; set; }
        public string timestamp { get; set; }

        public LegacyTag(string _epc, string _rssi, string _ip, string _client, int _erasetime, string _direction, string _timestamp)
        {
            EPC = _epc;
            rssi = _rssi;
            ip = _ip;
            client = _client;
            erasetime = _erasetime;
            direction = _direction;
            timestamp = _timestamp;
        }
    }
}
