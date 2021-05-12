using FoodApp.MainPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.LoginPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivateAccount : ContentPage
    {
        public ActivateAccount()
        {
            InitializeComponent();
        }
        void GoBackToMain(object sender, System.EventArgs e)
        {
            ((App)Parent).MainPage = new MainPage();
        }
        async void Activate_Clicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                lblError.Text = "Podaj kod aktywacyjny";
                lblError.TextColor = Color.Red;
            }
            else
            {
                var client = new HttpClient();
                string uri = ((App)Parent).restApiUrl + "account/ActivateAccount?login=" + ((App)Parent).userName + "&code=" + txtCode.Text;
                var result = await client.GetStringAsync(uri);
                if (result =="Done")
                {
                    
                    ((App)Parent).MainPage = new NavigationPage(new MainUserPage());
                }
                else
                {
                    lblError.Text = result;
                    lblError.TextColor = Color.Red;
                }
                
            }
            
        }

        async void SendAgain_Clicked(object sender, System.EventArgs e)
        {
            var client = new HttpClient();
            string uri = ((App)Parent).restApiUrl + "account/SendCodeAgain?login=" + ((App)Parent).userName;
            var result = await client.GetStringAsync(uri);
            lblError.Text = "Kod został wysłany ponownie na podany maila";
            lblError.TextColor = Color.Green;
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //lets the Entry be empty
            if (string.IsNullOrEmpty(e.NewTextValue)) return;

            if (!int.TryParse(e.NewTextValue, out int value))
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
            if (((Entry)sender).Text.Length > 6)
            {
                ((Entry)sender).Text = ((Entry)sender).Text.Substring(0, 6);
            }
        }
    }
}