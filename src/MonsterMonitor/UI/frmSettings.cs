using System;
using System.Windows.Forms;
using MonsterMonitor.Logic.Settings;
using MonsterMonitor.UI.Startup;

namespace MonsterMonitor.UI
{
    public partial class frmSettings : Form
    {
        private readonly AutoStartUp _autoStart;

        public frmSettings(AutoStartUp autoStart)
        {
            _autoStart = autoStart;
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            var settings = Settings.Load();
            if (settings != null)
            {
                SetSettings(settings);
            }

            chkAutostart.Checked = _autoStart.Get();
        }

        private void SetSettings(Settings settings)
        {
            txtSTerraPassword.Text = settings.SystemPassword;
            txtSshHost.Text = settings.SshHost;
            chkPingCheck.Checked = settings.PingCheck;
            txtUpperProxy.Text = settings.Proxy;
            txt3ProxyPassword.Text = settings.ThreeProxyPassword;
        }

        public Settings ReadSettingsAndLock()
        {
            var settings = new Settings
            {
                SystemPassword = txtSTerraPassword.Text,
                SshHost = txtSshHost.Text,
                PingCheck = chkPingCheck.Checked,
                Proxy = txtUpperProxy.Text,
                ThreeProxyPassword = txt3ProxyPassword.Text
            };

            return settings;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var settings = ReadSettingsAndLock();
            settings.Save();
            _autoStart.Set(chkAutostart.Checked);

            MessageBox.Show("Настройки сохранены.");

            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
