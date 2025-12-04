using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyMEMO.Models;
using System;
using System.Globalization;
using System.Windows;

namespace MoneyMEMO.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
        // 日期
        [ObservableProperty]
        private DateTime? date;

        [ObservableProperty]
        private DateTime? startDate;

        [ObservableProperty]
        private DateTime? endDate;

        [ObservableProperty]
        private bool useDateRange;

        partial void OnUseDateRangeChanged(bool value) // 监听 UseDateRange 变化
        {
            Date = null;
            StartDate = null;
            EndDate = null;
        }

        // 金额
        [ObservableProperty]
        private string amountText = ""; // 直接绑定 TextBox 输入

        public decimal? Amount => ParseDecimal(AmountText);

        [ObservableProperty]
        private string minAmountText = "";

        public decimal? MinAmount => ParseDecimal(MinAmountText);

        [ObservableProperty]
        private string maxAmountText = "";

        public decimal? MaxAmount => ParseDecimal(MaxAmountText);

        [ObservableProperty]
        private bool useAmountRange;
        partial void OnUseAmountRangeChanged(bool value) // 监听 UseMountRange 变化
        {
            AmountText = String.Empty;
            MinAmountText = String.Empty;
            MaxAmountText = String.Empty;
        }
        private decimal? ParseDecimal(string input)
        {
            if (decimal.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var val))
                return val;
            return null;
        }

        // 收支类型
        [ObservableProperty]
        private bool includeIncome = true;

        [ObservableProperty]
        private bool includeExpense = true;

        // 备注搜索
        [ObservableProperty]
        private string? noteKeyword;

        // 订阅事件触发查询确认
        public event Action<SearchQuery>? QueryConfirmed;
        // 查询按钮
        [RelayCommand]
        private void ConfirmQuery()
        {
            if (Date == null && (StartDate == null || EndDate == null))
            {
                ShowError("请选择日期或日期范围！");
                return;
            }
            if (string.IsNullOrEmpty(AmountText) &&
                (string.IsNullOrWhiteSpace(MinAmountText) || string.IsNullOrWhiteSpace(MaxAmountText)))
            {
                ShowError("请输入金额或金额范围！");
                return;
            }
            if (!IncludeIncome && !IncludeExpense)
            {
                ShowError("请选择至少一种收支类型！");
                return;
            }
            var query = new SearchQuery
            {
                Date = this.Date,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                Amount = this.Amount,
                MinAmount = this.MinAmount,
                MaxAmount = this.MaxAmount,
                UseAmountRange = this.UseAmountRange,
                UseDateRange = this.UseDateRange,
                IncludeIncome = this.IncludeIncome,
                IncludeExpense = this.IncludeExpense,
                NoteKeyword = this.NoteKeyword ?? String.Empty
            };
            QueryConfirmed?.Invoke(query);
        }
        // 订阅事件触发关闭窗口
        public event EventHandler RequestClose;
        // 取消按钮
        [RelayCommand]  
        private void Cancel()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        // 错误弹窗
        private void ShowError(string message)
        {
            MessageBox.Show(message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
