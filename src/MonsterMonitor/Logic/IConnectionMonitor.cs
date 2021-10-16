using System.Threading.Tasks;

namespace MonsterMonitor.Logic
{
    public interface IConnectionMonitor
    { 
        Task StartMonitor();
    }
}