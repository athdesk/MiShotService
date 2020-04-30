using System;
using System.Windows.Forms;

namespace MiShotService
{
    public partial class WelcomeForm : Form
    {

        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void ButtonRun_Click(object sender, EventArgs e)
        {
           
            Program.CaseStandalone(true);
        }

        private void ButtonKill_Click(object sender, EventArgs e)
        {
            Program.CaseKill(true);
        }

        private void CheckAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckAutoStart.Checked)
                Program.CaseInstall(true);
            else
                Program.CaseUninstall(true);
        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            CheckAutoStart.Checked = WinInterface.IsOnStartup();
        }
    }
}
