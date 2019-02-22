using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;
using Infrastructure.Commands;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Interfaces;

namespace Cerberus
{
    public enum ViewName
    {
        DashBoardView,
        CustomGridView,
        ExperimentView,
        TrendView
    }

    public class DashBoardViewModel : BaseViewModel
    {
        #region enums and constants
        private const string SUPERVISORYPASSWORD = "123";        
        #endregion

        #region fields and constructors
        private bool _hasPassword = false;
        private IWorkAsync _asyncService;
        public IEventAggregator _eventAggregator;
        private INavigationService _navigationService;

        public DashBoardViewModel(IWorkAsync service, IEventAggregator eventAggregator, INavigationService navaigation)
            : base(service, eventAggregator)
        {
            this._asyncService = service;
            this._eventAggregator = eventAggregator;
            this._navigationService = navaigation;
            this.SubscribeToEventsAndCommands();         
        }

        #endregion

        #region properties and delegates

        public ListCollectionView GroupSensorModules
        {
            get
            {
                var groupedSensorModules = new ListCollectionView(SensorModules);
                if (groupedSensorModules.GroupDescriptions != null)
                {
                    groupedSensorModules.GroupDescriptions.Add(new PropertyGroupDescription("ParentReader"));
                }
                return groupedSensorModules;
            }
        }      

        public bool HasPassword
        {
            get
            {
                return _hasPassword;
            }
            set
            {
                SetProperty(ref _hasPassword, value);
            }
        }

        public ICommand SaveCommand { get; set; }

        public ICommand ChangeReaderNameCommand { get; set; }

        public ICommand ViewSelectionCommand { get; set; }
       

        #endregion

        #region events and methods

        private void SubscribeToEventsAndCommands()
        {
            Logger.Instance.LogInformation("subscribing events and commands for the dashboard.");
            this.SaveCommand = new AwaitableDelegateCommand<object[]>(OnSensorSettingsSaved);
            this.ChangeReaderNameCommand = new AwaitableDelegateCommand<object[]>(OnReaderNameBeingSaved);
            this.ViewSelectionCommand = new AwaitableDelegateCommand<ViewName>(OnUIViewSelected);
            Logger.Instance.LogInformation("Done");
        }

        private void OnUIViewSelected(ViewName currentView)
        {
            this._eventAggregator.Publish<ViewSelectionMessage>(new ViewSelectionMessage(currentView));
        } 

        private void OnSensorSettingsSaved(object[] savedSettings)
        {
            if (this.ValidateUserAuthenticationStatus())
            {
                // APPLY CHANGES IN THE SELECTED SENSOR MODULE SETTINGS. 
                SensorModule sModule = savedSettings[0] as SensorModule;
                var desiredSModule = SensorModules.FindMatch(o => 
                    o.Name == sModule.Name && o.Type == sModule.Type && 
                    o.ParentReader == sModule.ParentReader);

                if (desiredSModule != null)
                {
                    desiredSModule.Name = (string)savedSettings[1];
                    desiredSModule.LogFile.Author = (string)savedSettings[2];
                }
            }
        }

        private void OnReaderNameBeingSaved(object[] pseudoNames)
        {
            string currentReaderName = (string)pseudoNames[0];
            string newReaderName = (string)pseudoNames[1];
            var sModulesHavingTheSameReader = SensorModules.FindMatchAll(s => s.ParentReader == currentReaderName);
            if (sModulesHavingTheSameReader != null)
            {
                foreach (var sModule in sModulesHavingTheSameReader)
                {
                    sModule.ParentReader = newReaderName;
                }
            }
            OnPropertyChanged(()=>this.GroupSensorModules);
        }

        private bool ValidateUserAuthenticationStatus()
        {
            if (!_hasPassword)
            {
                var authenticationWindow = new UserCredentialWindow(this);
                authenticationWindow.okayButton.Click += (o, e) =>
                {
                    if (authenticationWindow.maskedTextBox.Password.Equals(SUPERVISORYPASSWORD))
                    {
                        authenticationWindow.warningTextBlock.Visibility = Visibility.Collapsed;
                        authenticationWindow.Close();
                    }
                    else
                    {
                        authenticationWindow.warningTextBlock.Visibility = Visibility.Visible;
                    }
                };
                authenticationWindow.ShowDialog();
            }
            return _hasPassword;
        }

        #endregion       
    }

    public class ViewSelectionMessage : IMessage
    {
        public ViewSelectionMessage(ViewName view)
        {
            this.CurrentView = view;
        }
        public ViewName CurrentView { get; set; }
    }
}

