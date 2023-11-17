using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YourVault.Database;

namespace YourVault.Models
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
        public IntPtr DispatcherQueuePriority { get; internal set; }

        public Account(int id, string externalID, int bankProviderID, string name, string auth_info, string additional_params)
        {
            ID = id;
            ExternalID = externalID;
            this.bankProviderID = bankProviderID;
            Name = name;
            this.auth_info = auth_info;
            this.additional_params = additional_params;
            DispatcherQueuePriority = DispatcherQueuePriority;
        }

        public override int GetHashCode()
        {
            var inputBytes = Encoding.UTF8.GetBytes($"{ID}{Name}{auth_info}");

            using (var sha = SHA256.Create())
            {
                var hashBytes = sha.ComputeHash(inputBytes);

                var hash = BitConverter.ToInt32(hashBytes, 0);

                return hash;
            }
        }

        public Windows.UI.Color GetColor()
        {
            var accountHash = GetHashCode();
            return Windows.UI.Color.FromArgb(255, (byte)(accountHash % 126), (byte)((accountHash * 2) % 126), (byte)((accountHash * 3) % 126));
        }
    }
}
