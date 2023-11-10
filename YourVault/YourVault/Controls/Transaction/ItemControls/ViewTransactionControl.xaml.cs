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

            string text = $"���������� �{currentTransaction.ID}";
            text += $"\n\n������� ID: {currentTransaction.ExternalID}";
            text += $"\n�������: {currentTransaction.account.Name}";
            text += $"\n���: {currentTransaction.TransactionType}";
            text += $"\n����: {currentTransaction.Amount}";
            text += $"\n����: {currentTransaction.CreatedAt}";
            text += $"\n\n����: {currentTransaction.Description}";

            var dataPackage = new DataPackage();
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);

            ClipboardNotification.IsOpen = true;
        }
    }
}
