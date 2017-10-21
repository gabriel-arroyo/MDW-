using MDW_wf.Model;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;
using System.IO;

namespace MDW_wf.Controller
{
    public static class HardwareManager
    {
        public static Start StartWindow;
        public static Dictionary<string,ReaderModel> ReadersCollection = new Dictionary<string,ReaderModel>();
        public static Dictionary<string, Printer> PrintersList = new Dictionary<string, Printer>();
        public static List<ReaderModel> ReadersCollectionList
        {
            get
            {
                List<ReaderModel> list = (from keyValuePair in ReadersCollection
                                          select keyValuePair.Value).ToList();
                return list;
            }
        }
        public static void Initialize(Start window)
        {
            StartWindow = window;
            GetStoredAntennas();
            GetStoredPrinters();
        }

        private static List<Printer> GetStoredPrinters()
        {
            return new List<Printer>();
        }

        private static List<ReaderModel> GetStoredAntennas()
        {
            string line;
            List<ReaderModel> readers = new List<ReaderModel>();
            try
            {
                using (System.IO.StreamReader file =
                new System.IO.StreamReader(ConfigManager.AppPath + "\\readers.conf"))
                { 
                    string[] parts = new string[5];
                    while ((line = file.ReadLine()) != null)
                    {
                        parts = line.Split(',');
                        string model = "none";
                        if (parts[0].Contains("CS203"))
                            model = "CS203";
                        if (parts[0].Contains("CS469"))
                            model = "CS469";
                        if (parts[0].Contains("CS101"))
                            model = "CS101";
                        string ip = parts[1];
                        int power = Convert.ToInt32(parts[2]);
                        string alias = parts[3];
                        string hexPorts = "0x" + parts[4];
                        string binaryPorts = Convert.ToString(Convert.ToInt32(hexPorts, 16), 2).PadLeft(16, '0');
                        bool[] ports = new bool[16];
                        for (int i = 0; i < 16; i++)
                        {
                            ports[i] = binaryPorts[i] == '1';
                        }

                        ReaderModel a = new ReaderModel(model, ip, power, ports, alias);
                        AddReader(a);
                        readers.Add(a);
                    }
                }
            }
            catch { }
            return readers;

        }

        public static void AddReader(ReaderModel hardare)
        {
            Thread.Sleep(10);
            if (ReadersCollection.Count + PrintersList.Count == ConfigManager.MaxHardware)
            {
                StartWindow.Message("Ya ha alcanzado el número máximo de equipos. Para agregar más consulte a su proveedor",true);
                return;
            }
            ReadersCollection.Add(hardare.IP, hardare);
            StartWindow.UpdateUI_Antennas();
        }
        public static void RemoveReader(ReaderModel reader)
        {
            Thread.Sleep(10);
            try
            {
                string ip = reader.IP;
                ReadersCollection.TryGetValue(ip, out reader);
                if (reader != null)
                {
                    ReadersCollection.Remove(ip);
                }
            }
            catch { }
            StartWindow.UpdateUI_Antennas();
        }

        public static void RemoveReader(string ip)
        {
            Thread.Sleep(10);
            try
            {
                ReaderModel reader;
                ReadersCollection.TryGetValue(ip, out reader);
                if(reader != null)
                {
                    ReadersCollection.Remove(ip);
                }
            }
            catch { }
            StartWindow.UpdateUI_Antennas();
        }

        internal static void UpdateReader(string ip, string newIp, string alias, string model, int power, bool[] ports)
        {
            ReaderModel reader;
            HardwareManager.ReadersCollection.TryGetValue(ip, out reader);
            if (reader != null)
            {
                if (reader.Connected) return;
                if (model != reader.Model)
                {
                    reader.Disconnect();
                    var newreader = new ReaderModel(model, newIp, power, ports, alias);

                    HardwareManager.ReadersCollection.Remove(ip);
                    HardwareManager.ReadersCollection.Add(ip, newreader);

                    reader = newreader;
                }
                else
                {
                    reader.IP = newIp;
                    reader.Alias = alias;
                    reader.Model = model;
                    reader.Power = power;
                    reader.Ports = ports;
                }
                if(reader.RFID == null)
                {
                    var newreader = new ReaderModel(model, newIp, power, ports, alias);
                    HardwareManager.ReadersCollection.Remove(ip);
                    HardwareManager.ReadersCollection.Add(ip, newreader);
                    reader = newreader;
                }
                StartWindow.UpdateUI_Antennas();
            }

        }

        public static void AddPrinter(Printer printer)
        {
            Thread.Sleep(10);
            if (ReadersCollection.Count + PrintersList.Count == ConfigManager.MaxHardware)
            {
                StartWindow.Message("Ya ha alcanzado el número máximo de equipos. Para agregar más consulte a su proveedor", true);
                return;
            }
            PrintersList.Add(printer.Name, printer);
        }


        public static void SaveAll()
        {
            string[] readers = new string[ReadersCollection.Count];
            int i = 0;
            foreach(ReaderModel reader in ReadersCollection.Values)
            {
                string binaryPorts = "";
                for(int p = 0; p < 16; p++)
                {
                    binaryPorts += reader.Ports[p] ? "1" : "0";
                }
                string hex = Convert.ToInt32(binaryPorts, 2).ToString("X");
                readers[i] = string.Format("{0},{1},{2},{3},{4}", reader.Model, reader.IP, reader.Power.ToString(), reader.Alias, hex);
                i++;
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(ConfigManager.AppPath + "\\readers.conf"))
                {
                    for (int j = 0; j < ReadersCollection.Count; j++)
                    {
                        writer.WriteLine(readers[j]);
                    }
                }
            }
            catch { }
        }
    }
}
