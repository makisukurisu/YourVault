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
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using YourVault.Services.Account;
using YourVault.Services.Balance;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BalanceListPage : Page
    {

        public ObservableCollection<Models.Balance> balances { get; set; } = new ObservableCollection<Models.Balance>();
        public ObservableCollection<Models.Account> selectedAccounts = new ObservableCollection<Models.Account>();
        private IBalanceService balanceService;
        private bool filtering = false;

        public BalanceListPage()
        {
            this.InitializeComponent();

            balances.CollectionChanged += Balances_CollectionChanged;
            balanceService = (IBalanceService)((App)Application.Current).ServiceProvider.GetService(typeof(IBalanceService));
            var tmpBalances = balanceService.GetBalances;
            foreach(var balance in tmpBalances)
            {
                balances.Add(balance);
            }
            balanceService.GetBalances.CollectionChanged += AppendBalance;

            selectedAccounts = balanceOveviewPage.selectedAccounts;
            selectedAccounts.CollectionChanged += SelectedAccounts_CollectionChanged;
        }

        private void AppendBalance(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems == null) { return; }
            foreach(var item in e.NewItems)
            {
                balances.Insert(0, item as Models.Balance);
            }
        }

        private void SelectedAccounts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            balanceListPage.allBalances.Clear();
            List<Models.Account> Selected = selectedAccounts.ToList();
            List<Models.Balance> AllBalances = new List<Models.Balance>();
            foreach (var account in Selected) {
                AllBalances.AddRange(Database.BalanceDataAccess.GetBalances(account));
            }
            if(Selected.Count == 0)
            {
                AllBalances = Database.BalanceDataAccess.GetBalances();
            }
            AllBalances = AllBalances.OrderByDescending(b => b.dateTime).ToList();
            foreach (var balance in AllBalances)
            {
                balanceListPage.allBalances.Add(balance);
            }
        }

        private void Balances_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null) {
                if (filtering) { balanceListPage.allBalances.Clear(); }
                return;
            }
            foreach (var item in e.NewItems)
            {
                balanceOveviewPage.BalanceOverwies.Remove(balanceOveviewPage.BalanceOverwies.FirstOrDefault(b => b.AccountID == (item as Models.Balance).AccountID));
                balanceOveviewPage.BalanceOverwies.Insert(0, item as Models.Balance);
                balanceListPage.allBalances.Insert(0, item as Models.Balance);
            }
        }

        
    }
}
