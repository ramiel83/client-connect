using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Database;
using Microsoft.Win32;
using File = Database.File;
using Switch = Database.Switch;

namespace GUI
{
    /// <summary>
    ///     Interaction logic for EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        private readonly int? _switchIdToEdit;

        public EditPage(int? switchIdToEdit)
        {
            _switchIdToEdit = switchIdToEdit;

            InitializeComponent();

            if (IsNewSwitch) return;

            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                Switch switchToEdit = modelContainer.SwitchSet.Single(x => x.Id == _switchIdToEdit);
                ManufacturingNumber_Box.Text = _switchIdToEdit.ToString();

                CustomerNum_Box.Text = switchToEdit.CrmNum;
                Comments_Box.Text = switchToEdit.Comments;
                CustomerName_Box.Text = switchToEdit.Name;
                Release_ComboBox.Text = switchToEdit.SwRelease;
                MachineType_ComboBox.Text = switchToEdit.MachineType;

                PbxConnection allPbxData = switchToEdit.PbxConnection;
                if (allPbxData != null)
                {
                    PbxPhoneNumber_Box.Text = allPbxData.DialNum;
                    PbxUserName_Box.Text = allPbxData.LoginName;
                    PbxPass_Box.Text = allPbxData.LoginPassword;
                    PbxBoudrate_ComboBox.Text = allPbxData.BaudRate.ToString();
                    PbxDebugPass_Box.Text = allPbxData.DebugPassword;
                    PbxParDataStop_ComboBox.Text = allPbxData.ParDataStop;
                }

                KolanConnection allKolanData = switchToEdit.KolanConnection;
                if (allKolanData != null)
                {
                    KolanPhoneNumber_Box.Text = allKolanData.DialNum;
                    KolanBoudrate_ComboBox.Text = allKolanData.BaudRate.ToString();
                    KolanParDataStop_ComboBox.Text = allKolanData.ParDataStop;
                }

                TelnetConnection allTelnetData = switchToEdit.TelnetConnection;
                if (allTelnetData != null)
                {
                    TelnetAddress_Box.Text = allTelnetData.IpAddress;
                    TelnetUserNameCS_Box.Text = allTelnetData.UsernameCs;
                    TelnetPassCS_Box.Text = allTelnetData.PasswordCs;
                    TelnetUserNameSS_Box.Text = allTelnetData.UsernameSs;
                    TelnetPasswordSS_Box.Text = allTelnetData.PasswordSs;
                }

                CheckPointVpnConnection checkPointVpnConnection = switchToEdit.CheckPointVpnConnection;
                if (checkPointVpnConnection != null)
                {
                    ClientAddress_Box.Text = checkPointVpnConnection.IpAddress;
                    SiteName_Box.Text = checkPointVpnConnection.Site;
                    NetworkUserName_Box.Text = checkPointVpnConnection.Username;
                    NetworkPass_Box.Text = checkPointVpnConnection.Password;
                    if (checkPointVpnConnection.Port != null)
                        NetworkPort_Box.Text = checkPointVpnConnection.Port.ToString();
                }

                RefreshFilesList();
            }
        }

        private bool IsNewSwitch => _switchIdToEdit == null;

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (App.UserAccessLevel > AccessLevel.Manager)
            {
                MessageBox.Show("אינך מורשה לבצע שינויים בדף זה יש לפנות למנהל המערכת", "בעיה בהרשאות",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int parsedSwitchId;
            if (string.IsNullOrWhiteSpace(ManufacturingNumber_Box.Text) ||
                !int.TryParse(ManufacturingNumber_Box.Text, out parsedSwitchId))
            {
                MessageBox.Show("חובה להכניס מספר בתעשיה תקין", "מספר בתעשיה לא תקין",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                try
                {
                    Switch switchToEdit = null;
                    if (IsNewSwitch)
                    {
                        switchToEdit = new Switch();
                        modelContainer.SwitchSet.Add(switchToEdit);
                    }
                    else
                    {
                        switchToEdit = modelContainer.SwitchSet.Single(x => x.Id == _switchIdToEdit);
                    }

                    switchToEdit.Name = CustomerName_Box.Text;
                    switchToEdit.CrmNum = CustomerNum_Box.Text;
                    switchToEdit.Id = parsedSwitchId;
                    switchToEdit.MachineType = MachineType_ComboBox.Text;
                    switchToEdit.SwRelease = Release_ComboBox.Text;
                    switchToEdit.Comments = Comments_Box.Text;
                    // add the switch if necessary, because all the connections depend on it
                    modelContainer.SaveChanges();

                    if (!string.IsNullOrWhiteSpace(PbxPhoneNumber_Box.Text) ||
                        !string.IsNullOrWhiteSpace(PbxUserName_Box.Text) ||
                        !string.IsNullOrWhiteSpace(PbxPass_Box.Text) ||
                        !string.IsNullOrWhiteSpace(PbxDebugPass_Box.Text) ||
                        !string.IsNullOrWhiteSpace(PbxParDataStop_ComboBox.Text))
                    {
                        PbxConnection pbxEditData = switchToEdit.PbxConnection;
                        bool newPbxConnection = false;
                        if (pbxEditData == null)
                        {
                            pbxEditData = new PbxConnection();
                            pbxEditData.SwitchId = switchToEdit.Id;
                            modelContainer.PbxConnectionSet.Add(pbxEditData);
                        }

                        pbxEditData.DialNum = PbxPhoneNumber_Box.Text;
                        int parsedBaudRate = 0;
                        if (int.TryParse(PbxBoudrate_ComboBox.Text, out parsedBaudRate))
                            pbxEditData.BaudRate = parsedBaudRate;
                        pbxEditData.LoginName = PbxUserName_Box.Text;
                        pbxEditData.LoginPassword = PbxPass_Box.Text;
                        pbxEditData.DebugPassword = PbxDebugPass_Box.Text;
                        pbxEditData.ParDataStop = PbxParDataStop_ComboBox.Text;
                    }

                    if (!string.IsNullOrWhiteSpace(KolanPhoneNumber_Box.Text))
                    {
                        KolanConnection kolanEditData = switchToEdit.KolanConnection;
                        if (kolanEditData == null)
                        {
                            kolanEditData = new KolanConnection();
                            kolanEditData.SwitchId = switchToEdit.Id;
                            modelContainer.KolanConnectionSet.Add(kolanEditData);
                        }

                        kolanEditData.DialNum = KolanPhoneNumber_Box.Text;
                        int parsedKolanBaudRate = 0;
                        if (int.TryParse(KolanBoudrate_ComboBox.Text, out parsedKolanBaudRate))
                            kolanEditData.BaudRate = parsedKolanBaudRate;
                        kolanEditData.ParDataStop = KolanParDataStop_ComboBox.Text;
                    }

                    if (!string.IsNullOrWhiteSpace(TelnetAddress_Box.Text) ||
                        !string.IsNullOrWhiteSpace(TelnetUserNameCS_Box.Text) ||
                        !string.IsNullOrWhiteSpace(TelnetPassCS_Box.Text) ||
                        !string.IsNullOrWhiteSpace(TelnetUserNameSS_Box.Text) ||
                        !string.IsNullOrWhiteSpace(TelnetPasswordSS_Box.Text))
                    {
                        TelnetConnection telnetEditData = switchToEdit.TelnetConnection;
                        if (telnetEditData == null)
                        {
                            telnetEditData = new TelnetConnection();
                            telnetEditData.SwitchId = switchToEdit.Id;
                            modelContainer.TelnetConnectionSet.Add(telnetEditData);
                        }

                        telnetEditData.IpAddress = TelnetAddress_Box.Text;
                        telnetEditData.UsernameCs = TelnetUserNameCS_Box.Text;
                        telnetEditData.PasswordCs = TelnetPassCS_Box.Text;
                        telnetEditData.UsernameSs = TelnetUserNameSS_Box.Text;
                        telnetEditData.PasswordSs = TelnetPasswordSS_Box.Text;
                    }

                    if (!string.IsNullOrWhiteSpace(ClientAddress_Box.Text) ||
                        !string.IsNullOrWhiteSpace(SiteName_Box.Text) ||
                        !string.IsNullOrWhiteSpace(NetworkUserName_Box.Text) ||
                        !string.IsNullOrWhiteSpace(NetworkPass_Box.Text) ||
                        !string.IsNullOrWhiteSpace(NetworkPort_Box.Text))
                    {
                        CheckPointVpnConnection checkPointVpnConnectionEditData = switchToEdit.CheckPointVpnConnection;
                        if (checkPointVpnConnectionEditData == null)
                        {
                            checkPointVpnConnectionEditData = new CheckPointVpnConnection();
                            checkPointVpnConnectionEditData.SwitchId = switchToEdit.Id;
                            modelContainer.CheckPointVpnConnectionSet.Add(checkPointVpnConnectionEditData);
                        }

                        checkPointVpnConnectionEditData.IpAddress = ClientAddress_Box.Text;
                        checkPointVpnConnectionEditData.Site = SiteName_Box.Text;
                        checkPointVpnConnectionEditData.Username = NetworkUserName_Box.Text;
                        checkPointVpnConnectionEditData.Password = NetworkPass_Box.Text;
                        checkPointVpnConnectionEditData.Port = string.IsNullOrWhiteSpace(NetworkPort_Box.Text)
                            ? (short?) null
                            : short.Parse(NetworkPort_Box.Text);
                    }

                    modelContainer.SaveChanges();
                    MessageBox.Show("השינויים נשמרו בהצלחה");

                    SecondaryPage sp = new SecondaryPage();
                    Content = new Frame {Content = sp};
                }
                catch (DbEntityValidationException ex)
                {
                    string errors = string.Empty;
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                    foreach (DbValidationError dbValidationError in validationError.ValidationErrors)
                        errors += dbValidationError + "\n";
                    MessageBox.Show("בעיה בשמירת המתג:\n" + errors);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("בעיה בשמירת המתג." + ex);
                }
            }
        }


        private void textBox_attach_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
        }


        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void textBox_attach_Copy4_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            SecondaryPage sp = new SecondaryPage();
            Content = new Frame {Content = sp};
        }

        private void addFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.UserAccessLevel > AccessLevel.Manager)
            {
                MessageBox.Show("אינך מורשה לבצע שינויים בדף זה יש לפנות למנהל המערכת", "בעיה בהרשאות",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() != true) return;

            File newFile = new File();
            newFile.SwitchId = (int) _switchIdToEdit;
            newFile.DateTime = DateTime.Now;
            newFile.Content = System.IO.File.ReadAllBytes(openFileDialog.FileName);
            string newFileName = new FileInfo(openFileDialog.FileName).Name;
            newFile.Name = newFileName;
            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                modelContainer.FileSet.Add(newFile);
                modelContainer.SaveChanges();
            }

            RefreshFilesList();
        }

        private void RefreshFilesList()
        {
            FileListBox.Items.Clear();
            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                Switch switchToEdit = modelContainer.SwitchSet.Single(x => x.Id == _switchIdToEdit);

                foreach (File file in switchToEdit.Files)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.MouseDoubleClick += FileMouseDoubleClick;
                    item.Content = file.Name;
                    item.Tag = file.Id;
                    FileListBox.Items.Add(item);
                }
            }
        }

        private void FileMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                int selectedFileId = int.Parse(((ListBoxItem) FileListBox.SelectedItems[0]).Tag.ToString());
                File file = modelContainer.FileSet.Single(x => x.Id == selectedFileId);
                // save the file to temp path
                string tempPath = Path.GetTempPath();
                string newFilePath = Path.Combine(tempPath, file.Name);
                System.IO.File.WriteAllBytes(newFilePath, file.Content);
                Process.Start(newFilePath);
            }
        }

        private void deleteFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.UserAccessLevel > AccessLevel.Manager)
            {
                MessageBox.Show("אינך מורשה לבצע שינויים בדף זה יש לפנות למנהל המערכת", "בעיה בהרשאות",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (FileListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("לא נבחר אף קובץ", "בחר קובץ", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                int selectedFileId = int.Parse(((ListBoxItem) FileListBox.SelectedItems[0]).Tag.ToString());
                File file = modelContainer.FileSet.Single(x => x.Id == selectedFileId);
                modelContainer.FileSet.Remove(file);
                modelContainer.SaveChanges();
            }

            RefreshFilesList();
        }
    }
}