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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using YourVault.Services.Transaction;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransactionsListPage : Page
    {

        private ITransactionService _transactionService;
        public ObservableCollection<ViewModels.Transaction> transactions { get; set; }

        public TransactionsListPage()
        {
            
            this.InitializeComponent();

            _transactionService = (ITransactionService)((App)Application.Current).ServiceProvider.GetService(typeof(ITransactionService));
            transactions = _transactionService.GetTransactions();

            transactions.CollectionChanged += TransactionChanged;
            TransactionChanged(null, null);
        }

        private void TransactionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var root = this.XamlRoot;
            if (root is null) { return; }
            var Content = root.Content;
            if (Content is null) { return; }
            ((NavigationView)Content).Header = $"Всього {transactions.Count} трназакції";
        }

        private void SearchQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            var temp = _transactionService.GetTransactions(((TextBox)sender).Text);

            transactions.Clear();
            foreach (var item in temp)
            {
                transactions.Add(item);
            }
        }

    }
}
