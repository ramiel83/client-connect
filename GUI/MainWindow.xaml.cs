using System.Linq;
using System.Windows;
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

        public static AccessLevel UserAccessLevel { get; private set; }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            using (MainModel modelContainer = new MainModel())
            {
                string username = UserName_Box.Text;
                string passwordHash = Utilities.Sha256(Password_Box.Password);

                User user = modelContainer.UserSet.SingleOrDefault(u =>
                    u.Username == username && u.PasswordHash == passwordHash);
                if (user != null)
                {
                    UserAccessLevel = (AccessLevel) user.AccessLevel;
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