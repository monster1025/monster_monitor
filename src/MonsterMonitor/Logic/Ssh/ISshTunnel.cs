namespace MonsterMonitor.Logic.Ssh
{
    public interface ISshTunnel
    {
        void Start(string settingsSshHost, int settingsSshPort, string settingsSshUser, string settingsSshPassword);
    }
}