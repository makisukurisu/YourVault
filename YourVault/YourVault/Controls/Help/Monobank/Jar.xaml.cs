using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using YourVault.External;
using Microsoft.UI.Xaml.Documents;
using YourVault.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Controls.Help.Monobank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Jar : Page
    {
        public Jar()
        {
            this.InitializeComponent();
        }

        public Jar(MonoBankJarsResponse jar)
        {
            this.InitializeComponent();

            ID.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = jar.ID } } });
            Title.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = jar.Title } } });

            Description.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = jar.Description } } });
            Currency.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = jar.Currency } } });
            Balance.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = jar.Balance.ToString() } } });

            string GoalText = jar.Goal.ToString();
            if (jar.Goal == -1.0f) {
                GoalText = "Не встановлено";
            }
            Goal.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = GoalText } } });
            SendID.Blocks.Add(new Paragraph { Inlines = { new Run() { Text = jar.SendID } } });
        }
    }
}
