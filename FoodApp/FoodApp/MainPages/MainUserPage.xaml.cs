using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainUserPage : ContentPage
    {
        public MainUserPage()
        {
            InitializeComponent();

        }

        public void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            string text = ((ToolbarItem)sender).Text;
            if (text == "Wyloguj się")
            {
                ((App)((NavigationPage)Parent).Parent).login = "";
                ((App)((NavigationPage)Parent).Parent).userName = "";
                ((App)((NavigationPage)Parent).Parent).MainPage = new MainPage();
              
            }
            else if (text =="add")
            {
                Navigation.PushAsync(new AddRecipePage());
            }
            else if (text == "edit")
            {
                Navigation.PushAsync(new RecipesToEditPage());
            }
            else if (text == "dinner")
            {
                //dinner
            }
            else if (text == "recipes")
            {
                //recipes
            }
        }
    }
}

    
