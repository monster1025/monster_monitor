using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonsterMonitor.Log;
using Renci.SshNet;

namespace MonsterMonitor.Logic.Ssh
{
    public class SshTunnel : ISshTunnel
    {
        private readonly ILog _logger;

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

            Task.Run(async () => await StartSshTask(sshHost, sshPort, sshUser, sshPassword));
        }

        private async Task StartSshTask(string sshHost, int sshPort, string sshUser, string sshPassword)
        {
            while (true)
            {
                try
                {
                    Connect(sshHost, sshPort, sshUser, sshPassword);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        public void Connect(string sshHost, int sshPort, string sshUser, string sshPassword)
        {
            var connectionInfo = new ConnectionInfo(sshHost, sshPort, sshUser, 
                ProxyTypes.Http, "127.0.0.1", 3128, "", "",
            new PasswordAuthenticationMethod(sshUser, sshPassword));
            
            using (var client = new SshClient(connectionInfo))
            {
                client.Connect();

                if (!client.IsConnected)
                {
                    _logger.Error("Cant connect to ssh!");
                }
                var cts = new CancellationTokenSource();

                //add 3proxy port
                var port = new ForwardedPortRemote(3329, "127.0.0.1", 3328);
                client.AddForwardedPort(port);
                port.Start();
                port.Exception += (sender, args) =>
                {
                    _logger.Error(args.Exception.ToString());
                    cts.Cancel();
                    if (client?.IsConnected == true)
                    {
                        client.Disconnect();
                    }
                };

                _logger.Info("[+] SSH client connected.");

                var isWindows = TargetIsWindows(client);
                var os = isWindows ? "windows" : "linux";
                _logger.Info($"Target OS type: {os}");

                var command = isWindows ? "ping ya.ru -t0" : "ping ya.ru";
                ExecuteCommand(client, command, cts.Token);

                client.Disconnect();
            }
        }

        private bool TargetIsWindows(SshClient client)
        {
            var versionCommand = client.RunCommand("ver");
            if (versionCommand.ExitStatus == 0)
            {
                return true;
            }

            return false;
        }

        private void ExecuteCommand(SshClient client, string command, CancellationToken cancellationToken)
        {
            var cmd = client.CreateCommand(command);
            var result = cmd.BeginExecute();

            using (var reader = new StreamReader(cmd.OutputStream, Encoding.UTF8, true, 1024, true))
            {
                while (!result.IsCompleted || !reader.EndOfStream)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    string line = reader.ReadLine();
                    if (line != null)
                    {
                        _logger.Info(line);
                    }
                }
            }

            cmd.EndExecute(result);
        }
    }
}
