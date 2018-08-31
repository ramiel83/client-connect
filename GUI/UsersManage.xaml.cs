using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Database;

namespace GUI
{
    /// <summary>
    ///     Interaction logic for UsersManage.xaml
    /// </summary>
    public partial class UsersManage : Page
    {
        public UsersManage()
        {
            InitializeComponent();
            using (MainModel modelContainer = new MainModel())
            {
                RefreshUserListBox(modelContainer.UserSet);
            }
        }

        private void RefreshUserListBox(IEnumerable<User> users)
        {
            UsersList_ListBox.Items.Clear();
            foreach (User s in users) UsersList_ListBox.Items.Add(s.Username);
        }

        private void UserSearch_TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = SearchUsers_TextBox.Text;
            using (MainModel modelContainer = new MainModel())
            {
                if (!string.IsNullOrWhiteSpace(searchText))
                    RefreshUserListBox(modelContainer.UserSet.Where(s =>
                        s.Username.Contains(searchText)));
                else
                    RefreshUserListBox(modelContainer.UserSet);
            }
        }


       

        private void EditUserData_Click(object sender, RoutedEventArgs e)
        {
            if (UsersList_ListBox.SelectedItem == null)
            {
                MessageBox.Show("לא נבחר משתמש לעריכה", "בעיה בעריכת משתמש", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            string userNameString = UsersList_ListBox.SelectedItem.ToString();
            if (string.IsNullOrWhiteSpace(UserName_TextBox.Text) ||
                string.IsNullOrWhiteSpace(Password_TextBox1.Password) ||
                string.IsNullOrWhiteSpace(Password_TextBox2.Password) ||
                string.IsNullOrWhiteSpace(ClassLevel_TextBox.Text) || string.IsNullOrWhiteSpace(userNameString))
            {
                MessageBox.Show("יש למלא את כל השדות");
                return;
            }

            if (MainWindow.UserAccessLevel > AccessLevel.Administrator)
            {
                MessageBox.Show("אינך מורשה לבצע שינויים בדף זה יש לפנות למנהל המערכת");
                return;
            }

            using (MainModel modelContainer = new MainModel())
            {
                User userEditSelectedData = modelContainer.UserSet.Single(s =>
                    s.Username == userNameString);
                if (Password_TextBox1.Password == Password_TextBox2.Password)
                {
                    userEditSelectedData.PasswordHash = Utilities.Sha256(Password_TextBox1.Password);
                    userEditSelectedData.AccessLevel = (AccessLevel) Enum.Parse(typeof(AccessLevel), ClassLevel_TextBox.Text);                
                }
                else
                {
                    MessageBox.Show("סיסמאות לא תואמות נא לנסות שנית");
                }

                if (modelContainer.SaveChanges() > 0)
                {
                    MessageBox.Show("השינויים נשמרו בהצלחה");
                    ManagementPage mp = new ManagementPage();
                    Content = new Frame {Content = mp};
                }
                else
                {
                    MessageBox.Show("השינויים לא נשמרו פנה למנהל המערכת");
                }
            }
        }


        private void AddUserOk_Click(object sender, RoutedEventArgs e)
        {
           
            User newUserData = new User();
            newUserData.Username = AddUserName_TextBox.Text;
            newUserData.PasswordHash = Utilities.Sha256(AddPassword_TextBox1.Password);
            newUserData.AccessLevel = (AccessLevel) Enum.Parse(typeof(AccessLevel),AddClassLevel_TextBox.Text);
            if (string.IsNullOrWhiteSpace(AddUserName_TextBox.Text) ||
                string.IsNullOrWhiteSpace(AddPassword_TextBox1.Password) ||
                string.IsNullOrWhiteSpace(AddPassword_TextBox2.Password)||
                string.IsNullOrWhiteSpace(AddClassLevel_TextBox.Text))
            {
                MessageBox.Show("יש למלא את כל השדות");
                return;
            }

            if (MainWindow.UserAccessLevel > AccessLevel.Administrator)
            {
                MessageBox.Show("אינך מורשה לבצע שינויים בדף זה יש לפנות למנהל המערכת");
                return;
            }

            using (MainModel modelContainer = new MainModel())
            {
                if (modelContainer.UserSet.FirstOrDefault(u => u.Username == newUserData.Username) == null
                    && AddPassword_TextBox1.Password == AddPassword_TextBox2.Password)
                    modelContainer.UserSet.Add(newUserData);
                else
                    MessageBox.Show("משתמש קיים ו או סיסמאות לא תואמות נא לנסות שנית");

                if (modelContainer.SaveChanges() <= 0)
                {
                    MessageBox.Show("השינויים לא נשמרו פנה למנהל המערכת");
                    return;
                }

                MessageBox.Show("השינויים נשמרו בהצלחה");
                ManagementPage mp = new ManagementPage();
                Content = new Frame {Content = mp};
            }
        }

        private void EditCancelUserData_Click(object sender, RoutedEventArgs e)
        {
            ManagementPage mp = new ManagementPage();
            Content = new Frame {Content = mp};
        }

        private void AddCancelUserData_Click(object sender, RoutedEventArgs e)
        {
            ManagementPage mp = new ManagementPage();
            Content = new Frame {Content = mp};
        }

        private void SelectedUser(object sender, SelectionChangedEventArgs e)
        {
            {
                string userNameString = UsersList_ListBox.SelectedItem.ToString();
                using (MainModel modelContainer = new MainModel())
                {
                    User userSelectedData = modelContainer.UserSet.Single(s =>
                        s.Username == userNameString);
                    UserName_TextBox.Text = userSelectedData.Username;
                    Password_TextBox1.Password = userSelectedData.PasswordHash;
                    Password_TextBox2.Password = userSelectedData.PasswordHash;
                    ClassLevel_TextBox.Text = ((AccessLevel)(userSelectedData.AccessLevel)).ToString();
                }
            }

        }
    }
}