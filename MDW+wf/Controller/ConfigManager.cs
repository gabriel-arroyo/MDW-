using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace MDW_wf.Controller
{

    public static class ConfigManager
    {
        private static int maxHardware = 6;
        public static string WebServiceURL { get; set; }
        public static string User { get; set; }
        public static string Password { get; set; }
        public static string MacAddress { get; set; }
        public static string UserURL { get; set; }
        public static int MaxHardware { get { return maxHardware; } set { maxHardware = value; } }
        public static string SQLServer { get; set; }
        public static string SQLDatabase { get; set; }
        public static string SQLUser { get; set; }
        public static string SQLPassword { get; set; }
        public static string SocketIP { get; set; }
        public static int SocketPort { get; set; }
        public static string CSVPath { get; set; }
        public static string XMLPath { get; set; }
        public static string AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static int ConnectedImage = 2;
        public static int DisconnectedImage = 0;
        public static int ConnectingImage = 1;
        public static bool Activated = false;

        public static Start StartWindow;
        public static void Initialize(Start startwindow)
        {
            StartWindow = startwindow;
        }

        public static void Save()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AppPath + "\\config.conf"))
                {
                    writer.WriteLine(WebServiceURL ?? "");
                    writer.WriteLine(User ?? "");
                    writer.WriteLine(Password ?? "");
                    writer.WriteLine(UserURL ?? "");
                    writer.WriteLine(MaxHardware);
                    writer.WriteLine(SQLServer ?? "");
                    writer.WriteLine(SQLDatabase ?? "");
                    writer.WriteLine(SQLUser ?? "");
                    writer.WriteLine(SQLPassword ?? "");
                    writer.WriteLine(CSVPath ?? "");
                    writer.WriteLine(XMLPath ?? "");
                    writer.WriteLine(SocketIP ?? "");
                    writer.WriteLine(SocketPort);
                    writer.WriteLine(Activated);
                }
            }
            catch { }
        }

        public static void ReadConfig()
        {
            string line = "";
            List<string> data = new List<string>();
            int i = 0;
            try
            {
                if (!File.Exists(AppPath + "\\config.conf")) return;
                using (StreamReader file =
                new StreamReader(AppPath + "\\config.conf"))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        data.Add(line);
                        i++;
                    }
                }
                WebServiceURL = data[0];
                User = data[1];
                Password = data[2];
                UserURL = data[3];
                MaxHardware= Convert.ToInt32(data[4]) == 0 ? 6 : Convert.ToInt32(data[4]);
                SQLServer = data[5];
                SQLDatabase = data[6];
                SQLUser = data[7];
                SQLPassword = data[8];
                CSVPath = data[9];
                XMLPath = data[10];
                SocketIP = data[11];
                SocketPort = Convert.ToInt32(data[12]);
                Activated = Convert.ToBoolean(data[13]);
                MacAddress = HTKLibrary.Readers.Reader.GetPCMacAddress();
                if(StartWindow != null)
                    StartWindow.FillConfiguration();
                
            }
            catch(Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }
        }
    }
}
