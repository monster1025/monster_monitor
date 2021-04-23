using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace MonsterMonitor.Logic.Auth
{
    public class AuthMonitor: IAuthMonitor
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private readonly Settings.Settings _settings;

        public AuthMonitor(Settings.Settings settings)
        {
            _settings = settings;
        }

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            var buff = new StringBuilder(nChars);
            var handle = GetForegroundWindow();
            return GetWindowText(handle, buff, nChars) > 0 ? buff.ToString() : null;
        }

        public void StartMonitor()
        {
            Task.Run(async () => await CheckProcess());
        }

        private async Task CheckProcess()
        {
            var inputSimulator = new InputSimulator();
            while (true)
            {
                try
                {
                    Process[] processlist = Process.GetProcesses();
                    foreach (Process process in processlist)
                    {
                        if (!string.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowTitle.Contains("XAuth request"))
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                        }
                    }

                    var activeWindowTitle = GetActiveWindowTitle();
                    if (activeWindowTitle?.Contains("XAuth request") == true)
                    {
                        inputSimulator.Keyboard.TextEntry(_settings.SystemPassword).KeyPress(VirtualKeyCode.RETURN);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace);
                }

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }

    public interface IAuthMonitor
    {
        void StartMonitor();
    }
}
