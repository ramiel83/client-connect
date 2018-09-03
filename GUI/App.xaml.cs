using System.Windows;
using Database;

namespace GUI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static WorkMode WorkMode { get; set; }
        public static AccessLevel UserAccessLevel { get; set; }

        public App()
        {
            ProcommProcess.RunInstance();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (ProcommProcess.Instance != null && ProcommProcess.Instance.HasExited != true)
                ProcommProcess.Instance.Kill();
            base.OnExit(e);
        }
    }
}