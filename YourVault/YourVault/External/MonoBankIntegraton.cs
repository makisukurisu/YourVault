using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YourVault.Database;
using YourVault.Models;

namespace YourVault.External
{

    public class MonoBankTransaction
    {
        public string id { get; set; }
        public long time { get; set; }
        public string description { get; set; }
        public int amount { get; set; }

        public int balance { get; set; }
        //public string fullRecord { get; set; }
    }

    public class Converters
    {
        public string ConvertCodeToCurrency(int CurrencyCode)
        {
            if (CurrencyCode == 980) { return "UAH"; }
            if (CurrencyCode == 840) { return "USD"; }
            if (CurrencyCode == 978) { return "EUR"; }
            return "👽SD";
        }
        public static float CentsToFloat(int? cents)
        {
            if (cents.HasValue) {
                return (float)Math.Round(cents.Value / 100.0f, 2);
            }
            else
            {
                return -1.0f;
            }
        }

    }

    public class MonoBankAccountResponse : Converters
    {

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("sendId")]
        public string SendID { get; set; }

        [JsonProperty("balance")]
        private int _rawBalance { get; set; }
        public float Balance { get { return CentsToFloat(_rawBalance); } set {; } }

        [JsonProperty("creditLimit")]
        private int _rawCreditLimit { get; set; }
        public float CreditLimit { get { return CentsToFloat(_rawCreditLimit); } set {; } }

        [JsonProperty("type")]
        private string _rawType { get; set; }
        public string Type {
            get
            {
                string value = _rawType;
                if (value.Length < 1) { return value; }
                else
                {
                    return string.Concat(value[0].ToString().ToUpper(), value.AsSpan(1));
                }
            }
            set {; }
        }

        [JsonProperty("currencyCode")]
        public int CurrencyCode { get; set; }
        public string Currency { get { return ConvertCodeToCurrency(CurrencyCode); } set { ; } }

        [JsonProperty("cashbackType")]
        public string CashbackType { get; set; }

        [JsonProperty("maskedPan")]
        public List<string> MaskedPan { get; set; }

        [JsonProperty("iban")]
        public string IBAN { get; set; }
    }

    public class MonoBankJarsResponse : Converters
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("sendId")]
        public string SendID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("currencyCode")]
        public int CurrencyCode { get; set; }
        public string Currency { get { return ConvertCodeToCurrency(CurrencyCode); } set { ; } }

        [JsonProperty("balance")]
        private int _rawBalance { get; set; }
        public float Balance { get { return CentsToFloat(_rawBalance); } set { ; } }

        [JsonProperty("goal")]
        private int? _rawGoal { get; set; }
        public float Goal { get { return CentsToFloat(_rawGoal); } set { ; } }



    }

    public class MonoBankClientInfoResponse
    {
        [JsonProperty("clientId")]
        public string ClientID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("webHookUrl")]
        public string WebHookURL { get; set; }

        [JsonProperty("permissions")]
        public string Permissions { get; set; }

        [JsonProperty("accounts")]
        public List<MonoBankAccountResponse> Accounts { get; set; }

        [JsonProperty("jars")]
        public List<MonoBankJarsResponse> Jars { get; set; }

    }

    public class MonoBankIntegraton
    {
        private string XToken;
        private string CardID;
        private int AccountID;

        public MonoBankIntegraton(Models.Account account)
        {
            XToken = account.auth_info;
            CardID = account.ExternalID;
            AccountID = account.ID;
        }

        private List<Transaction> APIGetTransactions()
        {
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
                tr.FullRecord = JsonConvert.SerializeObject(MonoTr, Formatting.Indented);
                tr.CurrentAmount = int.Parse(MonoTr["balance"]) / 100.0;
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
