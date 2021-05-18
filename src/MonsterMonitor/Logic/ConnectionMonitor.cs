using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MonsterMonitor.Logic.ProcessMonitor;

namespace MonsterMonitor.Logic
{
    public class ConnectionMonitor: IConnectionMonitor
    {
        private readonly IEnumerable<IProcessMonitor> _processMonitors;
        private readonly ILog _logger;
        private readonly Settings.Settings _settings;
        private readonly HttpClient _client;

        public ConnectionMonitor(IEnumerable<IProcessMonitor> processMonitors, ILog logger, Settings.Settings settings)
        {
            _processMonitors = processMonitors;
            _logger = logger;
            _settings = settings;
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        public void StartMonitor()
        {
            Task.Run(async () => await CheckProcess());
        }

        private async Task CheckProcess()
        {
            int fails = 0;
            while (true)
            {
                if (_settings.PingCheck)
                {
                    try
                    {
                        var result = await _client.GetAsync("http://127.0.0.1:7777/raw");
                        if (result.StatusCode == HttpStatusCode.NotFound)
                        {
                            fails = 0;
                        }
                        else
                        {
                            fails++;
                        }
                    }
                    catch (Exception ex)
                    {
                        fails++;
                        _logger.Warn($"Error ping check ({fails}): {ex}");
                        //MessageBox.Show($"Error ping check ({fails}): {ex}", "");
                    }

                    if (fails >= 3)
                    {
                        var process = _processMonitors.FirstOrDefault(f => f.ProcessName.Contains("myentunnel"));
                        process?.Kill();
                        fails = 0;
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }
    }
}
