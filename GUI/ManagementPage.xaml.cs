using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Database;

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
            if (App.WorkMode == WorkMode.Local)
            {
                MessageBox.Show("לא ניתן לבצע סינכרון במצב מקומי.", "לא ניתן לבצע סינכרון", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("סנכרון מתחיל. נא לא לסגור את התוכנה עד לקבל הודעת סיום", "סנכרון מתחיל",
                MessageBoxButton.OK, MessageBoxImage.Exclamation);
            using (MainModel remoteModel = new MainModel(WorkMode.Main))
            using (MainModel localModel = new MainModel(WorkMode.Local))
            {
                // delete all local data
                localModel.PbxConnectionSet.RemoveRange(localModel.PbxConnectionSet);
                localModel.KolanConnectionSet.RemoveRange(localModel.KolanConnectionSet);
                localModel.TelnetConnectionSet.RemoveRange(localModel.TelnetConnectionSet);
                localModel.CheckPointVpnConnectionSet.RemoveRange(localModel.CheckPointVpnConnectionSet);
                localModel.FileSet.RemoveRange(localModel.FileSet);
                localModel.SwitchSet.RemoveRange(localModel.SwitchSet);
                localModel.UserSet.RemoveRange(localModel.UserSet);
                localModel.SaveChanges();

                // add new data
                localModel.SwitchSet.AddRange(remoteModel.SwitchSet.AsNoTracking());
                localModel.PbxConnectionSet.AddRange(remoteModel.PbxConnectionSet.AsNoTracking());
                localModel.KolanConnectionSet.AddRange(remoteModel.KolanConnectionSet.AsNoTracking());
                localModel.TelnetConnectionSet.AddRange(remoteModel.TelnetConnectionSet.AsNoTracking());
                localModel.CheckPointVpnConnectionSet.AddRange(remoteModel.CheckPointVpnConnectionSet.AsNoTracking());
                localModel.FileSet.AddRange(remoteModel.FileSet.AsNoTracking());
                localModel.UserSet.AddRange(remoteModel.UserSet.AsNoTracking());
                localModel.SaveChanges();

                MessageBox.Show("סנכרון הסתיים", "סנכרון הסתיים", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            SecondaryPage sp = new SecondaryPage();
            Content = new Frame {Content = sp};
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