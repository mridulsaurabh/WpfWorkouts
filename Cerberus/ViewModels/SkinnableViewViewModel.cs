using Cerberus.Events;
using Infrastructure.Commands;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cerberus.ViewModels
{
    public class SkinnableViewViewModel : ViewModelBase
    {
        private IWorkAsync _asyncWorker;
        private IEventAggregator _eventAggregator;
        public SkinnableViewViewModel(IWorkAsync asyncWorker, IEventAggregator eventAggregator)
            : base(asyncWorker, eventAggregator)
        {
            _asyncWorker = asyncWorker;
            _eventAggregator = eventAggregator;
            this.SubscribeToEventsAndCommands();
            this.Initialize();
        }

        private ObservableCollection<Skin> _skins;
        public ObservableCollection<Skin> Skins
        {
            get { return _skins; }
            set { SetProperty(ref _skins, value); }
        }

        private Skin _currentSkin;
        public Skin CurrentSkin
        {
            get { return _currentSkin; }
            set
            {
                SetProperty(ref _currentSkin, value);
                this.ChangeThemeCommand.Execute(null);
            }
        }

        private bool _isShowPreLoader;
        public bool IsShowPreLoader
        {
            get { return _isShowPreLoader; }
            set { SetProperty(ref _isShowPreLoader, value); }
        }

        private Background _currentBackground;
        public Background CurrentBackground
        {
            get { return _currentBackground; }
            set { SetProperty(ref _currentBackground, value); }
        }

        private void Initialize()
        {
            _skins = new ObservableCollection<Skin>();
            _skins.Add(new Skin() { Title = "Default", Color = new SolidColorBrush(Colors.DarkGreen) });

            _skins.Add(new Skin() { Title = "Lime", Color = new SolidColorBrush(Colors.Lime) });
            _skins.Add(new Skin() { Title = "Green", Color = new SolidColorBrush(Colors.Green) });
            _skins.Add(new Skin() { Title = "Emerald", Color = new SolidColorBrush(Colors.Turquoise) });
            _skins.Add(new Skin() { Title = "Teal", Color = new SolidColorBrush(Colors.Teal) });

            _skins.Add(new Skin() { Title = "Cyan", Color = new SolidColorBrush(Colors.Cyan) });
            _skins.Add(new Skin() { Title = "Cobalt", Color = new SolidColorBrush(Colors.Coral) });
            _skins.Add(new Skin() { Title = "Indigo", Color = new SolidColorBrush(Colors.Indigo) });
            _skins.Add(new Skin() { Title = "Violet", Color = new SolidColorBrush(Colors.Violet) });

            _skins.Add(new Skin() { Title = "Pink", Color = new SolidColorBrush(Colors.Pink) });
            _skins.Add(new Skin() { Title = "Magenta", Color = new SolidColorBrush(Colors.Magenta) });
            _skins.Add(new Skin() { Title = "Crimson", Color = new SolidColorBrush(Colors.Crimson) });
            _skins.Add(new Skin() { Title = "Red", Color = new SolidColorBrush(Colors.Red) });


            _skins.Add(new Skin() { Title = "Orange", Color = new SolidColorBrush(Colors.Orange) });
            _skins.Add(new Skin() { Title = "Amber", Color = new SolidColorBrush(Colors.AntiqueWhite) });
            _skins.Add(new Skin() { Title = "Yellow", Color = new SolidColorBrush(Colors.Yellow) });
            _skins.Add(new Skin() { Title = "Brown", Color = new SolidColorBrush(Colors.Brown) });

            _skins.Add(new Skin() { Title = "Olive", Color = new SolidColorBrush(Colors.Olive) });
            _skins.Add(new Skin() { Title = "Steel", Color = new SolidColorBrush(Colors.SteelBlue) });
            _skins.Add(new Skin() { Title = "Mauve", Color = new SolidColorBrush(Colors.Maroon) });
            _skins.Add(new Skin() { Title = "Taupe", Color = new SolidColorBrush(Colors.Tan) });

            _currentSkin = _skins.FirstOrDefault();
        }
        private void SubscribeToEventsAndCommands()
        {
            this.ChangeThemeCommand = new AwaitableDelegateCommand(OnChangeThemeCommandExecute);
            this.UpdateCommand = new AwaitableDelegateCommand(OnUpdateCommandExecute);
            this.UpdateBackCommand = new AwaitableDelegateCommand(OnUpdateBackCommandExecute);
        }
        private void OnUpdateBackCommandExecute()
        {
            _asyncWorker.RunAsync(async
                () =>
                {
                    this.IsShowPreLoader = true;
                    Cerberus.Properties.Settings.Default.Background = "Circles";
                    await Task.Delay(2000);
                    this.ChangeThemeCommand.Execute(null);
                },
                () =>
                {
                    this.IsShowPreLoader = false;
                });
        }
        private void OnUpdateCommandExecute()
        {
            _asyncWorker.RunAsync(async
                 () =>
                 {
                     this.IsShowPreLoader = true;
                     Cerberus.Properties.Settings.Default.Background = "Lines";
                     await Task.Delay(2000);
                     this.ChangeThemeCommand.Execute(null);
                 },
                 () =>
                 {
                     this.IsShowPreLoader = false;
                 });
        }
        private void OnChangeThemeCommandExecute()
        {
            Cerberus.Properties.Settings.Default.Skin = _currentSkin.Title;

            _eventAggregator.Publish<ApplicationSettingsEventMessage>(new ApplicationSettingsEventMessage()
            {
                IsBackgroundChanged = true,
                IsSkinChanged = true
            });
        }

        public ICommand ChangeThemeCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand UpdateBackCommand { get; set; }
    }

    public class Skin
    {
        public string Title { get; set; }
        public SolidColorBrush Color { get; set; }
    }

    public class Background
    {
        public string Titile { get; set; }
        public BitmapImage Content { get; set; }
    }
}
