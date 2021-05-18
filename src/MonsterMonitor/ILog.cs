﻿using System.ComponentModel;

namespace MonsterMonitor
{
    public interface ILog
    {
        void Info([Localizable(false)] string message);
        void Trace([Localizable(false)] string message);
        void Warn([Localizable(false)] string message);
        void Error([Localizable(false)] string message);
    }
}