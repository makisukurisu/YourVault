﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using YourVault.Database;
using YourVault.Models;

namespace YourVault.Services.BankProviders
{
    internal class BankProviderService : IBankProviderService, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private ObservableCollection<Models.BankProvider> providers;

        public BankProviderService()
        {
            providers = new ObservableCollection<Models.BankProvider>();
            UpdateBankProviders();
        }

        public ObservableCollection<BankProvider> GetBankProviders()
        {
            return providers;
        }

        public void UpdateBankProviders()
        {
            providers.Clear();
            var providersFromDB = BankProvidersDataAccess.GetBankProviders();
            foreach (var provider in providersFromDB)
            {
                providers.Add(provider);
            }
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
