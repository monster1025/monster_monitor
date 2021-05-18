﻿using System;
using System.ComponentModel;

namespace MonsterMonitor.Log
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