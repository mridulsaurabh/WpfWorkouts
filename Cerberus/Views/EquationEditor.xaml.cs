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
using System.ComponentModel;

namespace Cerberus
{
    /// <summary>
    /// Interaction logic for EquationEditor.xaml
    /// </summary>
    public partial class EquationEditor : UserControl, INotifyPropertyChanged
    {
        #region fields and constructors
        private string m_KeyPadValue = String.Empty;
        private string m_Equation = String.Empty;

        public EquationEditor()
        {
            InitializeComponent();
        }
        #endregion

        #region properties and delegates

        public string Equation
        {
            get
            {
                return m_Equation;
            }
            set
            {
                if (m_Equation != value)
                {
                    m_Equation = value;
                    OnPropertyChanged("Equation");
                }
            }
        }

        #endregion

        #region events and methods

        private void OnKeyPadButtonClicked(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                if (btn.Content.Equals("C"))
                {
                    Equation = String.Empty;
                }
                else if (btn.Content.Equals("«"))
                {
                    if (m_Equation.Length > 0)
                    {
                        Equation = m_Equation.Substring(0, m_Equation.Length - 1);
                    }
                }
                else
                {
                    m_KeyPadValue = btn.Content as string;
                    Equation = m_Equation + m_KeyPadValue;
                }
                equationTextBox.Text = m_Equation;
            }
        }

        #endregion

        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
