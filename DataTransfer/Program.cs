using System.Configuration;
using System.Data.OleDb;
using MySql.Data.MySqlClient;

namespace DataTransfer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var accessConnection = ConnectToAccess())
            using (var mysqlConnection = ConnectToMySql())
            {
                TransferData(accessConnection, mysqlConnection);
            }
        }

        private static void TransferData(OleDbConnection accessConnection, MySqlConnection mysqlConnection)
        {
            using (OleDbCommand oleDbCommand = new OleDbCommand("select * from Switch_DB", accessConnection))
            {
                var reader = oleDbCommand.ExecuteReader();
                while (reader.Read())
                {

                }
            }
        }

        private static MySqlConnection ConnectToMySql()
        {
            var conn = new MySqlConnection
            {
                ConnectionString = "server=127.0.0.1;uid=" + ConfigurationManager.AppSettings["mysql_username"] +
                                   ";pwd=" + ConfigurationManager.AppSettings["mysql_password"] + ";database=" +
                                   ConfigurationManager.AppSettings["mysql_schema"]
            };
            conn.Open();

            return conn;
        }

        private static OleDbConnection ConnectToAccess()
        {
            var connStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                          ConfigurationManager.AppSettings["access_db_filename"] + "; Jet OLEDB:Database Password=" +
                          ConfigurationManager.AppSettings["access_db_password"];
            var accessConnection = new OleDbConnection(connStr);
            accessConnection.Open();
            return accessConnection;
        }
    }
}