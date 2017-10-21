using MDW_wf.Model;
using MDW_wf.Controller;
using HTKLibrary.Classes;
using HTKLibrary.Classes.MDW;
using HTKLibrary.Comunications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDW_wf.Connectivity
{
    public static class WebService
    {
        public static string Token = "";
        public static MDWClient mdw = new MDWClient();
        public static MDWRestClient client = new MDWRestClient();
        private static bool Initialized = false;

        public static void Initialize()
        {
            if (string.IsNullOrEmpty(Program.configManager.WebServiceURL))
                return;

            mdw = new MDWClient(Program.configManager.User, Program.configManager.Password, Program.configManager.MacAddress);
            client = new MDWRestClient(Program.configManager.User, Program.configManager.Password, Program.configManager.WebServiceURL, mdw);
            if (Token == "")
            {
                Token = client.LoginRestClient();
                if (Token == "") return;
            }
            var id = client.LoginMDWClient();
            if (Token!= "" && id != "") Initialized = true;
        }
        public static void Write(HTKLibrary.Classes.MDW.Tag tag)
        {
            if (!Initialized) Initialize();
            if (!Initialized) return;
            client.AddTag(new Tag {
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
