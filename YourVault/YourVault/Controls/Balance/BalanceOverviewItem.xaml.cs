using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Controls.Balance
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BalanceOverviewItem : Page
    {

        public bool IsSelected { get; set; } = false;
        public static readonly DependencyProperty BalanceItemProperty = DependencyProperty.Register("balanceRecord", typeof(Models.Balance), typeof(BalanceOverviewItem), new PropertyMetadata(null, OnBalanceChanged));

        public BalanceOverviewItem()
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
            var control = d as BalanceOverviewItem;
            if (control.balanceRecord is not null)
            {
                var color = control.balanceRecord.account.GetColor();
                var brush = new AcrylicBrush();
                brush.TintColor = color;
                brush.TintOpacity = 0.5;
                control.nameElipse.Fill = brush;
                control.textOnElipse.Text = control.balanceRecord.account.Name[..1];
                control.nameTextBlock.Text = control.balanceRecord.account.Name;

                string short_date = control.balanceRecord.dateTime.ToShortDateString();
                string short_time = control.balanceRecord.dateTime.ToShortTimeString();
                control.balanceTextBlock.Text = $"На {short_date} {short_time} - {control.balanceRecord.Amount}";
            }
            else
            {
                Console.WriteLine("Balance is null");
            }
        }
    }
}
