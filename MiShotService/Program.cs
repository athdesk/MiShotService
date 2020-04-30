using System;
using System.Threading;
using System.Windows.Forms;

namespace MiShotService
{
    static class Program
    {
        public const string ARG_KILL = "kill";
        public const string ARG_INSTALL = "install";
        public const string ARG_UNINSTALL = "uninstall";
        public const string ARG_STANDALONE = "standalone";
        public const string DISPLAY_NAME = "MiShot";

        private static void StallThread()
        {
            while (true)
                Thread.Sleep(60000);
        }

        public static void CaseInstall(bool FromForm = false)
        {
            if (!WinInterface.IsProcessElevated)
            {
                WinInterface.ElevateMe(ARG_INSTALL);
            } else
            {
                WinInterface.SetStartup(true);
                if (!FromForm)
                    OpenForm();
            }
        }


        public static void CaseUninstall(bool FromForm = false)
        {
            if (!WinInterface.IsProcessElevated)
            {
                WinInterface.ElevateMe(ARG_UNINSTALL);
            }
            else
            {
                WinInterface.SetStartup(false);
                if (!FromForm)
                    OpenForm();
            }
        }

        public static void CaseStandalone(bool FromForm = false)
        {
            ScreenshotService.Stop();
            ScreenshotService.Start();
            if (!FromForm)
                StallThread();
        }

        public static void CaseKill(bool FromForm = false)
        {
            ScreenshotService.Stop();

            if (!WinInterface.KillOthers())
                WinInterface.ElevateMe(ARG_KILL);

            if (!FromForm)
                OpenForm();
        }

        private static void OpenForm()
        {
            WelcomeForm Form = new WelcomeForm();
            Application.EnableVisualStyles();
            Application.Run(Form);
        }

        static void Main(string[] Args)
        {
            try
            {
                switch (Args[0])
                {
                    case ARG_INSTALL:
                        CaseInstall();
                        break;
                    case ARG_STANDALONE:
                        CaseStandalone();
                        break;
                    case ARG_UNINSTALL:
                        CaseUninstall();
                        break;
                    case ARG_KILL:
                        CaseKill();
                        break;
                    default:
                        throw new Exception();
                }
            }
            catch (Exception)
            {
                OpenForm();
            }
        }
    }
}
