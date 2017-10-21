using MDW_wf.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDW_wf
{
    public class SlaveReader
    {
        public string id { get; set; }
        public string alias { get; set; }
        public bool connected { get; set; } = false;
        public ServerTags Server { get; set; }

        private int _readTime = 1;
        public int readTime
        {
            get
            {
                return _readTime * 1000;
            }
            set
            {
                _readTime = value;
            }
        }


        public SlaveReader(string _id, ServerTags server)
        {
            id = _id;
            Server = server;
        }
        public void Play()
        {
            connected = true;
            Server.NewTags += Server_NewTags;
        }

        private void Server_NewTags(object sender, List<HTKLibrary.Readers.Tag> tags)
        {
            List<HTKLibrary.Readers.Tag> fromReader = new List<HTKLibrary.Readers.Tag>();
            foreach (HTKLibrary.Readers.Tag tag in tags)
            {
                tag.IP = id;
                if (tag.IP == id)
                {
                    fromReader.Add(tag);
                }
            }
            if (fromReader.Count > 0)
                NotifyNewTags(fromReader);
        }

        public void Stop()
        {
            connected = false;
            Server.NewTags -= Server_NewTags;
        }
        protected void NotifyNewTags(List<HTKLibrary.Readers.Tag> tags)
        {
            if (NewTags != null)
                NewTags(this, tags);
        }
        public event NewTagEventHandler NewTags;
        public delegate void NewTagEventHandler(object sender, List<HTKLibrary.Readers.Tag> tags);

    }
}
