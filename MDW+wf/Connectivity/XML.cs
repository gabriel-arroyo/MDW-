using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDW_wf.Model;
using MDW_wf.Controller;
using System.IO;

namespace MDW_wf.Connectivity
{
    public static class XML
    {
        public static void Write(ListViewTagsData tag)
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(ListViewTagsData));

            string path = "";
            if (String.IsNullOrEmpty(ConfigManager.XMLPath))
                path = ConfigManager.AppPath + "\\" + "tags.xml";
            else
                path = ConfigManager.XMLPath.Contains(".xml") ?
                    ConfigManager.XMLPath :
                    ConfigManager.XMLPath + "\\tags.xml";

            string xml = "";
            using (StringWriter stringWriter = new StringWriter())
            {
                writer.Serialize(stringWriter, tag);
                xml = stringWriter.ToString();
            }
            try
            {
                using (StreamWriter fileWriter = File.AppendText(path))
                {
                    fileWriter.WriteLine(xml);
                }
            }
            catch { }
        }
    }
}
