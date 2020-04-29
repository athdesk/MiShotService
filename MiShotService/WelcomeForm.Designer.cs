namespace MiShotService
{
    partial class WelcomeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonInstall = new System.Windows.Forms.Button();
            this.ButtonUninstall = new System.Windows.Forms.Button();
            this.ButtonRun = new System.Windows.Forms.Button();
            this.ButtonKill = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonInstall
            // 
            this.ButtonInstall.Location = new System.Drawing.Point(12, 12);
            this.ButtonInstall.Name = "ButtonInstall";
            this.ButtonInstall.Size = new System.Drawing.Size(100, 80);
            this.ButtonInstall.TabIndex = 0;
            this.ButtonInstall.Text = "Install";
            this.ButtonInstall.UseVisualStyleBackColor = true;
            this.ButtonInstall.Click += new System.EventHandler(this.ButtonInstall_Click);
            // 
            // ButtonUninstall
            // 
            this.ButtonUninstall.Location = new System.Drawing.Point(12, 98);
            this.ButtonUninstall.Name = "ButtonUninstall";
            this.ButtonUninstall.Size = new System.Drawing.Size(100, 80);
            this.ButtonUninstall.TabIndex = 1;
            this.ButtonUninstall.Text = "Uninstall";
            this.ButtonUninstall.UseVisualStyleBackColor = true;
            this.ButtonUninstall.Click += new System.EventHandler(this.ButtonUninstall_Click);
            // 
            // ButtonRun
            // 
            this.ButtonRun.Location = new System.Drawing.Point(118, 12);
            this.ButtonRun.Name = "ButtonRun";
            this.ButtonRun.Size = new System.Drawing.Size(100, 80);
            this.ButtonRun.TabIndex = 2;
            this.ButtonRun.Text = "Run Standalone";
            this.ButtonRun.UseVisualStyleBackColor = true;
            this.ButtonRun.Click += new System.EventHandler(this.ButtonRun_Click);
            // 
            // ButtonKill
            // 
            this.ButtonKill.ForeColor = System.Drawing.Color.Red;
            this.ButtonKill.Location = new System.Drawing.Point(118, 98);
            this.ButtonKill.Name = "ButtonKill";
            this.ButtonKill.Size = new System.Drawing.Size(100, 80);
            this.ButtonKill.TabIndex = 3;
            this.ButtonKill.Text = "Kill All Running Instances";
            this.ButtonKill.UseVisualStyleBackColor = true;
            this.ButtonKill.Click += new System.EventHandler(this.ButtonKill_Click);
            // 
            // WelcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 190);
            this.Controls.Add(this.ButtonKill);
            this.Controls.Add(this.ButtonRun);
            this.Controls.Add(this.ButtonUninstall);
            this.Controls.Add(this.ButtonInstall);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WelcomeForm";
            this.ShowIcon = false;
            this.Text = "MiShot";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonInstall;
        private System.Windows.Forms.Button ButtonUninstall;
        private System.Windows.Forms.Button ButtonRun;
        private System.Windows.Forms.Button ButtonKill;
    }
}