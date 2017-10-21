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
            if (string.IsNullOrEmpty(ConfigManager.WebServiceURL))
                return;

            mdw = new MDWClient(ConfigManager.User, ConfigManager.Password, ConfigManager.MacAddress);
            client = new MDWRestClient(ConfigManager.User, ConfigManager.Password, ConfigManager.WebServiceURL, mdw);
            if (Token == "")
            {
                Token = client.LoginRestClient();
                if (Token == "") return;
            }
            var id = client.LoginMDWClient();
            if (Token!= "" && id != "") Initialized = true;
        }
        public static void Write(ListViewTagsData tag)
        {
            if (!Initialized) Initialize();
            if (!Initialized) return;
            client.AddTag(new Tag {
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
