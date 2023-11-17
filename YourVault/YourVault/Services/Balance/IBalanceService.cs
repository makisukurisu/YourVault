using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourVault.Services.Balance
{
    interface IBalanceService
    {
        ObservableCollection<Models.Balance> GetBalances { get; set; }
        public void UpdateBalances();

        public void InsertBalance(Models.Balance balance);
    }
}
