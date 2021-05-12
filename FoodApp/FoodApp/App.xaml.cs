using FoodApp.LoginPages;
using FoodApp.MainPages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp
{
    public partial class App : Application
    {
        public string restApiUrl = "http://192.168.1.100:45455/";
        public string userName = "";
        public int userOid = 8;
        public string login = "krakers13@o2.pl";
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
           MainPage = new NavigationPage(new MainUserPage());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
