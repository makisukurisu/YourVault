using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourVault.Services.Balance
{
    class BalanceService : IBalanceService
    {
        private ObservableCollection<Models.Balance> balances = new ObservableCollection<Models.Balance>();
        public ObservableCollection<Models.Balance> GetBalances { get { UpdateBalances(); return balances; } set {; } }

        public void InsertBalance(Models.Balance balance)
        {
            Database.BalanceDataAccess.AddBalance(balance);
        }

        public void UpdateBalances()
        {
            balances.Clear();
            var balancesFromDB = Database.BalanceDataAccess.GetBalances();
            balancesFromDB = balancesFromDB.OrderBy(b => b.dateTime).ToList();
            foreach (var balance in balancesFromDB)
            {
                balances.Add(balance);
            }
        }
    }
}
