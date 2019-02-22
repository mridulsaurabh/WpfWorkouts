using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Cerberus.ToBeUsed
{
    public class StringToIntValidationRule : ValidationRule
    {
        // ValidationRule class has a property named ValidationStep that controls when the binding engine will execute the Validate method. 
        // The System.Windows.Controls.ValidationStep enumeration has the following options:
        // A. RawProposedValue: The validation rule is run before the value conversion occurs. This is the default for a custom implementation.
        // B. ConvertedProposedValue: The validation rule is run after the value is converted but before the setter of the source property is called.
        // C. UpdatedValue: The validation rule is run after the source property has been updated.
        // D. CommittedValue: The validation rule is run after the value has been committed to the source.

        // How to use: 
         // <TextBox>
         //    <TextBox.Text>
         //        <Binding Path="Age" UpdateSourceTrigger="PropertyChanged">
         //            <Binding.ValidationRules>
         //                <local:StringToIntValidationRule ValidationStep="RawProposedValue"/>
         //            </Binding.ValidationRules>
         //        </Binding>
         //    </TextBox.Text>
         // </TextBox>

        // there is one generic validation rule for catching exceptions : ExceptionValidationRule.
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int i;
            if (int.TryParse(value.ToString(), out i))
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Please enter a valid integer value.");
        }
    }
}
