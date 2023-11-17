using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourVault.Database;

namespace YourVault.Models
{
    public class Balance
    {
        public int ID { get; set; }
        public Account account { get; set; }
        public int AccountID
        {
            get { return (int)(account?.ID); }
            set
            {
                account = AccountDataAccess.GetAccountByID(value);
            }
        }

        public double Amount { get; set; }

        public string Currency { get; set; }

        private DateTime _dateTime;
        public DateTime dateTime { get { return _dateTime; } set { _dateTime = value.ToLocalTime(); } }

        public Balance(int id, int accountID, double amount, DateTime dateTime)
        {
            ID = id;
            AccountID = accountID;
            Amount = amount;
            this.dateTime = dateTime;
        }

        public override int GetHashCode()
        {
            return $"{ID}{account.GetHashCode()}".GetHashCode();
        }

    }
}
