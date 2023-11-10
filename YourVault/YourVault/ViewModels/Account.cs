using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourVault.Database;

namespace YourVault.ViewModels
{
    public class Account
    {
        public int ID { get; set; }

        public string ExternalID { get; set; }

        public BankProvider bankProvider { get;set; }

        public int bankProviderID {
            get { return (int)(bankProvider?.ID); }
            set {
                bankProvider = BankProvidersDataAccess.GetProviderByID(value);
            }
        }

        public string Name { get; set; }
        public string auth_info { get; set; }

        public string additional_params { get; set; }

        public Account(int id, string externalID, int bankProviderID, string name, string auth_info, string additional_params)
        {
            ID = id;
            ExternalID = externalID;
            this.bankProviderID = bankProviderID;
            Name = name;
            this.auth_info = auth_info;
            this.additional_params = additional_params;
        }
    }
}
