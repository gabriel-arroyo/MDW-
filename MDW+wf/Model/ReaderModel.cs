using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HTKLibrary.Readers;
using MDW_wf.Controller;
using System.Threading;

namespace MDW_wf.Model
{
    public class ReaderModel : Hardware
    {
        public string Model { get; set; }
        private string ip;
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }
        private int power;
        public int Power
        {
            get { return power; }
            set { power = value; }
        }
        private string alias;
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }
        private bool[] ports = new bool[16] {true, false, false, false,
                                            false, false, false, false,
                                            false, false, false, false,
                                            false, false, false, false };
        public bool[] Ports
        {
            get
            {
                if (RFID == null) return ports;
                return RFID.Ports;
            }
            set
            {
                if (RFID == null)
                {
                    ports = value;
                    return;
                }
                RFID.Ports = value;
            }
        }
        public Reader RFID { get; set; }
        public bool Connected { get; set; }

        public ReaderModel(string model, string ip, int power)
        {
            this.Type = Types.Reader;
            this.Name = ip;
            Model = model;
            InitializeRFID(model, ip, power);
            IP = ip;
            Alias = ip;
            Power = power;
            Ports = new bool[1] { true };
            Connected = false;
        }
        public ReaderModel(string model, string ip, int power, bool[] ports)
        {
            this.Type = Types.Reader;
            this.Name = ip;
            Model = model;
            InitializeRFID(model, ip, power);
            IP = ip;
            Alias = ip;
            Power = power;
            Ports = ports;
            Connected = false;
        }
        public ReaderModel(string model, string ip, int power, string alias)
        {
            this.Type = Types.Reader;
            this.Name = ip;
            Model = model;
            InitializeRFID(model, ip, power);
            IP = ip;
            Alias = alias;
            Power = power;
            Ports = new bool[1] { true };
            Connected = false;
        }
        public ReaderModel(string model,string ip, int power, bool[] ports, string alias)
        {
            this.Type = Types.Reader;
            this.Name = ip;
            Model = model;
            InitializeRFID(model, ip, power);
            IP = ip;
            Alias = alias;
            Power = power;
            Ports = ports;
            Connected = false;
        }

        private void InitializeRFID(string model, string ip, int power)
        {
            RFID = Reader.CreateReader(model, ip);
            RFID.Ports = ports;
            RFID.SetPower((uint)power);
        }
        public bool Connect()
        {
            if (RFID != null)
            {
                if (RFID.Connected) return true;
            }
            else
            {
                RFID = Reader.CreateReader(Model, IP);
            }
            //Disconnect();
            RFID.Model = Model;
            RFID.IP = IP;
            RFID.SetPower((uint)Power);
            //RFID.PlayAsync();
            RFID.Stop();
            RFID.Play();
            RFID.ReaderConnected += RFID_ReaderConnected;
            RFID.ReaderDisconnected += RFID_ReaderDisconnected;
            return Connected;
        }

        private void RFID_ReaderDisconnected(string ip)
        {
            //System.Windows.Forms.MessageBox.Show("La antena se ha desconectado");
            NewTag -= Reader_NewTag;
            DeletedTag -= Reader_DeletedTag;
            Connected = false;
            TagManager.RemoveAll(IP);
            if (RFID == null) return;
            RFID.NewTagReaded -= RFID_NewTagReaded;
            RFID.TagDeleted -= RFID_TagDeleted;
            RFID.ReaderConnected -= RFID_ReaderConnected;
            RFID.ReaderDisconnected -= RFID_ReaderDisconnected;
            RFID = null;
        }

        private void RFID_ReaderConnected(string ip)
        {
            if (RFID == null) return;
            Connected = RFID.Connected;

            if (Connected)
            {
                RFID.TagReaded += RFID_NewTagReaded;
                RFID.TagDeleted += RFID_TagDeleted;
                NewTag += Reader_NewTag;
                DeletedTag += Reader_DeletedTag;
                RFID.EraseTime = 3;
            }

        }

        private void RFID_TagDeleted(Tag tag)
        {
            var tagdata = new ListViewTagsData() { IP = tag.IP, EPC = tag.EPC, RSSI = tag.RSSI.ToString(), Direction = "0", Timestamp = DateTime.Now.ToString() };
            OnDeletedTag(tagdata);
        }

        private void RFID_NewTagReaded(Tag tag)
        {
            var tagdata = new ListViewTagsData() { IP = tag.IP, EPC = tag.EPC, RSSI = tag.RSSI.ToString(), Direction = tag.Direction.ToString(), Timestamp = DateTime.Now.ToString() };
            OnNewTag(tagdata);
        }

        public void Disconnect()
        {
            if(RFID != null)
            {
                RFID.Stop();
            }
           
        }
        private void Reader_DeletedTag(ListViewTagsData tag)
        {
            TagManager.Remove(tag);
        }

        private void Reader_NewTag(ListViewTagsData tag)
        {
            TagManager.Add(tag);
        }

        #region Events

        public delegate void NewTagHandler(ListViewTagsData tag);
        public event NewTagHandler NewTag;
        protected virtual void OnNewTag(ListViewTagsData tag)
        {
            if (NewTag != null)
                NewTag(tag);
        }

        public delegate void DeletedTagHandler(ListViewTagsData tag);
        public event DeletedTagHandler DeletedTag;
        protected virtual void OnDeletedTag(ListViewTagsData tag)
        {
            if (DeletedTag != null)
                DeletedTag(tag);
        }
        #endregion

    }
}
