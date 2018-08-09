using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI
{
    /// <summary>
    ///     Interaction logic for SecondaryPage.xaml
    /// </summary>
    public partial class SecondaryPage : Page
    {
        public SecondaryPage()
        {
            InitializeComponent();
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void Management_Click(object sender, RoutedEventArgs e)
        {
            var mp = new ManagementPage();
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

        private void DailPbx_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dail to Pbx");
        }

        private void DailKolan_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dail To Kolan");
        }

        private void ConnectPbxInTelnet_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ConnectClientNetwork_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SecondaryPage1_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void Search_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}