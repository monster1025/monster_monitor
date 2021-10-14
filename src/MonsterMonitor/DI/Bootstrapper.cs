using System;
using Autofac;
using MonsterMonitor.Log;
using MonsterMonitor.Logic;
using MonsterMonitor.Logic.Auth;
using MonsterMonitor.Logic.NoSleep;
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

            _builder.RegisterType<AutoStartUp>().AsSelf().AsImplementedInterfaces();
            _builder.RegisterType<FrmMain>().AsSelf().AsImplementedInterfaces();
            _builder.RegisterType<NoSleep>().As<INoSleep>().AsImplementedInterfaces();
            _builder.RegisterType<AuthMonitor>().As<IAuthMonitor>().AsImplementedInterfaces();
            _builder.RegisterType<Updater>().As<IUpdater>().AsImplementedInterfaces();
            _builder.RegisterType<ConnectionMonitor>().As<IConnectionMonitor>().AsImplementedInterfaces();
            _builder.RegisterType<frmSettings>().AsSelf().SingleInstance();
            _builder.RegisterType<FrmMain>().AsSelf().SingleInstance();
            _builder.RegisterType<Logger>().As<ILog>().SingleInstance();
            _builder.RegisterType<SshTunnel>().As<ISshTunnel>().SingleInstance();

            _builder.Register(c=>
                new ProcessMonitor("3proxy", "App_Data\\3proxy\\3proxy.exe", "3pr.cfg")
            ).AsSelf().AsImplementedInterfaces();

            _builder.Register(c =>
                new ProcessMonitor("px", "App_Data\\px-v0.4.0\\px.exe")
            ).AsSelf().AsImplementedInterfaces();
            
            //_builder.Register(c =>
            //    new ProcessMonitor("myentunnel", "App_Data\\MyEnTunnel\\myentunnel.exe")
            //).AsSelf().AsImplementedInterfaces();

            var container = _builder.Build();
            return container;
        }
    }
}
