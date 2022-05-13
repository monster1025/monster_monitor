using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonsterMonitor.Log;
using Renci.SshNet;

namespace MonsterMonitor.Logic.Ssh
{
    public class SshTunnel : ISshTunnel
    {
        private readonly ILog _logger;
        private DateTimeOffset _lastPingDate = DateTimeOffset.Now;
        private SshClient _client = null;
        private CancellationTokenSource _cancellationTokenSource = null;
        
        public SshTunnel(ILog logger)
        {
            _logger = logger;
        }

        public void Start(string sshHost, int sshPort, string sshUser, string sshPassword)
        {
            if (string.IsNullOrEmpty(sshHost))
            {
                return;
            }
            if (string.IsNullOrEmpty(sshUser))
            {
                return;
            }
            if (string.IsNullOrEmpty(sshPassword))
            {
                return;
            }
            Task.Run(async () => await PingWatcher(sshHost, sshPort, sshUser, sshPassword));
        }

        private async Task PingWatcher(string sshHost, int sshPort, string sshUser, string sshPassword)
        {
            bool first = true;
            while (true)
            {
                if (DateTimeOffset.Now - _lastPingDate <= TimeSpan.FromMinutes(5) && !first)
                {
                    continue;
                }

                if (!first)
                {
                    _logger.Info("Не получен ответ от сервера за 5 сек. Переподключаюсь.");
                    _cancellationTokenSource?.Cancel();

                    if (_client?.IsConnected == true)
                    {
                        _client.Disconnect();
                        _client.Dispose();
                    }
                }

                _cancellationTokenSource = new CancellationTokenSource();
                Task.Run(async () => await StartSshTask(sshHost, sshPort, sshUser, sshPassword, _cancellationTokenSource));
                first = false;

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        private async Task StartSshTask(string sshHost, int sshPort, string sshUser, string sshPassword,
            CancellationTokenSource cancellationTokenSource)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    _logger.Info($"Пытаюсь подключиться по адресу: {sshHost}:{sshPort}.");
                    await Connect(sshHost, sshPort, sshUser, sshPassword, cancellationTokenSource);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        public async Task Connect(string sshHost, int sshPort, string sshUser, string sshPassword,
            CancellationTokenSource cancellationTokenSource)
        {
            var proxyType = ProxyTypes.Http;
            if (Debugger.IsAttached)
            {
                proxyType = ProxyTypes.None;
            }

            _logger.Info($"[Connect] Пытаюсь подключиться по адресу: {sshHost}:{sshPort}.");

            var connectionInfo = new ConnectionInfo(sshHost, sshPort, sshUser,
                proxyType, "127.0.0.1", 3128, "", "",
            new PasswordAuthenticationMethod(sshUser, sshPassword));

            _client = new SshClient(connectionInfo);
            _client.Connect();
            _client.KeepAliveInterval = TimeSpan.FromSeconds(5);

            if (!_client.IsConnected)
            {
                _logger.Error("[-] Cant connect to ssh!");
            }

            _client.ErrorOccurred += (sender, args) =>
            {
                _logger.Error($"[-] Client exception: {args.Exception}");
                cancellationTokenSource.Cancel();
                if (_client?.IsConnected == true)
                {
                    _logger.Info("[-] SSH client disconnected.");
                    _client.Disconnect();
                }
            };

            //add 3proxy port
            var port = new ForwardedPortRemote(3329, "127.0.0.1", 2180);
            _client.AddForwardedPort(port);
            port.Start();
            port.Exception += (sender, args) =>
            {
                _logger.Error($"[-] Port forward exception: {args.Exception}");
                _logger.Info("[-] SSH client disconnected.");
                cancellationTokenSource.Cancel();
                if (_client?.IsConnected == true)
                {
                    _client.Disconnect();
                }
            };

            _logger.Info("[+] SSH client connected.");

            var isWindows = TargetIsWindows(_client);
            var os = isWindows ? "windows" : "linux";
            _logger.Info($"Target OS type: {os}");

            var command = isWindows ? "ping ya.ru -t0" : "ping ya.ru";
            await ExecuteCommand(_client, command, cancellationTokenSource.Token);

            _client.Disconnect();
        }

        private bool TargetIsWindows(SshClient client)
        {
            var versionCommand = client.RunCommand("ver");
            return versionCommand.ExitStatus == 0;
        }

        private async Task ExecuteCommand(SshClient client, string command, CancellationToken cancellationToken)
        {
            var cmd = client.CreateCommand(command);
            var result = cmd.BeginExecute();

            using (var reader = new StreamReader(cmd.OutputStream, Encoding.UTF8, true, 1024, true))
            {
                while (!result.IsCompleted || !reader.EndOfStream)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    string line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        _logger.Info(line);
                        _lastPingDate = DateTimeOffset.Now;
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                }
            }

            cmd.EndExecute(result);
        }
    }
}
