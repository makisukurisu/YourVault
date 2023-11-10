using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourVault.ViewModels;

namespace YourVault.Database
{
    internal class AccountDataAccess
    {
        public AccountDataAccess() { }

        public static List<Account> GetAllAccounts()
        {
            var accounts = new List<Account>();

            var command = DataAccess.GetCommand("Accounts", new string[] { "ID", "ExternalID", "BankProviderID", "Name", "Auth_Info", "Additional_Params" }, null, new string[] { });
            command.Connection.Open();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                accounts.Add(new Account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5)));
            }

            return accounts;

        }

        public static Account GetAccountByID(int accountID)
        {
            var command = DataAccess.GetCommand("Accounts", new string[] { "ID", "ExternalID", "BankProviderID", "Name", "Auth_Info", "Additional_Params" }, "ID = @0", new string[] { accountID.ToString() });
            command.Connection.Open();
            var reader = command.ExecuteReader();

            Account account = null;
            while (reader.Read())
            {
                account = new Account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
            }
            return account;
        }

        public static void DeleteAccountByID(int id)
        {
            DataAccess.DeleteRecordByID("Accounts", id);
        }

        public static void UpdateAccount(Account account)
        {
            string commandText = $"UPDATE Accounts SET ExternalID = @0, BankProviderID = @1, Name = @2, Auth_Info = @3, Additional_Params = @4 WHERE ID = @5";
            var command = new SqliteCommand(commandText, DataAccess.GetConnecton());
            
            command.Parameters.AddWithValue("@0", account.ExternalID);
            command.Parameters.AddWithValue("@1", account.bankProviderID);
            command.Parameters.AddWithValue("@2", account.Name);
            command.Parameters.AddWithValue("@3", account.auth_info);
            command.Parameters.AddWithValue("@4", account.additional_params);
            command.Parameters.AddWithValue("@5", account.ID);

            command.Connection.Open();

            command.ExecuteNonQuery();

            command.Connection.Close();
        }

        public static void CreateAccount(Account account)
        {
            string commandText = $"INSERT INTO Accounts (ExternalID, BankProviderID, Name, Auth_Info, Additional_Params) VALUES (@0, @1, @2, @3, @4)";
            var command = new SqliteCommand(commandText, DataAccess.GetConnecton());

            command.Parameters.AddWithValue("@0", account.ExternalID);
            command.Parameters.AddWithValue("@1", account.bankProviderID);
            command.Parameters.AddWithValue("@2", account.Name);
            command.Parameters.AddWithValue("@3", account.auth_info);
            command.Parameters.AddWithValue("@4", account.additional_params);

            command.Connection.Open();

            command.ExecuteNonQuery();

            command.Connection.Close();
        }

    }
}
