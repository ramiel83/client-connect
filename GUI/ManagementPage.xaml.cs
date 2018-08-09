using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ManagementPage.xaml
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

        private void logs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void machinesTypes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            SecondaryPage sp = new SecondaryPage();
            Content = new Frame { Content = sp};
            
        }
    }
}



