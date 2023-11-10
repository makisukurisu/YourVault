using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using YourVault.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Controls
{
    public sealed partial class BankProviderItem : UserControl
    {
        public static readonly DependencyProperty BankProviderProperty =
        DependencyProperty.Register("bankProvider", typeof(ViewModels.BankProvider), typeof(BankProviderItem), new PropertyMetadata(null, OnBankProviderChanged));

        public ViewModels.BankProvider bankProvider
        {
            get { return (ViewModels.BankProvider)GetValue(BankProviderProperty); }
            set { SetValue(BankProviderProperty, value); }
        }

        public BankProviderItem()
        {
            this.InitializeComponent();
        }

        private static void OnBankProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BankProviderItem;
            if(control.bankProvider is not null) { 
                control.IDTextBlock.Text = control.bankProvider.ID.ToString();
                control.NameTextBlock.Text = control.bankProvider.Name;
                control.WebsiteTextBlock.Text = control.bankProvider.Website;
            }
            else
            {
                Console.WriteLine("BankProvider is null");
            }
        }
    }
}
