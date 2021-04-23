using System.Windows.Forms;

namespace MonsterMonitor.UI.Tray
{
    public interface ITrayMenu
    {
        NotifyIcon Create(FrmMain form);
        void Show();
        void Hide();
    }
}