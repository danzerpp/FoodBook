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
    public partial class ResetPassword : ContentPage
    {
        public ResetPassword()
        {
            InitializeComponent();
        }
        void GoBackToMain(object sender, System.EventArgs e)
        {
            ((App)Parent).MainPage = new MainPage();
        }
        async void Reset_Clicked(object sender, System.EventArgs e)
        {
            if (txtEmail.Text ==null)
            {
                lblError.Text = "Podaj email!";

            }
            else
            {
                var client = new HttpClient();
                string uri = ((App)Parent).restApiUrl + "account/ResetUserPassword?login=" + txtEmail.Text;
                var result = await client.GetStringAsync(uri);
                if (result == "Done")
                {
                    ((App)Parent).MainPage = new MainPage();
                }
                else
                {
                    lblError.Text = result;
                }
            }
            

        }
    }
}