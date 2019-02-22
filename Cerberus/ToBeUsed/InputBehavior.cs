using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cerberus.ToBeUsed
{
    public class InputBehavior
    {
        // How to use: <TextBox local:InputBehaviour.IsDigitOnly="True" />
        public static bool GetIsDigitOnly(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDigitOnlyProperty);
        }

        public static void SetIsDigitOnly(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDigitOnlyProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsDigitOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDigitOnlyProperty =
            DependencyProperty.RegisterAttached("IsDigitOnly", typeof(bool), typeof(InputBehavior),
            new PropertyMetadata(false, OnDigitOnlyChanged));


        private static void OnDigitOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // write logic to accept only digits.
            TextBox selectedTextBox = d as TextBox;
            if (selectedTextBox == null)
                return;
            if ((bool)e.NewValue)
            {
                selectedTextBox.TextChanged += OnTextBoxTextChanged;
            }
            else
            {
                selectedTextBox.TextChanged -= OnTextBoxTextChanged;
            }
        }

        static void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            // Only react to the TextChanged event raised by the TextBox
            // whose Text property was modified. Ignore all ancestors
            // who are merely reporting that a descendant's Selected fired.
            if (!Object.ReferenceEquals(sender, e.OriginalSource))
                return;

            TextBox tb = e.OriginalSource as TextBox;
            foreach (char ch in tb.Text)
            {
                if (!char.IsDigit(ch))
                    e.Handled = true;
            }
           
        }
    }
}
