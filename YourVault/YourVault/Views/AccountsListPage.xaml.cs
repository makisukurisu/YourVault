using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using Windows.Foundation;
using Windows.Foundation.Collections;
using YourVault.Controls;
using YourVault.Controls.Account.ItemControls;
using YourVault.Database;
using YourVault.Services.Account;
using YourVault.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AccountsListPage : Page
    {

        private IAccountService _accountService;
        public ObservableCollection<Account> accountItems { get; set; }

        public AccountsListPage()
        {
            this.InitializeComponent();

            _accountService = (IAccountService)((App)Application.Current).ServiceProvider.GetService(typeof(IAccountService));
            accountItems = _accountService.GetAccounts();

        }

        private async void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            AccountCreateControl editControl = new AccountCreateControl();

            ContentDialog dialog = new ContentDialog
            {
                Content = editControl,
                CloseButtonText = "Скасувати",
                PrimaryButtonText = "Додати",
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync(ContentDialogPlacement.Popup);

            if (result == ContentDialogResult.Primary)
            {
                editControl.CreateAccount();
                _accountService.UpdateAccounts();
            }
        }
    }
}
