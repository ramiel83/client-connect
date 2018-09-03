using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Database;

namespace GUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            App.WorkMode = (WorkMode) ((ComboBoxItem)WorkModeBox.SelectedItem).Tag;

            using (MainModel modelContainer = new MainModel(App.WorkMode))
            {
                string username = UserName_Box.Text;
                string passwordHash = Utilities.Sha256(Password_Box.Password);

                User user = modelContainer.UserSet.SingleOrDefault(u =>
                    u.Username == username && u.PasswordHash == passwordHash);
                if (user != null)
                {
                    App.UserAccessLevel = user.AccessLevel;
                    SecondaryPage sp = new SecondaryPage();
                    Content = sp;
                }
                else

                {
                    MessageBox.Show("שגיאה בכניסה למערכת -בדוק את שם המשתמש ו\\או הסיסמא.", "שגיאה בכניסה למערכת",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}