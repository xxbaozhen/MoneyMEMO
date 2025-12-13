using MoneyMEMO.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MoneyMEMO.Converters
{
    public class CategoryToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var radio = parameter as System.Windows.Controls.RadioButton;
            if (radio?.Tag is MoneyCategory tag && value is MoneyCategory category)
            {
                return category == tag;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is System.Windows.Controls.RadioButton radio &&
                value is bool isChecked &&
                isChecked &&
                radio.Tag is MoneyCategory tag)
            {
                return tag;
            }

            return Binding.DoNothing;
        }
    }

}
