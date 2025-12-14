using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyMEMO.Models;
using MoneyMEMO.Services;
using System;
using System.Windows;

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
            if (query.IsEmpty)
            {
                MessageBox.Show("查询条件为空!\n请设置至少一个查询条件以进行查找", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            QueryConfirmed?.Invoke(query);
        }

        // 获取外部查询条件
        public void LoadFromQuery(SearchQuery query)
        {
            this.UseAmountRange = query.UseAmountRange;
            this.UseDateRange = query.UseDateRange;
            this.Date = query.Date;
            this.StartDate = query.StartDate;
            this.EndDate = query.EndDate;
            this.AmountText = query.Amount?.ToString() ?? String.Empty;
            this.MinAmountText = query.MinAmount?.ToString() ?? String.Empty;
            this.MaxAmountText = query.MaxAmount?.ToString() ?? String.Empty;
            this.IncludeIncome = query.IncludeIncome;
            this.IncludeExpense = query.IncludeExpense;
        }
    }
}