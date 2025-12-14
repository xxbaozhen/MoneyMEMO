using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyMEMO.Models;
using MoneyMEMO.Services;
using System;

namespace MoneyMEMO.ViewModels
{
    public partial class AddViewModel : BaseViewModel
    {
        // --- 特有属性 ---
        [ObservableProperty]
        private MoneyCategory category = MoneyCategory.Income;
        public bool IsIncome
        {
            get => Category == MoneyCategory.Income;
            set
            {
                if (value)
                {
                    Category = MoneyCategory.Income;
                }
            }
        }
        public bool IsExpense
        {
            get => Category == MoneyCategory.Expense;
            set
            {
                if (value)
                {
                    Category = MoneyCategory.Expense;
                }
            }
        }

        public AddViewModel()
        {
            Date = DateTime.Now;
        }

        // 确认添加
        public event Action<MoneyInfo>? RecordAdded;
        [RelayCommand]
        private void Confirm()
        {
            // 输入校验
            if (Date == null)
            {
                ShowError("请选择日期！");
                return;
            }
            if (string.IsNullOrWhiteSpace(AmountText) || Amount is null)
            {
                ShowError("请输入有效的金额！");
                return;
            }
            // 创建记录
            var newRecord = new MoneyInfo
            {
                Date = Date.Value,
                Category = Category,
                Amount = (decimal)Amount,
                Note = NoteKeyword ?? string.Empty
            };
            RecordAdded?.Invoke(newRecord);
        }
    }
}