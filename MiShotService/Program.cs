using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace MiShotService
{
    static class Program
    {

        #region UacHelper
        private const string uacRegistryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
        private const string uacRegistryValue = "EnableLUA";

        private static uint STANDARD_RIGHTS_READ = 0x00020000;
        private static uint TOKEN_QUERY = 0x0008;
        private static uint TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, uint TokenInformationLength, out uint ReturnLength);

        public enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUIAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            MaxTokenInfoClass
        }

        public enum TOKEN_ELEVATION_TYPE
        {
            TokenElevationTypeDefault = 1,
            TokenElevationTypeFull,
            TokenElevationTypeLimited
        }

        public static bool IsUacEnabled
        {
            get
            {
                RegistryKey uacKey = Registry.LocalMachine.OpenSubKey(uacRegistryKey, false);
                bool result = uacKey.GetValue(uacRegistryValue).Equals(1);
                return result;
            }
        }

        public static bool IsProcessElevated
        {
            get
            {
                if (IsUacEnabled)
                {
                    IntPtr tokenHandle;
                    if (!OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_READ, out tokenHandle))
                    {
                        throw new ApplicationException("Could not get process token.  Win32 Error Code: " + Marshal.GetLastWin32Error());
                    }

                    TOKEN_ELEVATION_TYPE elevationResult = TOKEN_ELEVATION_TYPE.TokenElevationTypeDefault;

                    int elevationResultSize = Marshal.SizeOf((int)elevationResult);
                    uint returnedSize = 0;
                    IntPtr elevationTypePtr = Marshal.AllocHGlobal(elevationResultSize);

                    bool success = GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenElevationType, elevationTypePtr, (uint)elevationResultSize, out returnedSize);
                    if (success)
                    {
                        elevationResult = (TOKEN_ELEVATION_TYPE)Marshal.ReadInt32(elevationTypePtr);
                        bool isProcessAdmin = elevationResult == TOKEN_ELEVATION_TYPE.TokenElevationTypeFull;
                        return isProcessAdmin;
                    }
                    else
                    {
                        throw new ApplicationException("Unable to determine the current elevation.");
                    }
                }
                else
                {
                    WindowsIdentity identity = WindowsIdentity.GetCurrent();
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    bool result = principal.IsInRole(WindowsBuiltInRole.Administrator);
                    return result;
                }
            }
        }
        #endregion

        public const string ARG_KILL = "kill";
        public const string ARG_INSTALL = "install";
        public const string ARG_UNINSTALL = "uninstall";
        public const string ARG_STANDALONE = "standalone";
        private const string SERVICE_NAME = "MiShotService";
        private const string DISPLAY_NAME = "MiShot";

        public static MiShot RunningInstance = null;
        private static string ExePath;

        private static void RunService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MiShot()
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static void ElevateMe(string CmdArgs)
        {
            ProcessStartInfo StartMe = new ProcessStartInfo(ExePath, CmdArgs)
            {
                UseShellExecute = true,
                Verb = "runas"
            };

            Process.Start(StartMe);
            Application.Exit();
        }

        public static void CaseInstall(bool FromForm = false)
        {
            if (!IsProcessElevated)
            {
                ElevateMe(ARG_INSTALL);
            } else
            {
                bool Reinstall = ServiceInstaller.ServiceIsInstalled(SERVICE_NAME);
                          

                Application.EnableVisualStyles();
                if (Reinstall)
                {
                    MessageBox.Show("Service is already installed");
                }
                else
                {
                    ServiceInstaller.InstallAndStart(SERVICE_NAME, DISPLAY_NAME, ExePath);
                    MessageBox.Show("Successfully installed the MiShot Service");
                }

                
                if (!FromForm)
                {
                    OpenForm();
                }
            }
        }


        public static void CaseUninstall(bool FromForm = false)
        {
            if (!IsProcessElevated)
            {
                ElevateMe(ARG_UNINSTALL);
            }
            else
            {
                Application.EnableVisualStyles();
                bool WasInstalled = ServiceInstaller.ServiceIsInstalled(SERVICE_NAME);
                if (WasInstalled)
                {
                    ServiceInstaller.Uninstall(SERVICE_NAME);
                    MessageBox.Show("Successfully uninstalled the MiShot Service");
                }
                else
                {
                    MessageBox.Show("Service was not installed");
                }
                
                if (!FromForm)
                {
                    OpenForm();
                }
            }
        }

        public static void CaseStandalone()
        {
            if (RunningInstance == null) 
            { 
                RunningInstance = new MiShot(); 
            } else
            {
                RunningInstance.Stop();
            }
            RunningInstance.Start();
        }

        public static void CaseKill(bool FromForm = false)
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

            if (NeedPrivileges)
            {
                ElevateMe(ARG_KILL);
            }

            if (!FromForm)
            {
                OpenForm();
            }

        }

        private static void OpenForm()
        {
            WelcomeForm Form = new WelcomeForm();
            Application.EnableVisualStyles();
            Application.Run(Form);
        }

        static void Main(string[] Args)
        {
            ExePath = Assembly.GetEntryAssembly().Location;
            if (Environment.UserInteractive)
            {
                if (Args.Length > 0)
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
                            OpenForm();
                            break;
                    }
                } else
                {
                    OpenForm();
                }
            }
            else
            {
                RunService();
            }
        
        }
    }
}
