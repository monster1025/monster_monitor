namespace MonsterMonitor.Logic.ProcessMonitor
{
    public interface IProcessMonitor
    {
        void StartMonitor();
        bool IsRunning();
        void Kill();
    }
}
