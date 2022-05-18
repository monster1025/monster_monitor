using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using MihaZupan;
using MonsterMonitor.Log;
using MonsterMonitor.Logic.NoSleep;

namespace MonsterMonitor.Logic.PortMonitor
{
    public class PortMonitor: IPortMonitor
    {
        private readonly ILog _logger;

        public PortMonitor(ILog logger)
        {
            _logger = logger;
        }

        public Task StartMonitor()
        {
            _ = Task.Run(async () => await CheckProcess());
            return Task.CompletedTask;
        }

        private async Task CheckProcess()
        {
            try
            {
                const int port = 2180;
                _logger.Info("Starting port monitor");
                while (true)
                {
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    
                    var isOpen = await CheckProxyAsync(port); // ScanPort(port);
                    if (isOpen)
                    {
                        _logger.Info($"[PingCheck] Прокси проверен и жив.");
                    }
                    else
                    {
                        _logger.Info($"Прокси на порту {port} не отвечает. Перезапускаю.");
                        Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        public void Kill()
        {
            var processInfos = Process.GetProcesses().Where(f => f.ProcessName == "socks");
            foreach (var processInfo in processInfos)
            {
                processInfo?.Kill();
            }
        }

        private async Task<bool> CheckProxyAsync(int port)
        {
            var handler = new HttpClientHandler
            {
                Proxy = new HttpToSocks5Proxy("127.0.0.1", port),
                //Proxy = new WebProxy { Address = new Uri($"socks5://127.0.0.1:{port}") },
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
            var client = new HttpClient(handler);
            try
            {
                var ok = await client.GetAsync("http://jira.sibur.local");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Info($"[PingCheck] Ошибка подключения к прокси: {ex.Message}");
                return false;
            }
        }

        private bool ScanPort(int port)
        {
            using TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect("127.0.0.1", port);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
