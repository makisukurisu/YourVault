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
using YourVault.Database;
using YourVault.Services.BankProviders;
using YourVault.ViewModels;


namespace YourVault.Controls.Account.ItemControls
{
    public sealed partial class AccountCreateControl : Page
    {
        private IBankProviderService _bankProviderService;

        public AccountCreateControl()
        {
            this.InitializeComponent();

            _bankProviderService = (IBankProviderService)((App)Application.Current).ServiceProvider.GetService(typeof(IBankProviderService));


            foreach (var provider in _bankProviderService.GetBankProviders())
            {
                BankProviderComboBox.Items.Add(new BankProviderComboBoxItem(provider));
            }
            BankProviderComboBox.SelectedIndex = 0;
        }

        public void CreateAccount()
        {
            AccountDataAccess.CreateAccount(new ViewModels.Account(
                id: -1,
                externalID: ExternalIDTextBox.Text,
                bankProviderID: ((BankProviderComboBoxItem)BankProviderComboBox.SelectedItem).bankProvider.ID,
                name: NameTextBox.Text,
                auth_info: AuthInfoTextBox.Text,
                additional_params: ""
                ));
        }

    }
}
