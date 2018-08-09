using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace GUI
{
    /// <summary>
    /// Interaction logic for EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        public EditPage()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {

        }

       

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SecondaryPage sp = new SecondaryPage();
            Content = new Frame { Content = sp };

        }

      

        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox_attach_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox_attach_Copy4_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
