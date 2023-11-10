using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace YourVault.Database
{
    public class TransactionDataAccess
    {
        //tableCommand = "CREATE TABLE IF NOT EXISTS Transactions (ID INTEGER PRIMARY KEY AUTOINCREMENT, externalID NVARCHAR(2048) NULL, AccountID INTEGER NULL, transactionType NVARCHAR(2048) NULL, amount NVARCHAR(2048) NULL, description NVARCHAR(2048) NULL, createdAt DATETIME NULL, fullRecord BLOB NULL)";
        public static void AddTransaction(ViewModels.Transaction transaction)
        {
            string query = "INSERT INTO Transactions (externalID, AccountID, transactionType, amount, description, createdAt, fullRecord) VALUES (@externalID, @AccountID, @transactionType, @amount, @description, @createdAt, @fullRecord)";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@externalID", transaction.ExternalID },
                { "@AccountID", transaction.AccountID },
                { "@transactionType", transaction.TransactionType },
                { "@amount", transaction.Amount },
                { "@description", transaction.Description },
                { "@createdAt", transaction.CreatedAt },
                { "@fullRecord", transaction.FullRecord }
            };

            var con = DataAccess.GetConnecton();
            var command = new SqliteCommand(query, con);
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        private static ViewModels.Transaction extractFromReader(SqliteDataReader reader)
        {
            var value = new ViewModels.Transaction();
            value.AccountID = reader.GetInt32(2);
            value.Amount = double.Parse(reader.GetString(4).Replace(".", ","));
            value.CreatedAt = reader.GetDateTime(6);
            value.Description = reader.GetString(5);
            value.ExternalID = reader.GetString(1);
            value.FullRecord = reader.GetString(7);
            value.ID = reader.GetInt32(0);
            value.TransactionType = reader.GetString(3);
            return value;
        }

        public static ObservableCollection<ViewModels.Transaction> GetTransactions()
        {
            var transactions = new ObservableCollection<ViewModels.Transaction>();
            
            var comand = DataAccess.GetCommand("Transactions", new string[] { "ID", "externalID", "AccountID", "transactionType", "amount", "description", "createdAt", "fullRecord" }, null, new string[] { });
            comand.Connection.Open();
            var reader = comand.ExecuteReader();

            while (reader.Read())
            {
                transactions.Add(extractFromReader(reader));
            }

            return transactions;
        }

        public static ObservableCollection<ViewModels.Transaction> GetTransactions(int accountID)
        {
            var transactions = new ObservableCollection<ViewModels.Transaction>();

            var comand = DataAccess.GetCommand("Transactions", new string[] { "ID", "externalID", "AccountID", "transactionType", "amount", "description", "createdAt", "fullRecord" }, "AccountID = @0", new string[] { accountID.ToString() });
            comand.Connection.Open();
            var reader = comand.ExecuteReader();

            while (reader.Read())
            {
                transactions.Add(extractFromReader(reader));
            }

            return transactions;
        }

        public static ObservableCollection<ViewModels.Transaction> GetTransactions(int accountID, string externalID)
        {
            var transactions = new ObservableCollection<ViewModels.Transaction>();

            var command = DataAccess.GetCommand("Transactions", new string[] { "ID", "externalID", "AccountID", "transactionType", "amount", "description", "createdAt", "fullRecord" }, "AccountID = @0 AND externalID = @1", new string[] { accountID.ToString(), externalID });
            command.Connection.Open();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                transactions.Add(extractFromReader(reader));
            }
            return transactions;
        }

        public static ObservableCollection<ViewModels.Transaction> GetTransactions(string description)
        {
            var transactions = new ObservableCollection<ViewModels.Transaction>();

            var comand = DataAccess.GetCommand("Transactions", new string[] { "ID", "externalID", "AccountID", "transactionType", "amount", "description", "createdAt", "fullRecord" }, "description LIKE @0", new string[] { $"%{description}%" });
            comand.Connection.Open();

            var reader = comand.ExecuteReader();
            while (reader.Read())
            {
                transactions.Add(extractFromReader(reader));
            }
            return transactions;
        }
    }
}
