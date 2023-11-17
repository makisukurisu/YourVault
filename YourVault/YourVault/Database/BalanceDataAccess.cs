using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourVault.Database
{
    public class BalanceDataAccess
    {
        public static void AddBalance(Models.Balance balance)
        {
            string commandText = $"INSERT INTO Balances (AccountID, Balance, createdAt) VALUES (@0, @1, @2)";
            var command = new SqliteCommand(commandText, DataAccess.GetConnecton());

            command.Parameters.AddWithValue("@0", balance.AccountID);
            command.Parameters.AddWithValue("@1", balance.Amount);
            command.Parameters.AddWithValue("@2", balance.dateTime);
            
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        public static List<Models.Balance> GetBalances()
        {
            var balances = new List<Models.Balance>();

            string commandText = $"SELECT ID, AccountID, Balance, CreatedAt FROM Balances ORDER BY -CreatedAt LIMIT 100";
            var command = new SqliteCommand(commandText, DataAccess.GetConnecton());
            command.Connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                   balances.Add(new Models.Balance(reader.GetInt32(0), reader.GetInt32(1), reader.GetDouble(2), reader.GetDateTime(3)));
            }
            command.Connection.Close();

            return balances;
        }

        public static List<Models.Balance> GetBalances(Models.Account account, int? limit=null, int? startingFromID = null, int? endID = null)
        {
            var balances = new List<Models.Balance>();

            string commandText = $"SELECT ID, AccountID, Balance, CreatedAt FROM Balances WHERE AccountID = @0 ORDER BY -CreatedAt";
            var command = new SqliteCommand(commandText, DataAccess.GetConnecton());
            command.Parameters.AddWithValue("@0", account.ID);

            if(limit != null)
            {
                commandText += " LIMIT @1";
                command.Parameters.AddWithValue("@1", limit);
            }

            if(startingFromID is not null || endID is not null)
            {
                string replacement = "WHERE AccountID = @0";
                if(startingFromID is not null)
                {
                    replacement += " AND ID > @2";
                    command.Parameters.AddWithValue("@2", startingFromID);
                }
                if(endID is not null)
                {
                    replacement += " AND ID < @3";
                    command.Parameters.AddWithValue("@3", endID);
                }
                commandText = commandText.Replace("WHERE AccountID = @0", replacement);
                command.CommandText = commandText;
            }

            command.Connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                balances.Add(new Models.Balance(reader.GetInt32(0), reader.GetInt32(1), reader.GetDouble(2), reader.GetDateTime(3)));
            }
            command.Connection.Close();
            return balances;
        }
    }
}
