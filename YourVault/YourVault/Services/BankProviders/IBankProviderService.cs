using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourVault.Services.BankProviders
{
    internal interface IBankProviderService
    {

        ObservableCollection<ViewModels.BankProvider> GetBankProviders();

        public void UpdateBankProviders();


    }
}
