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
using YourVault.Controls.Transaction.ItemControls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Controls.Transaction
{
    public sealed partial class TransactionListItem : UserControl
    {

        public static readonly DependencyProperty TransactionProperty = DependencyProperty.Register("Transaction", typeof(ViewModels.Transaction), typeof(TransactionListItem), new PropertyMetadata(null, OnTransactionChanged));
        public ViewModels.Transaction Transaction
        {
            get { return (ViewModels.Transaction)GetValue(TransactionProperty); }
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
                control.IDBlock.Text = control.Transaction.ID.ToString();
                control.ExternalIDBlock.Text = control.Transaction.ExternalID;
                control.AccountIDBlock.Text = control.Transaction.account.Name;
                control.TransactionTypeBlock.Text = control.Transaction.TransactionType;
                control.AmountBlock.Text = control.Transaction.Amount.ToString();
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
