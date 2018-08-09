using System;
using System.Configuration;
using System.Data.OleDb;

namespace DataTransfer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (OleDbConnection accessConnection = ConnectToAccess())
            {
                TransferData(accessConnection);
            }
        }

        private static void TransferData(OleDbConnection accessConnection)
        {
            using (OleDbCommand oleDbCommand = new OleDbCommand("select * from Switch_DB", accessConnection))
            {
                OleDbDataReader reader = oleDbCommand.ExecuteReader();
                while (reader.Read())
                {
                    // create switch
                    Switch sw = new Switch();
                    sw.Id = int.Parse(reader.GetString(reader.GetOrdinal("SwitchID")));
                    sw.CrmNum = null;
                    sw.Comments = reader["Comment"] as string;
                    sw.Name = reader.GetString(reader.GetOrdinal("Switch_Name"));
                    sw.SwRelease = reader.GetString(reader.GetOrdinal("SW_Release"));
                    sw.MachineType =
                        GetMachineTypeComment(reader.GetString(reader.GetOrdinal("Machine")), accessConnection);
                    int? customerId = reader["CustomerID"] as int?;
                    sw.SiteId = customerId != null && customerId == 0 ? null : customerId;
                    sw.Tid = reader.GetString(reader.GetOrdinal("TID"));
                    using (ClientConnectModelContainer modelContainer = new ClientConnectModelContainer())
                    {
                        modelContainer.SwitchSet.Add(sw);
                        modelContainer.SaveChanges();
                    }

                    Console.WriteLine("added switch id = {0}", sw.Id);

                    // pbx connection
                    string dialNum = reader.GetString(reader.GetOrdinal("DialNum"));
                    if (!string.IsNullOrWhiteSpace(dialNum))
                    {
                        PbxConnection pbxConnection = new PbxConnection();
                        pbxConnection.SwitchId = sw.Id;
                        pbxConnection.DialNum = reader.GetString(reader.GetOrdinal("Area")) + dialNum;
                        pbxConnection.LoginName = reader.GetString(reader.GetOrdinal("UserMerk"));
                        pbxConnection.LoginPassword = reader.GetString(reader.GetOrdinal("Password"));
                        pbxConnection.DebugPassword = reader.GetString(reader.GetOrdinal("DebugPass"));
                        pbxConnection.BaudRate = GetBaudRate(reader.GetString(reader.GetOrdinal("Baudrate")),
                            accessConnection);
                        pbxConnection.ParDataStop = GetParDataStop(reader.GetString(reader.GetOrdinal("Parity")),
                            accessConnection);
                        using (ClientConnectModelContainer modelContainer = new ClientConnectModelContainer())
                        {
                            modelContainer.PbxConnectionSet.Add(pbxConnection);
                            modelContainer.SaveChanges();
                        }

                        Console.WriteLine("added pbx connection for switch id = {0}", sw.Id);
                    }
                }
            }

            //kolan connection
            using (OleDbCommand oleDbCommand =
                new OleDbCommand("select * from Kolan where LCase(SysName) = 'kolan'", accessConnection))
            {
                OleDbDataReader reader = oleDbCommand.ExecuteReader();
                while (reader.Read())
                    try
                    {
                        KolanConnection kolanConnection = new KolanConnection();
                        kolanConnection.SwitchId = int.Parse(reader.GetString(reader.GetOrdinal("SwitchID")));
                        kolanConnection.DialNum = reader.GetString(reader.GetOrdinal("Area")) +
                                                  reader.GetString(reader.GetOrdinal("DialNum"));
                        kolanConnection.BaudRate = GetBaudRate(reader.GetString(reader.GetOrdinal("Baudrate")),
                            accessConnection);
                        kolanConnection.ParDataStop = GetParDataStop(
                            reader.GetString(reader.GetOrdinal("Parity")),
                            accessConnection);
                        using (ClientConnectModelContainer modelContainer = new ClientConnectModelContainer())
                        {
                            modelContainer.KolanConnectionSet.Add(kolanConnection);
                            modelContainer.SaveChanges();
                        }

                        Console.WriteLine("added kolan connection for switch id = {0}",
                            kolanConnection.SwitchId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("exception happened while inserting kolan: " + ex);
                    }
            }
        }

        private static string GetParDataStop(string parityId, OleDbConnection connection)
        {
            using (OleDbCommand oleDbCommand =
                new OleDbCommand("select ParDatStop from ParityDB where ParityKod = @id", connection))
            {
                oleDbCommand.Parameters.AddWithValue("@id", parityId);
                return (string) oleDbCommand.ExecuteScalar();
            }
        }

        private static int GetBaudRate(string baudRateId, OleDbConnection connection)
        {
            using (OleDbCommand oleDbCommand =
                new OleDbCommand("select Baudr from Baudrate_DB where BaudKod = @id", connection))
            {
                oleDbCommand.Parameters.AddWithValue("@id", baudRateId);
                return int.Parse((string) oleDbCommand.ExecuteScalar());
            }
        }

        private static string GetMachineTypeComment(string machineId, OleDbConnection connection)
        {
            using (OleDbCommand oleDbCommand =
                new OleDbCommand("select Comment from MachineDB where Machine = @id", connection))
            {
                oleDbCommand.Parameters.AddWithValue("@id", machineId);
                return (string) oleDbCommand.ExecuteScalar();
            }
        }

        private static OleDbConnection ConnectToAccess()
        {
            string connStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                             ConfigurationManager.AppSettings["access_db_filename"] +
                             "; Jet OLEDB:Database Password=" +
                             ConfigurationManager.AppSettings["access_db_password"];
            OleDbConnection accessConnection = new OleDbConnection(connStr);
            accessConnection.Open();
            return accessConnection;
        }
    }
}