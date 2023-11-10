using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using YourVault.ViewModels;

namespace YourVault.Database
{
    internal class DataAccess
    {
        public static string getDBPath()
        {
            ApplicationData.Current.LocalFolder.CreateFileAsync("Database.sqlite", CreationCollisionOption.OpenIfExists).AsTask().Wait();
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, "Database.sqlite");
        }

        public static void InitializeTables()
        {
            using (var db = new SqliteConnection($"Filename={getDBPath()}"))
            {
                db.Open();

                string tableCommand = "CREATE TABLE IF NOT EXISTS BankProviders (ID INTEGER PRIMARY KEY AUTOINCREMENT, Name NVARCHAR(2048) UNIQUE, Website NVARCHAR(2048) NULL)";
                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();
                tableCommand = "CREATE TABLE IF NOT EXISTS Accounts (ID INTEGER PRIMARY KEY AUTOINCREMENT, externalID NVARCHAR(2048) NULL, BankProviderID INTEGER NULL, Name NVARCHAR(2048) NULL, auth_info NVARCHAR(4096) NULL, additional_params NVARCHAR(4096) NULL)";
                createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();
                tableCommand = "CREATE TABLE IF NOT EXISTS Transactions (ID INTEGER PRIMARY KEY AUTOINCREMENT, externalID NVARCHAR(2048) NULL, AccountID INTEGER NULL, transactionType NVARCHAR(2048) NULL, amount NVARCHAR(2048) NULL, description NVARCHAR(2048) NULL, createdAt DATETIME NULL, fullRecord BLOB NULL)";
                createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();
                tableCommand = "CREATE TABLE IF NOT EXISTS Comments (ID INTEGER PRIMARY KEY AUTOINCREMENT, TransactionID INTEGER NULL, comment NVARCHAR(2048) NULL, file BLOB NULL, createdAt DATETIME NULL)";
                createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();

                tableCommand = "PRAGMA foreign_keys = ON";
                createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();
                tableCommand ="CREATE TRIGGER IF NOT EXISTS delete_account_transactions " +
                    "AFTER DELETE ON Accounts " +
                    "FOR EACH ROW " +
                    "BEGIN " +
                    "DELETE FROM Transactions WHERE AccountID = OLD.ID;" +
                    "END;";
                createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();

                tableCommand = "CREATE TRIGGER IF NOT EXISTS delete_transaction_comments " +
                    "AFTER DELETE ON Transactions " +
                    "FOR EACH ROW " +
                    "BEGIN " +
                    "DELETE FROM Comments WHERE TransactionID = OLD.ID;" +
                    "END;";
                createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();

                db.Close();
            }
            
            CreateDefaultData();
        }

        public static void CreateDefaultData()
        {
            var connection = GetConnecton();
            using (connection)
            {
                connection.Open();
                List<BankProvider> bankProviders = new List<BankProvider>
                {
                    new BankProvider(1, "Монобанк", "https://api.monobank.ua/"),
                };

                foreach (var bankProvider in bankProviders)
                {
                    var command = GetCommand("BankProviders", new string[] { "MAX(ID)" }, null, new string[] {});
                    command.Connection = connection;
                    var reader = command.ExecuteReader();

                    reader.Read();

                    int max_id = 0;
                    if (!reader.IsDBNull(0))
                    {
                        max_id = reader.GetInt32(0);
                    }

                    reader.Close();
                    

                    try { 
                        WriteData("BankProviders", new object[] { max_id+1, bankProvider.Name, bankProvider.Website });
                    }
                    catch (SqliteException e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
                connection.Close();
            }
        }

        public static void WriteData(string tableName, object[] data)
        {
            using (var db = new SqliteConnection($"Filename={getDBPath()}"))
            {
                db.Open();

                string tableCommand = $"INSERT INTO {tableName} VALUES (";
                for (int i = 0; i < data.Length; i++)
                {
                    tableCommand += $"@{i},";
                }
                tableCommand = tableCommand.Remove(tableCommand.Length - 1);
                tableCommand += ")";
                SqliteCommand insertCommand = new SqliteCommand(tableCommand, db);
                for (int i = 0; i < data.Length; i++)
                {
                    insertCommand.Parameters.AddWithValue($"@{i}", data[i]);
                }
                insertCommand.ExecuteReader();
                db.Close();
            }
        }


        public static SqliteConnection GetConnecton()
        {
            return new SqliteConnection($"Filename={getDBPath()}");
        }

        public static SqliteCommand GetCommand(string tableName, string[] columns, string whereClause, string[] whereClauseParams)
        {
            using (var db = GetConnecton())
            {
                db.Open();
                string tableCommand = $"SELECT {string.Join(",", columns)} FROM {tableName}";
                if (whereClause != null)
                {
                    tableCommand += $" WHERE {whereClause}";
                }
                SqliteCommand selectCommand = new SqliteCommand(tableCommand, db);
                for (int i = 0; i < whereClauseParams.Length; i++)
                {
                    selectCommand.Parameters.AddWithValue($"@{i}", whereClauseParams[i]);
                }
                db.Close();
                return selectCommand;
            }
        }
        

        public static void DeleteRecordByID(string tableName, int id)
        {
            using (var db = GetConnecton())
            {
                db.Open();
                string tableCommand = $"DELETE FROM {tableName} WHERE ID = @0";
                SqliteCommand deleteCommand = new SqliteCommand(tableCommand, db);
                deleteCommand.Parameters.AddWithValue($"@0", id);
                deleteCommand.ExecuteReader();
                db.Close();
            }
        }

    }
}
