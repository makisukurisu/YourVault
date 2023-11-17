using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using YourVault.Controls.Transaction.ItemControls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Controls.Transaction
{
    public sealed partial class TransactionListItem : UserControl
    {

        public static readonly DependencyProperty TransactionProperty = DependencyProperty.Register("Transaction", typeof(Models.Transaction), typeof(TransactionListItem), new PropertyMetadata(null, OnTransactionChanged));
        public Models.Transaction Transaction
        {
            get { return (Models.Transaction)GetValue(TransactionProperty); }
            set { SetValue(TransactionProperty, value); }
        }

        public TransactionListItem()
        {
            this.InitializeComponent();
        }

        public static void OnTransactionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TransactionListItem;
            if (control.Transaction is not null)
            {
                var color = control.Transaction.account.GetColor();
                var brush = new AcrylicBrush();
                brush.TintColor = color;
                brush.TintOpacity = 0.5;
                control.AccountElipse.Fill = brush;
                control.IDBlock.Text = $"#{control.Transaction.ID}";
                control.ExternalIDBlock.Text = control.Transaction.ExternalID;
                control.AccountIDBlock.Text = control.Transaction.account.Name;
                control.TransactionTypeBlock.Text = control.Transaction.TransactionType;
                string sign = "-";
                if (control.Transaction.Amount > 0)
                {
                    sign = "+";
                }
                control.AmountBlock.Text = $"{sign} {Math.Abs(control.Transaction.Amount)}";
                control.DescriptionBlock.Text = control.Transaction.Description;
                control.dateTimeBlock.Text = control.Transaction.CreatedAt.ToString(DateTimeFormatInfo.CurrentInfo.FullDateTimePattern);
            }
            else
            {
                Console.WriteLine("Transaction is null");
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            ViewTransactionControl control = new ViewTransactionControl();
            control.setTransacton(Transaction);

            ContentDialog dialog = new ContentDialog
            {
                Title = $"Деталі транзакції {Transaction.ID}",
                Content = control,
                CloseButtonText = "Ок",
                XamlRoot = this.XamlRoot
            };

            _ = dialog.ShowAsync();
        }
    }
}
