using MoneyMEMO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MoneyMEMO.Views
{
    /// <summary>
    /// Update.xaml 的交互逻辑
    /// </summary>
    public partial class Update : Window
    {
        public Update()
        {
            InitializeComponent();
            if (DataContext is UpdateViewModel vm)
            {
                vm.RequestClose += (s, e) => this.Close();
            }
        }
    }
}
