using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyMEMO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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
        [ObservableProperty]
        private decimal moneyCount = 0.00m;

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
            var search = new Views.Search();
            var vm = (SearchViewModel)search.DataContext;
            vm.QueryConfirmed += query =>
            {
                Debug.WriteLine(query);
                search.Close();
            };
            search.ShowDialog();
        }
        [RelayCommand]
        private void Exit()
        {
            App.Current.Shutdown();
        }
    }
}
