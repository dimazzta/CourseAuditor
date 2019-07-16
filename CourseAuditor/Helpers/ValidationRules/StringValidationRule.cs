using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace CourseAuditor.Helpers.ValidationRules
{
    public class StringValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(!string.IsNullOrEmpty(value as string), "Это поле не может быть пустым");
        }
    }
}