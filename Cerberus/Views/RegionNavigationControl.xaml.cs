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
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Cerberus
{
    /// <summary>
    /// Interaction logic for RegionNavigationControl.xaml
    /// </summary>
    public partial class RegionNavigationControl : UserControl
    {
        #region enum and constants
        public enum RegionTransitionType { Fade, Slide, SlideAndFade, Grow, GrowAndFade, Flip, FlipAndFade, Spin, SpinAndFade }
        #endregion

        #region fields and constructors
        private Stack<UserControl> regions = new Stack<UserControl>();

        public RegionNavigationControl()
        {
            InitializeComponent();           
        }
        #endregion

        #region properties and delegates

        public UserControl CurrentRegion
        {
            get;
            set;
        }

        public static readonly DependencyProperty TransitionTypeProperty = DependencyProperty.Register("TransitionType",
                                                                          typeof(RegionTransitionType),
                                                                          typeof(RegionNavigationControl), new PropertyMetadata(RegionTransitionType.SlideAndFade));

        public RegionTransitionType TransitionType
        {
            get
            {
                return (RegionTransitionType)GetValue(TransitionTypeProperty);
            }
            set
            {
                SetValue(TransitionTypeProperty, value);
            }
        }
        #endregion

        #region events and methods

        public void ShowRegion(UserControl newRegion)
        {
            regions.Push(newRegion);
            Task.Factory.StartNew(() => LaunchNewRegion());
        }

        private void LaunchNewRegion()
        {
            Dispatcher.Invoke((Action)delegate
            {
                if (currentPresenter.Content != null)
                {
                    UserControl oldRegion = currentPresenter.Content as UserControl;
                    if (oldRegion != null)
                    {
                        oldRegion.Loaded -= OnNewRegionLoaded;
                        OnCurrentRegionUnloaded(oldRegion);
                    }
                }
                else
                {
                    LaunchNextRegion();
                }
            });
        }

        private void LaunchNextRegion()
        {
            UserControl newPage = regions.Pop();
            newPage.Loaded += OnNewRegionLoaded;
            currentPresenter.Content = newPage;
        }

        private void OnCurrentRegionUnloaded(UserControl region)
        {
            Storyboard hideRegion = (Resources[string.Format("{0}Out", TransitionType.ToString())] as Storyboard).Clone();
            hideRegion.Completed += OnHideRegionCompleted;
            hideRegion.Begin(currentPresenter);
        }

        private void OnNewRegionLoaded(object sender, RoutedEventArgs e)
        {
            Storyboard showNewPage = Resources[string.Format("{0}In", TransitionType.ToString())] as Storyboard;
            showNewPage.Begin(currentPresenter);
            CurrentRegion = sender as UserControl;
        }

        private void OnHideRegionCompleted(object sender, EventArgs e)
        {
            currentPresenter.Content = null;
            LaunchNextRegion();
        }

        #endregion
    }
}
