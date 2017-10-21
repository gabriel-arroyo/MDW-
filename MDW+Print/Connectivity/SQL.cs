using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace MDW_Print.Connectivity
{
    public static class SQL
    {
        public static string Server = "";
        public static string DataBase = "";
        public static string User = "";
        public static string Password = "";

        public static void TasteDatabase()
        {
            if (!DatabaseExists())
            {
                CreateDatabase();
            }
            if (!ExistsTable())
            {
                CreateTable();
            }
        }
        public static void Write(HTKLibrary.Classes.MDW.Tag tag)
        {
           
            try
            {
                Server = Program.configManager.SQLServer;
                DataBase = Program.configManager.SQLDatabase;
                User = Program.configManager.SQLUser;
                Password = Program.configManager.SQLPassword;
                if (Server == "" || DataBase == "" || User == "" || Password == "") return;
                using (SqlConnection openCon = new SqlConnection("Data Source=" + Server + ";Initial Catalog=" + DataBase + ";User ID=" + User + ";Password=" + Password))
                {
                    string saveStaff = "INSERT into TagList (TIMESTAMP,IP,EPC,RSSI,DIRECCION) VALUES (@timestamp,@ip,@epc,@rssi,@direccion)";

                    using (SqlCommand querySaveStaff = new SqlCommand(saveStaff))
                    {
                        querySaveStaff.Connection = openCon;
                        querySaveStaff.Parameters.Add("@timestamp", SqlDbType.VarChar, 50).Value = tag.timestamp;
                        querySaveStaff.Parameters.Add("@ip", SqlDbType.VarChar, 15).Value = tag.ip;
                        querySaveStaff.Parameters.Add("@epc", SqlDbType.VarChar, 24).Value = tag.epc;
                        querySaveStaff.Parameters.Add("@rssi", SqlDbType.VarChar, 4).Value = tag.rssi;
                        querySaveStaff.Parameters.Add("@direccion", SqlDbType.VarChar, 1).Value = tag.direction;
                        openCon.Open();
                        querySaveStaff.ExecuteNonQuery();
                        openCon.Close();
                    }
                }
            }
            catch { Logger.WriteLog("Error de escritura SQL"); }
          
        }
        public static bool DatabaseExists()
        {
            Server = Program.configManager.SQLServer;
            DataBase = Program.configManager.SQLDatabase;
            User = Program.configManager.SQLUser;
            Password = Program.configManager.SQLPassword;
            string connectionString = "Data Source=" + Server + ";User ID=" + User + ";Password=" + Password;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT db_id('{DataBase}')", connection))
                {
                    connection.Open();
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }
        public static bool ExistsTable()
        {
            Server = Program.configManager.SQLServer;
            DataBase = Program.configManager.SQLDatabase;
            User = Program.configManager.SQLUser;
            Password = Program.configManager.SQLPassword;
            SqlConnection SC = new SqlConnection("Data Source="+Server+";Initial Catalog="+DataBase+";Integrated Security=True");

            string cmdText = @"IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
                       WHERE TABLE_NAME='TagList') SELECT 1 ELSE SELECT 0";
            SC.Open();
            SqlCommand TableCheck = new SqlCommand(cmdText, SC);
            int x = Convert.ToInt32(TableCheck.ExecuteScalar());
            if (x == 1)
                return true;
            else
                return false;
            SC.Close();
        }

        public static void CreateTable()
        {
            string tableName = "TagList";
            string[] columnNames = new string[5] { "TIMESTAMP", "IP", "EPC", "RSSI", "DIRECCION" } ;
            string[] columnTypes = new string[5] { "varchar(50)", "varchar(15)", "varchar(24)", "varchar(4)", "varchar(1)" };
            Server = Program.configManager.SQLServer;
            DataBase = Program.configManager.SQLDatabase;
            User = Program.configManager.SQLUser;
            Password = Program.configManager.SQLPassword;
            SqlConnection sqlConn = new SqlConnection("Data Source=" + Server + ";Initial Catalog=" + DataBase + ";Integrated Security=True");
            StringBuilder query = new StringBuilder();
            query.Append("CREATE TABLE ");
            query.Append(tableName);
            query.Append(" ( ");

            for (int i = 0; i < columnNames.Length; i++)
            {
                query.Append(columnNames[i]);
                query.Append(" ");
                query.Append(columnTypes[i]);
                query.Append(", ");
            }

            if (columnNames.Length > 1) { query.Length -= 2; }  //Remove trailing ", "
            query.Append(")");
            sqlConn.Open();
            SqlCommand sqlQuery = new SqlCommand(query.ToString(), sqlConn);
            SqlDataReader reader = sqlQuery.ExecuteReader();
        }

        public static void CreateDatabase()
        {
            Server = Program.configManager.SQLServer;
            DataBase = Program.configManager.SQLDatabase;
            User = Program.configManager.SQLUser;
            Password = Program.configManager.SQLPassword;
            if (Server == "" || DataBase == "" || User == "" || Password == "") return;
            string connectionString = "Data Source=" + Server + ";Initial Catalog=" + DataBase + ";User ID=" + User + ";Password=" + Password;

            string path = GetSQLPath();

            string str = "CREATE DATABASE " + DataBase + " ON PRIMARY " +
                   "(NAME = " + DataBase + "_Data, " +
                   "FILENAME = '" + path + DataBase + ".mdf', " +
                   "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
                   "LOG ON (NAME = " + DataBase + "_Log, " +
                   "FILENAME = '" + path + DataBase + "Log.ldf', " +
                   "SIZE = 1MB, " +
                   "MAXSIZE = 5MB, " +
                   "FILEGROWTH = 10%)";

            SqlConnection myConn = new SqlConnection("Server="+Server+";Integrated security=SSPI;database=master");
            SqlCommand myCommand = new SqlCommand(str, myConn);
            try
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
                //MessageBox.Show("DataBase is Created Successfully", "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }

        public static string GetSQLPath()
        {
            string path = "";
            using (RegistryKey sqlServerKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server"))
            {
                foreach (string subKeyName in sqlServerKey.GetSubKeyNames())
                {
                    string[] parts = subKeyName.Split('.');
                    if (subKeyName.StartsWith("MSSQL") && parts[1].Contains("SQL"))
                    {
                        using (RegistryKey instanceKey = sqlServerKey.OpenSubKey(subKeyName))
                        {
                            string instanceName = instanceKey.GetValue("").ToString();

                            if (instanceName.Contains("SQL"))//say
                            {
                                path = instanceKey.OpenSubKey(@"Setup").GetValue("SQLBinRoot").ToString();
                                path = path.Replace("Binn", "");
                                path = Path.Combine(path, "DATA\\");
                                return path;
                            }
                        }
                    }
                }
            }
            return path;
        }
    }
}
