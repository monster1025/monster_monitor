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
            this.txtUpperProxy = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt3ProxyPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSshPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSshUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSshPass = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkAutostart
            // 
            this.chkAutostart.AutoSize = true;
            this.chkAutostart.Location = new System.Drawing.Point(104, 171);
            this.chkAutostart.Name = "chkAutostart";
            this.chkAutostart.Size = new System.Drawing.Size(96, 17);
            this.chkAutostart.TabIndex = 40;
            this.chkAutostart.Text = "Автозагрузка";
            this.chkAutostart.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Пароль STerra:";
            // 
            // txtSTerraPassword
            // 
            this.txtSTerraPassword.Location = new System.Drawing.Point(104, 4);
            this.txtSTerraPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSTerraPassword.Name = "txtSTerraPassword";
            this.txtSTerraPassword.PasswordChar = '*';
            this.txtSTerraPassword.Size = new System.Drawing.Size(144, 20);
            this.txtSTerraPassword.TabIndex = 52;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(104, 207);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 32;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtSshHost
            // 
            this.txtSshHost.Location = new System.Drawing.Point(104, 27);
            this.txtSshHost.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSshHost.Name = "txtSshHost";
            this.txtSshHost.Size = new System.Drawing.Size(144, 20);
            this.txtSshHost.TabIndex = 54;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "SSH-Host:";
            // 
            // txtUpperProxy
            // 
            this.txtUpperProxy.Location = new System.Drawing.Point(104, 118);
            this.txtUpperProxy.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtUpperProxy.Name = "txtUpperProxy";
            this.txtUpperProxy.Size = new System.Drawing.Size(144, 20);
            this.txtUpperProxy.TabIndex = 57;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "Корп. Proxy:";
            // 
            // txt3ProxyPassword
            // 
            this.txt3ProxyPassword.Location = new System.Drawing.Point(104, 141);
            this.txt3ProxyPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txt3ProxyPassword.Name = "txt3ProxyPassword";
            this.txt3ProxyPassword.Size = new System.Drawing.Size(144, 20);
            this.txt3ProxyPassword.TabIndex = 59;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 58;
            this.label3.Text = "Пароль 3Proxy:";
            // 
            // txtSshPort
            // 
            this.txtSshPort.Location = new System.Drawing.Point(104, 50);
            this.txtSshPort.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSshPort.Name = "txtSshPort";
            this.txtSshPort.Size = new System.Drawing.Size(144, 20);
            this.txtSshPort.TabIndex = 61;
            this.txtSshPort.Text = "443";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 60;
            this.label4.Text = "SSH-Port:";
            // 
            // txtSshUser
            // 
            this.txtSshUser.Location = new System.Drawing.Point(104, 72);
            this.txtSshUser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSshUser.Name = "txtSshUser";
            this.txtSshUser.Size = new System.Drawing.Size(144, 20);
            this.txtSshUser.TabIndex = 63;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 62;
            this.label5.Text = "SSH User:";
            // 
            // txtSshPass
            // 
            this.txtSshPass.Location = new System.Drawing.Point(104, 95);
            this.txtSshPass.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSshPass.Name = "txtSshPass";
            this.txtSshPass.Size = new System.Drawing.Size(144, 20);
            this.txtSshPass.TabIndex = 65;
            this.txtSshPass.UseSystemPasswordChar = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(36, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 64;
            this.label7.Text = "SSH Pass:";
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 239);
            this.Controls.Add(this.txtSshPass);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSshUser);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSshPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt3ProxyPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUpperProxy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSshHost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSTerraPassword);
            this.Controls.Add(this.chkAutostart);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
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
        private System.Windows.Forms.TextBox txtUpperProxy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt3ProxyPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSshPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSshUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSshPass;
        private System.Windows.Forms.Label label7;
    }
}