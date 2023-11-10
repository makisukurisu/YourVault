using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourVault.ViewModels;

namespace YourVault.Services.Account
{
    public interface IAccountService
    {
        ObservableCollection<ViewModels.Account> GetAccounts();

        public void UpdateAccounts();

    }
}
