using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
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
        private const string DISPLAY_NAME = "MiShot";

        public static string ExePath;
        private static string RegCmdLine;

        public static void SetStartup(bool AutoStart)
        {
            RegistryKey RunKey = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (AutoStart)
                RunKey.SetValue(DISPLAY_NAME, RegCmdLine);
            else
                RunKey.DeleteValue(DISPLAY_NAME, false);
        }

        public static bool IsOnStartup()
        {
            RegistryKey RunKey = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            var RunValue = RunKey.GetValue(DISPLAY_NAME);
            if (RunValue == null)
                return false;
            else
                return RunValue.ToString().Equals(RegCmdLine);
            
        }

        private static void StallThread()
        {
            while (true)
                Thread.Sleep(60000);
        }

        public static void CaseInstall(bool FromForm = false)
        {
            if (!IsProcessElevated)
            {
                ExecUtil.ElevateMe(ARG_INSTALL);
            } else
            {
                SetStartup(true);
                if (!FromForm)
                    OpenForm();
            }
        }


        public static void CaseUninstall(bool FromForm = false)
        {
            if (!IsProcessElevated)
            {
                ExecUtil.ElevateMe(ARG_UNINSTALL);
            }
            else
            {
                SetStartup(false);
                if (!FromForm)
                    OpenForm();
            }
        }

        public static void CaseStandalone(bool FromForm = false)
        {
            MiShotUser.Stop();
            MiShotUser.Start();
            if (!FromForm)
                StallThread();
        }

        public static void CaseKill(bool FromForm = false)
        {
            MiShotUser.Stop();

            if (!ExecUtil.KillOthers())
                ExecUtil.ElevateMe(ARG_KILL);

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
            ExePath = Assembly.GetEntryAssembly().Location;
            RegCmdLine = ExePath + " " + ARG_STANDALONE;
            IsOnStartup();
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
    }
}
