using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyMEMO.Models;
using MoneyMEMO.Services;
using System;

namespace MoneyMEMO.ViewModels
{
    public partial class UpdateViewModel : BaseViewModel
    {
        // --- 特有属性 ---
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsIncome))]
        [NotifyPropertyChangedFor(nameof(IsExpense))]
        private MoneyCategory? category = null;
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
        public int Id { get; set; }

        // 确认添加
        public event Action<MoneyInfo>? RecordUpdated;
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
            // 修改记录
            var info = new MoneyInfo
            {
                Id = Id,
                Date = Date.Value,
                Category = (MoneyCategory)Category!,
                Amount = Amount.Value,
                Note = NoteKeyword?.Trim() ?? string.Empty
            };
            RecordUpdated?.Invoke(info);
        }

        public void LoadFromMoneyInfo(MoneyInfo info)
        {
            Id = info.Id;
            Date = info.Date;
            Category = info.Category;
            AmountText = info.Amount.ToString("F2");
            NoteKeyword = info.Note;
        }
    }
}