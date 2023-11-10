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
using YourVault.Database;
using YourVault.Services.Account;
using YourVault.ViewModels;
using YourVault.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Controls
{
    public sealed partial class AccountListItem : UserControl
    {

        private IAccountService _accountService;
        public static readonly DependencyProperty AccountEntity = DependencyProperty.Register("account", typeof(ViewModels.Account), typeof(AccountListItem), new PropertyMetadata(null, OnAccountChanged));
        
        public ViewModels.Account account
        {
            get { return (ViewModels.Account)GetValue(AccountEntity); }
            set { SetValue(AccountEntity, value); }
        }
        public AccountListItem()
        {
            this.InitializeComponent();

            _accountService = (IAccountService)((App)Application.Current).ServiceProvider.GetService(typeof(IAccountService));
        }

        public static void OnAccountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AccountListItem;
            if(control.account is not null)
            {
                control.IDBlock.Text = control.account.ID.ToString();
                control.ExternalIDBlock.Text = control.account.ExternalID;
                control.BankProviderBlock.Text = control.account.bankProvider.Name;
                control.AccountNameBlock.Text = control.account.Name;
            }
            else
            {
                Console.WriteLine("Account is null");
            }
        }

        private void ClickDelete(object sender, RoutedEventArgs e)
        {
            AccountDataAccess.DeleteAccountByID(account.ID);
            _accountService.UpdateAccounts();
        }

        private async void ClickEdit(object sender, RoutedEventArgs e)
        {
            AccountEditControl editControl = new AccountEditControl();
            editControl.SetAccount(account);

            ContentDialog dialog = new ContentDialog
            {
                Content = editControl,
                CloseButtonText = "—касувати",
                PrimaryButtonText = "«бер≥гти",
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync(ContentDialogPlacement.InPlace);

            if(result == ContentDialogResult.Primary)
            {
                AccountDataAccess.UpdateAccount(editControl.GetAccount());
                _accountService.UpdateAccounts();
            }
        }
    }
}
