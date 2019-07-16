using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CourseAuditor.Helpers
{
    public class DiscountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return (double.Parse(value as string, System.Globalization.CultureInfo.InvariantCulture) * 100).ToString() + "%";
            }
            catch{
                return "0%";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string numberOnly = Regex.Replace((string)value, "[^0-9.]", "");
                return System.Convert.ToDouble(numberOnly) / 100;
            }
            catch
            {
                return 0.0;
            }
        }
    }
}
