using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CourseAuditor.Helpers
{
    public class ModuleDatesConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime endDate = ((Module)value).DateEnd ?? DateTime.Now;
            return $"Модуль закрыт ({((Module)value).DateStart.ToString("dd MMM yyyy")} - {endDate.ToString("dd MMM yyyy")})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}