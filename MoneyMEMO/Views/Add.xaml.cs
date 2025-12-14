using MoneyMEMO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Search.xaml 的交互逻辑
    /// </summary>
    public partial class Add : Window
    {
        private static readonly Regex _decimalRegex = new(@"^\d{0,4}(\.\d{0,2})?$");
        public Add()
        {
            InitializeComponent();
            if (DataContext is AddViewModel vm)
            {
                vm.RequestClose += (s, e) => this.Close();
            }
        }
        private void DecimalTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox tb) return;

            string fullText = GetFullTextAfterInput(tb, e.Text);

            if (!_decimalRegex.IsMatch(fullText))
                e.Handled = true; // 拦截非法输入
        }
        // 拦截粘贴
        private void DecimalTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true; // 禁止空格
            if (e.Key == Key.Back && sender is TextBox tb && tb.Text.Length == 1) e.Handled = true;
        }
        // 计算插入后的完整文本
        private string GetFullTextAfterInput(TextBox tb, string input)
        {
            var text = tb.Text;
            int selStart = tb.SelectionStart;
            int selLength = tb.SelectionLength;

            string newText = text.Remove(selStart, selLength);
            newText = newText.Insert(selStart, input);
            return newText;
        }

    }
}
