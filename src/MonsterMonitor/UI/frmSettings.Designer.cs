namespace MonsterMonitor.UI
{
    partial class frmSettings
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
            this.chkAutostart = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSTerraPassword = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtSshHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkPingCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkAutostart
            // 
            this.chkAutostart.AutoSize = true;
            this.chkAutostart.Location = new System.Drawing.Point(139, 62);
            this.chkAutostart.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutostart.Name = "chkAutostart";
            this.chkAutostart.Size = new System.Drawing.Size(118, 21);
            this.chkAutostart.TabIndex = 40;
            this.chkAutostart.Text = "Автозагрузка";
            this.chkAutostart.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 9);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 17);
            this.label6.TabIndex = 35;
            this.label6.Text = "Пароль STerra:";
            // 
            // txtSTerraPassword
            // 
            this.txtSTerraPassword.Location = new System.Drawing.Point(139, 5);
            this.txtSTerraPassword.Name = "txtSTerraPassword";
            this.txtSTerraPassword.PasswordChar = '*';
            this.txtSTerraPassword.Size = new System.Drawing.Size(191, 22);
            this.txtSTerraPassword.TabIndex = 52;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(139, 126);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 32;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtSshHost
            // 
            this.txtSshHost.Location = new System.Drawing.Point(139, 33);
            this.txtSshHost.Name = "txtSshHost";
            this.txtSshHost.Size = new System.Drawing.Size(191, 22);
            this.txtSshHost.TabIndex = 54;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 17);
            this.label1.TabIndex = 53;
            this.label1.Text = "SSH-Host:";
            // 
            // chkPingCheck
            // 
            this.chkPingCheck.AutoSize = true;
            this.chkPingCheck.Location = new System.Drawing.Point(139, 91);
            this.chkPingCheck.Margin = new System.Windows.Forms.Padding(4);
            this.chkPingCheck.Name = "chkPingCheck";
            this.chkPingCheck.Size = new System.Drawing.Size(139, 21);
            this.chkPingCheck.TabIndex = 55;
            this.chkPingCheck.Text = "\"PING\" проверка";
            this.chkPingCheck.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 177);
            this.Controls.Add(this.chkPingCheck);
            this.Controls.Add(this.txtSshHost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSTerraPassword);
            this.Controls.Add(this.chkAutostart);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAutostart;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSTerraPassword;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtSshHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkPingCheck;
    }
}