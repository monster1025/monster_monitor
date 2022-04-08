using System.Threading.Tasks;

namespace MonsterMonitor.Logic.PortMonitor
{
    public interface IPortMonitor
    {
        Task StartMonitor();
    }
}
