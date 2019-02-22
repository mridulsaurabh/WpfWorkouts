using Cerberus.Events;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Cerberus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string PRODUCT_NAME = "Cerberus";
        private static BootStrapper _strapper;
        private Mutex m_InstanceMutex = null;
        public static SimpleIocContainer CerberusContainer
        {
            get
            {
                return _strapper.ServiceLocator;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            bool isCreateNewInstance = true;
            m_InstanceMutex = new Mutex(true, PRODUCT_NAME, out isCreateNewInstance);
            if (!isCreateNewInstance)
            {
                m_InstanceMutex = null;
                Application.Current.Shutdown();
                return;
            }
            _strapper = new BootStrapper();
            _strapper.Run();

            var eventAggregator = CerberusContainer.Resolve<IEventAggregator>();
            eventAggregator.Subscribe<ApplicationSettingsEventMessage>(OnApplicationSettingChanged);
            // Apply default application theme
            this.AddCurrentSkin();
            this.AddCurrentTheme();
            this.AddCurrentBackground();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            if (m_InstanceMutex != null)
            {
                m_InstanceMutex.ReleaseMutex();
            }
            base.OnExit(e);
        }

        private void AddCurrentSkin()
        {
            try
            {
                ResourceDictionary skin =
                    Application.LoadComponent(new Uri("/Cerberus;component/Resources/Skins/" +
                                        Cerberus.Properties.Settings.Default.Skin + ".xaml", UriKind.Relative)) as ResourceDictionary;
                this.Resources.MergedDictionaries.Add(skin);
            }
            catch
            {
                // skin missing. Reset to default.
                ResourceDictionary skin =
                   Application.LoadComponent(new Uri("/Cerberus;component/Resources/Skins/Default.xaml", UriKind.Relative)) as ResourceDictionary;
                this.Resources.MergedDictionaries.Add(skin);
                Cerberus.Properties.Settings.Default.Skin = "Default";
            }
        }
        private void AddCurrentTheme()
        {
            try
            {
                ResourceDictionary theme =
                    Application.LoadComponent(new Uri("/Cerberus;component/Resources/Themes/" +
                                        Cerberus.Properties.Settings.Default.Theme + ".xaml", UriKind.Relative)) as ResourceDictionary;
                this.Resources.MergedDictionaries.Add(theme);
            }
            catch
            {
                // theme missing. Reset to default.
                ResourceDictionary theme =
                   Application.LoadComponent(new Uri("/Cerberus;component/Resources/Themes/Dark.xaml", UriKind.Relative)) as ResourceDictionary;
                this.Resources.MergedDictionaries.Add(theme);
                Cerberus.Properties.Settings.Default.Skin = "Dark";
            }
        }
        private void AddCurrentBackground()
        {
            try
            {
                ResourceDictionary background =
                    Application.LoadComponent(new Uri("/Cerberus;component/Resources/Backgrounds/" +
                                        Cerberus.Properties.Settings.Default.Background + ".xaml", UriKind.Relative)) as ResourceDictionary;
                this.Resources.MergedDictionaries.Add(background);
            }
            catch
            {
                // background missing. Reset to default.
                ResourceDictionary background =
                   Application.LoadComponent(new Uri("/Cerberus;component/Resources/Backgrounds/Plain.xaml", UriKind.Relative)) as ResourceDictionary;
                this.Resources.MergedDictionaries.Add(background);
                Cerberus.Properties.Settings.Default.Skin = "Plain";
            }
        }
        private void OnApplicationSettingChanged(ApplicationSettingsEventMessage message)
        {
            if (this.Resources.MergedDictionaries.Any())
            {
                if (message.IsThemeChanged)
                {
                    this.AddCurrentTheme();
                }
                if (message.IsSkinChanged)
                {
                    this.AddCurrentSkin();
                }
                if (message.IsBackgroundChanged)
                {
                    this.AddCurrentBackground();
                }
            }
        }
    }
}
