using HTKLibrary.Classes.MDW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MDW_Print.Connectivity
{
    public class XML<T>
    {
        public XML(string file)
        {
            XmlFile = file;
            ExistsFile = File.Exists(file);
        }
        public XML()
        {
            XmlFile = null;
            ExistsFile = false;
        }

        public bool ExistsFile { get; set; }
        public string XmlFile { get; set; }

        //public static void Write(Tag tag, string path)
        //{

        //}
        //public void Write(Tag tag);
        public T Deserialize()
        {
            T objects = default(T);
            try
            {
                if (!ExistsFile) return default(T);
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                TextReader textReader = new StreamReader(XmlFile);
                
                objects = (T)deserializer.Deserialize(textReader);
                textReader.Close();

            }
            catch
            {
                objects = default(T);
            }
          
            return objects;
        }
        public T DeserializeString(string s)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(s);
            T objects;
            objects = (T)deserializer.Deserialize(stringReader);
            stringReader.Close();

            return objects;
        }
        public List<T> DeserializeList()
        {
            if (!ExistsFile) return new List<T>();
            XmlSerializer deserializer = new XmlSerializer(typeof(List<T>));
            TextReader textReader = new StreamReader(XmlFile);
            List<T> objects;
            objects = (List<T>)deserializer.Deserialize(textReader);
            textReader.Close();

            return objects;
        }
        public void Serialize(List<T> ob)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextWriter textWriter = new StreamWriter(XmlFile);
            serializer.Serialize(textWriter, ob);
            textWriter.Close();
        }
            
        public void Serialize(T ob)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter textWriter = new StreamWriter(XmlFile);
            serializer.Serialize(textWriter, ob);
            textWriter.Close();
        }
        
    }
}
