using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NiceLabel5WR;
using System.Threading;
using System.IO;
using HTKLibrary.Comunications;
using HTKLibrary.Classes.MDW;
using System.Runtime.InteropServices;

namespace MDW_Print
{
    public partial class Print : Form
    {
        public System.Timers.Timer WebServiceTimer = new System.Timers.Timer(500);
        public MDWRestClient restClient;
        public PrintSocketServer socketServer = new PrintSocketServer(504);
        public string token = "";
        public string id = "";
        public Print()
        {
            InitializeComponent();
        }
        private void Print_Load(object sender, EventArgs e)
        {
            Activate();
            token = MDW_Print.Connectivity.RestClient.getToken();
            id = MDW_Print.Connectivity.RestClient.getId();
            nice = new NiceApp();
            btOpenDefaultLabel_Click(null, null);
            FillConfiguration();
            HideTabHeaders();
            if (lbPrinters.Items.Count > 0)
            {
                lbPrinters.SelectedIndex = 0;
            }
            
            txtNum.Text = _numValue.ToString();
            stkAjusteEPC.Visible = false;
            socketServer.Start();

        }

        private void SocketServer_PrintFromSocket(PrintTag tag)
        {
            PrintFromRemote(tag);
        }

        private void HideTabHeaders()
        {
            Tab.Appearance = TabAppearance.FlatButtons;
            Tab.ItemSize = new Size(0, 1);
            Tab.SizeMode = TabSizeMode.Fixed;
        }
        public void FillConfiguration()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(FillConfiguration), new object[] { });
                return;
            }
            txtNum.Value = Program.printersConfigManager.adjust;
            tbPrefix.Text = Program.printersConfigManager.prefix;
            foreach(printer p in Program.printersConfigManager.printers)
            {
                lbPrinters.Items.Add(p.name);
                PrintersAlias.Add(p.name, p.alias);
                PrintersName.Add(p.alias, p.name);
            }
            tgWebServicePrint.Checked = Program.printersConfigManager.webService;
            tgWebSocketPrint.Checked = Program.printersConfigManager.webSocket;

            if (!string.IsNullOrEmpty(Program.printersConfigManager.file))
            {
                OpenLabel(Program.printersConfigManager.file);
            }
            

            var address = NetworkInterface
               .GetAllNetworkInterfaces()
               .Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
               .SelectMany(i => i.GetIPProperties().UnicastAddresses)
               .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
               .Select(a => a.Address.ToString())
               .ToList();
            if (address.Count > 0)
                lbToolbarIP.Invoke(new MethodInvoker(delegate { lbToolbarIP.Text = address[0]; }));

            var addresswifi = NetworkInterface
               .GetAllNetworkInterfaces()
               .Where(i => i.Name == "Wi-Fi")
               .SelectMany(i => i.GetIPProperties().UnicastAddresses)
               .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
               .Select(a => a.Address.ToString())
               .ToList();
            if (address.Count > 0)
                lbToolbarIP.Invoke(new MethodInvoker(delegate { lbToolbarIPWifi.Text = addresswifi.Count > 0 ? addresswifi[0] : "127.0.0.1"; }));

            if (tgWebServicePrint.Checked)
            {
                StartPrintingFromService();
            }
        }

        #region Print
        #region Properties
        public NiceLabel LabelIntf;
        public string LabelFileNameBMP;
        public string ActualLabelFileName;
        public string PreviewFileName;
        public bool Result;
        public NiceApp nice;
        public List<string> Columns = new List<string>();
        public List<string> Values = new List<string>();
        Thread PublishPrinterChange;
        public static System.Timers.Timer WebServicePrintTimer = new System.Timers.Timer(5000);
        public bool LabelReady = false;
        public bool Printing = false;
        public bool AdjustEPC = false;
        public DataTable PrintListData { get; set; }
        private string labelDir = "Etiqueta";
        public string LabelDir
        {
            get { return labelDir; }
            set { labelDir = value; }
        }
        #endregion

        private void btOpenLabel_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Label files (*.lbl)|*.lbl|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                OpenLabel(ofd.FileName);
            }
            Program.printersConfigManager.file = ofd.FileName;
            PrintersConfigManager.Save(Program.printersConfigManager);
        }
        private void btOpenDefaultLabel_Click(object sender, EventArgs e)
        {
            OpenLabel(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\etiqueta_base_copia.lbl");
            Thread.Sleep(200);
            OpenLabel(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\etiqueta_base.lbl");
            Program.printersConfigManager.file = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\etiqueta_base.lbl";
            PrintersConfigManager.Save(Program.printersConfigManager);
        }
        private string GetActivePrinter()
        { 
            int selected = (int)lbPrinters.Invoke(new Func<int>(() => lbPrinters.SelectedItems.Count));
            if (selected == 0) return "";
            //return (string)cbPrinters.Invoke(new Func<string>(() => cbPrinters.Text));
            return (string)lbPrinters.Invoke(new Func<string>(() => lbPrinters.SelectedItems[0].ToString()));
        }
        public Dictionary<string, string> PrintersAlias = new Dictionary<string, string>();
        public Dictionary<string, string> PrintersName = new Dictionary<string, string>();
        private string GetActivePrinterAlias()
        {
            try
            {
                return (string)tbPrinterAlias.Invoke(new Func<string>(() => tbPrinterAlias.Text));
            }
            catch (Exception)
            {
                return "";
            }
        }
        private int GetEPCAjust()
        {
            return Convert.ToInt32(txtNum.Invoke(new Func<string>(() => txtNum.Text)));
        }
        private void DefaultLabelVariableValues()
        {
            AdjustEPC = false;
            stkAjusteEPC.Visible = false;
            for (int i = 0; i < LabelIntf.Variables.Count; i++)
            {
                var Var = (WRVar)LabelIntf.Variables.Item(i);
                if (!(Var == null))
                {
                    if (LabelIntf.Variables.Item(i).Name.ToString() == "EPC")
                    {
                        Var.SetValue(new string('0', 24));
                    }
                    else if (LabelIntf.Variables.Item(i).Name.ToString() == "EPC2")
                    {
                        Var.SetValue(new string('0', 23) + "1");
                        stkAjusteEPC.Visible = true;
                        AdjustEPC = true;
                    }
                    else
                    {
                        Var.SetValue(LabelIntf.Variables.Item(i).Name);
                    }
                }
            }
        }
        private void DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception) { }
            }
        }
        private void ShowImage(string BMPfile)
        {
            Bitmap bitmap = new Bitmap(LabelFileNameBMP);
            Picture.Image = bitmap;
            Picture.Visible = true;
            bitmap = null;
        }
        private List<string> GetPrinters()
        {
            List<string> Printers = new List<string>();
            EventArgs arg = new EventArgs();
            btOpenDefaultLabel_Click(this, arg);
            string printers = LabelIntf.GetPrintersList();
            foreach (var item in printers.Split(','))
                Printers.Add(item.Replace("\"", ""));
            return Printers;
        }
        private void ShowConnectedPrinters()
        {
            string printers = LabelIntf.GetPrintersList();
            foreach (var item in printers.Split(','))
                cbPrinters.Items.Add(item.Replace("\"", ""));

            if (cbPrinters.Items.Count > 0)
            {
                cbPrinters.Text = cbPrinters.Items[0].ToString();
                for (int i = cbPrinters.Items.Count - 1; i > 0; i--)
                {
                    if (cbPrinters.Items[i].ToString().ToLower().Contains("zdesigner"))
                    {
                        cbPrinters.Text = cbPrinters.Items[i].ToString();
                        break;
                    }
                }
                LabelIntf.PrinterName = GetActivePrinter();
            }
        }
        private void OpenLabel(string LabelFileName)
        {
            CloseLabel();
            LabelIntf = nice.LabelOpenEx(LabelFileName);
            LabelFileNameBMP = LabelFileName.Replace(".lbl", ".bmp");
            DeleteFile(LabelFileNameBMP);
            DefaultLabelVariableValues();
            if (LabelIntf.GetLabelPreview(LabelFileNameBMP, 500, 250))
            {
                ShowImage(LabelFileNameBMP);
                ShowConnectedPrinters();
                PrintListData = GetPrintTable();
                PrintList.DataSource = PrintListData.AsDataView();
                LabelDir = LabelFileName;
                lbLabelName.Text = LabelFileName.Split('\\')[LabelFileName.Split('\\').Length - 1];
                LabelIntf.PrinterName = GetActivePrinter();
                btPrint.Enabled = true;
                if (tgWebServicePrint.Checked == true) { PublishPrinter(); }
                LabelReady = true;

            }
            else
            {
                Picture.Visible = true;
                btPrint.Enabled = false;
                LabelReady = false;
            }
        }
        private DataTable GetPrintTable()
        {
            DataTable table = new DataTable();
            List<string> variables = new List<string>();
            for (int i = 0; i < LabelIntf.Variables.Count; i++)
            {
                try
                {
                    if (LabelIntf.Variables.Item(i).Name.ToString() == "EPC")
                        continue;
                    if (LabelIntf.Variables.Item(i).Name.ToString() == "EPC2")
                    {
                        AdjustEPC = true;
                        continue;
                    }
                    table.Columns.Add(LabelIntf.Variables.Item(i).Name, typeof(string));
                }
                catch (Exception) { }
            }
            table.Columns.Add("Cantidad", typeof(string));

            return table;
        }
        private void CloseLabel()
        {
            if (LabelIntf != null)
            {
                DeleteFile(LabelFileNameBMP);
                LabelIntf.Free();
                LabelIntf.Free();
                LabelIntf.Free();
                LabelIntf.Free();
                Thread.Sleep(100);
                LabelIntf = null;
                LabelIntf = null;
                LabelIntf = null;
                LabelIntf = null;
                Thread.Sleep(100);
                //Picture.Image.Dispose();
                Picture.Visible = false;
                Bitmap bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\default.bmp");
                Picture.Image = bitmap;
            }
        }
        private void SetLabelVariable(string name, string value)
        {
            var Var = (WRVar)LabelIntf.Variables.FindByName(name);
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Var != null)
                    {
                        Var.SetValue(value);
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        private void btPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (LabelIntf == null) return;
                LabelIntf.PrinterName = GetActivePrinter();
                int adjust = 0;
                if (AdjustEPC)
                    adjust = GetEPCAjust();
                int total = PrintList.Rows.Count - 1;
                if (total < 1) return; //if no data return
                int send = 0;
                // go over every row
                foreach (DataGridViewRow item in PrintList.Rows)
                {
                    Columns = new List<string>();
                    Values = new List<string>();
                    string datestring = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    // go over every column
                    for (int i = 0; i < PrintList.Columns.Count - 1; i++)
                    {
                        // get column name and set for the variable name
                        string variablesName = PrintList.Columns[i].HeaderText.ToString();
                        Columns.Add(variablesName);
                        var Var = LabelIntf.Variables.FindByName(variablesName);
                        for (int k = 0; k < 4; k++)
                        {
                            Var = LabelIntf.Variables.FindByName(variablesName);
                        }
                        try
                        {
                            if (Var != null)
                            {
                                // get the variable value
                                if (item.Cells[i].Value != null)
                                {
                                    var value = item.Cells[i].Value.ToString();
                                    Var.SetValue(value);
                                    Values.Add(value);
                                }
                                else
                                {
                                    Var.SetValue("");
                                    Values.Add("");
                                }
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                    }
                    try
                    {
                        int nullvalues = 0;
                        for (int i = 0; i < PrintList.Columns.Count; i++)
                        {
                            if (item.Cells[i].Value == null)
                            {
                                nullvalues++;
                            }
                        }
                        if (nullvalues == PrintList.Columns.Count)
                        {
                            continue;
                        }

                        try
                        {
                            string strquantity = item.Cells[PrintList.Columns.Count - 1].Value.ToString();
                            int quantity = Convert.ToInt32(strquantity);
                            for (int j = 0; j < quantity; j++)
                            {
                                string prefix = tbPrefix.Text + datestring;
                                string epc = prefix + new string('0', 24 - prefix.Length - j.ToString().Length) + (j + 1).ToString();
                                var Var = (WRVar)LabelIntf.Variables.FindByName("EPC");
                                try
                                {
                                    if (Var != null)
                                    {
                                        Var.SetValue(epc);
                                    }
                                }
                                catch (Exception)
                                {
                                    continue;
                                }
                                if (Columns.Find(x => x == "EPC") != null)
                                {
                                    Values[Columns.IndexOf("EPC")] = epc;
                                }
                                else
                                {
                                    Columns.Add("EPC");
                                    Values.Add(epc);
                                }
                                if (AdjustEPC)
                                {
                                    string epc2 = prefix + new string('0', 24 - prefix.Length - j.ToString().Length) + (j + 1 + adjust).ToString();
                                    var VarEPC2 = (WRVar)LabelIntf.Variables.FindByName("EPC2");
                                    try
                                    {
                                        if (VarEPC2 != null)
                                        {
                                            VarEPC2.SetValue(epc2);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        continue;
                                    }
                                    if (Columns.Find(x => x == "EPC2") != null)
                                    {
                                        Values[Columns.IndexOf("EPC2")] = epc2;
                                    }
                                    else
                                    {
                                        Columns.Add("EPC2");
                                        Values.Add(epc2);
                                    }
                                }
                                if (LabelIntf.Print("1"))
                                {
                                    send++;
                                    //MessageBox.Show("Imprimiendo " + send + " de " + quantity);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception) { }
        }

        private void PublishPrinter()
        {
            //RestClient.PublishPrinter(true, (string)cbPrinters.Invoke(new Func<string>(() => cbPrinters.Text)), (string)cbPrinters.Invoke(new Func<string>(() => tbAlias.Text)));
            //restClient.AddPrinter(new Printer() { alias = GetActivePrinterAlias(), name = GetActivePrinter() });
            //publishPrinter();
        }
        private void PublishPrinterDelay()
        {
            Thread.Sleep(3000);
            restClient.AddPrinter(new Printer() { alias = GetActivePrinterAlias(), name = GetActivePrinter() });
            //RestClient.PublishPrinter(true, (string)cbPrinters.Invoke(new Func<string>(() => cbPrinters.Text)), (string)cbPrinters.Invoke(new Func<string>(() => tbAlias.Text)));
        }
        private void tbAlias_TextChanged(object sender, EventArgs e)
        {
            string printer = GetActivePrinter();

            PrintersAlias[printer] = (string)tbPrinterAlias.Invoke(new Func<string>(() => tbPrinterAlias.Text));
            PrintersName[PrintersAlias[printer]] = printer;
            Program.printersConfigManager.printers.Find(x => x.name == printer).alias = tbPrinterAlias.Text;
            PrintersConfigManager.Save(Program.printersConfigManager);
        }
        private void unpublishPrinters()
        {
            try
            {
                MDW_Print.Connectivity.RestClient rClientDelete = new Connectivity.RestClient(
                   Connectivity.httpVerb.DELETE,
                   Program.configManager.WebServiceURL + "api/Printers?" + string.Format("clientId={0}", id),
                   "application/json", token);

                rClientDelete.makeRequest();
            }
            catch { }
        }
        private void publishPrinter()
        {
            unpublishPrinters();

            foreach (printer printer in Program.printersConfigManager.printers)
            {
                MDW_Print.Connectivity.RestClient rClient = new Connectivity.RestClient(
               Connectivity.httpVerb.POST,
               Program.configManager.WebServiceURL + "api/Printers?",
               string.Format("clientId={0}&printer={1}", id, printer.alias),
               "application/json", token);

                rClient.makeRequest();
            }
            
            
            //if ((string)cbPrinters.Invoke(new Func<string>(() => cbPrinters.Text)) == "") return;
            //if (restClient == null) return;
            //if (restClient.mdw_client_id == "") return;
            //List<Printer> printers = restClient.GetPrinters();
            //Printer printer = new Printer() { alias = GetActivePrinterAlias(), name = GetActivePrinter() };
            //if (printers.Contains(printer)) return;
            //if (PublishPrinterChange == null)
            //{
            //    PublishPrinterChange = new Thread(new ThreadStart(PublishPrinterDelay));
            //}
            //else
            //{
            //    PublishPrinterChange.Abort();
            //    PublishPrinterChange.Join();
            //    PublishPrinterChange = new Thread(new ThreadStart(PublishPrinterDelay));
            //}
            //PublishPrinterChange.Start();
        }
        #region NumericUpDown
        private int _numValue = 0;
        private MDWClient mdw;

        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                txtNum.Text = value.ToString();
            }
        }
        private void cmdUp_Click(object sender, EventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, EventArgs e)
        {
            NumValue--;
        }

        private void txtNum_TextChanged(object sender, EventArgs e)
        {
            if (txtNum == null)
            {
                return;
            }

            if (!int.TryParse(txtNum.Text, out _numValue))
                txtNum.Text = _numValue.ToString();
        }
        #endregion
        #region Rest
        private void tgWebServicePrint_Checked(object sender, EventArgs e)
        {
            if (tgWebServicePrint.Checked == true)
            {
                WebServicePrintTimer.Elapsed += WebServicePrintTimer_Elapsed;
                WebServicePrintTimer.Start();
            }
            else
            {
                WebServicePrintTimer.Elapsed -= WebServicePrintTimer_Elapsed;
                WebServicePrintTimer.Stop();
            }
        }

        private void WebServicePrintTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (LabelIntf == null) return;
                if (!LabelReady) { return; }
                if (!Printing)
                {
                    Printing = true;
                    //MessageCenter.Print("Imprimiendo desde: " + ConfigManager.UserURL);
                    //var pendingList = RestClient.GetPrintPendingList();
                    //RestClient.DeleteFromPrintPendingList(pendingList);

                    //foreach (WebServiceLabel label in pendingList)
                    //{
                    //    if (label.Printer == RestClient.lastPrinterAlias)
                    //    {
                    //        PrintFromRest(label);
                    //    }
                    //}
                    //MessageCenter.Print("");
                    Printing = false;
                }


            }
            catch (Exception)
            {

            }
        }

        private void PrintFromRemote(PrintTag tag)
        {
            try
            {
                string epcfromlabel = "";
                string propfromlabel = "";
                while (epcfromlabel == "")
                {
                    try
                    {
                        epcfromlabel = LabelIntf.Variables.FindByName("EPC").GetValue();
                        while (epcfromlabel != tag.epc)
                        {
                            SetLabelVariable("EPC", tag.epc);
                            epcfromlabel = LabelIntf.Variables.FindByName("EPC").GetValue();
                            Thread.Sleep(100);
                        }
                    }
                    catch (Exception) { }
                    foreach (var property in tag.fields)
                    {
                        try
                        {
                            propfromlabel = LabelIntf.Variables.FindByName(property.Key).GetValue();
                            while (propfromlabel != property.Value)
                            {
                                SetLabelVariable(property.Key, property.Value);
                                propfromlabel = LabelIntf.Variables.FindByName(property.Key).GetValue();
                                Thread.Sleep(100);
                            }
                        }
                        catch (Exception) { }
                    }
                }
                if (tag.fields.ContainsKey("printer"))
                {
                    string printer = tag.fields["printer"];
                    if (PrintersName.ContainsKey(printer))
                    {
                        LabelIntf.PrinterName = PrintersName[printer];
                        LabelIntf.Print("1");
                    }
                    else
                    {
                        LabelIntf.PrinterName = printer;
                        LabelIntf.Print("1");
                    }
                }
                if (tag.fields.ContainsKey("Printer"))
                {
                    string printer = tag.fields["Printer"];
                    if (PrintersName.ContainsKey(printer))
                    {
                        LabelIntf.PrinterName = PrintersName[printer];
                        LabelIntf.Print("1");
                    }
                    else
                    {
                        LabelIntf.PrinterName = printer;
                        LabelIntf.Print("1");
                    }
                   
                }
                else
                {
                    LabelIntf.PrinterName = GetActivePrinter();
                    LabelIntf.Print("1");
                }
                

            }
            catch (Exception) { }
        }
        #endregion

        #endregion

        private void tgWebServicePrint_CheckedChanged(object sender, EventArgs e)
        {
            if (tgWebServicePrint.Checked)
            {
                //Activate();
                //if (!Program.configManager.Activated)
                //{
                //    if (Program.configManager.User == "" || Program.configManager.Password == "")
                //    {
                //        MessageBox.Show("Por favor ingrese las credenciales para el web service");
                //        using (Login login = new Login())
                //        {
                //            login.ShowDialog();
                //        }
                //    }
                //    Activate();
                //    if (!Program.configManager.Activated)
                //    {
                //        StartPrintingFromService();
                //    }
                //}
                //else
                //{
                    StartPrintingFromService();
                //}

                //WebServicePrintTimer_Elapsed1(null, null);
            }
            else
            {
                WebServicePrintTimer.Elapsed -= WebServicePrintTimer_Elapsed1;
                WebServicePrintTimer.Stop();
            }
            Program.printersConfigManager.webService = tgWebServicePrint.Checked;
            PrintersConfigManager.Save(Program.printersConfigManager);
        }
        private void StartPrintingFromService()
        {
            publishPrinter();
            

            //var tag = restClient.PullTagFromPrinterPrintQueue(GetActivePrinterAlias());
            MDW_Print.Connectivity.RestClient rClient = new Connectivity.RestClient(
                Connectivity.httpVerb.GET,
                Program.configManager.WebServiceURL + "api/PullOne?client_id=" + id,
                "application/json", token);

            string response = rClient.makeRequest();
            if (response != "null")
            {
                PrintTag tag = rClient.Deserialize<PrintTag>(response);


                if (tag != null && tag.epc != null)
                {

                    string printer = "";
                    if (tag.fields.ContainsKey("printer"))
                    {
                        printer = tag.fields["printer"];
                        if (PrintersName.ContainsKey(printer))
                            printer = PrintersName[printer];
                        PrintTag(tag, printer);
                    }
                    else if (tag.fields.ContainsKey("Printer"))
                    {
                        printer = tag.fields["Printer"];
                        if (PrintersName.ContainsKey(printer))
                            printer = PrintersName[printer];
                        PrintTag(tag, printer);
                    }
                    else
                    {
                        PrintTag(tag);
                    }

                }
            }

            WebServicePrintTimer.Elapsed += WebServicePrintTimer_Elapsed1;
            WebServicePrintTimer.Start();
        }
        private void Activate()
        {
            ////mdw = new HTKLibrary.Classes.MDW.MDWClient(Program.configManager.User, Program.configManager.Password, Program.configManager.MacAddress);
            //mdw = new HTKLibrary.Classes.MDW.MDWClient("middleware@htk-id.com", "Middleware2016!", Program.configManager.MacAddress);
            //restClient = new MDWRestClient(Program.mdwEmail, Program.mdwPwd, Program.configManager.WebServiceURL, mdw);
            ////MDWRestClient client = new MDWRestClient("middleware@htk-id.com", "Middleware2016!", "http://localhost:81", mdw);
            //string token = restClient.LoginRestClient();
            //if (token != "")
            //{
            //    Program.configManager.Token = token;
            //    var id = restClient.LoginMDWClient();
            //    if (id != "")
            //    {
            //        restClient.mdw_client_id = id;
            //        Program.configManager.Activated = true;
            //        Program.configManager.UserId = id;
            //    }
            //}
            //else
            //    Program.configManager.Activated = false;
        }
        private void WebServicePrintTimer_Elapsed1(object sender, System.Timers.ElapsedEventArgs e)
        {
            //var tag = restClient.PullTagFromPrinterPrintQueue(GetActivePrinterAlias());
            MDW_Print.Connectivity.RestClient rClient = new Connectivity.RestClient(Connectivity.httpVerb.GET,
               Program.configManager.WebServiceURL + "api/PullOne?client_id=" + id,
               "application/json", token);

            string response = rClient.makeRequest();
            if (response == "null") return;

            PrintTag tag = rClient.Deserialize<PrintTag>(response);
            if (tag != null && tag.epc != null)
            {
                PrintTag(tag);
            }
        }
        private void PrintTag(PrintTag tag)
        {
            try
            {
                if (LabelIntf == null) return;
                LabelIntf.PrinterName = GetActivePrinter();

                List<string> tagProperties = new List<string>();
                foreach (var item in tag.fields.Keys)
                {
                    if (item.ToLower() == "printer")
                        LabelIntf.PrinterName = Program.printersConfigManager.printers.Find(x => x.alias == tag.fields[item]).name;
                    else
                        tagProperties.Add(item);
                }
                
                int adjust = 0;
                if (AdjustEPC)
                    adjust = GetEPCAjust();
                int send = 0;
                Columns = new List<string>();
                Values = new List<string>();
                string datestring = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                // go over every column
                for (int i = 0; i < tagProperties.Count - 1; i++)
                {
                    // get column name and set for the variable name
                    string variablesName = tagProperties[i];
                    Columns.Add(variablesName);
                    var Var = LabelIntf.Variables.FindByName(variablesName);
                    for (int k = 0; k < 4; k++)
                    {
                        Var = LabelIntf.Variables.FindByName(variablesName);
                    }
                    try
                    {
                        if (Var != null)
                        {
                            // get the variable value
                            if (tag.fields[tagProperties[i]] != null)
                            {
                                var value = tag.fields[tagProperties[i]];
                                Var.SetValue(value);
                                Values.Add(value);
                            }
                            else
                            {
                                Var.SetValue("");
                                Values.Add("");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
                try
                {
                    int quantity = 1;
                    for (int j = 0; j < quantity; j++)
                    {
                        var Var = (WRVar)LabelIntf.Variables.FindByName("EPC");
                        try
                        {
                            if (Var != null)
                            {
                                Var.SetValue(tag.epc);
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        if (Columns.Find(x => x == "EPC") != null)
                        {
                            Values[Columns.IndexOf("EPC")] = tag.epc;
                        }
                        else
                        {
                            Columns.Add("EPC");
                            Values.Add(tag.epc);
                        }
                        if (AdjustEPC)
                        {
                            int value = Int32.Parse(tag.epc, System.Globalization.NumberStyles.HexNumber);
                            int value2 = value + 1;
                            string epc2 = string.Format("{0:X2}", value2);
                            var VarEPC2 = (WRVar)LabelIntf.Variables.FindByName("EPC2");
                            try
                            {
                                if (VarEPC2 != null)
                                {
                                    VarEPC2.SetValue(epc2);
                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            if (Columns.Find(x => x == "EPC2") != null)
                            {
                                Values[Columns.IndexOf("EPC2")] = epc2;
                            }
                            else
                            {
                                Columns.Add("EPC2");
                                Values.Add(epc2);
                            }
                        }
                        if (LabelIntf.Print("1"))
                        {
                            send++;
                            //MessageBox.Show("Imprimiendo " + send + " de " + quantity);
                        }
                    }

                }
                catch (Exception)
                {
                }
            }
            catch (Exception) { }
        }
        private void PrintTag(PrintTag tag, string printer)
        {
            try
            {
                List<string> tagProperties = new List<string>();
                foreach (var item in tag.fields.Keys)
                {
                    tagProperties.Add(item);
                }
                if (LabelIntf == null) return;
                LabelIntf.PrinterName = printer;
                int adjust = 0;
                if (AdjustEPC)
                    adjust = GetEPCAjust();
                int send = 0;
                Columns = new List<string>();
                Values = new List<string>();
                string datestring = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                // go over every column
                for (int i = 0; i < tagProperties.Count - 1; i++)
                {
                    // get column name and set for the variable name
                    string variablesName = tagProperties[i];
                    Columns.Add(variablesName);
                    var Var = LabelIntf.Variables.FindByName(variablesName);
                    for (int k = 0; k < 4; k++)
                    {
                        Var = LabelIntf.Variables.FindByName(variablesName);
                    }
                    try
                    {
                        if (Var != null)
                        {
                            // get the variable value
                            if (tag.fields[tagProperties[i]] != null)
                            {
                                var value = tag.fields[tagProperties[i]];
                                Var.SetValue(value);
                                Values.Add(value);
                            }
                            else
                            {
                                Var.SetValue("");
                                Values.Add("");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
                try
                {
                    int quantity = 1;
                    for (int j = 0; j < quantity; j++)
                    {
                        var Var = (WRVar)LabelIntf.Variables.FindByName("EPC");
                        try
                        {
                            if (Var != null)
                            {
                                Var.SetValue(tag.epc);
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        if (Columns.Find(x => x == "EPC") != null)
                        {
                            Values[Columns.IndexOf("EPC")] = tag.epc;
                        }
                        else
                        {
                            Columns.Add("EPC");
                            Values.Add(tag.epc);
                        }
                        if (AdjustEPC)
                        {
                            int value = Int32.Parse(tag.epc, System.Globalization.NumberStyles.HexNumber);
                            int value2 = value + 1;
                            string epc2 = string.Format("{0:X2}", value2);
                            var VarEPC2 = (WRVar)LabelIntf.Variables.FindByName("EPC2");
                            try
                            {
                                if (VarEPC2 != null)
                                {
                                    VarEPC2.SetValue(epc2);
                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            if (Columns.Find(x => x == "EPC2") != null)
                            {
                                Values[Columns.IndexOf("EPC2")] = epc2;
                            }
                            else
                            {
                                Columns.Add("EPC2");
                                Values.Add(epc2);
                            }
                        }
                        if (LabelIntf.Print("1"))
                        {
                            send++;
                            //MessageBox.Show("Imprimiendo " + send + " de " + quantity);
                        }
                    }

                }
                catch (Exception)
                {
                }
            }
            catch (Exception) { }
        }
        private void tgWebSocketPrint_CheckedChanged(object sender, EventArgs e)
        {
            if(tgWebSocketPrint.Checked)
            {
                socketServer.PrintFromSocket += SocketServer_PrintFromSocket;
            }
            else
            {
                socketServer.PrintFromSocket -= SocketServer_PrintFromSocket;
            }
            Program.printersConfigManager.webSocket = tgWebSocketPrint.Checked;
            PrintersConfigManager.Save(Program.printersConfigManager);
        }

        private void btAddPrinter_Click(object sender, EventArgs e)
        {
            foreach (var item in lbPrinters.Items)
            {
                if (item.ToString() == cbPrinters.Text)
                    return;
            }
            if (!string.IsNullOrEmpty(cbPrinters.Text))
                lbPrinters.Items.Add(cbPrinters.Text);
            Program.printersConfigManager.printers.Add(new printer() { alias = "", name = cbPrinters.Text });
            PrintersConfigManager.Save(Program.printersConfigManager);
        }

        private void lbPrinters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPrinters.Items.Count <1)
            {
                return;
            }
            if (lbPrinters.SelectedItems.Count < 1)
            {
                lbPrinters.SelectedIndex = 0;
            }
            btPrint.Enabled = lbPrinters.SelectedIndices.Count > 0;
            if (PrintersAlias.ContainsKey(lbPrinters.SelectedItems[0].ToString()))
            {
                UpdateAliasTextBox(PrintersAlias[lbPrinters.SelectedItems[0].ToString()]);
            }
            else
            {
                UpdateAliasTextBox("");
            }
        }

        private void UpdateAliasTextBox(string v)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateAliasTextBox), new object[] { v });
                return;
            }
            tbPrinterAlias.Text = v;
        }

        private void tbPrefix_TextChanged(object sender, EventArgs e)
        {
            Program.printersConfigManager.prefix = tbPrefix.Text;
            PrintersConfigManager.Save(Program.printersConfigManager);
        }

        private void btQuitar_Click(object sender, EventArgs e)
        {
            if (lbPrinters.SelectedItems.Count >0)
            {
                var name = lbPrinters.SelectedItems[0].ToString();
                PrintersName.Remove(PrintersAlias[name]);
                PrintersAlias.Remove(name);
                Program.printersConfigManager.printers.RemoveAll(p => p.name == name);
                lbPrinters.Items.RemoveAt(lbPrinters.SelectedIndex);
                if (lbPrinters.Items.Count > 0)
                {
                    lbPrinters.SelectedIndex = 0;
                }
            }
            if (lbPrinters.Items.Count < 1)
            {
                btPrint.Enabled = false;
                UpdateAliasTextBox("");
            }
            PrintersConfigManager.Save(Program.printersConfigManager);
            unpublishPrinters();
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        public void DoMouseClick(uint X, uint Y)
        {
            //Call the imported function with the cursor's current position
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }
        private void lbPrinters_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DoMouseClick((uint)e.X, (uint)e.Y);
            }
        }

        private void Print_FormClosing(object sender, FormClosingEventArgs e)
        {
            unpublishPrinters();
        }
    }
}
