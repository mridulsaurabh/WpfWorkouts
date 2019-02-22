using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class InclueLaborHourAttribute : ValidationAttribute
    {
        private readonly bool _canLogHoursMoreThanDay;
        public InclueLaborHourAttribute(bool canLogHoursMoreThanDay)
        {
            this._canLogHoursMoreThanDay = canLogHoursMoreThanDay;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string _matchToLogLaborWithInDayLimit = "^([0-9]|0[0-9]|1[0-9]|2[0-4]):([0-5][0-9])$";
            string _matchToLogLaborExceedingDayLimit = "^(\\d{1,3}):([0-5][0-9])$";
            string fieldDisplayName = validationContext.DisplayName;
            string fieldValue = Convert.ToString(value).Trim();

            var property = validationContext.ObjectType.GetProperty(fieldDisplayName);
            if (property == null)
            {
                return new ValidationResult(string.Format("Unknown property: {0}", fieldDisplayName), new List<string>() { fieldDisplayName });
            }

            var hasAlphabets = fieldValue.Any(t => Char.IsLetter(t));
            if (hasAlphabets)
            {
                return new ValidationResult(string.Format("{0} can not have alphabets or special characters.", fieldDisplayName), new List<string>() { fieldDisplayName });
            }

            var isHourlyFormat = fieldValue.Any(t => t == ':');
            if (!isHourlyFormat)
            {
                int totalMinutes;
                if (int.TryParse(fieldValue, out totalMinutes))
                {
                    if (totalMinutes == 0)
                    {
                        return new ValidationResult(string.Format("{0} can not be zero.", fieldDisplayName), new List<string>() { fieldDisplayName });
                    }
                    if (!this._canLogHoursMoreThanDay && totalMinutes > 1440)
                    {
                        return new ValidationResult(string.Format("{0} can not exceed more than 24 hours.", fieldDisplayName), new List<string>() { fieldDisplayName });
                    }
                }
            }
            else
            {
                bool isHourFormatAccepted =
                          (this._canLogHoursMoreThanDay) ?
                           new RegularExpressionAttribute(_matchToLogLaborExceedingDayLimit).IsValid(fieldValue) :
                           new RegularExpressionAttribute(_matchToLogLaborWithInDayLimit).IsValid(fieldValue);

                if (!isHourFormatAccepted)
                {
                    return new ValidationResult(this.FormatErrorMessage(fieldDisplayName), new List<string>() { fieldDisplayName });
                }
            }

            return ValidationResult.Success;
        }
    }

}
