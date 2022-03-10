using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonsterMonitor.Log;
using MonsterMonitor.Logic;
using MonsterMonitor.Logic.Auth;
using MonsterMonitor.Logic.NoSleep;
using MonsterMonitor.Logic.ProcessMonitor;
using MonsterMonitor.Logic.Settings;
using MonsterMonitor.Logic.Ssh;
using MonsterMonitor.Logic.Update;
using MonsterMonitor.UI.Tray;

namespace MonsterMonitor.UI
{
    public partial class FrmMain : Form
    {
        private readonly ITrayMenu _trayIcon;
        private readonly IEnumerable<IProcessMonitor> _processMonitors;
        private readonly INoSleep _noSleep;
        private readonly IAuthMonitor _authMonitor;
        private readonly IConnectionMonitor _connectionMonitor;
        private readonly ISshTunnel _sshTunnel;
        private readonly IUpdater _updater;
        private readonly ILog _logger;
        private readonly Settings _settings;
        private readonly frmSettings _frmSettings;
        private bool _updateChecked;

        public FrmMain(
            ITrayMenu trayIcon, 
            IEnumerable<IProcessMonitor> processMonitors, 
            INoSleep noSleep, 
            IAuthMonitor authMonitor, 
            IConnectionMonitor connectionMonitor,
            ISshTunnel sshTunnel,
            IUpdater updater, 
            ILog logger,
            Settings settings,
            frmSettings frmSettings)
        {
            InitializeComponent();

            _trayIcon = trayIcon;
            _processMonitors = processMonitors;
            _noSleep = noSleep;
            _authMonitor = authMonitor;
            _connectionMonitor = connectionMonitor;
            _sshTunnel = sshTunnel;
            _updater = updater;
            _logger = logger;
            _settings = settings;
            _frmSettings = frmSettings;
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

        private void Generate3ProxyConfig(Settings settings)
        {
            var config = $"users admin:CL:{settings.ThreeProxyPassword}\r\n" +
                               "auth strong\r\n"+
                               "proxy -n -p3328";
            File.WriteAllText(Path
                .Combine(Application.StartupPath, "App_Data", "3proxy", "3pr.cfg"), config);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _logger.SetTarget(message =>
            {
                txtLog.Invoke(new Action<string>(text =>
                {
                    if (txtLog.Text.Length > 100000)
                    {
                        txtLog.ResetText();
                    }
                    txtLog.AppendText(text);
                }), message + Environment.NewLine);
            });

            this.Text = this.Text + " v" + Application.ProductVersion;
            var checkboxes = new List<CheckBox> {checkBox1, checkBox2, checkBox3};

            //Generate3ProxyConfig(_settings);
            //_logger.Info($"Вы можете авторизоваться на прокси по кредам admin:{_settings.ThreeProxyPassword}");

            int i = 0;
            foreach (var processMonitor in _processMonitors)
            {
                processMonitor.StartMonitor();
                Task.Delay(TimeSpan.FromMilliseconds(500)).GetAwaiter().GetResult();
                checkboxes[i].Checked = processMonitor.IsRunning();
                i++;
            }
            _noSleep.StartMonitor().GetAwaiter().GetResult();
            _authMonitor.StartMonitor().GetAwaiter().GetResult();
            _connectionMonitor.StartMonitor().GetAwaiter().GetResult();

            if (System.Diagnostics.Debugger.IsAttached)
            {
                _logger.Info("Приложение запущено в режиме отладки. Отключаю обновление.");
                tmrUpdate.Enabled = false;
            }
            else
            {
                tmrUpdate.Enabled = true;
            }

            _sshTunnel.Start(_settings.SshHost, _settings.SshPort, _settings.SshUser, _settings.SshPassword);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _trayIcon?.Hide();

            foreach (var processMonitor in _processMonitors)
            {
                processMonitor.Stop();
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
                    _updater.SelfRestart();
                }
            }
            catch (Exception ex)
            {
                //в процессе обновления ни при каком раскладе мы не должны уложить бота. 
                _logger.Info($"Фатальная ошибка в процессе обновления: {ex.Message}");
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            _frmSettings.ShowDialog();
        }
    }
}
