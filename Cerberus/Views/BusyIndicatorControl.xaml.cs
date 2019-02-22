using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cerberus
{
    public partial class BusyIndicatorControl : UserControl, IBusyIndicatorService
    {
        #region fields and constructors
        private static object m_ClassLock = new object();
        public BusyIndicatorControl()
        {
            InitializeComponent();
        }
        #endregion

        #region events and methods
        public void Start(string message)
        {
            lock (m_ClassLock)
            {
                txtProgressText.Text = message;
                busyIndicatorGrid.Visibility = Visibility.Visible;
            }
        }

        public void Stop()
        {
            lock (m_ClassLock)
            {
                txtProgressText.Text = string.Empty;
                busyIndicatorGrid.Visibility = Visibility.Collapsed;
            }
        }
        #endregion
    }
}
