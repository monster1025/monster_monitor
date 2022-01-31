using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonsterMonitor.Log;

namespace MonsterMonitor.Logic.NoSleep
{
    public class NoSleep: INoSleep
    {
        private readonly ILog _logger;

        public NoSleep(ILog logger)
        {
            _logger = logger;
        }

        public async Task StartMonitor()
        {
            _ = Task.Run(async () => await CheckProcess());
        }

        private async Task CheckProcess()
        {
            try
            {
                _logger.Info("Starting no-sleep");
                var noSleepHelper = new NoSleepHelper();
                noSleepHelper.EnableConstantDisplayAndPower(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
    }
}