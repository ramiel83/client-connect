using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    ///     Interaction logic for ManagementPage.xaml
    /// </summary>
    public partial class ManagementPage : Page
    {
        public ManagementPage()
        {
            InitializeComponent();
        }

        private void synchronizeDataBase_Click(object sender, RoutedEventArgs e)
        {
        }


        private void back_Click(object sender, RoutedEventArgs e)
        {
            SecondaryPage sp = new SecondaryPage();
            Content = new Frame {Content = sp};
        }

        private void UsersMange_Click(object sender, RoutedEventArgs e)
        {
            UsersManage sp = new UsersManage();
            Content = new Frame {Content = sp};
        }
    }
}