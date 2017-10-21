using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDW_wf
{
    public class VirtualReader
    {
        public string id { get; set; }
        public string alias { get; set; }
        public bool connected { get; set; } = false;

        private int _readTime = 1;
        public int readTime {
            get
            {
                return _readTime * 1000;
            }
            set
            {
                _readTime = value;
                ReadTimer.Stop();
                ReadTimer.Interval = value * 1000;
                ReadTimer.Start();
            }
        }


        private System.Timers.Timer ReadTimer = new System.Timers.Timer(1000);

        public VirtualReader(string _id)
        {
            id = _id;
        }
        public void Play()
        {
            ReadTimer.Elapsed += ReadTimer_Elapsed;
            ReadTimer.Start();
            connected = true;
        }
        public void Stop()
        {
            ReadTimer.Elapsed -= ReadTimer_Elapsed;
            ReadTimer.Stop();
            connected = false;
        }
        protected void NotifyNewTags(List<HTKLibrary.Readers.Tag> tags)
        {
            if (NewTags!= null)
                NewTags (this, tags);
        }
        public event NewTagEventHandler NewTags;
        public delegate void NewTagEventHandler(object sender, List<HTKLibrary.Readers.Tag> tags);
        private void ReadTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<HTKLibrary.Readers.Tag> tags = new List<HTKLibrary.Readers.Tag>();
            int number = 1;
            if(id.ToLower().StartsWith("0"))
            {
                number = 10;
                try
                {
                    number = Convert.ToInt32(id.Substring(2, 2));
                }
                catch (Exception) { }
                
            }
            for (int i = 0; i < number; i++)
            {
                Random rnd = new Random();
                var rssi = rnd.Next(30, 95);
                string datestring = DateTime.Now.ToString("yyyyMMddHHmmss");
                string prefix = "AAAA" + datestring;
                string epc = prefix + new string('0', 24 - prefix.Length - i.ToString().Length) + (i + 1).ToString();
                tags.Add(new HTKLibrary.Readers.Tag { Direction = -1, EPC = epc, IP = id, RSSI = rnd.Next(30,95), TimeStamp = DateTime.Now.ToString() });
            }

            NotifyNewTags(tags);

        }
    }
}
