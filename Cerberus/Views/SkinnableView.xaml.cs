using Cerberus.ViewModels;
using System.Windows;

namespace Cerberus.Views
{
    /// <summary>
    /// Interaction logic for SkinnableView.xaml
    /// </summary>
    public partial class SkinnableView : Window
    {
        public SkinnableView()
        {
            InitializeComponent();
            this.DataContext = App.CerberusContainer.Resolve<SkinnableViewViewModel>();
        }
    }
}
