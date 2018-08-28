using System;
using System.Data.Entity.Validation;
using System.Windows;
using System.Windows.Controls;
using Database;

namespace GUI
{
    /// <summary>
    ///     Interaction logic for EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        private readonly Switch _switchToEdit;

        public EditPage(Switch switchToEdit)
        {
            _switchToEdit = switchToEdit;

            InitializeComponent();

            if (!IsNewSwitch) ManufacturingNumber_Box.Text = _switchToEdit.Id.ToString();

            CustomerNum_Box.Text = _switchToEdit.CrmNum;
            Comments_Box.Text = _switchToEdit.Comments;
            CustomerName_Box.Text = _switchToEdit.Name;
            Release_ComboBox.Text = _switchToEdit.SwRelease;
            MachineType_ComboBox.Text = _switchToEdit.MachineType;

            PbxConnection allPbxData = _switchToEdit.PbxConnection;
            if (allPbxData != null)
            {
                PbxPhoneNumber_Box.Text = allPbxData.DialNum;
                PbxUserName_Box.Text = allPbxData.LoginName;
                PbxPass_Box.Text = allPbxData.LoginPassword;
                PbxBoudrate_ComboBox.Text = allPbxData.BaudRate.ToString();
                PbxDebugPass_Box.Text = allPbxData.DebugPassword;
                PbxParDataStop_ComboBox.Text = allPbxData.ParDataStop;
            }

            KolanConnection allKolanData = _switchToEdit.KolanConnection;
            if (allKolanData != null)
            {
                KolanPhoneNumber_Box.Text = allKolanData.DialNum;
                KolanBoudrate_ComboBox.Text = allKolanData.BaudRate.ToString();
                KolanParDataStop_ComboBox.Text = allKolanData.ParDataStop;
            }

            TelnetConnection allTelnetData = _switchToEdit.TelnetConnection;
            if (allTelnetData != null)
            {
                TelnetAddress_Box.Text = allTelnetData.IpAddress;
                TelnetUserNameCS_Box.Text = allTelnetData.UserNameCS;
                TelnetPassCS_Box.Text = allTelnetData.PasswordCS;
                TelnetUserNameSS_Box.Text = allTelnetData.UserNameSS;
                TelnetPasswordSS_Box.Text = allTelnetData.PasswordSS;
            }
        }

        private bool IsNewSwitch => _switchToEdit.Id == 0;

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.UserAccessLevel != AccessLevel.Administrator)
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

            using (ClientConnectModelContainer modelContainer = new ClientConnectModelContainer())
            {
                try
                {
                    if (IsNewSwitch) modelContainer.SwitchSet.Add(_switchToEdit);

                    _switchToEdit.Name = CustomerName_Box.Text;
                    _switchToEdit.CrmNum = CustomerNum_Box.Text;
                    _switchToEdit.Id = parsedSwitchId;
                    _switchToEdit.MachineType = MachineType_ComboBox.Text;
                    _switchToEdit.SwRelease = Release_ComboBox.Text;
                    _switchToEdit.Comments = Comments_Box.Text;
                    modelContainer
                        .SaveChanges(); // add the switch if necessary, because all the connections depend on it

                    PbxConnection pbxEditData = _switchToEdit.PbxConnection;
                    bool newPbxConnection = false;
                    if (pbxEditData == null)
                    {
                        pbxEditData = new PbxConnection();
                        pbxEditData.SwitchId = _switchToEdit.Id;
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

                    KolanConnection kolanEditData = _switchToEdit.KolanConnection;
                    if (kolanEditData == null)
                    {
                        kolanEditData = new KolanConnection();
                        kolanEditData.SwitchId = _switchToEdit.Id;
                        modelContainer.KolanConnectionSet.Add(kolanEditData);
                    }

                    kolanEditData.DialNum = KolanPhoneNumber_Box.Text;
                    int parsedKolanBaudRate = 0;
                    if (int.TryParse(KolanBoudrate_ComboBox.Text, out parsedKolanBaudRate))
                        kolanEditData.BaudRate = parsedKolanBaudRate;
                    kolanEditData.ParDataStop = KolanParDataStop_ComboBox.Text;

                    TelnetConnection telnetEditData = _switchToEdit.TelnetConnection;
                    if (telnetEditData == null)
                    {
                        telnetEditData = new TelnetConnection();
                        telnetEditData.SwitchId = _switchToEdit.Id;
                        modelContainer.TelnetConnectionSet.Add(telnetEditData);
                    }

                    telnetEditData.IpAddress = TelnetAddress_Box.Text;
                    telnetEditData.UserNameCS = TelnetUserNameCS_Box.Text;
                    telnetEditData.PasswordCS = TelnetPassCS_Box.Text;
                    telnetEditData.UserNameSS = TelnetUserNameSS_Box.Text;
                    telnetEditData.PasswordSS = TelnetPasswordSS_Box.Text;


                    modelContainer.SaveChanges();
                    MessageBox.Show("השינויים נשמרו בהצלחה");

                    SecondaryPage sp = new SecondaryPage();
                    Content = new Frame {Content = sp};
                }
                catch (DbEntityValidationException ex)
                {
                    string errors = string.Empty;
                    foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                        errors += validationError + "\n";
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
    }
}