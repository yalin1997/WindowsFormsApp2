using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace WindowsFormsApp2
{

    class eegDB
    {
        private SQLiteConnection sqlite_connect;
        private SQLiteCommand sqlite_cmd;
        public static readonly eegDB _instance=new eegDB();
        private string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),@"/Local/eegDB/eegDB.db");
        private eegDB()
        {
            if (!File.Exists(Path))
            {
                SQLiteConnection.CreateFile(Path);
            }
            Connect();
        }
        private void Connect()
        {
            sqlite_connect = new SQLiteConnection("Data source="+Path);
            sqlite_connect.Open();// Open
        }
        public void ExecuteCommamd(string sql)
        {
            if (sqlite_connect != null)
            {
                sqlite_cmd = sqlite_connect.CreateCommand();//create command
                sqlite_cmd.CommandText = sql;
                sqlite_cmd.ExecuteNonQuery();
            }
        }
        public void Close()
        {
            sqlite_connect.Close();
        }
    }
}
