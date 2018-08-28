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
                Instance = Process.Start("C:\\Program Files (x86)\\Symantec\\Procomm Plus\\PROGRAMS\\PW5.EXE");
            else
                Instance = null;
        }
    }
}