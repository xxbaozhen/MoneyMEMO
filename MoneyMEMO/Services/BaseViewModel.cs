using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyMEMO.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MoneyMEMO.Services
{
    public abstract partial class BaseViewModel : ObservableObject
    {
        // 日期
        [ObservableProperty]
        private DateTime? date;
        protected virtual void OnDateChangedCore(DateTime? oldValue, DateTime? newValue) { }
        partial void OnDateChanged(DateTime? oldValue, DateTime? newValue)
        {
            OnDateChangedCore(oldValue, newValue);
        }
        // 金额
        [ObservableProperty]
        public string amountText = String.Empty;
        public decimal? Amount => ParseDecimal(AmountText);
        // 收支类型
        [ObservableProperty]
        private MoneyCategory category = MoneyCategory.Income;
        // 备注
        [ObservableProperty]
        private string? noteKeyword;

        // 取消添加
        public event EventHandler RequestClose;
        [RelayCommand]
        private void Cancel()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        // --- 共同辅助方法 ---

        // 解析金额字符串为 decimal
        protected decimal? ParseDecimal(string input)
        {
            if (decimal.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var val))
                return val;
            return null;
        }

        // 错误弹窗
        protected void ShowError(string message)
        {
            MessageBox.Show(message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
