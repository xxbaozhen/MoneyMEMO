using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyMEMO.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MoneyMEMO.ViewModels
{
    public partial class StartViewModel : ObservableObject
    {
        // 绑定数据集合
        [ObservableProperty]
        private ObservableCollection<MoneyInfo> allMoneyInfos = new();
        public ICollectionView MoneyInfosView { get; }
        // 绑定视图
        [ObservableProperty]
        private SearchQuery? searchQuery;
        partial void OnSearchQueryChanged(SearchQuery? oldValue, SearchQuery? newValue)
        {
            ApplySearch();
        }
        private void ApplySearch()
        {
            if (SearchQuery == null)
            {
                MoneyInfosView.Filter = null;
            }
            else
            {
                MoneyInfosView.Filter = obj =>
                {
                    if (obj is not MoneyInfo m) return false;
                    return SearchQuery.Match(m);
                };
            }
            MoneyInfosView.Refresh();
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateRecordCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteRecordCommand))]
        private MoneyInfo? selectedMoneyInfo = null;
        [ObservableProperty]
        private decimal moneyCount = 0.00m;
        // 数据库
        private readonly DataBaseManager dbManager = new();

        // 初始化
        public StartViewModel()
        {
            InitRecords();
            MoneyInfosView = CollectionViewSource.GetDefaultView(AllMoneyInfos);
        }

        // 初始化记录
        private void InitRecords()
        {
            AllMoneyInfos = new ObservableCollection<MoneyInfo>(dbManager.QueryAll().OrderBy(x => x.Date));

            foreach (var item in AllMoneyInfos)
            {
                item.PropertyChanged += AllMoneyInfos_PropertyChanged;
            }

            AllMoneyInfos.CollectionChanged += AllMoneyInfos_CollectionChanged;

            RecalculateTotal();
        }

        // 命令定义
        [RelayCommand]
        private void AddRecord()
        {
            var add = new Views.Add();
            var vm = (AddViewModel)add.DataContext;
            vm.RecordAdded += record =>
            {
                AllMoneyInfos.Add(record);
                dbManager.Insert(record);
                add.Close();
            };
            add.Owner = App.Current.MainWindow;
            add.ShowDialog();
        }
        [RelayCommand(CanExecute = nameof(CanDelete))]
        private void DeleteRecord()
        {
            if (SelectedMoneyInfo is not null)
            {
                if (MessageBox.Show("确定要删除该记录吗？", "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                     != MessageBoxResult.Yes)
                {
                    return;
                }
                // 如果该记录已有数据库 Id，则先从数据库删除
                if (SelectedMoneyInfo.Id > 0)
                {
                    dbManager.Delete(SelectedMoneyInfo.Id);
                }
                AllMoneyInfos.Remove(SelectedMoneyInfo);
            }
        }
        private bool CanDelete() => SelectedMoneyInfo is not null;
        [RelayCommand(CanExecute = nameof(CanUpdate))]
        private void UpdateRecord() 
        { 
            if (SelectedMoneyInfo is not null)
            {
                var update = new Views.Update();
                var vm = (UpdateViewModel)update.DataContext;
                // 传递当前选中的记录
                vm.LoadFromMoneyInfo(SelectedMoneyInfo);
                vm.RecordUpdated += info =>
                {
                    var index = AllMoneyInfos.IndexOf(SelectedMoneyInfo);
                    AllMoneyInfos[index] = info;
                    dbManager.Update(info.Id, info);
                    update.Close();
                };
                update.Owner = App.Current.MainWindow;
                update.ShowDialog();
            }
        }
        private bool CanUpdate() => SelectedMoneyInfo is not null;
        [RelayCommand]
        private void SearchRecord()
        {
            var search = new Views.Search();
            var vm = (SearchViewModel)search.DataContext;
            if (SearchQuery is not null && !SearchQuery.IsEmpty)
            {
                vm.LoadFromQuery(SearchQuery);
            }

            vm.QueryConfirmed += query =>
            {
                if (!query.IsEmpty)
                {
                    this.SearchQuery = query; // StartViewModel 接收
                }
                else
                {
                    this.SearchQuery = null;
                }
                search.Close();
            };
            search.Owner = App.Current.MainWindow;
            search.ShowDialog();
        }
        [RelayCommand]
        private void ClearQuery()
        {
            SearchQuery = null;
        }
        [RelayCommand]
        private void Reset()
        {
            if (MessageBox.Show("确定重置数据库吗？\n此操作将删除所有记录且不可恢复！", "确认重置", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                 != MessageBoxResult.Yes)
            {
                return;
            }
            AllMoneyInfos = [];
            SelectedMoneyInfo = null;
            dbManager.ClearAndDeleteDatabase();
            Application.Current.Shutdown();

            Process.Start(Environment.ProcessPath);
        }
        [RelayCommand]
        private void Exit()
        {
            App.Current.Shutdown();
        }

        // 监听集合变化
        private void AllMoneyInfos_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (MoneyInfo item in e.NewItems)
                {
                    item.PropertyChanged += AllMoneyInfos_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (MoneyInfo item in e.OldItems)
                {
                    item.PropertyChanged -= AllMoneyInfos_PropertyChanged;
                }
            }

            RecalculateTotal();
        }
        private void AllMoneyInfos_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MoneyInfo.Amount))
            {
                RecalculateTotal();
            }
        }
        private void RecalculateTotal()
        {
            decimal total = AllMoneyInfos.Sum(x => x.Amount * (x.Category == MoneyCategory.Income ? 1 : -1));
            MoneyCount = total;
        }

    }
}
