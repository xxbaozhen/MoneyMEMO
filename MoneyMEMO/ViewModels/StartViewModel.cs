using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyMEMO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMEMO.ViewModels
{
    public partial class StartViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<MoneyInfo> moneyInfos = [];
        [ObservableProperty]
        private MoneyInfo? selectedMoneyInfo = null;

        [RelayCommand]
        private void AddRecord()
        {
            var newRecord = new MoneyInfo
            {
                Date = DateTime.Now,
                Category = MoneyCategory.Expense,
                Amount = 0,
                Note = string.Empty
            };
            MoneyInfos.Add(newRecord);
        }
        [RelayCommand]
        private void DeleteRecord()
        {
            if (SelectedMoneyInfo is not null)
            {
                MoneyInfos.Remove(SelectedMoneyInfo);
            }
        }
        [RelayCommand]
        private void SearchRecord()
        {

        }
        [RelayCommand]
        private void Exit()
        {
            App.Current.Shutdown();
        }
    }
}
