using FoodApp.LoginPages;
using FoodApp.MainPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoodApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
        }

         void Register_Clicked(object sender, System.EventArgs e)
        {
            ((App)Parent).MainPage = new RegisterPage();
        }
        
        void ResetPassword_Clicked(object sender, System.EventArgs e)
        {
            ((App)Parent).MainPage = new ResetPassword();
        }

        async void Login_Clicked(object sender, System.EventArgs e)
        {
            if (txtPassword.Text == null  || txtLogin.Text == null)
            {
                lblError.Text = "Podaj email i hasło";

            }
            else
            {
                var client = new HttpClient();
                string password = Base64Encode(txtPassword.Text);
                string uri = ((App)Parent).restApiUrl + "account/LoginToAccount?login=" + txtLogin.Text + "&password=" + password;
                var result = await client.GetStringAsync(uri);
                var split = result.Split(';');
                if (result.Contains("Błędny login"))
                {
                    lblError.Text = result;

                }
                else if (split[0] == "False")
                {
                    ((App)Parent).login = txtLogin.Text;
                    ((App)Parent).userName = split[2];
                    ((App)Parent).userOid = int.Parse(split[1]);
                    ((App)Parent).MainPage = new ActivateAccount();

                }
                else
                {
                    ((App)Parent).login = txtLogin.Text;
                    ((App)Parent).userName = split[2];
                    ((App)Parent).userOid = int.Parse(split[1]);
                    ((App)Parent).MainPage = new NavigationPage(new MainUserPage(((App)Parent)));

                }
            }
            
        }
        async Task<string> GetNewValue()
        {
            return "";
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
