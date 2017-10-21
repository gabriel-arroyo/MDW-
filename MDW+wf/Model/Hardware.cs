using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDW_wf.Model
{
    public class Hardware
    {
        public string Name { get; set; }
        public enum Types
        {
            Reader,
            Printer
        }
        public Types Type { get; set; }
    }
    
}
