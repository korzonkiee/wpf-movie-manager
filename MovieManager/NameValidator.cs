using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MovieManager
{
    public class NameValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var stringValue = (string)value;
            if (string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(false, "Text field cannot be empty.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
