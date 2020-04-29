using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace MiShotService
{
    class ExecUtil
    {
        public static bool KillOthers()
        {
            Process MeProcess = Process.GetCurrentProcess();
            string ProcName = MeProcess.ProcessName;
            int MePID = MeProcess.Id;

            bool NeedPrivileges = false;

            Process[] GlobInstances = Process.GetProcessesByName(ProcName);
            foreach (Process Instance in GlobInstances)
            {
                if (Instance.Id != MePID)
                {
                    try
                    {
                        Instance.Kill();
                    }
                    catch (Win32Exception)
                    {
                        NeedPrivileges = true;
                    }
                }
            }
            return !NeedPrivileges;
        }

        public static void ElevateMe(string CmdArgs)
        {
            string ExePath = Assembly.GetEntryAssembly().Location;
            ProcessStartInfo StartMe = new ProcessStartInfo(ExePath, CmdArgs)
            {
                UseShellExecute = true,
                Verb = "runas"
            };

            Process.Start(StartMe);
            Environment.Exit(0);
        }
    }
}
