using System.Collections.ObjectModel;
using System.Collections.Specialized;
using YourVault.Database;

namespace YourVault.Services.Account
{
    public class AccountService : IAccountService, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private ObservableCollection<ViewModels.Account> accounts;

        public AccountService()
        {
            accounts = new ObservableCollection<ViewModels.Account>();
            UpdateAccounts();
        }

        public void UpdateAccounts()
        {
            accounts.Clear();
            var accountsFromDB = AccountDataAccess.GetAllAccounts();
            foreach (var account in accountsFromDB)
            {
                accounts.Add(account);
            }
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public ObservableCollection<ViewModels.Account> GetAccounts()
        {
            return accounts;
        }
    }
}
