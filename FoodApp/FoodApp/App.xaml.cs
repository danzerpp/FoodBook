using FoodApp.LoginPages;
using FoodApp.MainPages;
using PCLStorage;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp
{
    public partial class App : Application
    {
        public string restApiUrl = "http://192.168.1.100:45457/";
        public string userName = "";
        public int userOid = 0;
        public string login = "";
        public App()
        {
            InitializeComponent();
            //Application.Current.Properties["id"] = 8;


            //if (loggedIn)
            //{
            //    MainPage = new NavigationPage(new MainUserPage(this));

            //}
            //else
            //{
                MainPage = new MainPage(this);
            //}

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
