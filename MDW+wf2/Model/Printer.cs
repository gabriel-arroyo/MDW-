using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDW_wf.Model
{
    public class Printer:Hardware
    {
        public Printer(string name)
        {
            Name = name;
            Type = Types.Printer;
        }
        public static Printer CreatePrinter(string name)
        {
            return new Printer(name);
        }
    }
    public class PrinterMongo
    {
        public string Client { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public PrinterMongo()
        {
            Name = "";
            Alias = "";
            Client = "";
        }
        public PrinterMongo(string client, string name)
        {
            Name = name;
            Alias = "";
            Client = "";
        }
        public PrinterMongo(string client, string name, string alias)
        {
            Name = name;
            Alias = alias;
            Client = client;
        }
    }
}
