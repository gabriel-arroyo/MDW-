using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MDW_wf.Model
{
    //[XmlRoot(Namespace = "urn:mdw")]
    public class GPIO
    {
        public string ip { get; set; }
        public bool gpio0 { get; set; }
        public bool gpio1 { get; set; }
        public GPIO()
        {
            try
            {
                ip = "";
                gpio0 = false;
                gpio0 = false;
            }
            catch { }
        }
        public GPIO(string _ip, bool _gpio0, bool _gpio1)
        {
            try
            {
                ip = _ip;
                gpio0 = _gpio0;
                gpio1 = _gpio1;
            }
            catch { }
        }
        public GPIO(string xmlText)
        {
            try
            {
                var stringReader = new System.IO.StringReader(xmlText);
                var serializer = new XmlSerializer(this.GetType());
                GPIO gpio = (GPIO)serializer.Deserialize(stringReader);
                ip = gpio.ip;
                gpio0 = gpio.gpio0;
                gpio1 = gpio.gpio1;
            }
            catch { }
        }
        public string ToXML()
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stringwriter, this);
                return stringwriter.ToString();
            }
            catch
            {
                return "";
            }
        }


    }
}
