using Infrastructure.Commands;
using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Data;
using Infrastructure.Interfaces;
using Infrastructure.Utilities;
using ServiceClient;
using System.Net;
using ServiceClient.Interfaces;
using Infrastructure;
using System.ComponentModel.DataAnnotations;
using Infrastructure.ValidationAttributes;

namespace Cerberus.ViewModels
{
    public class UpdateEventMessage : IMessage
    {
        public UpdateEventMessage(string status)
        {
            this.Status = status;
        }
        public string Status { get; set; }
    }

    public static class CollectionExtensions
    {
        public static ObservableCollection<T> EnableSynchronization<T>(this ObservableCollection<T> collection)
        {
            object _syncRoot = new object();
            BindingOperations.EnableCollectionSynchronization(collection, _syncRoot);
            return collection;
        }
    }

    public class TestViewViewModel : ViewModelBase
    {
        #region fields and constructors
        private Random rand = new Random();
        private IWorkAsync _asyncService;
        private IEventAggregator _eventAggregator;
        private IProxy _proxy;
        private CancellationTokenSource _executeCancellationTokenSource;

        public TestViewViewModel(IWorkAsync asyncService, IEventAggregator eventAggregator, IProxy proxy)
        {
            this._asyncService = asyncService;
            this._eventAggregator = eventAggregator;
            this._proxy = proxy;
            this.SubscribeToCommandsAndEvents();
            this._valueItems = new ObservableCollection<string>().EnableSynchronization();
            this.WelcomeText = string.Format("Instance ID : {0}", rand.Next(10, 50));
        }

        public TestViewViewModel()
        {
            this.SubscribeToCommandsAndEvents();
            this._valueItems = new ObservableCollection<string>().EnableSynchronization();
            this.WelcomeText = string.Format("Instance ID : {0}", rand.Next(10, 50));
        }

        #endregion

        #region properties and delegates
        public ICommand RunCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private string _welcomeText;
        public string WelcomeText
        {
            get { return _welcomeText; }
            set
            {
                SetProperty(ref this._welcomeText, value);
            }
        }

        private ObservableCollection<string> _valueItems;
        public ObservableCollection<string> ValueItems
        {
            get { return _valueItems; }
            set
            {
                SetProperty(ref this._valueItems, value);
            }
        }

        private bool _showPreLoader;
        public bool ShowPreLoader
        {
            get { return _showPreLoader; }
            set
            {
                SetProperty(ref this._showPreLoader, value);
            }
        }

        private string _actualLaborMinutes;

        [Required]
        [InclueLaborHour(false)]
        public string ActualLaborMinutes
        {
            get { return _actualLaborMinutes; }
            set
            {
                SetProperty(ref this._actualLaborMinutes, value);
                ValidateProperty(this.ActualLaborMinutes);
            }
        }
        #endregion

        #region events and methods
        private void SubscribeToCommandsAndEvents()
        {
            this.RunCommand = new AwaitableDelegateCommand(ExecuteRunCommand, CanExecuteRunCommand);
            this.UpdateCommand = new AwaitableDelegateCommand(ExecuteUpdateCommand);
            this.CancelCommand = new AwaitableDelegateCommand(ExecuteCancelCommand);
            var subscriptionOne = this._eventAggregator.Subscribe<UpdateEventMessage>(OnEventMessageUpdated, true);
            // ways to unsubscribe the message
            // this._eventAggregator.UnSubscribe(subscriptionOne); 
            // subscriptionOne.Dispose();
        }

        private void ExecuteCancelCommand()
        {
            if (_executeCancellationTokenSource != null)
            {
                _executeCancellationTokenSource.Cancel();
                _executeCancellationTokenSource.Token.Register(() =>
                {
                    _eventAggregator.Publish<UpdateEventMessage>(new UpdateEventMessage("Task cancelled."));
                });
            }
        }

        private void OnEventMessageUpdated(UpdateEventMessage message)
        {
            this.WelcomeText = "Subscription callback triggered with status : " + message.Status;
        }

        private bool CanExecuteRunCommand()
        {
            return true;
        }

        private void ExecuteRunCommand()
        {
            this._asyncService.RunAsync(
                () =>
                {
                    this.ShowPreLoader = true;
                    // long running process...                         
                    for (int i = 1; i <= 5; i++)
                    {
                        Thread.Sleep(200);
                        this.ValueItems.Add(string.Format("adding value {0}", i));
                    }

                    this.WelcomeText = EncryptionManager.Instance.Encrypt(this.WelcomeText, "012345678901234567890123");
                    Thread.Sleep(500);
                    this.WelcomeText = EncryptionManager.Instance.Decrypt(this.WelcomeText, "012345678901234567890123");
                    Thread.Sleep(1000);
                },
                () =>
                {
                    this.ShowPreLoader = false;
                    this.WelcomeText = App.CerberusContainer.Resolve<TestViewViewModel>().WelcomeText;
                    this._eventAggregator.Publish<UpdateEventMessage>(new UpdateEventMessage("Fuck You !"));
                },
                (ex) =>
                {
                    Logger.Instance.LogException(ex);
                });


            //BackgroundWorker worker = new BackgroundWorker();
            //worker.RunWorkerCompleted += (s, e) =>
            //{
            //    this.ShowPreLoader = false;
            //    this.WelcomeText = App.CerberusContainer.Resolve<TestViewViewModel>().WelcomeText;
            //    this._eventAggregator.Publish<UpdateEventMessage>(new UpdateEventMessage("Fuck You !"));
            //};
            //worker.DoWork += (s, e) =>
            //    {
            //        try
            //        {
            //            this.ShowPreLoader = true;
            //            // long running process...                         
            //            for (int i = 1; i <= 5; i++)
            //            {
            //                Thread.Sleep(200);
            //                this.ValueItems.Add(string.Format("adding value {0}", i));
            //            }

            //            this.WelcomeText = EncryptionManager.Instance.Encrypt(this.WelcomeText, "012345678901234567890123");
            //            Thread.Sleep(500);
            //            this.WelcomeText = EncryptionManager.Instance.Decrypt(this.WelcomeText, "012345678901234567890123");
            //            Thread.Sleep(1000);
            //        }
            //        catch (Exception)
            //        {
            //            throw;
            //        }
            //    };
            //worker.RunWorkerAsync();
        }

        private void ExecuteUpdateCommand()
        {
            this.WelcomeText = "long running process executing...";
            //this._asyncService.RunAsync<ServiceClient.Entities.LateReasonCodes>(
            //    () =>
            //    {
            //        this.ShowPreLoader = true;
            //        return this._proxy.GetLateReasonCodes();
            //    },
            //    (result) =>
            //    {
            //        var context = new AppContext();
            //        this.WelcomeText = "Completed !!!";
            //        this.ShowPreLoader = false;
            //    },
            //    (ex) =>
            //    {
            //        Logger.Instance.LogException(ex);
            //    });

            _executeCancellationTokenSource = new CancellationTokenSource();
            this._asyncService.RunAsync(
               () =>
               {
                   this.ShowPreLoader = true;
                   for (int i = 0; i < 15; i++)
                   {
                       this.WelcomeText = string.Format("Invoking process iteration {0}", i);
                       if (_executeCancellationTokenSource.Token.IsCancellationRequested)
                       {
                           _executeCancellationTokenSource.Token.ThrowIfCancellationRequested();
                       }
                       this._valueItems.Add(string.Format("item {0} modified. ", i));
                       Thread.Sleep(1000);
                   }
               },
               _executeCancellationTokenSource,
               () =>
               {
                   var context = new AppContext();
                   this.WelcomeText = "Completed !!!";
                   this.ShowPreLoader = false;
               },
               (ex) =>
               {
                   this.ShowPreLoader = false;
                   this.WelcomeText = "Task got cancelled by user.";
                   Logger.Instance.LogException(ex);
               });
        }

        #endregion
    }   
}
