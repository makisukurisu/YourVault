using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YourVault.Database;
using YourVault.ViewModels;

namespace YourVault.External
{

    public class MonoBankTransaction
    {
        public string id { get; set; }
        public long time { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
        //public string fullRecord { get; set; }
    }

    public class MonoBankIntegraton
    {
        private string XToken;
        private string CardID;
        private int AccountID;

        public MonoBankIntegraton(ViewModels.Account account)
        {
            XToken = account.auth_info;
            CardID = account.ExternalID;
            AccountID = account.ID;
        }

        private List<Transaction> APIGetTransactions()
        {
            //Make request to: https://api.monobank.ua/personal/statement/{account}/{from}/{to}
            //Where {account} is card ID
            //Where {from} is unix timestamp of start date (15 days ago from now)
            //Where {to} is unix timestamp of end date (now)
            List<Dictionary<string, string>> monoBankTransactions = null;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Token", XToken);
            client.BaseAddress = new Uri("https://api.monobank.ua/personal/statement/");
            HttpResponseMessage response = client.GetAsync($"{CardID}/{DateTimeOffset.Now.AddDays(-15).ToUnixTimeSeconds()}/{DateTimeOffset.Now.ToUnixTimeSeconds()}").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                //Decompose response into a list of dictionries
                monoBankTransactions = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(result);
            }
            else
            {
                Console.WriteLine("Error");
                return null;
            }

            List<Transaction> transactions = new List<Transaction>();
            foreach(var MonoTr in monoBankTransactions)
            {
                var tr = new Transaction();
                tr.ExternalID = MonoTr["id"];

                string FullDescription = MonoTr["description"];
                string comment;

                MonoTr.TryGetValue("comment", out comment);
                if(comment is not null)
                {
                    FullDescription += $"\n{comment}";
                }

                tr.Description = FullDescription;
                tr.Amount = int.Parse(MonoTr["amount"]) / 100.0;
                tr.AccountID = AccountID;
                tr.TransactionType = "Витрата";
                if (tr.Amount > 0)
                {
                    tr.TransactionType = "Поповнення";
                }
                tr.CreatedAt = DateTimeOffset.FromUnixTimeSeconds(int.Parse(MonoTr["time"])).DateTime;
                //Write a string representation of transaction (Dictionary) to FullRecord
                tr.FullRecord = JsonConvert.SerializeObject(MonoTr, Formatting.Indented);
                transactions.Add(tr);
            }
            return transactions;
        }

        public List<Transaction> GetNewTransactions()
        {
            List<Transaction> newTransactions = new List<Transaction>();

            List<Transaction> allTransactions = APIGetTransactions();

            if (allTransactions is null) { return newTransactions; }

            foreach(var tr in allTransactions)
            {
                var inDB = TransactionDataAccess.GetTransactions(AccountID, tr.ExternalID);
                if (inDB is null) { continue; }
                if (inDB.Count != 0) { continue;  }
                
                newTransactions.Add(tr);
            }

            return newTransactions;
        }
    }
}
