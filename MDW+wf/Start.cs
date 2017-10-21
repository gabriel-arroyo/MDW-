using System;
using System.ComponentModel;
using System.Data;
using MDW_wf.Controller;
using HTKLibrary.Readers;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using MDW_wf.Model;
using System.Linq;
using System.Timers;

namespace MDW_wf
{
    public partial class Start : Form, IDisposable
    {
        #region Global Constants
        public const int Tab_Dashboard = 0;
        public const int Tab_Readers = 1;
        public const int Tab_Print = 2;
        public const int Tab_Config = 3;
        public const int Tab_AddHardware = 4;

        public Color darkgreen = Color.FromArgb(19, 84, 36);

        public BindingSource bs = new BindingSource();

        #endregion
        #region Global Variables
        public DataTable PrintListData { get; set; }
        private string labelDir = "Etiqueta";
        public string LabelDir
        {
            get { return labelDir; }
            set { labelDir = value; NotifyPropertyChanged("LabelDir"); }
        }

        public Color ThemeColor = Color.FromArgb(32, 50, 72);
        public System.Timers.Timer EraseTimer = new System.Timers.Timer(3000);
        //public Rest RestClient = new Rest();
        private string filter = "";
        public string Filter { get { return filter.ToLower(); } set { filter = value; } }
        #region Notify
        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #endregion
        #region Common
        public Start()
        {
            InitializeComponent();
            TagManager.Initialize(this);
            HardwareManager.Initialize(this);
            EraseTimer.Elapsed += EraseTimer_Elapsed;
            EraseTimer.Start();
            #region PrintTab
            //nice = new NiceApp();
            //txtNum.Text = _numValue.ToString();
            //stkAjusteEPC.Visibility = System.Windows.Visibility.Hidden;
            //stkAntennaSetting.Visibility = System.Windows.Visibility.Hidden;
            #endregion
        }

      

        private void Start_Load(object sender, EventArgs e)
        {
            Tab.Appearance = TabAppearance.FlatButtons;
            Tab.ItemSize = new Size(0, 1);
            Tab.SizeMode = TabSizeMode.Fixed;

            lbToolbarIP.Text = Reader.GetLocalIPAddress();
            int hh = HardwareManager.ReadersCollectionList.FindAll(x => x.Model == "CS101").Count;
            int readers = HardwareManager.ReadersCollection.Count - hh;
            int printers = HardwareManager.PrintersList.Count;

            lbNumberHandhleds.Text = hh.ToString();
            lbNumberPrinters.Text = printers.ToString();
            lbNumerAntennas.Text = readers.ToString();
            lbNumerHardware.Text = (readers + hh).ToString();
            lbToolbalPrinters.Text = printers.ToString();
            lbToolbarReaders.Text = (readers + hh).ToString();

            lvReaders.LargeImageList = connectionImages;
            lvReaders.SmallImageList = connectionImages;
            lvReaders.StateImageList = connectionImages;
            lvReaders.View = View.Tile;
            pnlReaderSettings.Enabled = false;

            bs.DataSource = TagsCollections.TagsCollectionsList;
            dataGridView1.DataSource = bs;
        }
        public void Message(string text, bool messageBox = false)
        {
            lbStatus.Text = text;
            if(messageBox)
            {
                MessageBox.Show(text);
            }
        }
        #region TabButtons
        private void btDashboardTab_Click(object sender, EventArgs e)
        {
            int hh = HardwareManager.ReadersCollectionList.FindAll(x => x.Model == "CS101").Count;
            int readers = HardwareManager.ReadersCollection.Count - hh;
            int printers = HardwareManager.PrintersList.Count;

            lbNumberHandhleds.Text = hh.ToString();
            lbNumberPrinters.Text = printers.ToString();
            lbNumerAntennas.Text = readers.ToString();
            lbNumerHardware.Text = (readers + hh).ToString();
            lbToolbalPrinters.Text = printers.ToString();
            lbToolbarReaders.Text = (readers+hh).ToString();

            Tab.Invoke(new MethodInvoker(delegate { Tab.SelectedIndex = Tab_Dashboard; } ));
            lbStatus.Text = "Dashboard";
        }
        private void btReaderTab_Click(object sender, EventArgs e)
        {
            Tab.Invoke(new MethodInvoker(delegate { Tab.SelectedIndex = Tab_Readers; } ));
            lbStatus.Text = "Módulo de lectura de antenas";
        }

        private void ProbarAntena()
        {
           var reader = Reader.CreateReader("CS203", "192.168.2.203");

            reader.SetPower(100);
            reader.PlayAsync();
            reader.NewTagReaded += Reader_NewTagReaded;
            reader.TagReaded += Reader_TagReaded;
            System.Timers.Timer timer = new System.Timers.Timer(100);
            reader.TagDeleted += Reader_TagDeleted;
            timer.Start();
        }

        private void Reader_TagDeleted(Tag tag)
        {
            Reader.StaticMessage("Tag deleted: " + tag.EPC);
        }

        private void Reader_TagReaded(Tag tag)
        {
            Reader.StaticMessage(tag.EPC); ;
        }

        private void Reader_NewTagReaded(Tag tag)
        {
            throw new NotImplementedException();
        }

        private void btPrintTab_Click(object sender, EventArgs e)
        {
            Tab.Invoke(new MethodInvoker(delegate { Tab.SelectedIndex = Tab_Print; }));
            lbStatus.Text = "Módulo de impresión";
        }
        private void btConfigurationTab_Click(object sender, EventArgs e)
        {
            FillConfiguration();
            Tab.Invoke(new MethodInvoker(delegate { Tab.SelectedIndex = Tab_Config; }));
            lbStatus.Text = "Módulo de configuración";
        }
        #endregion
        #endregion
        #region Antennas
        #region ReadersList
        private string SelectedReader = "";
        private int SelectedIndex = -1;
        private string tempSelectedReader = "";
        private int tempSelectedIndex = -1;
        private void lvReaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(SelectedReader != "")
            {
                HardwareManager.UpdateReader(SelectedReader, GetIPFromToolBox(), GetAliasFromToolBox(), GetModelFromToolBox(), GetPowerFromToolBox(), GetPortsFromToolBox());
            }
            if (lvReaders.SelectedIndices.Count > 0)
            {
                pnlReaderSettings.Enabled = true;
                SelectedReader = lvReaders.SelectedItems[0].Text;
                tempSelectedReader = SelectedReader;
                SelectedIndex = lvReaders.SelectedIndices[0];
                tempSelectedIndex = SelectedIndex;
                ReaderModel reader;
                HardwareManager.ReadersCollection.TryGetValue(SelectedReader, out reader);
                if (reader != null)
                {
                    tbPower.Value = reader.Power;
                    tbToolBoxAlias.Text = reader.Alias;
                    tbToolBoxModel.Text = reader.Model;
                    tbPower.Enabled = !reader.Model.ToLower().Contains("cs101");
                    tbToolBoxIP.Text = reader.IP;
                    ConnectButtonState(!reader.Connected);
                    TooglePorts(reader.Ports);
                  
                }
            }
            else
            {
                pnlReaderSettings.Enabled = false;
                tbToolBoxAlias.Text = "";
                tbToolBoxModel.Text = "";
                tbToolBoxIP.Text = "";
                SelectedReader = "";
                SelectedIndex = -1;
                ConnectButtonState(true);
            }
        }
       
        private void btToolStripAddReader_Click(object sender, EventArgs e)
        {
            Tab.Invoke(new MethodInvoker(delegate { Tab.SelectedIndex = Tab_AddHardware; }));
            Message("Añadir Lector");
        }
        private void btToolStripRemoveReader_Click(object sender, EventArgs e)
        {
            HardwareManager.RemoveReader(SelectedReader);
        }
        delegate void AntennaUpdater();
        public void UpdateUI_Antennas()
        {
            try
            {
                if (InvokeRequired)
                {
                    // We're not in the UI thread, so we need to call Invoke
                    Invoke(new AntennaUpdater(UpdateUI_Antennas));
                    return;
                }
                //Clear List
                lvReaders.Items.Clear();

                //Generate new Items
                List<ReaderModel> copy = HardwareManager.ReadersCollectionList;
                ListViewItem[] items = new ListViewItem[copy.Count];

                for (int i = 0; i < copy.Count; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = copy[i].IP;
                    item.SubItems.Add(copy[i].Model + ", " + copy[i].Alias);
                    item.ImageIndex = copy[i].Connected ? 2 : 0;
                    items[i] = item;
                }

                //Populate List
                    lvReaders.Items.AddRange(items);

              
            }
            catch (Exception) { }
        }
        public void AddReaderToListview(string ip, string model)
        {
            for (int i = 0; i < lvReaders.Items.Count; i++)
            {
                if (lvReaders.Items[i].Text == ip)
                {
                    lbStatus.Text = "Ya existe una antenna con la ip " + ip;
                    return;
                }
            }
            ListViewItem item = new ListViewItem(ip);
            item.SubItems.Add(model);
            item.Tag = ip;
            item.ImageIndex = 0;
            lvReaders.Invoke(new MethodInvoker(delegate { lvReaders.Items.Add(item); }));
        }
        public enum ReaderState
        {
            Disconnected,
            Connecting,
            Connected
        }
        public void ChangeReaderState(string ip, ReaderState state)
        {
            int index = GetReaderIndex(ip);
            if (index == -1) return;
            switch (state)
            {
                case ReaderState.Disconnected:
                    lvReaders.Invoke(new MethodInvoker(delegate
                    {
                        lvReaders.Items[index].ImageIndex = 0;
                    }));
                    break;
                case ReaderState.Connecting:
                    lvReaders.Invoke(new MethodInvoker(delegate
                    {
                        lvReaders.Items[index].ImageIndex = 1;
                    }));
                    break;
                case ReaderState.Connected:
                    lvReaders.Invoke(new MethodInvoker(delegate
                    {
                        lvReaders.Items[index].ImageIndex = 2;
                    }));
                    break;

            }
        }
        private delegate int GetReaderIndexDelegate(string ip);
        int GetReaderIndex(string ip)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call Invoke
                return (int)Invoke(new GetReaderIndexDelegate(GetReaderIndex), ip);
            }
            int index = -1;
            for (int i = 0; i < lvReaders.Items.Count; i++)
            {
                if (lvReaders.Items[i].Text == ip)
                {
                    index = i;
                }
            }
            return index;
        }
       
        private delegate string GetEPCFromListviewDelegate(int index);
        string GetEPCFromListview(int index)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call Invoke
                return (string)Invoke(new GetEPCFromListviewDelegate(GetEPCFromListview), index);
            }
            // Property returns a string
            if (index >= lvTags.Items.Count) return "";
            return lvTags.Items[index].SubItems[2].Text.ToString();
        }
     
        private delegate string GetIPFromListviewDelegate(int index);
        string GetIPFromListview(int index)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call Invoke
                return (string)Invoke(new GetIPFromListviewDelegate(GetIPFromListview), index);
            }
            // Property returns a string
            if (lvTags.Items.Count == 0) return "";
            if (index >= lvTags.Items.Count) return "";
            return lvTags.Items[index].SubItems[1].Text.ToString();

        }
        
        private delegate bool ItemContainsFilterDelegate(int index, string filter);
        //bool ItemContainsFilter(int index, string filter)
        //{
        //    if (InvokeRequired)
        //    {
        //        // We're not in the UI thread, so we need to call Invoke
        //        return (bool)Invoke(new ItemContainsFilterDelegate(ItemContainsFilter), index, filter);
        //    }

        //    // Property returns a string
        //    if (index >= lvTags.Items.Count) return false;
        //    if (lvTags.FindItemWithText(filter,true, 0))
        //        return lvTags.Items[index].SubItems[1].Text.ToString();
        //}
        private delegate string GetIPFromToolBoxDelegate();
        string GetIPFromToolBox()
        {
            if (InvokeRequired)
            {
                return (string)Invoke(new GetIPFromToolBoxDelegate(GetIPFromToolBox));
            }
            return tbToolBoxIP.Text;
        }
        private delegate string GetModelFromToolBoxDelegate();
        string GetModelFromToolBox()
        {
            if (InvokeRequired)
            {
                return (string)Invoke(new GetModelFromToolBoxDelegate(GetModelFromToolBox));
            }
            return tbToolBoxModel.Text;
        }
        private delegate string GetAliasFromToolBoxDelegate();
        string GetAliasFromToolBox()
        {
            if (InvokeRequired)
            {
                return (string)Invoke(new GetAliasFromToolBoxDelegate(GetAliasFromToolBox));
            }
            return tbToolBoxAlias.Text;
        }
        private delegate int GetPowerFromToolBoxDelegate();
        int GetPowerFromToolBox()
        {
            if (InvokeRequired)
            {
                return (int)Invoke(new GetPowerFromToolBoxDelegate(GetPowerFromToolBox));
            }
            return tbPower.Value;
        }
        private delegate bool[] GetPortsFromToolBoxDelegate();
        bool[] GetPortsFromToolBox()
        {
            bool[] port = new bool[16] {true, false, false, false,
                                            false, false, false, false,
                                            false, false, false, false,
                                            false, false, false, false };
            if (InvokeRequired)
            {
                return (bool[])Invoke(new GetPortsFromToolBoxDelegate(GetPortsFromToolBox));
            }
            port[0] = tgPort0.Checked;
            port[1] = tgPort1.Checked;
            port[2] = tgPort2.Checked;
            port[3] = tgPort3.Checked;
            port[4] = tgPort4.Checked;
            port[5] = tgPort5.Checked;
            port[6] = tgPort6.Checked;
            port[7] = tgPort7.Checked;
            port[8] = tgPort8.Checked;
            port[9] = tgPort9.Checked;
            port[10] = tgPort10.Checked;
            port[11] = tgPort11.Checked;
            port[12] = tgPort12.Checked;
            port[13] = tgPort13.Checked;
            port[14] = tgPort14.Checked;
            port[15] = tgPort15.Checked;
            return port;
        }
      
        #endregion
        #region TagsList
        delegate void TagUpdater();
        public void UpdateUI_Tags()
        {
            try
            {
                if (InvokeRequired)
                {
                    // We're not in the UI thread, so we need to call Invoke
                    Invoke(new TagUpdater(UpdateUI_Tags));
                    return;
                }
                bs.ResetBindings(false);
                //Clear Listview
                lvTags.Invoke(new MethodInvoker(delegate { lvTags.Items.Clear(); }));

                //Create items
                List<ListViewTagsData> copy = TagsCollections.TagsCollectionsList;
                copy = copy.OrderByDescending(t => t.RSSI).ToList();
                List<ListViewItem> items = new List<ListViewItem>();

                for (int i = 0; i < copy.Count; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = copy[i].Timestamp;
                    item.SubItems.Add(copy[i].IP);
                    item.SubItems.Add(copy[i].EPC);
                    item.SubItems.Add(copy[i].RSSI.ToString());
                    item.SubItems.Add(copy[i].Direction.ToString());
                    if (string.IsNullOrEmpty(Filter))
                    {
                        items.Add(item);
                    }
                    else
                    {
                        if (
                            copy[i].Timestamp.ToLower().Contains(Filter) ||
                            copy[i].IP.ToLower().Contains(Filter) ||
                            copy[i].EPC.ToLower().Contains(Filter) ||
                            copy[i].RSSI.ToLower().Contains(Filter) ||
                            copy[i].Direction.ToLower().Contains(Filter)
                       )
                        {
                            items.Add(item);
                        }
                    }
                   
                }
                //Populate Listview
                lvTags.Invoke(new MethodInvoker(delegate
                {
                    lvTags.Items.AddRange(items.ToArray());
                }));
            }
            catch (Exception ex) { }
        }
        delegate void AddTagDelegate(string timestamp, string ip, string epc, string rssi, string direction);
        public void AddTagToListview(string timestamp, string ip, string epc, string rssi, string direction)
        {
            if(InvokeRequired)
            {
                Invoke(new AddTagDelegate(AddTagToListview), timestamp, ip, epc, rssi, direction);
                return;
            }
            int index = -1;
            if (lvTags.Items.Count != 0)
            {
                for (int i = 0; i < lvTags.Items.Count; i++)
                {
                    string itemEpc = GetEPCFromListview(i);
                    if (itemEpc == epc)
                    {
                            index = i;
                    }
                }
            }
            if (index == -1)
            {
                ListViewItem item = new ListViewItem();
                item.Text = timestamp;
                item.SubItems.Add(ip);
                item.SubItems.Add(epc);
                item.SubItems.Add(rssi.ToString());
                item.SubItems.Add(direction.ToString());
                item.ForeColor = Color.Green;

                if (!string.IsNullOrEmpty(Filter))
                {

                    if (timestamp.ToLower().Contains(Filter) || 
                        ip.ToLower().Contains(Filter) || 
                        epc.ToLower().Contains(Filter) || 
                        rssi.ToLower().Contains(Filter) || 
                        direction.ToLower().Contains(Filter))
                    {
                       
                        lvTags.Invoke(new MethodInvoker(delegate {
                            lvTags.Items.Add(item);
                        }));
                    }
                }
                else
                {
                    lvTags.Invoke(new MethodInvoker(delegate {
                        lvTags.Items.Add(item);
                    }));
                }
             
            }
            else
            {
                lvTags.Invoke(new MethodInvoker(delegate {
                    lvTags.Items[index].Text = timestamp;
                    lvTags.Items[index].SubItems[3].Text = rssi.ToString();
                    lvTags.Items[index].SubItems[4].Text = direction.ToString();
                    lvTags.Items[index].ForeColor = Color.Green;
                }));
            }
        }
        public void RemoveTagFromList(string ip, string epc)
        {
            //Thread t = new Thread(new ThreadStart(delegate {
                int index = -1;
            if (lvTags.Items.Count != 0)
            {
                for (int i = 0; i < lvTags.Items.Count; i++)
                {
                    string itemIp = GetIPFromListview(i);
                    string itemEpc = GetEPCFromListview(i);
                    if (itemEpc == epc && itemIp == ip)
                    {
                        index = i;
                    }
                }
            }
            if (index != -1 && index < lvTags.Items.Count)
            {
                lvTags.Invoke(new MethodInvoker(delegate {
                    lvTags.Items[index].ForeColor =darkgreen;
                }));
            }
            //}));
        }
        public void RemoveTagFromList(string ip)
        {
            //Thread t = new Thread(new ThreadStart(delegate {
            List<int> index = new List<int>();
                if (lvTags.Items.Count != 0)
                {
                    for (int i = 0; i < lvTags.Items.Count; i++)
                    {
                        string itemIp = GetIPFromListview(i);
                        if (ip == itemIp)
                        {
                            index.Add(i);
                        }
                    }
                }
                if (index.Count > 0)
                {
                    lvTags.Invoke(new MethodInvoker(delegate {
                        //lvTags.Items.RemoveAt(index);
                        foreach(int i in index)
                        {
                            lvTags.Items[i].ForeColor =darkgreen;
                        }
                    }));
                }
            //}));
        }
        private void EraseTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Thread t = new Thread(new ThreadStart(delegate {
                RedTagEraser();
            //}));
        }
        delegate void RedTagEraserDelegate();
        public void RedTagEraser()
        {
            try
            {
                if (InvokeRequired)
                {
                    // We're not in the UI thread, so we need to call Invoke
                    Invoke(new RedTagEraserDelegate(RedTagEraser));
                    return;
                }
                //Clear Listview
                lvTags.Invoke(new MethodInvoker(delegate { if(lvTags.Items.Count < 1) return; }));
                int index = -1;
                List<int> todelete = new List<int>();
                if (lvTags.Items.Count != 0)
                {
                    for (int i = 0; i < lvTags.Items.Count; i++)
                    {
                        if (lvTags.Items[i].ForeColor == Color.DarkRed)
                        {
                            todelete.Add(i);
                        }
                        if (lvTags.Items[i].ForeColor ==darkgreen)
                        {
                            lvTags.Items[i].ForeColor = Color.DarkRed;
                        }
                    }
                }
                if (todelete.Count > 0)
                {
                   foreach(int i in todelete)
                    {
                        lvTags.Items.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex) { }
        }
        #endregion
        #region ToolBox
        private void btConnect_Click(object sender, EventArgs e)
        {
            HardwareManager.UpdateReader(SelectedReader, GetIPFromToolBox(), GetAliasFromToolBox(), GetModelFromToolBox(), GetPowerFromToolBox(), GetPortsFromToolBox());
            ReaderModel reader;
            HardwareManager.ReadersCollection.TryGetValue(SelectedReader, out reader);
            if (reader != null)
            {
                tbPower.Value = reader.Power;
                tbToolBoxAlias.Text = reader.Alias;
                tbToolBoxModel.Text = reader.Model;
                if (!reader.Connected)
                {
                    ChangeReaderState(SelectedReader, ReaderState.Connecting);
                    reader.Connect();
                    reader.RFID.ReaderConnected += RFID_ReaderConnected;
                    reader.RFID.ReaderDisconnected += RFID_ReaderDisconnected;
                    ConnectButtonState(false);
                }
                else
                {
                    ChangeReaderState(SelectedReader, ReaderState.Disconnected);
                    reader.Disconnect();
                    ConnectButtonState(true);
                    RemoveTagFromList(reader.IP);
                }
            }

            pnlReaderSettings.Enabled = false;
            SelectedReader = "";
        }
        private void ConnectButtonState(bool connect)
        {
            try
            {
                if (connect)
                {
                    btConnectReader.Invoke(new MethodInvoker(delegate
                    {
                        btConnectReader.Text = "Conectar";
                        btConnectReader.BackColor = ThemeColor;
                        btConnectReader.ForeColor = Color.AliceBlue;
                    }));
                }
                else
                {
                    btConnectReader.Invoke(new MethodInvoker(delegate
                    {
                        btConnectReader.Text = "Desconectar";
                        btConnectReader.BackColor = Color.Red;
                        btConnectReader.ForeColor = Color.White;
                    }));

                }
            }
            catch { }
        }
        private void RFID_ReaderDisconnected(string ip)
        {
            ChangeReaderState(ip, ReaderState.Disconnected);
            ConnectButtonState(true);
        }

        private void RFID_ReaderConnected(string ip)
        {
            ChangeReaderState(ip, ReaderState.Connected);
            ConnectButtonState(false);
        }

        #region Ports
        private void SetPort(int port)
        {
            bool[] ports = GetPortsFromToolBox();
            ports[port] = FindToogleChecked(tablePorts, "tgPort" + port);
            HardwareManager.UpdateReader(SelectedReader, GetIPFromToolBox(), GetAliasFromToolBox(), GetModelFromToolBox(), GetPowerFromToolBox(), ports);
            ReaderModel reader;
            HardwareManager.ReadersCollection.TryGetValue(SelectedReader, out reader);
            if (reader != null)
            {
                if (reader.RFID == null)
                {
                    reader = new ReaderModel(GetModelFromToolBox(), SelectedReader, GetPowerFromToolBox(), GetPortsFromToolBox(), GetAliasFromToolBox());
                }
                reader.RFID.SetPort((uint)port, FindToogleChecked(tablePorts, "tgPort" + port));
            }
            //pnlReaderSettings.Enabled = false;
            SelectedReader = "";
        }
        private CheckBox FindToogle(Control parent, string name)
        {
            foreach (CheckBox ctl in parent.Controls)
            {
                if (ctl.Name == name) return ctl;
            }
            return null;
        }
        private bool FindToogleChecked(Control parent, string name)
        {
            var control = FindToogle(parent, name);
            if (control == null) return false;
            return control.Checked;
        }
        object mylock = new object();
        private void TooglePorts(bool[] port)
        {
            tgPort0.Invoke(new MethodInvoker(delegate { tgPort0.Checked = port[0]; }));
            tgPort1.Invoke(new MethodInvoker(delegate { tgPort1.Checked = port[1]; }));
            tgPort2.Invoke(new MethodInvoker(delegate { tgPort2.Checked = port[2]; }));
            tgPort3.Invoke(new MethodInvoker(delegate { tgPort3.Checked = port[3]; }));
            tgPort4.Invoke(new MethodInvoker(delegate { tgPort4.Checked = port[4]; }));
            tgPort5.Invoke(new MethodInvoker(delegate { tgPort5.Checked = port[5]; }));
            tgPort6.Invoke(new MethodInvoker(delegate { tgPort6.Checked = port[6]; }));
            tgPort7.Invoke(new MethodInvoker(delegate { tgPort7.Checked = port[7]; }));
            tgPort8.Invoke(new MethodInvoker(delegate { tgPort8.Checked = port[8]; }));
            tgPort9.Invoke(new MethodInvoker(delegate { tgPort9.Checked = port[9]; }));
            tgPort10.Invoke(new MethodInvoker(delegate { tgPort10.Checked = port[10]; }));
            tgPort11.Invoke(new MethodInvoker(delegate { tgPort11.Checked = port[11]; }));
            tgPort12.Invoke(new MethodInvoker(delegate { tgPort12.Checked = port[12]; }));
            tgPort13.Invoke(new MethodInvoker(delegate { tgPort13.Checked = port[13]; }));
            tgPort14.Invoke(new MethodInvoker(delegate { tgPort14.Checked = port[14]; }));
            if (port.Length < 16) return;
            tgPort15.Invoke(new MethodInvoker(delegate { tgPort15.Checked = port[15]; }));
            Application.DoEvents();
            SelectedReader = tempSelectedReader;
            SelectedIndex = tempSelectedIndex;
            lvReaders.Select();
            try
            {
                lvReaders.Items[SelectedIndex].Selected = true;
            }
            catch { }
            Application.DoEvents();
        }
        private void tgPort0_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(0);
        }

        private void tgPort1_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(1);
        }

        private void tgPort2_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(2);
        }

        private void tgPort3_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(3);
        }

        private void tgPort4_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(4);
        }

        private void tgPort5_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(5);
        }

        private void tgPort6_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(6);
        }

        private void tgPort7_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(7);
        }

        private void tgPort8_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(8);
        }

        private void tgPort9_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(9);
        }

        private void tgPort10_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(10);
        }

        private void tgPort11_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(11);
        }

        private void tgPort12_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(12);
        }

        private void tgPort13_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(13);
        }

        private void tgPort14_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(14);
        }

        private void tgPort15_CheckedChanged(object sender, EventArgs e)
        {
            SetPort(15);
        }
        #endregion
        #endregion
        #endregion
        #region Print
        #region Properties
        //public NiceLabel LabelIntf;
        public string LabelFileNameBMP;
        public string ActualLabelFileName;
        public string PreviewFileName;
        public bool Result;
        //public NiceApp nice;
        public List<string> Columns = new List<string>();
        public List<string> Values = new List<string>();
        Thread PublishPrinterChange;
        public static System.Timers.Timer WebServicePrintTimer = new System.Timers.Timer(5000);
        public bool LabelReady = false;
        public bool Printing = false;
        public bool AdjustEPC = false;






        #endregion

        #endregion

        private void cbActuators_CheckedChanged(object sender, EventArgs e)
        {
            cbGPO0.Enabled = true;
            cbGPO1.Enabled = true;
        }

        private void cbSensors_CheckedChanged(object sender, EventArgs e)
        {
            ReaderModel reader;
            HardwareManager.ReadersCollection.TryGetValue(SelectedReader, out reader);
            if (reader != null)
            {
                if (reader.RFID != null)
                {
                    reader.RFID.Sensors = cbSensors.Checked;
                    if(cbSensors.Checked)
                    {
                        reader.RFID.GPI0Changed += RFID_GPI0Changed;
                        reader.RFID.GPI1Changed += RFID_GPI1Changed;
                    }
                    else
                    {
                        reader.RFID.GPI0Changed -= RFID_GPI0Changed;
                        reader.RFID.GPI1Changed -= RFID_GPI1Changed;
                    }
                }
            }
        }

        private void RFID_GPI1Changed(string ip, bool on)
        {
            if(ip == GetIPFromToolBox())
                cbGPI1.Invoke(new MethodInvoker(delegate { cbGPI1.Checked = on; }));
        }

        private void RFID_GPI0Changed(string ip, bool on)
        {
            if(ip == GetIPFromToolBox())
                cbGPI0.Invoke(new MethodInvoker(delegate { cbGPI0.Checked = on; }));
        }

        private void cbGPO0_CheckedChanged(object sender, EventArgs e)
        {
            ReaderModel reader;
            HardwareManager.ReadersCollection.TryGetValue(SelectedReader, out reader);
            if (reader != null)
            {
                if(reader.RFID != null)
                {
                    reader.RFID.SetGPO0(cbGPO0.Checked);
                }
            }
        }

        private void cbGPO1_CheckedChanged(object sender, EventArgs e)
        {
            ReaderModel reader;
            HardwareManager.ReadersCollection.TryGetValue(SelectedReader, out reader);
            if (reader != null)
            {
                if (reader.RFID != null)
                {
                    reader.RFID.SetGPO1(cbGPO1.Checked);
                }
            }
        }

        private void btAddReader_Click(object sender, EventArgs e)
        {
            if(tbAddIP.Text == "")
            {
                lbStatus.Text = "Ingrese la direciión IP del lector";
                return;
            }
            if(cbAddReaderModels.Text != "")
            {
                string ip = tbAddIP.Text;
                string alias = tbAddAlias.Text;
                string model = cbAddReaderModels.Text;
                bool[] ports = new bool[16] {true, false, false, false,
                                            false, false, false, false,
                                            false, false, false, false,
                                            false, false, false, false };
                HardwareManager.AddReader(new ReaderModel(model, ip, 300, ports, alias));
                tbAddIP.Text = "";
                tbAddAlias.Text = "";
                cbAddReaderModels.Text = "";
                lbToolbarReaders.Text = HardwareManager.ReadersCollection.Count.ToString();
                Tab.Invoke(new MethodInvoker(delegate { Tab.SelectedIndex = Tab_Readers; }));
                lbStatus.Text = "Módulo de lectura de antenas";
            }
            else
            {
                lbStatus.Text = "Seleccione un modelo de antena";
            }
        }

        private void Start_FormClosing(object sender, FormClosingEventArgs e)
        {
            HardwareManager.SaveAll();
        }

        private void cbPublishXML_CheckedChanged(object sender, EventArgs e)
        {
            TagManager.PublishXML = cbPublishXML.Checked;
        }

        private void btPublishSQL_CheckedChanged(object sender, EventArgs e)
        {
            TagManager.PublishSQL = cbPublishSQL.Checked;
        }

        private void btSaveConfXML_Click(object sender, EventArgs e)
        {
            ConfigManager.XMLPath = tbConfXMLDir.Text;
        }

        private void btSaveConfSQL_Click(object sender, EventArgs e)
        {
            ConfigManager.SQLServer = tbConfSQLServer.Text;
            ConfigManager.SQLDatabase = tbConfSQLDB.Text;
            ConfigManager.SQLUser = tbConfSQLUser.Text;
            ConfigManager.SQLPassword = tbConfSQLPass.Text;
        }

        private void btSaveConfCSV_Click(object sender, EventArgs e)
        {
            ConfigManager.CSVPath = tbConfCSVDir.Text;
        }

        private void cbPublishCSV_CheckedChanged(object sender, EventArgs e)
        {
            TagManager.PublishCSV = cbPublishCSV.Checked;
        }
        private void btPublishWebService_CheckedChanged(object sender, EventArgs e)
        {
            TagManager.PublishWebService = btPublishWebService.Checked;
        }

        private void btPublishWebSocket_CheckedChanged(object sender, EventArgs e)
        {
            TagManager.PublishWebSocket = btPublishWebSocket.Checked;
        }
        public void FillConfiguration()
        {
            tbConfCSVDir.Invoke(new MethodInvoker(delegate { tbConfCSVDir.Text = ConfigManager.CSVPath; }));
            tbConfSQLDB.Invoke(new MethodInvoker(delegate { tbConfSQLDB.Text = ConfigManager.SQLDatabase; }));
            tbConfSQLPass.Invoke(new MethodInvoker(delegate { tbConfSQLPass.Text = ConfigManager.SQLPassword; }));
            tbConfSQLServer.Invoke(new MethodInvoker(delegate { tbConfSQLServer.Text = ConfigManager.SQLServer; }));
            tbConfSQLUser.Invoke(new MethodInvoker(delegate { tbConfSQLUser.Text = ConfigManager.SQLUser; }));
            tbConfXMLDir.Invoke(new MethodInvoker(delegate { tbConfXMLDir.Text = ConfigManager.XMLPath; }));
        }

        private void tbSearchEPC_TextChanged(object sender, EventArgs e)
        {
            bool empty = false;
            tbSearchEPC.Invoke(new MethodInvoker(delegate { empty = (tbSearchEPC.Text == ""); }));
            if (empty) Filter = "";
        }

        private void btSearchTag_Click(object sender, EventArgs e)
        {
            tbSearchEPC.Invoke(new MethodInvoker(delegate { Filter = tbSearchEPC.Text; }));
            UpdateUI_Tags();
        }

        private void tbSearchEPC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)ConsoleKey.Enter)
            {
                btSearchTag_Click(null, null);
            }
        }

        private void btSaveConfWService_Click(object sender, EventArgs e)
        {
            ConfigManager.WebServiceURL = tbConfWServiceURL.Text;
        }

        private void btSaveConfWSocket_Click(object sender, EventArgs e)
        {
            string port = tbConfWSocketPort.Text;
            int portInt = 0;
            if (!int.TryParse(port, out portInt))
            {
                MessageBox.Show("El puerto debe ser un número");
                return;
            }
            ConfigManager.SocketIP = tbConfWsocketIP.Text;
            ConfigManager.SocketPort = Convert.ToInt32(tbConfWSocketPort.Text);
        }
    }
}
