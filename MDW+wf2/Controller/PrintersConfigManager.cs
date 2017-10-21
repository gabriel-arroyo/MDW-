using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MDW_wf.Model;

namespace MDW_wf.Controller
{
    public class PrintersConfigManager
    {
        public List<printer> printers = new List<printer>();

        public PrintersConfigManager()
        {
            
        }

        public static void Save(PrintersConfigManager config)
        {
            try
            {
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Connectivity.XML<PrintersConfigManager> xml = new Connectivity.XML<PrintersConfigManager>(path + "\\printersconfig.xml");
                xml.Serialize(config);
            }
            catch { }
        }

        public static PrintersConfigManager ReadConfig()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Connectivity.XML<PrintersConfigManager> xml = new Connectivity.XML<PrintersConfigManager>(path + "\\printersconfig.xml");
            return xml.Deserialize() ?? new PrintersConfigManager();
        }
    }
    public class printer
    {
        public string name = "";
        public string alias = "";
        public string file = "";
        public bool webService = false;
        public bool webSocket = false;
        public string prefix = "";
        public int adjust = 0;
        public printer()
        {

        }
    }
}
