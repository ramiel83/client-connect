using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Database;
using NDde.Client;
using File = System.IO.File;
using Switch = Database.Switch;

namespace GUI
{
    /// <summary>
    ///     Interaction logic for SecondaryPage.xaml
    /// </summary>
    public partial class SecondaryPage : Page
    {
        private List<Switch> sw;

        public SecondaryPage()
        {
            InitializeComponent();

            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                RefreshListBox(modelContainer.SwitchSet);
            }
        }


        private void Management_Click(object sender, RoutedEventArgs e)
        {
            ManagementPage mp = new ManagementPage();
            //over between some page
            Content = new Frame {Content = mp};
            //over between one page
            //this.Content = mp;
        }


        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerList_Box.SelectedItem == null)
            {
                MessageBox.Show("לא נבחר אף לקוח לעריכה", "בעיה בעריכת לקוח", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            string switchIdString = CustomerList_Box.SelectedItem.ToString();
            string[] selectedCustomerArr = switchIdString.Split(' ');
            int switchId = int.Parse(selectedCustomerArr[0]);

            EditPage ep = new EditPage(switchId);
            //over between some page
            Content = new Frame {Content = ep};
        }

        private void DialPbx_Click(object sender, RoutedEventArgs e)
        {
            string dialNum = null;
            string loginName = null;
            string loginPassword = null;
            long baudRate;
            int dataBits;
            int stopBits;
            int parity;
            string selectedCustomer = CustomerList_Box.SelectedItem.ToString();
            string[] selectedCustomerArr = selectedCustomer.Split(' ');
            int switchIdSelcted = int.Parse(selectedCustomerArr[0]);

            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                PbxConnection allSwitchIdData = modelContainer.PbxConnectionSet.Single(s =>
                    s.SwitchId == switchIdSelcted);
                dialNum = allSwitchIdData.DialNum;

                loginName = allSwitchIdData.LoginName;

                loginPassword = allSwitchIdData.LoginPassword;

                if (allSwitchIdData.ParDataStop != null && allSwitchIdData.BaudRate != 0)
                {
                    string[] parDataStopArr = allSwitchIdData.ParDataStop.Split('-');
                    switch (parDataStopArr[0])
                    {
                        case "N":
                            parity = 0;
                            break;
                        case "O":
                            parity = 1;
                            break;
                        case "E":
                            parity = 2;
                            break;
                        case "M":
                            parity = 3;
                            break;
                        case "S":
                            parity = 4;
                            break;
                        default:
                            throw new Exception("unexpected parity: " + parDataStopArr[0]);
                    }

                    dataBits = int.Parse(parDataStopArr[1]);
                    stopBits = int.Parse(parDataStopArr[2]);

                    baudRate = allSwitchIdData.BaudRate;
                }
                else
                {
                    parity = 0;
                    dataBits = 8;
                    stopBits = 1;
                    baudRate = 9600;
                }
            }

            string filePath = WriteScript(dialNum, loginName, loginPassword, baudRate, parity, dataBits, stopBits);

            try
            {
                using (DdeClient client = new DdeClient("PW5", "System"))
                {
                    client.Connect();
                    client.Execute($"EXECUTE {filePath}", 30);
                }
            }
            catch (Exception ex)
            {
            }

            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private string WriteScript(string dialNum, string loginName, string loginPassword, long baudRate, int parity,
            int dataBits, int stopBits)
        {
            string fileFormat = File.ReadAllText("scriptformat.txt");
            string formattedScript = string.Format(fileFormat, dialNum, loginName, loginPassword, baudRate, parity,
                dataBits, stopBits);
            string scriptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "script.was");
            File.WriteAllText(scriptFilePath, formattedScript);
            return scriptFilePath;
        }

        private void DialKolan_Click(object sender, RoutedEventArgs e)
        {
            string dialNum = null;
            long baudRate = 0;
            string selectedCustomer = CustomerList_Box.SelectedItem.ToString();
            string[] selectedCustomerArr = selectedCustomer.Split(' ');
            int switchIdSelcted = int.Parse(selectedCustomerArr[0]);

            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                KolanConnection allSwitchIdData = modelContainer.KolanConnectionSet.SingleOrDefault(s =>
                    s.SwitchId == switchIdSelcted);
                if (allSwitchIdData != null)
                {
                    dialNum = allSwitchIdData.DialNum;
                    baudRate = allSwitchIdData.BaudRate;
                }
            }

            string filePath = WriteKolanScript(dialNum, baudRate);

            try
            {
                using (DdeClient client = new DdeClient("PW5", "System"))
                {
                    client.Connect();
                    client.Execute($"EXECUTE {filePath}", 30);
                }
            }
            catch (Exception ex)
            {
            }

            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private string WriteKolanScript(string dialNum, long baudRate)
        {
            string fileFormat = File.ReadAllText("scriptKolanformat.txt");
            string formattedScript = string.Format(fileFormat, dialNum, baudRate);
            string scriptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "kolanscript.was");
            File.WriteAllText(scriptFilePath, formattedScript);
            return scriptFilePath;
        }


        //if (BaudRate == 2400)
        //    Process.Start("C:\\Program Files (x86)\\Symantec\\Procomm Plus\\PROGRAMS\\PW5.EXE",
        //        "TERMINAL \"C:\\TX1-PS\\MyProg\\connectKolan24.was\" " + s0DialNum);
        //else
        //    Process.Start("C:\\Program Files (x86)\\Symantec\\Procomm Plus\\PROGRAMS\\PW5.EXE",
        //        "TERMINAL \"C:\\TX1-PS\\MyProg\\connectKolan96.was\" " + s0DialNum);


        private void ConnectPbxInTelnet_Click(object sender, RoutedEventArgs e)
        {
            string ipAdress;
            string siganlServerLogi = null;
            string siganlServerPass = null;
            string callServerLogi = null;
            string callServerPass = null;
            int switchIdSelcted = GetSelctedSwitchId();

            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                TelnetConnection allSwitchIdData = modelContainer.TelnetConnectionSet.Single(s =>
                    s.SwitchId == switchIdSelcted);
                ipAdress = allSwitchIdData.IpAddress;
                siganlServerLogi = allSwitchIdData.UsernameSs;
                siganlServerPass = allSwitchIdData.PasswordSs;
                callServerLogi = allSwitchIdData.UsernameCs;
                callServerPass = allSwitchIdData.PasswordCs;
            }


            string filePath = WriteTelnetScript(ipAdress, siganlServerLogi, siganlServerPass, callServerLogi,
                callServerPass);

            try
            {
                using (DdeClient client = new DdeClient("PW5", "System"))
                {
                    client.Connect();
                    client.Execute($"EXECUTE {filePath}", 30);
                }
            }
            catch (Exception ex)
            {
            }

            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private int GetSelctedSwitchId()
        {
            string selectedCustomer = CustomerList_Box.SelectedItem.ToString();
            string[] selectedCustomerArr = selectedCustomer.Split(' ');
            int switchIdSelcted = int.Parse(selectedCustomerArr[0]);
            return switchIdSelcted;
        }

        //  private string WriteTelnetScript(string ipAdress, string siganlServerLogi, string siganlServerPass, string callServerLogi, string callServerPass)
        private string WriteTelnetScript(string ipAdress, string siganlServerLogi, string siganlServerPass,
            string callServerLogi, string callServerPass)
        {
            string fileFormat = File.ReadAllText("scripttelnetformat.txt");
            string formattedScript = string.Format(fileFormat, ipAdress, siganlServerLogi, siganlServerPass,
                callServerLogi, callServerPass);
            string scriptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "telnetscript.was");
            File.WriteAllText(scriptFilePath, formattedScript);
            return scriptFilePath;
        }

        private void ConnectClientNetwork_Click(object sender, RoutedEventArgs e)
        {
            int switchIdSelcted = GetSelctedSwitchId();
            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                CheckPointVpnConnection checkPointVpnConnection =
                    modelContainer.CheckPointVpnConnectionSet.Single(s => s.SwitchId == switchIdSelcted);
                Process.Start(ConfigurationManager.AppSettings["CheckPointEndPointConnectCommandLinePath"],
                    $"connect -s {checkPointVpnConnection.Site} -u {checkPointVpnConnection.Username} -p {checkPointVpnConnection.Password}");
            }
        }

        private void RefreshListBox(IEnumerable<Switch> switches)
        {
            CustomerList_Box.Items.Clear();
            foreach (Switch s in switches) CustomerList_Box.Items.Add(s.Id + " - " + s.Name + " - " + s.CrmNum);
        }

        private void Search_TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = Search_TextBox.Text;
            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                if (!string.IsNullOrWhiteSpace(searchText))
                    RefreshListBox(modelContainer.SwitchSet.Where(s =>
                        s.CrmNum.Contains(searchText) || s.Name.Contains(searchText) ||
                        s.Id.ToString().Contains(searchText)));
                else
                    RefreshListBox(modelContainer.SwitchSet);
            }
        }

        private void AddNew_Client(object sender, RoutedEventArgs e)
        {
            if (App.UserAccessLevel > AccessLevel.Manager)
            {
                MessageBox.Show("אינך מורשה להוסיף לקוח יש לפנות למנהל המערכת");
                return;
            }

            try
            {
                EditPage ep = new EditPage(null);
                Content = new Frame {Content = ep};
            }
            catch (Exception exception)
            {
                MessageBox.Show("לא ניתן להוסיף לקוח זה פנה למנהל המערכת");
                // throw;
            }
        }
    }
}