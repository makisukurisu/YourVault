using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using YourVault.Database;
using YourVault.External;
using YourVault.Services.Account;
using YourVault.Services.Balance;
using YourVault.Services.BankProviders;
using YourVault.Services.Transaction;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {

        private static Timer timer;
        public static Microsoft.UI.Dispatching.DispatcherQueue DispatcherQueue { get; private set; }

        public App()
        {
            this.InitializeComponent();

            DataAccess.InitializeTables();

            ConfigureServices();

            timer = new Timer(TimeSpan.FromSeconds(60).TotalMilliseconds);
            timer.Start();
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(UpdateTransactions);
        }

        private static void UpdateTransactions(object sender, ElapsedEventArgs e)
        {
            var _accountsService = (IAccountService)((App)Current).ServiceProvider.GetService(typeof(IAccountService));
            var _transactionService = (ITransactionService)((App)Current).ServiceProvider.GetService(typeof(ITransactionService));
            var _transactionUpdateService = (ITransactionUpdateService)((App)Current).ServiceProvider.GetService(typeof(ITransactionUpdateService));
            var _balanceService = (IBalanceService)((App)Current).ServiceProvider.GetService(typeof(IBalanceService));

            var accounts = _accountsService.GetAccounts();
            foreach(var account in accounts)
            {
                if(account.bankProvider.Name == "Монобанк")
                {
                    var integration = new MonoBankIntegraton(account);
                    List<Models.Transaction> transactions;
                    try {
                        transactions = integration.GetNewTransactions();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }

                    foreach(var transaction in transactions)
                    {
                        _transactionService.AddTransaction(transaction, false);
                    }
                    if (transactions.Count > 0) {
                        var lastTranasction = transactions.First();
                        _balanceService.InsertBalance(new Models.Balance(0, account.ID, lastTranasction.CurrentAmount, lastTranasction.CreatedAt));
                    }
                    App.DispatcherQueue.TryEnqueue(() => _transactionService.UpdateTransactions());
                }
                else
                {
                    Console.Write("Unsupported bank provider");
                }
            }
            App.DispatcherQueue.TryEnqueue(() => _transactionUpdateService.SetUpdateTime());
        }

        private void ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IBankProviderService, BankProviderService>();
            services.AddSingleton<ITransactionService, TransactionService>();
            services.AddSingleton<ITransactionUpdateService, TransactionUpdateService>();
            services.AddSingleton<IBalanceService, BalanceService>();
            ServiceProvider = services.BuildServiceProvider();
        }
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
            DispatcherQueue = m_window.DispatcherQueue;
        }

        private Window m_window;

        public ServiceProvider ServiceProvider { get; internal set; }
    }
}
