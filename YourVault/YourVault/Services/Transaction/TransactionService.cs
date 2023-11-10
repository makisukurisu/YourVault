using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourVault.Database;

namespace YourVault.Services.Transaction
{
    internal class TransactionService : ITransactionService, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private ObservableCollection<ViewModels.Transaction> transactions;

        public TransactionService()
        {
            transactions = new ObservableCollection<ViewModels.Transaction>();
            UpdateTransactions();
        }

        public void AddTransaction(ViewModels.Transaction transaction)
        {
            TransactionDataAccess.AddTransaction(transaction);
            UpdateTransactions();
        }
        public void AddTransaction(ViewModels.Transaction transaction, bool Update)
        {
            TransactionDataAccess.AddTransaction(transaction);
            if (Update) {
                UpdateTransactions();
            }
        }

        public ObservableCollection<ViewModels.Transaction> GetTransactions()
        {
            return transactions;
        }

        public void UpdateTransactions()
        {
            transactions = TransactionDataAccess.GetTransactions();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public ObservableCollection<ViewModels.Transaction> GetTransactions(int accountID)
        {
            return TransactionDataAccess.GetTransactions(accountID);
        }

        public ObservableCollection<ViewModels.Transaction> GetTransactions(string description)
        {
            return TransactionDataAccess.GetTransactions(description);
        }
    }
}
