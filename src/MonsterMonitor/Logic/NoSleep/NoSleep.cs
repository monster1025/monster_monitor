using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonsterMonitor.Log;

namespace MonsterMonitor.Logic.NoSleep
{
    public class NoSleep: INoSleep
    {
        private readonly ILog _logger;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        public NoSleep(ILog logger)
        {
            _logger = logger;
        }

        public void StartMonitor()
        {
            Task.Run(async () => await CheckProcess());
        }

        private async Task CheckProcess()
        {
            try
            {
                _logger.Info("Starting no-sleep");
                if (SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS
                                            | EXECUTION_STATE.ES_DISPLAY_REQUIRED
                                            | EXECUTION_STATE.ES_SYSTEM_REQUIRED
                                            | EXECUTION_STATE.ES_AWAYMODE_REQUIRED) == 0)
                {
                    SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS
                                            | EXECUTION_STATE.ES_DISPLAY_REQUIRED
                                            | EXECUTION_STATE.ES_SYSTEM_REQUIRED); //Windows < Vista, forget away mode
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
    }


    [FlagsAttribute]
    public enum EXECUTION_STATE : uint
    {
        ES_SYSTEM_REQUIRED = 0x00000001,
        ES_DISPLAY_REQUIRED = 0x00000002,
        // Legacy flag, should not be used.
        // ES_USER_PRESENT   = 0x00000004,
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
    }
}