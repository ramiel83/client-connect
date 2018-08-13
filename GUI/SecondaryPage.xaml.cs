using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataTransfer;
using Switch = DataTransfer.Switch;

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

            using (ClientConnectModelContainer modelContainer = new ClientConnectModelContainer())
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
            EditPage ep = new EditPage();
            //over between some page
            Content = new Frame {Content = ep};
            //over between one page
            //this.Content = ep;
        }

        private void DialPbx_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dial to Pbx");
            Process.Start("C:\\Program Files\\Symantec\\Procomm Plus\\PROGRAMS\\PW5.EXE",
                "TERMINAL \"C:\\TX1 - PS\\MyProg\\newtest.was\" 089157312 bezeqint __Ngen@14");
        }

        private void DialKolan_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dial To Kolan");
        }

        private void ConnectPbxInTelnet_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ConnectClientNetwork_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RefreshListBox(IEnumerable<Switch> switches)
        {
            CustomerList_Box.Items.Clear();
            foreach (Switch s in switches) CustomerList_Box.Items.Add(s.Id + " - " + s.Name + " - " + s.CrmNum);
        }

        private void Search_TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = Search_TextBox.Text;
            using (ClientConnectModelContainer modelContainer = new ClientConnectModelContainer())
            {
                if (!string.IsNullOrWhiteSpace(searchText))
                    RefreshListBox(modelContainer.SwitchSet.Where(s =>
                        s.CrmNum.Contains(searchText) || s.Name.Contains(searchText) ||
                        s.Id.ToString().Contains(searchText)));
                else
                    RefreshListBox(modelContainer.SwitchSet);
            }
        }
    }
}