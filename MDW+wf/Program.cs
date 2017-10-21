using HTKLibrary.Classes.MDW;
using HTKLibrary.Comunications;
using MDW_wf.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MDW_wf
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigManager.MacAddress = HTKLibrary.Readers.Reader.GetPCMacAddress();
            ConfigManager.ReadConfig();
            //Application.Run(new Start());
            int count = 0;
            try
            {
                using (StreamReader file = new StreamReader(ConfigManager.AppPath + "\\tnuoc.c"))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        count = Convert.ToInt32(line);
                        break;
                    }
                }
            }
            catch { }
            if (count < 30)
            {
                try
                {
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
                        }
                    }
                }
                catch { }
            }
            else
            {
                Activate();
                while(!ConfigManager.Activated)
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
                        }
                    }
            }
            try
            {

                using (StreamWriter writer = new StreamWriter(ConfigManager.AppPath + "\\tnuoc.c"))
                {
                    writer.WriteLine((count + 1).ToString());
                }
                ConfigManager.Save();
            }
            catch { }

        }
        static void Activate()
        {
            MDWClient mdw = new MDWClient(ConfigManager.User, ConfigManager.Password, ConfigManager.MacAddress);
            MDWRestClient client = new MDWRestClient(ConfigManager.User, ConfigManager.Password, "http://webservice.assetsapp.com/mdw", mdw);
            //MDWRestClient client = new MDWRestClient(ConfigManager.User, ConfigManager.Password, "http://localhost:58682", mdw);
            client.LoginRestClient();
            var id = client.LoginMDWClient();
            if (id != "") ConfigManager.Activated = true;
        }
    }
}
