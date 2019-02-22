using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace Cerberus
{
    /// <summary>
    /// Interaction logic for ExperimentSetupView.xaml
    /// </summary>
    public partial class ExperimentSetupView : UserControl
    {
        #region fields and  constructors
        public ExperimentSetupView(Experiment currentExperiment)
        {
            InitializeComponent();
            DataContext = currentExperiment;        
        }
        #endregion

        #region events and methods
        private void OnEquationEditorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName == "Equation")
            {
                var eqEditor = sender as EquationEditor;
                if (eqEditor != null)
                {
                    string equation = eqEditor.Equation;
                }
            }
        }

        private void OnStartButtonClicked(object sender, RoutedEventArgs e)
        {
            Notifier.Instance.Notify("Application","Experiment creation failed !!!", Category.Error);
        }
        #endregion


    }
}
