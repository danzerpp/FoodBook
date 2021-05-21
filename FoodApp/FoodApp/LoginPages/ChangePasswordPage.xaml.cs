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
    public partial class ChangePasswordPage : ContentPage
    {
        App _app;
        public ChangePasswordPage(App app)
        {
            _app = app;
            InitializeComponent();
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtNewPass.Text) && !(txtNewPass.Text.Length < 2 || txtNewPass.Text.Length > 9) && txtNewPass.Text == txtPassAgain.Text && !string.IsNullOrEmpty(txtOldPass.Text)  )
            {
                string password = Base64Encode(txtNewPass.Text);
                var client = new HttpClient();
                string uri = _app.restApiUrl + "account/SetNewUserPassword?login=" + _app.login + "&password=" + password + "&oldPassword=" + Base64Encode(txtOldPass.Text)  ;
                var result = await client.GetStringAsync(uri);
                txtOldPass.Text = "";
                txtNewPass.Text = "";
                txtPassAgain.Text = "";
                await DisplayAlert("Powodzenie!", result, "OK");
            }
            else if(string.IsNullOrEmpty(txtOldPass.Text))
            {
                await DisplayAlert("Błąd!", "Proszę podać stare hasło!", "OK");
            }
            else
            {
                await DisplayAlert("Błąd!", "Hasła muszą być identyczne i zawierać od 2 do 9 znaków", "OK");

            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}