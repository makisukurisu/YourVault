using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourVault.Models;

namespace YourVault.Database
{
    internal class BankProvidersDataAccess
    {
        public BankProvidersDataAccess() { }

        public static List<BankProvider> GetBankProviders()
        {
            var bankProviders = new List<BankProvider>();

            var command = DataAccess.GetCommand("BankProviders", new string[] {"ID", "Name", "Website"}, null, new string[] {});
            command.Connection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                bankProviders.Add(new BankProvider(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            }

            command.Connection.Close();

            return bankProviders;
        }

        public static BankProvider GetProviderByID(int providerID)
        {
            BankProvider provider = null;
            var command = DataAccess.GetCommand("BankProviders", new string[] { "ID", "Name", "Website" }, "ID = @0", new string[] { providerID.ToString() });
            command.Connection.Open();

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                provider = new BankProvider(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
            }
            if (provider is null)
            {
                provider = new BankProvider(providerID, "null", "null");
            }
            command.Connection.Close();

            return provider;
        }
    }
}
