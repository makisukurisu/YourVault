using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using YourVault.Controls.Help.Monobank;
using YourVault.External;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YourVault.Views.AccountView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpPage : Page
    {
        public HelpPage()
        {
            this.InitializeComponent();
        }

        private void GetMonobankData(object sender, RoutedEventArgs e)
        {

            string XToken = MonobankXTokenInput.Text.Trim();

            if (XToken == "")
            {
                MonobankXTokenTeachingTip.Subtitle = "Введіть X-Token!";
                MonobankXTokenTeachingTip.IsOpen = true;
                return;
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Token", XToken);
            client.BaseAddress = new Uri("https://api.monobank.ua/personal/client-info");
            HttpResponseMessage response = client.GetAsync("").Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var parsed_result = JsonConvert.DeserializeObject<MonoBankClientInfoResponse>(result);

                MonoClientInfoListView.Items.Clear();
                MonoClientAccountsListView.Items.Clear();
                MonoClientJarsListView.Items.Clear();

                MonoClientInfoListView.Items.Add(
                    new UserInfo(
                        parsed_result.ClientID,
                        parsed_result.Name,
                        parsed_result.Permissions,
                        parsed_result.WebHookURL
                    )
                );
                foreach(var account in parsed_result.Accounts)
                {
                    MonoClientAccountsListView.Items.Add(
                        new Account(account)
                    );
                }
                foreach (var jar in parsed_result.Jars)
                {
                    MonoClientJarsListView.Items.Add(new Jar(jar));
                }
            }
            else
            {
                string errorMessage = "Помилка. Первірте правильність X-Token-у, роботу сервісу, чи повідмоте про помилку розробникам додатку!";
                errorMessage += "\n\n";
                errorMessage += "Код помилки: " + response.StatusCode.ToString();
                errorMessage += "\n\n";
                errorMessage += "Повідомлення про помилку: " + response.Content.ReadAsStringAsync().Result;
                MonobankXTokenTeachingTip.Subtitle = errorMessage;
                MonobankXTokenTeachingTip.IsOpen = true;
            }
        }
    }
}
