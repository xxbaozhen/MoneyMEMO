using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyMEMO.Models;
using MoneyMEMO.Services;
using System;

namespace MoneyMEMO.ViewModels
{
    public partial class SearchViewModel : BaseViewModel
    {
        // --- 特有属性：日期范围 ---

        [ObservableProperty]
        private DateTime? startDate;

        [ObservableProperty]
        private DateTime? endDate;

        [ObservableProperty]
        private bool useDateRange;

        partial void OnUseDateRangeChanged(bool value)
        {
            Date = null;
            StartDate = null;
            EndDate = null;
        }

        // --- 特有属性：金额范围 ---

        [ObservableProperty]
        private string minAmountText = "";
        public decimal? MinAmount => ParseDecimal(MinAmountText);

        [ObservableProperty]
        private string maxAmountText = "";
        public decimal? MaxAmount => ParseDecimal(MaxAmountText);

        [ObservableProperty]
        private bool useAmountRange;

        partial void OnUseAmountRangeChanged(bool value)
        {
            AmountText = String.Empty;
            MinAmountText = String.Empty;
            MaxAmountText = String.Empty;
        }

        [ObservableProperty]
        private bool includeIncome = true;

        [ObservableProperty]
        private bool includeExpense = true;

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

            if (string.IsNullOrWhiteSpace(AmountText) &&
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
            };
            QueryConfirmed?.Invoke(query);
        }
    }
}