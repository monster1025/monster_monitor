using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonsterMonitor.Logic.ProcessMonitor
{
    public class ProcessMonitor : IProcessMonitor
    {
        public string ProcessName { get; }
        private readonly string _relativePath;
        private readonly string _args;
        private CancellationTokenSource _ct { get; set; }

        public ProcessMonitor(string processName, string relativePath, string args = null)
        {
            ProcessName = processName;
            _relativePath = relativePath;
            _args = args;
            _ct = new CancellationTokenSource();
        }

        public void StartMonitor()
        {
            Task.Run(async () => await CheckProcess());
        }

        public bool IsRunning()
        {
            var processList = Process.GetProcesses();
            var processInfo = processList.FirstOrDefault(f => f.ProcessName == ProcessName);
            return (processInfo != null);
        }

        public void Kill()
        {
            var processInfos = Process.GetProcesses().Where(f => f.ProcessName == ProcessName);
            foreach (var processInfo in processInfos)
            {
                processInfo?.Kill();
            }
        }

        public void Stop()
        {
            _ct.Cancel();
            Kill();
        }

        private async Task CheckProcess()
        {
            while (!_ct.IsCancellationRequested)
            {
                var processList = Process.GetProcesses();
                var processInfo = processList.FirstOrDefault(f => f.ProcessName == ProcessName);
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