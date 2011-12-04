using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Diagnostics;

namespace Microsoft.Phone.Applications.UnitConverter.Converters
{

    /// <summary>
    /// 
    /// </summary>
    public class AutomationListConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            return string.Format("auto:{0}:{1}", parameter, value);
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
