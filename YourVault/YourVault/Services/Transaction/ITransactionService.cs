using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourVault.Services.Transaction
{
    internal interface ITransactionService
    {
        public ObservableCollection<ViewModels.Transaction> GetTransactions();
        public ObservableCollection<ViewModels.Transaction> GetTransactions(int accountID);

        public ObservableCollection<ViewModels.Transaction> GetTransactions(string Comment);

        public void UpdateTransactions();

        public void AddTransaction(ViewModels.Transaction transaction);
        public void AddTransaction(ViewModels.Transaction transaction, bool Update);
    }
}
