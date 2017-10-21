using HTKLibrary.Comunications.Net35.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDW_Print
{
    public class ConfigManager
    {
        private int maxHardware = 6;
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
        public string AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
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
                Connectivity.XML<ConfigManager> xml = new Connectivity.XML<ConfigManager>(path + "\\config.xml");
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
