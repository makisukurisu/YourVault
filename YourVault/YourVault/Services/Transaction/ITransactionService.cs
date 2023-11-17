using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourVault.Services.Transaction
{
    public class TransactionSearchParams
    {
        public int page { get; set; }
        public int? accountID { get; set; }
        public string Comment { get; set; }

        public TransactionSearchParams(int page, int? accountID, string Comment)
        {
            this.page = page;
            this.accountID = accountID;
            this.Comment = Comment;
        }
    }
    internal interface ITransactionService
    {
        public TransactionSearchParams searchParams { get; set; }

        public ObservableCollection<Models.Transaction> GetTransactions();
        public ObservableCollection<Models.Transaction> GetTransactions(int accountID);
        public ObservableCollection<Models.Transaction> GetTransactions(string Comment);

        public ObservableCollection<Models.Transaction> GetTransactionsPage(TransactionSearchParams searchParams);

        public void UpdateTransactions();

        public void AddTransaction(Models.Transaction transaction);
        public void AddTransaction(Models.Transaction transaction, bool Update);
    }
}
