using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourVault.Database;

namespace YourVault.Models
{
    public class Transaction : IEquatable<Transaction>
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

        public double CurrentAmount { get; set; }
        public string Description { get; set; }

        private DateTime _createdAt;
        public DateTime CreatedAt { get { return _createdAt; } set { _createdAt = value.ToLocalTime(); } }
        public string FullRecord { get; set; }

        public bool Equals(Transaction other)
        {
            if(other == null)
            {
                return false;
            }
            return this.ExternalID.Equals(other.ExternalID) && this.AccountID == other.AccountID;
        }
    }
}
