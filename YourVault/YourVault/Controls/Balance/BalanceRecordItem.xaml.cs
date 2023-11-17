using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Controls.Balance
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BalanceRecordItem : Page
    {
        public static readonly DependencyProperty BalanceItemProperty = DependencyProperty.Register("balanceRecord", typeof(Models.Balance), typeof(BalanceRecordItem), new PropertyMetadata(null, OnBalanceChanged));

        public BalanceRecordItem()
        {
            this.InitializeComponent();
        }

        public Models.Balance balanceRecord
        {
            get { return (Models.Balance)GetValue(BalanceItemProperty); }
            set { SetValue(BalanceItemProperty, value); }
        }

        public static void OnBalanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BalanceRecordItem;
            if (control.balanceRecord is not null)
            {
                var color = control.balanceRecord.account.GetColor();
                var brush = new AcrylicBrush();
                brush.TintColor = color;
                brush.TintOpacity = 0.5;
                control.nameElipse.Fill = brush;
                control.textOnElipse.Text = control.balanceRecord.account.Name[..1];
                control.accountNameBlock.Text = control.balanceRecord.account.Name;

                control.balanceValueBlock.Text = $"{control.balanceRecord.Amount}";

                control.idText.Text = $"#{control.balanceRecord.ID}";
                control.dateText.Text = control.balanceRecord.dateTime.ToLongDateString();
                control.timeText.Text = control.balanceRecord.dateTime.ToLongTimeString();
            }
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            var balanceRecordItem = (sender as FrameworkElement).DataContext as Models.Balance;
            var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();

            string short_date = balanceRecordItem.dateTime.ToShortDateString();
            string short_time = balanceRecordItem.dateTime.ToShortTimeString();

            string Information = $"Баланс \"{balanceRecordItem.account.Name}\" - {balanceRecordItem.Amount} ({short_date} | {short_time})";
            dataPackage.SetText(Information);
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
            ClipboardNotification.IsOpen = true;
        }
    }
}
