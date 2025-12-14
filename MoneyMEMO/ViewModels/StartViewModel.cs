using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyMEMO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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

        public StartViewModel()
        {
            MoneyInfos.CollectionChanged += MoneyInfos_CollectionChanged;
        }

        // 命令定义
        [RelayCommand]
        private void AddRecord()
        {
            //var newRecord = new MoneyInfo
            //{
            //    Date = DateTime.Now,
            //    Category = MoneyCategory.Expense,
            //    Amount = 0,
            //    Note = string.Empty
            //};
            //MoneyInfos.Add(newRecord);
            var add = new Views.Add();
            var vm = (AddViewModel)add.DataContext;
            add.ShowDialog();
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
                search.Close();
            };
            search.ShowDialog();
        }
        [RelayCommand]
        private void Exit()
        {
            App.Current.Shutdown();
        }

        // 监听集合变化
        private void MoneyInfos_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (MoneyInfo item in e.NewItems)
                {
                    item.PropertyChanged += MoneyInfo_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (MoneyInfo item in e.OldItems)
                {
                    item.PropertyChanged -= MoneyInfo_PropertyChanged;
                }
            }

            RecalculateTotal();
        }
        private void MoneyInfo_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MoneyInfo.Amount))
            {
                RecalculateTotal();
            }
        }
        private void RecalculateTotal()
        {
            decimal total = MoneyInfos.Sum(x => x.Amount * (x.Category == MoneyCategory.Income ? 1 : -1));
            MoneyCount = total;
        }
    }
}
