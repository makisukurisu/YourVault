using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourVault.Models;

namespace YourVault.Services.Account
{
    public interface IAccountService
    {
        ObservableCollection<Models.Account> GetAccounts();

        public void UpdateAccounts();

    }
}
