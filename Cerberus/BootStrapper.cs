using Cerberus.Common;
using Cerberus.ViewModels;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using ServiceClient;
using ServiceClient.Infrastructure;
using ServiceClient.Interfaces;

namespace Cerberus
{
    public class BootStrapper
    {
        private SimpleIocContainer _container;
        public BootStrapper()
        {
            this._container = new SimpleIocContainer();
        }

        public SimpleIocContainer ServiceLocator
        {
            get
            {
                return this._container;
            }
        }

        public void Run()
        {
            _container.RegisterType<Cerberus.Interfaces.ISettingsProvider, SettingsProvider>(LifeCycle.Singleton);
            _container.RegisterType<IWorkAsync, WorkAsync>(LifeCycle.Singleton);
            _container.RegisterType<INavigationService, NavigationService>(LifeCycle.Singleton);

            //var rootFrame = new NavigationService( new System.Windows.Controls.Frame();;
            //_container.RegisterInstance<INavigationService>(rootFrame);

            _container.RegisterType<IProxy, Proxy>();
            _container.RegisterType<ServiceContext, ServiceContext>();
            _container.RegisterType<ServiceBus, ServiceBus>();
            _container.RegisterType<IEventAggregator, EventAggregator>(LifeCycle.Singleton);

            _container.RegisterType<DashBoardViewModel>();
            _container.RegisterType<TrendViewViewModel>();
            _container.RegisterType<CommunicationManagerViewModel>();
            _container.RegisterType<TestViewViewModel>();
            _container.RegisterType<SkinnableViewViewModel>();
        }
    }
}
