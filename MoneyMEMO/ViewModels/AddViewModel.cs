using CommunityToolkit.Mvvm.ComponentModel;
using MoneyMEMO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMEMO.ViewModels
{
    public partial class AddViewModel : ObservableObject
    {
        // 日期
        [ObservableProperty]
        private DateTime? date;
        // 金额
        [ObservableProperty]
        private string amountText = String.Empty;
        // 收支类型
        [ObservableProperty]
        private MoneyCategory category = MoneyCategory.Income;
        // 备注
        [ObservableProperty]
        private string? noteKeyword;
    }
}
