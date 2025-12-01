using MoneyMEMO.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMEMO.Converters
{
    class CategoryToStringConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Models.MoneyCategory category)
            {
                return category switch 
                {
                    Models.MoneyCategory.Income => "收入",
                    Models.MoneyCategory.Expense => "支出",
                    _ => "",
                };
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                "收入" => MoneyCategory.Income,
                "支出" => MoneyCategory.Expense,
                _ => MoneyCategory.Expense,
            };
        }
    }
}
