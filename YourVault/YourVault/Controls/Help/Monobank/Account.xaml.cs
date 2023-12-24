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
using YourVault.External;
using Microsoft.UI.Xaml.Documents;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Controls.Help.Monobank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Account : Page
    {
        public Account()
        {
            this.InitializeComponent();
        }

        public Account(MonoBankAccountResponse account)
        {
            this.InitializeComponent();

            AccountID.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = account.ID } } });
            AccountType.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = account.Type } } });

            AccountCurrency.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = account.Currency } } });
            Balance.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = account.Balance.ToString() } } });
            CreditLimit.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = account.CreditLimit.ToString() } } });

            foreach (var item in account.MaskedPan)
            {
                CardNumbers.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = item.ToString() } } });
            }
            IBAN.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = account.IBAN } } });

            SendID.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = account.SendID } } });
            CashbackType.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = account.CashbackType } } });
        }
    }
}
