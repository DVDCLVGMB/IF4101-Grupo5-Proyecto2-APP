
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Steady_Management_App.Views.Converters
{
    public class RoleToDeleteVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value?.ToString(), out int roleId))
            {
                return roleId == 21 ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
