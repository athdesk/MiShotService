using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiShotService
{
    public partial class HelperForm : Form
    {
		[DllImport("user32.dll")]
		private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

		public bool Ready = false;

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

		public HelperForm()
        {
            InitializeComponent();
        }

		private void HelperForm_Load(object sender, EventArgs e)
		{
			this.Activate();
			OpenScreenshotTool();
			Ready = true;
			this.Close();
		}
	}
}
