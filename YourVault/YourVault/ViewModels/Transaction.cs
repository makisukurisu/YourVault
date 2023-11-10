using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourVault.Database;

namespace YourVault.ViewModels
{
    public class Transaction
    {
        public int ID { get; set; }
        public string ExternalID { get; set; }
        public Account account { get; set; }
        public int AccountID {
            get { return account.ID; }
            set { account = AccountDataAccess.GetAccountByID(value); }
        }

        public string TransactionType { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FullRecord { get; set; }
    }
}
