using System.Configuration;
using System.Diagnostics;

namespace GUI
{
    internal static class ProcommProcess
    {
        public static Process Instance { get; private set; }

        public static void RunInstance()
        {
            if (bool.Parse(ConfigurationManager.AppSettings["RunProcomm"]))
                Instance = Process.Start(ConfigurationManager.AppSettings["ProcommPath"]);
            else
                Instance = null;
        }
    }
}