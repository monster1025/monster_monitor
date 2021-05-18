using System;
using System.ComponentModel;
using System.ServiceModel.Dispatcher;
using System.Windows.Forms;

namespace MonsterMonitor
{
    public interface ILog
    {
        void SetTarget(Action<string> logAction);

        void Info([Localizable(false)] string message);
        void Trace([Localizable(false)] string message);
        void Warn([Localizable(false)] string message);
        void Error([Localizable(false)] string message);
    }
}