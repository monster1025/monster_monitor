using System;
using Autofac;
using MonsterMonitor.Log;
using MonsterMonitor.Logic;
using MonsterMonitor.Logic.Auth;
using MonsterMonitor.Logic.NoSleep;
using MonsterMonitor.Logic.PortMonitor;
using MonsterMonitor.Logic.ProcessMonitor;
using MonsterMonitor.Logic.Settings;
using MonsterMonitor.Logic.Ssh;
using MonsterMonitor.Logic.Update;
using MonsterMonitor.UI;
using MonsterMonitor.UI.Startup;
using MonsterMonitor.UI.Tray;

namespace MonsterMonitor.DI
{
    public class Bootstrapper
    {
        private readonly ContainerBuilder _builder;
        public Bootstrapper()
        {
            _builder = new ContainerBuilder();
        }

        public IContainer Build()
        {
            _builder.Register(f => new TrayMenu()).As<ITrayMenu>().AsSelf();

            _builder.Register(c=>Settings.Load()).AsSelf().AsImplementedInterfaces().SingleInstance();

            _builder.RegisterType<AutoStartUp>().AsSelf().SingleInstance();
            _builder.RegisterType<FrmMain>().AsSelf().SingleInstance();
            _builder.RegisterType<NoSleep>().As<INoSleep>().SingleInstance();
            _builder.RegisterType<AuthMonitor>().As<IAuthMonitor>().SingleInstance();
            _builder.RegisterType<PortMonitor>().As<IPortMonitor>().SingleInstance();
            _builder.RegisterType<Updater>().As<IUpdater>().SingleInstance();
            _builder.RegisterType<frmSettings>().AsSelf().SingleInstance();
            _builder.RegisterType<FrmMain>().AsSelf().SingleInstance();
            _builder.RegisterType<Logger>().As<ILog>().SingleInstance();
            _builder.RegisterType<SshTunnel>().As<ISshTunnel>().SingleInstance();

            _builder.Register(c=>
                new ProcessMonitor("socks", "App_Data\\socks\\socks.exe", "socks.cfg")
            ).AsSelf().AsImplementedInterfaces();

            _builder.Register(c =>
                new ProcessMonitor("px", "App_Data\\px-v0.4.0\\px.exe")
            ).AsSelf().AsImplementedInterfaces();

            var container = _builder.Build();
            return container;
        }
    }
}
