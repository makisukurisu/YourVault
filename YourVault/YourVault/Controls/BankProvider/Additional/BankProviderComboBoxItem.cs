using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourVault.Controls.BankProvider.Additional
{
    public sealed partial class BankProviderComboBoxItem : ComboBoxItem
    {
        public Models.BankProvider bankProvider;

        public BankProviderComboBoxItem(Models.BankProvider bankProvider)
        {
            this.bankProvider = bankProvider;
            this.Content = bankProvider.Name;
        }
    }
}
