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
            this.ButtonRun = new System.Windows.Forms.Button();
            this.ButtonKill = new System.Windows.Forms.Button();
            this.CheckAutoStart = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ButtonRun
            // 
            this.ButtonRun.Location = new System.Drawing.Point(12, 12);
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
            this.ButtonKill.Location = new System.Drawing.Point(119, 12);
            this.ButtonKill.Name = "ButtonKill";
            this.ButtonKill.Size = new System.Drawing.Size(100, 80);
            this.ButtonKill.TabIndex = 3;
            this.ButtonKill.Text = "Stop";
            this.ButtonKill.UseVisualStyleBackColor = true;
            this.ButtonKill.Click += new System.EventHandler(this.ButtonKill_Click);
            // 
            // CheckAutoStart
            // 
            this.CheckAutoStart.AutoSize = true;
            this.CheckAutoStart.Location = new System.Drawing.Point(12, 112);
            this.CheckAutoStart.Name = "CheckAutoStart";
            this.CheckAutoStart.Size = new System.Drawing.Size(199, 21);
            this.CheckAutoStart.TabIndex = 4;
            this.CheckAutoStart.Text = "Run at boot (Current User)";
            this.CheckAutoStart.UseVisualStyleBackColor = true;
            this.CheckAutoStart.CheckedChanged += new System.EventHandler(this.CheckAutoStart_CheckedChanged);
            // 
            // WelcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 145);
            this.Controls.Add(this.CheckAutoStart);
            this.Controls.Add(this.ButtonKill);
            this.Controls.Add(this.ButtonRun);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WelcomeForm";
            this.ShowIcon = false;
            this.Text = "MiShot";
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ButtonRun;
        private System.Windows.Forms.Button ButtonKill;
        private System.Windows.Forms.CheckBox CheckAutoStart;
    }
}