using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using MonsterMonitor.DI;
using MonsterMonitor.UI;
using MonsterMonitor.UI.Tray;

namespace MonsterMonitor
{
    static class Program
    {
        private static readonly Mutex Mutex = new Mutex(true, "{44426B3E-B901-4792-ACEA-1385D79DBAD1}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                var container = new Bootstrapper().Build();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(container.Resolve<FrmMain>());
                Mutex.ReleaseMutex();
            }
            else
            {
                SingleInstance.ShowFirstInstance();
            }
        }
    }
}
