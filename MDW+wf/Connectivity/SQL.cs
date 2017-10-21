using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MDW_wf.Model;
using System.Data;
using MDW_wf.Controller;

namespace MDW_wf.Connectivity
{
    public static class SQL
    {
        public static string Server = "";
        public static string DataBase = "";
        public static string User = "";
        public static string Password = "";
        public static void Write(ListViewTagsData tag)
        {
            Server = ConfigManager.SQLServer;
            DataBase = ConfigManager.SQLDatabase;
            User = ConfigManager.SQLUser;
            Password = ConfigManager.SQLPassword;
            if (Server == "" || DataBase == "" || User == "" || Password == "") return;
            using (SqlConnection openCon = new SqlConnection("Data Source=" + Server + ";Initial Catalog=" + DataBase + ";User ID=" + User + ";Password=" + Password))
            {
                string saveStaff = "INSERT into TagList (TIMESTAMP,IP,EPC,RSSI,DIRECCION) VALUES (@timestamp,@ip,@epc,@rssi,@direccion)";

                using (SqlCommand querySaveStaff = new SqlCommand(saveStaff))
                {
                    querySaveStaff.Connection = openCon;
                    querySaveStaff.Parameters.Add("@timestamp", SqlDbType.VarChar, 50).Value = tag.Timestamp;
                    querySaveStaff.Parameters.Add("@ip", SqlDbType.VarChar, 15).Value = tag.IP;
                    querySaveStaff.Parameters.Add("@epc", SqlDbType.VarChar, 24).Value = tag.EPC;
                    querySaveStaff.Parameters.Add("@rssi", SqlDbType.VarChar, 3).Value = tag.RSSI;
                    querySaveStaff.Parameters.Add("@direccion", SqlDbType.VarChar, 1).Value = tag.Direction;
                    openCon.Open();
                    querySaveStaff.ExecuteNonQuery();
                    openCon.Close();
                }
            }
        }

    }
}
