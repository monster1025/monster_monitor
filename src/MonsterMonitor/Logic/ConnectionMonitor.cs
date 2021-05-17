using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;
using System.Text;
using System.Threading.Tasks;
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
                string result = "";
                try
                { 
                    result = await _client.GetStringAsync("http://127.0.0.1:7777/raw");
                }
                catch (Exception ex)
                {
                    _logger.Warn($"Error ping check ({fails}): {ex}");
                }

                if (string.IsNullOrEmpty(result))
                {
                    fails++;
                }
                else
                {
                    fails = 0;
                }

                if (fails > 10)
                {
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    var process = _processMonitors.FirstOrDefault(f => f.ProcessName.Contains("myentunnel"));
                    process?.Kill();
                    fails = 0;
                }
            }
        }
    }
}
