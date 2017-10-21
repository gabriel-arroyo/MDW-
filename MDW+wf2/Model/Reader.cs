using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDW_wf.Model
{
    public class ReaderModel
    {
        public string ip { get; set; }
        public string alias { get; set; }
        public string model { get; set; }
        public int power { get; set; } = 300;
        public bool[] ports { get; set; } = Convert.ToString(int.Parse("8000", System.Globalization.NumberStyles.HexNumber), 2).Select(s => s.Equals('1')).ToArray();
        public bool connected { get; set; } = false;
        public bool started { get; set; } = false;
        public bool auto { get; set; } = false;
    }
}
