using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Infrastructure.Common
{
    public class NavigationService : INavigationService
    {
        private Frame _mainFrame;
        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        public event NavigatingCancelEventHandler Navigating;

        public void Navigate(Type type)
        {
            _mainFrame.Navigate(type);
        }

        public void Navigate(Type type, object parameter)
        {
            _mainFrame.Navigate(type, parameter);
        }

        public void Navigate(string type, object parameter)
        {
            _mainFrame.Navigate(Type.GetType(type), parameter);
        }

        public void Navigate(string type)
        {
            _mainFrame.Navigate(Type.GetType(type));
        }

        public void GoBack()
        {
            if (_mainFrame.CanGoBack)
            {
                _mainFrame.GoBack();
            }
        }
    }
}
