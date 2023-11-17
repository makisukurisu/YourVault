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
using YourVault.Controls.BankProvider.Additional;
using YourVault.Services.BankProviders;
using YourVault.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Controls
{

    public sealed partial class AccountEditControl : Page
    {

        private IBankProviderService _bankProviderService;
        private Models.Account currentAccount;

        public AccountEditControl()
        {
            this.InitializeComponent();

            _bankProviderService = (IBankProviderService)((App)Application.Current).ServiceProvider.GetService(typeof(IBankProviderService));
        }

        public void SetAccount(Models.Account account)
        {
            currentAccount = account;

            NameTextBox.Text = account.Name;
            ExternalIDTextBox.Text = account.ExternalID;
            BankProviderComboBox.Items.Clear();
            BankProviderComboBoxItem currentItem = null;
            foreach (var provider in _bankProviderService.GetBankProviders())
            {
                BankProviderComboBox.Items.Add(new BankProviderComboBoxItem(provider));
                if (provider.ID == account.bankProviderID)
                {
                    currentItem = (BankProviderComboBoxItem)BankProviderComboBox.Items.Last();
                }
            }
            BankProviderComboBox.SelectedItem = currentItem;
            AuthInfoTextBox.Text = account.auth_info;
        }

        public Models.Account GetAccount()
        {
            return new Models.Account(
                id: currentAccount.ID,
                name: NameTextBox.Text,
                externalID: ExternalIDTextBox.Text,
                bankProviderID: ((BankProviderComboBoxItem)BankProviderComboBox.SelectedItem).bankProvider.ID,
                auth_info: AuthInfoTextBox.Text,
                additional_params: currentAccount.additional_params
            );
        }
    }
}
