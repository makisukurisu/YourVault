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
using Windows.Foundation;
using Windows.Foundation.Collections;
using YourVault.Controls;
using YourVault.Database;
using YourVault.Services.BankProviders;
using YourVault.ViewModels;

namespace YourVault.Views
{
    public sealed partial class BankProvidersPage : Page
    {

        private IBankProviderService _bankProviderService;
        public ObservableCollection<BankProvider> bankProviderItems { get; set; }

        public BankProvidersPage()
        {
            this.InitializeComponent();

            _bankProviderService = (IBankProviderService)((App)Application.Current).ServiceProvider.GetService(typeof(IBankProviderService));
            bankProviderItems = _bankProviderService.GetBankProviders();
        }
    }
}
