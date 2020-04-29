using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
