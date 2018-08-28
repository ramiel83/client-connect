using System.Windows;

namespace GUI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ProcommProcess.RunInstance();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (ProcommProcess.Instance != null) ProcommProcess.Instance.Kill();
            base.OnExit(e);
        }
    }
}