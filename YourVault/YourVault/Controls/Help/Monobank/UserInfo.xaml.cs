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
using Microsoft.UI.Xaml.Documents;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Controls.Help.Monobank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserInfo : Page
    {
        public UserInfo()
        {
            this.InitializeComponent();
        }

        public UserInfo(string clientID, string clientName, string clientPermissions, string webhookURL)
        {
            this.InitializeComponent();
            ClientID.Blocks.Add(new Paragraph { Inlines = { new Run { Text = clientID } } });
            ClientName.Blocks.Add(new Paragraph { Inlines = { new Run { Text = clientName } } });
            ClientPermissions.Blocks[1] = new Paragraph { Inlines = { new Run { Text = clientPermissions } } };
            WebhookURL.Blocks.Add(new Paragraph { Inlines = { new Run { Text = webhookURL } } });
        }
    }
}
