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
    /// <summary>
    /// Interaction logic for TrendView.xaml
    /// </summary>
    public partial class TrendView : UserControl
    {
        #region fields and constructors
        private TrendViewViewModel m_ViewModel;
        public TrendView(TrendViewViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }       

        #endregion

        #region events and methods

        #endregion
    }
}
