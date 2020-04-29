﻿using System;
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

        private static MiShot StandaloneInstance = null;

        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void ButtonInstall_Click(object sender, EventArgs e)
        {
            Program.CaseInstall();
        }

        private void ButtonUninstall_Click(object sender, EventArgs e)
        {
            Program.CaseUninstall();
        }

        private void ButtonRun_Click(object sender, EventArgs e)
        {
            if (StandaloneInstance != null)
            {
                StandaloneInstance.Stop();
                StandaloneInstance.Dispose();
                StandaloneInstance = null;
            }
            StandaloneInstance = Program.CaseStandalone();
        }

        private void ButtonKill_Click(object sender, EventArgs e)
        {
            if (StandaloneInstance != null)
            {
                StandaloneInstance.Stop();
                StandaloneInstance.Dispose();
                StandaloneInstance = null;
            }
            Program.CaseKill(true);
        }
    }
}
