using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MonsterMonitor
{
    public class Logger : ILog
    {
        private Action<string> _logAction;

        public void SetTarget(Action<string> logAction)
        {
            _logAction = logAction;
        }

        private void AppendText(string message)
        {
            message = $"{DateTime.Now:HH:mm} {message}";
            _logAction?.Invoke(message);
        }

        public void Info([Localizable(false)] string message)
        {
            AppendText(message);
        }

        public void Trace([Localizable(false)] string message)
        {
            AppendText(message);
        }

        public void Warn([Localizable(false)] string message)
        {
            AppendText(message);
        }

        public void Error([Localizable(false)] string message)
        {
            AppendText(message);
        }
    }
}