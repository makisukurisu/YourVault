using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using YourVault.Database;

namespace YourVault.Services.Transaction
{
    internal class TransactionService : ITransactionService, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private ObservableCollection<Models.Transaction> transactions;
        private TransactionSearchParams _searchParams;
        public TransactionSearchParams searchParams { get { if (_searchParams is null) { _searchParams = new TransactionSearchParams(0, null, null); } return _searchParams; } set { _searchParams = value; } }

        public TransactionService()
        {
            transactions = new ObservableCollection<Models.Transaction>();
            UpdateTransactions();
            searchParams = new TransactionSearchParams(0, null, null);
        }

        public void AddTransaction(Models.Transaction transaction)
        {
            TransactionDataAccess.AddTransaction(transaction);
            UpdateTransactions();
        }
        public void AddTransaction(Models.Transaction transaction, bool Update)
        {
            TransactionDataAccess.AddTransaction(transaction);
            if (Update) {
                UpdateTransactions();
            }
        }

        public ObservableCollection<Models.Transaction> GetTransactions()
        {
            return transactions;
        }

        public void UpdateTransactions()
        {
            var fromDB = TransactionDataAccess.GetTransactionsPage(searchParams);
            foreach(Models.Transaction tr in fromDB)
            {
                if (!transactions.Contains(tr))
                {
                    transactions.Add(tr);
                }
            }
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public ObservableCollection<Models.Transaction> GetTransactions(int accountID)
        {
            searchParams.accountID = accountID;
            return TransactionDataAccess.GetTransactionsPage(searchParams);
        }

        public ObservableCollection<Models.Transaction> GetTransactions(string description)
        {
            searchParams.Comment = description;
            return TransactionDataAccess.GetTransactionsPage(searchParams);
        }

        public ObservableCollection<Models.Transaction> GetTransactionsPage(TransactionSearchParams searchParams)
        {
            if(searchParams == null) { searchParams = this.searchParams; }
            return TransactionDataAccess.GetTransactionsPage(searchParams);
        }
    }
}
