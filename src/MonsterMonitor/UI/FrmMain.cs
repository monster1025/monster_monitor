using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonsterMonitor.Logic.Auth;
using MonsterMonitor.Logic.NoSleep;
using MonsterMonitor.Logic.ProcessMonitor;
using MonsterMonitor.Logic.Settings;
using MonsterMonitor.Logic.Update;
using MonsterMonitor.UI.Tray;
using NLog;

namespace MonsterMonitor.UI
{
    public partial class FrmMain : Form
    {
        private readonly ITrayMenu _trayIcon;
        private readonly IEnumerable<IProcessMonitor> _processMonitors;
        private readonly INoSleep _noSleep;
        private readonly IAuthMonitor _authMonitor;
        private readonly IUpdater _updater;
        private readonly ILogger _logger;
        private readonly Settings _settings;
        private bool _updateChecked;

        public FrmMain(ITrayMenu trayIcon, IEnumerable<IProcessMonitor> processMonitors, INoSleep noSleep, IAuthMonitor authMonitor, IUpdater updater, ILogger logger, Settings settings)
        {
            InitializeComponent();

            _trayIcon = trayIcon;
            _processMonitors = processMonitors;
            _noSleep = noSleep;
            _authMonitor = authMonitor;
            _updater = updater;
            _logger = logger;
            _settings = settings;
            _trayIcon.Create(this);
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                _trayIcon.Hide();
            }

            base.WndProc(ref message);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " v" + Application.ProductVersion;

            txtPassword.Text = _settings.SystemPassword;
            var checkboxes = new List<CheckBox> {checkBox1, checkBox2};

            int i = 0;
            foreach (var processMonitor in _processMonitors)
            {
                processMonitor.StartMonitor();
                Task.Delay(TimeSpan.FromMilliseconds(100)).GetAwaiter().GetResult();
                checkboxes[i].Checked = processMonitor.IsRunning();
                i++;
            }
            _noSleep.StartMonitor();
            _authMonitor.StartMonitor();

            if (System.Diagnostics.Debugger.IsAttached)
            {
                _logger.Trace("Приложение запущено в режиме отладки. Отключаю обновление.");
                tmrUpdate.Enabled = true;
            }
            else
            {
                tmrUpdate.Enabled = true;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _trayIcon?.Hide();

            foreach (var processMonitor in _processMonitors)
            {
                processMonitor.Kill();
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                _trayIcon?.Show();
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                _trayIcon?.Hide();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _settings.SystemPassword = txtPassword.Text;
            _settings.Save();
            MessageBox.Show("Saving done", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            try
            {
                var firstTime = !_updateChecked;
                _updateChecked = true;
                tmrUpdate.Interval = 1 * 60 * 60 * 1000;

                
                if (_updater.UpdateToNewVersion(firstTime))
                {
                    //если обновились - отключаем до рестарта
                    tmrUpdate.Enabled = false;
                    var result = MessageBox.Show($"Приложение обновлено до новой версии. Перезапустить сейчас?",
                        "Обновление",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        _updater.SelfRestart();
                    }
                }
            }
            catch (Exception ex)
            {
                //в процессе обновления ни при каком раскладе мы не должны уложить бота. 
                _logger.Error($"Фатальная ошибка в процессе обновления: {ex.Message}");
            }
        }
    }
}
