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
        [RelayCommand]
        private void AddRecord()
        {

        }
        [RelayCommand]
        private void DeleteRecord()
        {

        }
        [RelayCommand]
        private void SearchRecord()
        {

        }
        [RelayCommand]
        private void Exit()
        {

        }
    }
}
