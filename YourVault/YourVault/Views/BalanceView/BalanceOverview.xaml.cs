using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using YourVault.Controls.Balance;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Views.BalanceView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BalanceOverview : Page
    {
        public ObservableCollection<Models.Balance> BalanceOverwies = new ObservableCollection<Models.Balance>();
        public ObservableCollection<Models.Account> selectedAccounts = new ObservableCollection<Models.Account>();

        public BalanceOverview()
        {
            this.InitializeComponent();
        }

        private void ItemsView_SelectionChanged(ItemsView sender, ItemsViewSelectionChangedEventArgs args)
        {
            selectedAccounts.Clear();
            foreach (Models.Balance item in overviews.SelectedItems)
            {
                selectedAccounts.Add(item.account);
            }
        }
    }
}
