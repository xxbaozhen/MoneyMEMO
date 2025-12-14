using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMEMO.Models
{
    public enum MoneyCategory { Income, Expense }; // 收入，支出
    public partial class MoneyInfo : ObservableObject
    {
        // 数据库自增 Id（用于删除对照，不显示在 UI）
        [ObservableProperty]
        private int id;

        // 日期
        [ObservableProperty]
        private DateTime date = DateTime.Now;
        // 类型
        [ObservableProperty]
        private MoneyCategory category;
        // 金额
        [ObservableProperty]
        private decimal amount;
        // 备注
        [ObservableProperty]
        private string note = string.Empty; 
    }
}
