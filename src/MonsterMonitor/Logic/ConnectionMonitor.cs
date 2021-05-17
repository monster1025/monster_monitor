using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonsterMonitor.Logic.ProcessMonitor;
using NLog;

namespace MonsterMonitor.Logic
{
    public class ConnectionMonitor: IConnectionMonitor
    {
        private readonly IEnumerable<IProcessMonitor> _processMonitors;
        private readonly ILogger _logger;
        private HttpClient _client;

        public ConnectionMonitor(IEnumerable<IProcessMonitor> processMonitors, ILogger logger)
        {
            _processMonitors = processMonitors;
            _logger = logger;
            _client = new HttpClient();
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
                    _logger.Warn($"Error ping check ({fails}): {ex}");
                    MessageBox.Show($"Error ping check ({fails}): {ex}", "");
                }

                if (fails > 3)
                {
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    var process = _processMonitors.FirstOrDefault(f => f.ProcessName.Contains("myentunnel"));
                    process?.Kill();
                    fails = 0;
                }

                await Task.Delay(TimeSpan.FromSeconds(60));
            }
        }
    }
}
