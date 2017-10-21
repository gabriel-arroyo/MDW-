using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDW_wf.Model
{
    public class WebServiceLabel
    {
        public string Client { get; set; }
        public string Printer { get; set; }
        public int Design { get; set; }
        public string EPC { get; set; }
        public List<Property> Properties { get; set; }
    }
}
