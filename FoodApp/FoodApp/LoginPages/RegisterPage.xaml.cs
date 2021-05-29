using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        async void RegisterAccount_Clicked(object sender, System.EventArgs e)
        {
            if(txtLogin.Text == null || txtPassword.Text == null || txtUserName.Text == null)
            {
                lblError.Text = "Podaj email, hasło i nazwę użytkownika!";

            }
            else if (txtLogin.Text.Length <5)
            {
                lblError.Text = "Email musi zawierać co najmniej 5 znaków";
            }
            else if (txtPassword.Text.Length <2 || txtPassword.Text.Length > 9)
            {
                lblError.Text = "Hasło musi zawierać pomiędzy 5 a 9 znaków";
            }
            else if (txtUserName.Text.Length <4)
            {
                lblError.Text = "Nazwa użytkowika musi zawierać co najmniej 4 znaki";

            }
            else
            {
                var client = new HttpClient();
                string password = Base64Encode(txtPassword.Text);
                string uri = ((App)Parent).restApiUrl + "account/CreateNewUser?login=" + txtLogin.Text + "&password=" + password + "&userName=" + txtUserName.Text;
                var result = await client.GetStringAsync(uri);
                if (result !="Done")
                {
                    lblError.Text = result;
                }
                else
                {
                    ((App)Parent).MainPage = new MainPage(((App)Parent));
                }
            }
            
        }
        void GoBackToMain(object sender, System.EventArgs e)
        {
            ((App)Parent).MainPage = new MainPage(((App)Parent));
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        
    }
}