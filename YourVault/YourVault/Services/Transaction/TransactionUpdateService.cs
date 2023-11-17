using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourVault.Services.Transaction
{
    internal class TransactionUpdateService : ITransactionUpdateService
    {
        private DateTime _updateTime;
        public DateTime GetUpdateTime()
        {
            return _updateTime;
        }

        public void SetUpdateTime()
        {
            _updateTime = DateTime.Now;
        }
    }
}
