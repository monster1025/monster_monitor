using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonsterMonitor.Logic.ProcessMonitor
{
    public class ProcessMonitor : IProcessMonitor
    {
        private readonly string _processName;
        private readonly string _relativePath;
        private readonly string _args;

        public ProcessMonitor(string processName, string relativePath, string args = null)
        {
            _processName = processName;
            _relativePath = relativePath;
            _args = args;
        }

        public void StartMonitor()
        {
            Task.Run(async () => await CheckProcess());
            //CheckProcess().GetAwaiter().GetResult();
        }

        public bool IsRunning()
        {
            var processList = Process.GetProcesses();
            var processInfo = processList.FirstOrDefault(f => f.ProcessName == _processName);
            return (processInfo != null);
        }

        public void Kill()
        {
            var processInfos = Process.GetProcesses().Where(f => f.ProcessName == _processName);
            foreach (var processInfo in processInfos)
            {
                processInfo?.Kill();
            }
        }

        private async Task CheckProcess()
        {
            while (true)
            {
                var processList = Process.GetProcesses();
                var processInfo = processList.FirstOrDefault(f => f.ProcessName == _processName);
                if (processInfo == null)
                {
                    var path = Path.Combine(new FileInfo(Application.StartupPath).FullName, _relativePath);
                    var workDir = new FileInfo(path).DirectoryName;

                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = path,
                            WorkingDirectory = workDir,
                            Arguments = _args,
                            UseShellExecute = false,
                            //RedirectStandardOutput = true,
                            CreateNoWindow = true,
                        }
                    };
                    process.Start();
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}