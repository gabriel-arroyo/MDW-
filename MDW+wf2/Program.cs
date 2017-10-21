using HTKLibrary.Classes.MDW;
using HTKLibrary.Comunications;
using MDW_wf.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CSLibrary;
using CSLibrary.Constants;
using CSLibrary.Structures;
using CSLibrary.Diagnostics;
using HTKLibrary.Readers;
using System.Collections;
using MDW_wf.Model;

namespace MDW_wf
{
    static class Program
    {
        public static ConfigManager configManager;
        public static PrintersConfigManager printersConfigManager;
        public static List<HighLevelInterface> CS203List = new List<HighLevelInterface>();
        public static List<HTKLibrary.Readers.CS101> CS101List = new List<CS101>();
        public static List<VirtualReader> VRList = new List<VirtualReader>();
        public static List<SlaveReader> SlaveList = new List<SlaveReader>();
        public static List<ReaderModel> Readers = new List<ReaderModel>();
        public static List<Model.Printer> Printers = new List<Model.Printer>();
        public static string applicationSettings = "application.config";
        public static appSettings appSetting = new appSettings();
        public static string mdwEmail = "middleware@htk-id.com";
        public static string mdwPwd = "Middleware2016!";
        //public static string mdwURL = "http://webservice.assetsapp.com/mdw";
        public static MDWClient mdw;
        public static MDWRestClient restClient;
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            configManager = ConfigManager.ReadConfig();
            printersConfigManager = PrintersConfigManager.ReadConfig();
            Readers = ConfigManager.LoadReaders();
            mdw = new MDWClient(configManager.User, configManager.Password, configManager.MacAddress);
            restClient = new MDWRestClient(mdwEmail, mdwPwd, configManager.WebServiceURL, mdw);
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //Application.Run(new Start());
            int count = 0;
            try
            {
                Activate();
                using (StreamReader file = new StreamReader(path + "\\tnuoc.c"))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        count = Convert.ToInt32(line);
                        break;
                    }
                }
            }
            catch(Exception extnuouc)
            {
                Connectivity.Logger.WriteLog(extnuouc.Message);
            }
            if (count == 0) configManager.firstUse = true;
            else configManager.firstUse = false;
            if (count < 15 || configManager.Activated)
            {
                try
                {
                    if (!configManager.Activated)
                    {
                        using (Login login = new Login())
                        {
                            login.ShowDialog();
                            Activate();
                        }
                        if (configManager.Activated)
                        {
                            MessageBox.Show("Su copia de MDW+ ha sido activada.");
                        }
                        else
                        {
                            MessageBox.Show("No se pudo activar.");
                        }
                    }
                    using (Start start = new Start())
                    {
                        try
                        {
                            start.ShowDialog();
                            try
                            {

                                using (StreamWriter writer = new StreamWriter(path + "\\tnuoc.c"))
                                {
                                    writer.WriteLine((count + 1).ToString());
                                }
                                ConfigManager.Save(configManager);
                                ConfigManager.SaveReaders(Readers);
                            }
                            catch { }
                        }
                        catch (Exception ex)
                        {
                            try
                            {

                                using (StreamWriter writer = new StreamWriter(path + "\\tnuoc.c"))
                                {
                                    writer.WriteLine((count + 1).ToString());
                                }
                                ConfigManager.Save(configManager);
                                ConfigManager.SaveReaders(Readers);
                            }
                            catch { }
                            MessageBox.Show(ex.Message);
                            start.Close();
                            start.Dispose();
                            Environment.Exit(0);
                        }

                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
            {
                if(!GetLicence())
                {
                    Activate();
                }
                while(!configManager.Activated)
                {
                    MessageBox.Show("El periodo de prueba ha caducado");
                    using(Login login = new Login())
                    {
                        login.ShowDialog();
                        Activate();
                    }
                }

                  using (Start start = new Start())
                    {
                        try
                        {
                            start.ShowDialog();
                        }
                        catch(Exception)
                        {
                            start.Close();
                            start.Dispose();
                            Environment.Exit(0);
                    }
                    }
            }
            //try
            //{

            //    using (StreamWriter writer = new StreamWriter(path + "\\tnuoc.c"))
            //    {
            //        writer.WriteLine((count + 1).ToString());
            //    }
            //    ConfigManager.Save(configManager);
            //    ConfigManager.SaveReaders(Readers);
            //}
            //catch { }
            Environment.Exit(0);

        }
        class TokenClass
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
            public string userName { get; set; }
        }
        public static void Activate()
        {
            //Register
            configManager.Activated = false;
            //var client = new RestSharp.RestClient("http://localhost:50893/");
            var client = new RestSharp.RestClient(Program.configManager.WebServiceURL);
            var requestRegister = new RestSharp.RestRequest("api/Account/Register", RestSharp.Method.POST);
            requestRegister.AddParameter("UserName", "middleware@htk-id.com");
            requestRegister.AddParameter("Email", "middleware@htk-id.com");
            requestRegister.AddParameter("Password", "Middleware2016!");
            requestRegister.AddParameter("ConfirmPassword", "Middleware2016!");
            requestRegister.AddHeader("Content-Type", "application/json");
            RestSharp.IRestResponse responseRegister = client.Execute(requestRegister);
            var contentRegister = responseRegister.Content;
            var statusRegister = responseRegister.StatusDescription;
            if (contentRegister.Contains("is already taken"))
                statusRegister = "OK";
            if (statusRegister != "OK")
                return;
            //Token
            var requestToken = new RestSharp.RestRequest("Token", RestSharp.Method.POST);
            requestToken.AddParameter("UserName", "middleware@htk-id.com");
            requestToken.AddParameter("Password", "Middleware2016!");
            requestToken.AddParameter("grant_type","password");
            requestToken.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            RestSharp.IRestResponse responseToken = client.Execute(requestToken);
            var contentToken = responseToken.Content;
            TokenClass responseToken2 = client.Execute<TokenClass>(requestToken).Data;
            var token = responseToken2.access_token;
            var statusToken = responseToken.StatusDescription;
            if (statusToken != "OK" || token == null || token == "")
                return;
            //License
            var requestLicense = new RestSharp.RestRequest("api/MDWAccount/Login?name={name}&password={password}&key={key}", RestSharp.Method.GET);
            requestLicense.AddUrlSegment("name", configManager.User);
            requestLicense.AddUrlSegment("password", configManager.Password);
            requestLicense.AddUrlSegment("key", "0000");
            requestLicense.AddParameter("UserName", "middleware@htk-id.com");
            requestLicense.AddParameter("Password", "Middleware2016!");
            requestLicense.AddParameter("grant_type", "password");
            requestLicense.AddHeader("Content-Type", "application/json");
            requestLicense.AddHeader("Authorization", "Bearer " + token);
            RestSharp.IRestResponse responseLicense = client.Execute(requestLicense);
            var id = responseLicense.Content;
            var statusLicense = responseLicense.StatusDescription;
            if (statusLicense != "OK" || id == null || id == "")
                return;
            id = id.Replace("\"", "");

            configManager.Activated = true;
            configManager.UserId = id;

            try
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                using (StreamWriter writer = new StreamWriter(path + "\\tnuoc.c"))
                {
                    writer.WriteLine("1");
                }
                using (StreamWriter writer = new StreamWriter(path + "\\token.txt"))
                {
                    writer.WriteLine(token);
                }
                using (StreamWriter writer = new StreamWriter(path + "\\id.txt"))
                {
                    writer.WriteLine(id);
                }
                ConfigManager.Save(configManager);
            }
            catch { }
            configManager.firstUse = true;
        }
        static bool GetLicence()
        {
            //License
            configManager.Activated = false;
            //var client = new RestSharp.RestClient("http://localhost:50893/");
            var client = new RestSharp.RestClient(Program.configManager.WebServiceURL);
            var requestLicense = new RestSharp.RestRequest("api/MDWAccount/Login?name={name}&password={password}&key={key}", RestSharp.Method.GET);
            requestLicense.AddUrlSegment("name", configManager.User);
            requestLicense.AddUrlSegment("password", configManager.Password);
            requestLicense.AddUrlSegment("key", "0000");
            RestSharp.IRestResponse responseLicense = client.Execute(requestLicense);
            var id = responseLicense.Content;
            var statusLicense = responseLicense.StatusDescription;
            if (statusLicense != "OK" || id == null || id == "")
                return false;
            id = id.Replace("\"", "");

            configManager.Activated = true;
            configManager.UserId = id;

            try
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                using (StreamWriter writer = new StreamWriter(path + "\\tnuoc.c"))
                {
                    writer.WriteLine("0");
                }
                ConfigManager.Save(configManager);
            }
            catch { }

            return true;
        }
        //Legacy
        //static void Activate()
        //{
        //    mdw = new MDWClient(configManager.User, configManager.Password, configManager.MacAddress);
        //    restClient = new MDWRestClient(mdwEmail, mdwPwd, mdwURL, mdw);
        //    //restClient = new MDWRestClient(configManager.User, configManager.Password, mdwURL, mdw);
        //    //MDWRestClient client = new MDWRestClient("middleware@htk-id.com", "Middleware2016!", "http://localhost:81", mdw);
        //    string token = restClient.LoginRestClient();
        //    if (token != "")
        //    {
        //        configManager.Token = token;

            //        var id = restClient.LoginMDWClient();
            //        if (id != "")
            //        {
            //            restClient.mdw_client_id = id;
            //            configManager.Activated = true;
            //            configManager.UserId = id;
            //            try
            //            {
            //                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //                using (StreamWriter writer = new StreamWriter(path + "\\tnuoc.c"))
            //                {
            //                    writer.WriteLine("0");
            //                }
            //                ConfigManager.Save(configManager);
            //            }
            //            catch { }
            //        }

            //    }
            //    else
            //        configManager.Activated = false;
            //}

        }
}
