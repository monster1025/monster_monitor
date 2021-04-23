namespace MonsterMonitor.Logic.Update
{
    public interface IUpdater
    {
        bool UpdateToNewVersion(bool firstTime);
        void SelfRestart();
    }
}
