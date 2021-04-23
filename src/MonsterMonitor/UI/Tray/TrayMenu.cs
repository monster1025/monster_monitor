﻿using System;
using System.Windows.Forms;

namespace MonsterMonitor.UI.Tray
{
    public class TrayMenu : ITrayMenu
    {
        private NotifyIcon _trayIcon;

        public NotifyIcon Create(FrmMain form)
        {
            var trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            _trayIcon = new NotifyIcon
            {
                Text = "MonsterMonitor",
                Icon = form.Icon,
                ContextMenu = trayMenu,
                Visible = false,
                Tag = form
            };
            _trayIcon.Click += btnTray_Click;
            _trayIcon.DoubleClick += btnTray_Click;

            return _trayIcon;
        }

        public void Show()
        {
            _trayIcon.Visible = true;
        }

        public void Hide()
        {
            _trayIcon.Visible = false;
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTray_Click(object sender, EventArgs e)
        {
            var icon = (NotifyIcon)sender;
            var form = (FrmMain)icon.Tag;
            form.Visible = true;
            form.Activate();
            form.BringToFront();
            RestoreFromTray.Restore(form);
        }
    }
}
