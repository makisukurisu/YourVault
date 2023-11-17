using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourVault.Services.Transaction
{
    internal interface ITransactionUpdateService
    {
        public void SetUpdateTime();

        public DateTime GetUpdateTime();

    }
}
