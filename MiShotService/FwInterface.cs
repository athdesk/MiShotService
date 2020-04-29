using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32;

public delegate void EventHandler();

namespace MiShotService
{
    class FwInterface
    {
        #region DllImports
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern int WaitForMultipleObjects(int nCount, ref int lpHandles, int bWaitAll, int dwMilliseconds);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern int WaitForSingleObject(int hHandle, int dwMilliseconds);

		[DllImport("kernel32", CharSet = CharSet.Ansi, EntryPoint = "CreateEventA", ExactSpelling = true, SetLastError = true)]
		private static extern int CreateEvent(int lpEventAttributes, int bManualReset, int bInitialState, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern int ResetEvent(int hEvent);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern int SetEvent(int hEvent);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern int CloseHandle(int hObject);
        #endregion

        private static ManagementEventWatcher AWatch = new ManagementEventWatcher();
        private static WqlEventQuery AQuery = new WqlEventQuery();
        public static event EventHandler EventScreenshot;
        
        private static void HandleScreenshot(object sender, EventArrivedEventArgs e)
        {
            EventScreenshot.Invoke();
        }

        public static void HandleSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            bool SessionOff = e.Reason == SessionSwitchReason.ConsoleDisconnect | e.Reason == SessionSwitchReason.SessionLock |
                e.Reason == SessionSwitchReason.SessionLogoff | e.Reason == SessionSwitchReason.RemoteDisconnect;

            bool SessionOn = e.Reason == SessionSwitchReason.ConsoleConnect | e.Reason == SessionSwitchReason.SessionUnlock |
                e.Reason == SessionSwitchReason.SessionLogon | e.Reason == SessionSwitchReason.RemoteConnect;

            if (SessionOff)
            {
                AWatch.Stop();
            }
            else if (SessionOn)
            {
                AWatch.Start();
            }
        }

        public static void AttachEvents()
        {
            AQuery = new WqlEventQuery("SELECT * FROM INVHK7_Event");
            AWatch = new ManagementEventWatcher(new ManagementScope("root/WMI"), AQuery);
            AWatch.EventArrived += HandleScreenshot;
            AWatch.Start();

            SystemEvents.SessionSwitch += HandleSessionSwitch;
        }

    }
}
