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
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using YourVault.Services.Account;
using YourVault.Services.Transaction;


namespace YourVault.Controls.Transaction.ItemControls
{
    public sealed partial class ViewTransactionControl : Page
    {
        private IAccountService _accountService;
        private ITransactionService _transactionService;

        private ViewModels.Transaction currentTransaction;

        public ViewTransactionControl()
        {
            this.InitializeComponent();

            _accountService = (IAccountService)((App)Application.Current).ServiceProvider.GetService(typeof(IAccountService));
            _transactionService = (ITransactionService)((App)Application.Current).ServiceProvider.GetService(typeof(ITransactionService));
        }

        public void setTransacton(ViewModels.Transaction transaction)
        {
            currentTransaction = transaction;

            trID.Text = currentTransaction.ID.ToString();
            trExternalID.Text = currentTransaction.ExternalID;
            trAccount.Text = currentTransaction.account.Name;
            trType.Text = currentTransaction.TransactionType;
            trAmount.Text = currentTransaction.Amount.ToString();
            trCreateAt.Text = currentTransaction.CreatedAt.ToString();
            trDescription.Text = currentTransaction.Description;
            trFullRecord.Text = currentTransaction.FullRecord;
        }

        private void CopyToClipboard(object sender, RoutedEventArgs e)
        {

            string text = $"Транзакція №{currentTransaction.ID}";
            text += $"\n\nЗовнішній ID: {currentTransaction.ExternalID}";
            text += $"\nРахунок: {currentTransaction.account.Name}";
            text += $"\nТип: {currentTransaction.TransactionType}";
            text += $"\nСума: {currentTransaction.Amount}";
            text += $"\nДата: {currentTransaction.CreatedAt}";
            text += $"\n\nОпис: {currentTransaction.Description}";

            var dataPackage = new DataPackage();
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);

            ClipboardNotification.IsOpen = true;
        }
    }
}
