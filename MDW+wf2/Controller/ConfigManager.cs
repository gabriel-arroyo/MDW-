using System;
using System.Collections.Generic;
using HTKLibrary.Comunications.Net35.DB;
using System.IO;
using System.Windows;
using MDW_wf.Model;

namespace MDW_wf.Controller
{

    public class ConfigManager
    {
        private int maxHardware = 6;
        public bool firstUse = false;
        public string WebServiceURL { get; set; } = "";
        public string UserId { get; set; } = "";
        public string User { get; set; } = "";
        public string Password { get; set; } = "";
        public string MacAddress { get; set; } = "";
        public string UserURL { get; set; } = "";
        public int MaxHardware { get { return maxHardware; } set { maxHardware = value; } }
        public string SQLServer { get; set; } = "";
        public string SQLDatabase { get; set; } = "";
        public string SQLUser { get; set; } = "";
        public string SQLPassword { get; set; } = "";
        public string SocketIP { get; set; } = "";
        public int SocketPort { get; set; }
        public string CSVPath { get; set; } = "";
        public string XMLPath { get; set; } = "";
        public string LabelPath { get; set; } = "";
        public bool PublishSQL { get; set; } = false;
        public bool PublishAssetsApp { get; set; } = false;
        public bool PublishCSV { get; set; } = false;
        public bool PublishXML { get; set; } = false;
        public bool PublishWebService { get; set; } = false;
        public bool PublishWebSocket { get; set; } = false;
        public int EraseTime { get; set; } = 5;
        public int WaitTime { get; set; } = 0;
        public string AppPath
        {
            get { return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }
        public int ConnectedImage = 2;
        public int DisconnectedImage = 0;
        public int ConnectingImage = 1;
        public bool Activated = false;
        public string Token { get; set; } = "";

        public ConfigManager()
        {
            MacAddress = HTKLibrary.Readers.Reader.GetPCMacAddress();
        }

        public static void Save(ConfigManager config)
        {
            try
            {
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Connectivity.XML<ConfigManager> xml = new Connectivity.XML<ConfigManager>(path +"\\config.xml");
                xml.Serialize(config);
            }
            catch { }
        }

        public static ConfigManager ReadConfig()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Connectivity.XML<ConfigManager> xml = new Connectivity.XML<ConfigManager>(path + "\\config.xml");
            return xml.Deserialize() ?? new ConfigManager();
        }

        public static void SaveReaders(List<ReaderModel> readers)
        {
            try
            {
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Connectivity.XML<List<ReaderModel>> xml = new Connectivity.XML<List<ReaderModel>>(path + "\\readers.xml");
                xml.Serialize(readers);
            }
            catch { }
        }
        public static List<ReaderModel> LoadReaders()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Connectivity.XML<List<ReaderModel>> xml = new Connectivity.XML<List<ReaderModel>>(path + "\\readers.xml");
            return xml.Deserialize() ?? new List<ReaderModel>();
        }
    }
}