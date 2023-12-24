using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using YourVault.Views;
using YourVault.Views.AccountView;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(TitleGrid);
        }

        public void NavigateToPage(Type pageType)
        {
            MainFrame.Navigate(pageType);
        }

        public void SetTitle(string title)
        {
            MainNavigationView.Header = title;
        }

        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;
            var selectedItemTag = ((string)selectedItem.Tag);


            if (selectedItemTag != null) {
                MainNavigationView.Header = selectedItem.Content.ToString();
                switch (selectedItemTag)
                {
                    case "Accounts":
                        MainFrame.Navigate(typeof(AccountsListPage));
                        break;
                    case "Balances":
                        MainFrame.Navigate(typeof(BalanceListPage));
                        break;
                    case "Transactions":
                        MainFrame.Navigate(typeof(TransactionsListPage));
                        break;
                    case "Settings":
                        MainFrame.Navigate(typeof(BlankMainPage));
                        break;
                    case "Help":
                        MainFrame.Navigate(typeof(HelpPage));
                        break;
                    default:
                        MainFrame.Navigate(typeof(BlankMainPage));
                        break;
                }
            }
        }
    }
}
