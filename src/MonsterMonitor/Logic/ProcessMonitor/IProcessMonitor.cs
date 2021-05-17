namespace MonsterMonitor.Logic.ProcessMonitor
{
    public interface IProcessMonitor
    {
        string ProcessName { get; }

        void StartMonitor();
        bool IsRunning();
        void Kill();
    }
}
