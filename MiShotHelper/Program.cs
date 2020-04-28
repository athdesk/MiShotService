using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MiShotHelper
{
	class Program
	{
        #region DllImports
        [DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		[DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
		static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		#endregion

		#region ApiConstants
		const int SW_HIDE = 0;
		const int SW_SHOW = 5;
		const int WM_COMMAND = 0x111;
		const int MIN_ALL = 419;
		const int MIN_ALL_UNDO = 416;
        #endregion


        private static void OpenScreenshotTool()
		{
			const byte LWin_Key = 0x5B;
			const byte LShift_Key = 0xA0;
			const byte S_Key = 0x53;

			PressKey(LWin_Key);
			PressKey(LShift_Key);
			PressKey(S_Key);

			ReleaseKey(S_Key);
			ReleaseKey(LShift_Key);
			ReleaseKey(LWin_Key);
		}
		private static void PressKey(byte KeyCode)
		{
			const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
			keybd_event(KeyCode, 0, KEYEVENTF_EXTENDEDKEY | 0, 0);
		}

		private static void ReleaseKey(byte KeyCode)
		{
			const uint KEYEVENTF_KEYUP = 0x0002;
			const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
			keybd_event(KeyCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
		}
		private static Process GetExplorerProcess()
		{
			var all = Process.GetProcessesByName("dwm");
			Process process = null;
			foreach (var p in all)
				if (process == null || p.StartTime < process.StartTime)
					process = p;
			return process;
		}

		private static void UnfocusAll()
		{
			SetForegroundWindow(GetExplorerProcess().MainWindowHandle);
		}

		static void Main(string[] args)
		{
			UnfocusAll();
			ShowWindow(GetConsoleWindow(), SW_HIDE);
			OpenScreenshotTool();
		}
	}
}
