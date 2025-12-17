using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MoneyMEMO.Behaviors
{
    public static class DecimalInputBehavior
    {
        #region IsEnabled 附加属性

        public static bool GetIsEnabled(DependencyObject obj)
            => (bool)obj.GetValue(IsEnabledProperty);

        public static void SetIsEnabled(DependencyObject obj, bool value)
            => obj.SetValue(IsEnabledProperty, value);

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(DecimalInputBehavior),
                new PropertyMetadata(false, OnIsEnabledChanged));

        #endregion

        #region DecimalPlaces 附加属性

        public static int GetDecimalPlaces(DependencyObject obj)
            => (int)obj.GetValue(DecimalPlacesProperty);

        public static void SetDecimalPlaces(DependencyObject obj, int value)
            => obj.SetValue(DecimalPlacesProperty, value);

        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.RegisterAttached(
                "DecimalPlaces",
                typeof(int),
                typeof(DecimalInputBehavior),
                new PropertyMetadata(2));

        #endregion

        private static void OnIsEnabledChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox tb) return;

            if ((bool)e.NewValue)
            {
                // 英文输入
                InputMethod.SetIsInputMethodEnabled(tb, false);

                tb.PreviewTextInput += OnPreviewTextInput;
                tb.PreviewKeyDown += OnPreviewKeyDown;
                DataObject.AddPastingHandler(tb, OnPaste);
            }
            else
            {
                InputMethod.SetIsInputMethodEnabled(tb, true);

                tb.PreviewTextInput -= OnPreviewTextInput;
                tb.PreviewKeyDown -= OnPreviewKeyDown;
                DataObject.RemovePastingHandler(tb, OnPaste);
            }
        }

        private static void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox tb) return;
            // 校验非数字字符
            if (!IsMatch(tb, e.Text)) e.Handled = true;
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is not TextBox tb) return;

            if (!e.SourceDataObject.GetDataPresent(DataFormats.Text))
            {
                e.CancelCommand();
                return;
            }

            string text = e.SourceDataObject.GetData(DataFormats.Text) as string ?? "";
            if (!IsMatch(tb, text)) e.CancelCommand();
        }

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // 禁止空格
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private static bool IsMatch(TextBox tb, string input)
        {
            int decimals = GetDecimalPlaces(tb);
            var regex = new Regex($@"^\d{{0,5}}(\.\d{{0,{decimals}}})?$");

            string text = tb.Text;
            int start = tb.SelectionStart;
            int length = tb.SelectionLength;

            // 模拟插入后的完整字符串
            string newText = text.Remove(start, length)
                                 .Insert(start, input);

            return regex.IsMatch(newText);
        }
    }
}