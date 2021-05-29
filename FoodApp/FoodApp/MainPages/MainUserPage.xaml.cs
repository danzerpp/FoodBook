using FoodApp.LoginPages;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainUserPage : ContentPage
    {
        App _app;
        Recipe _recipe;
        DateTime date = DateTime.Now;
        public MainUserPage(App app)
        {
           
            InitializeComponent();
            _app = app;
            txtUser.Text = "Witaj " + app.userName + "!";
            txtDate.Text = date.ToString("dd.MM.yy");
        }
        

            async void DoNote(Object Sender, EventArgs args)
        {
            var result = await DisplayAlert("Oceń potrawę :)", "Czy danie, które wylosowałeś na ten dzień ci smakowało ?", "Tak, daje plusa!", "Niestety nie");
           
            var client = new HttpClient();
            string uri = _app.restApiUrl + "user/DoNote?recipeOid=" + _recipe.Oid + "&userOid="+ _app.userOid +"&isLiked="+result;
            await client.GetStringAsync(uri);
            if (result)
            {
                txtLike.Text = (int.Parse(txtLike.Text) + 1).ToString();
            }
            else
            {
                txtUnLike.Text = (int.Parse(txtUnLike.Text) -1).ToString();
            }
            noteImage.IsVisible = false;
        }
        async void DoLottery(Object Sender, EventArgs args)
        {
            if (_recipe.Oid > 0) 
            {
               var result = await DisplayAlert("Uwaga!", "Danie na ten dzień jest już wylosowane, zamienić je na nowe ?", "Tak", "Nie");
                if (result)
                {
                    var client = new HttpClient();
                    string uri = _app.restApiUrl + "user/DoLottery?userOid=" + _app.userOid + "&date=" + date.ToShortDateString();
                    int oid = int.Parse(await client.GetStringAsync(uri));
                    PageAppears(null, null);
                }
            }
            else
            {
                var client = new HttpClient();
                string uri = _app.restApiUrl + "user/DoLottery?userOid=" + _app.userOid + "&date=" + date.ToShortDateString();
                
                string tempOid = await client.GetStringAsync(uri);
                int oid = int.Parse(tempOid);
                PageAppears(null, null);
            }
        }
       
        async void GoLeft(Object Sender, EventArgs args)
        {
            date = date.AddDays(-1);
            txtDate.Text = date.ToString("dd.MM.yy");
            PageAppears(null, null);
        }

        async void GoRight(Object Sender, EventArgs args)
        {
            date = date.AddDays(1);
            txtDate.Text = date.ToString("dd.MM.yy");
            PageAppears(null, null);
        }
        async void PageAppears(Object Sender, EventArgs args)
        {
            var client = new HttpClient();
            string uri = _app.restApiUrl + "user/CheckRecipeSelected?date=" + date.ToShortDateString() + "&userOid="+_app.userOid;
            int result = int.Parse(await client.GetStringAsync(uri));
            GetRecipe(result);
        }

        async void GetRecipe(int oid)
        {
            
            var client = new HttpClient();
            string uri = _app.restApiUrl + "user/GetRecipe?recipeOid=" + oid;
            var result = await client.GetStringAsync(uri);
            Recipe recipe = JsonSerializer.Deserialize<Recipe>(result);
            _recipe = recipe;
            if (oid == 0)
            {
                recipeImage.Source = null;
                noteImage.IsVisible = false;
            }
            else
            {
                uri = _app.restApiUrl + "user/HasRecipeNote?recipeOid=" + oid + "&userOid=" + _app.userOid;
                result = await client.GetStringAsync(uri);
                if (Convert.ToBoolean(result))
                {
                    noteImage.IsVisible = false;
                }
                else
                {
                    noteImage.IsVisible = true;
                }
                var imageArray = Convert.FromBase64String(recipe.ImageBase64);
                var stream = new MemoryStream(imageArray);
                recipeImage.Source = ImageSource.FromStream(() => stream);
                recipeImage.HeightRequest = 170;
                recipeImage.WidthRequest = 170;
            }
            txtRecipeName.Text = recipe.Name;
            txtUserName.Text = recipe.UserName;
            txtDescription.Text = recipe.Description;
            txtLike.Text = recipe.Likes.ToString();
            txtUnLike.Text = recipe.UnLikes.ToString();
            ObservableCollection<Ingredient> _ingredientList = new ObservableCollection<Ingredient>();
            foreach (var item in recipe.Ingredients)
            {
                _ingredientList.Add(item);
            }

            ingredients.ItemsSource = _ingredientList;
        }

        async public void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            string text = ((ToolbarItem)sender).Text;
            if (text == "Wyloguj się")
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("Cookies",
                    CreationCollisionOption.OpenIfExists);

                IFile file = await rootFolder.CreateFileAsync("data.txt",
                     CreationCollisionOption.ReplaceExisting);
                await file.WriteAllTextAsync("");
                ((App)((NavigationPage)Parent).Parent).login = "";
                ((App)((NavigationPage)Parent).Parent).userName = "";
                ((App)((NavigationPage)Parent).Parent).userOid = 0;
                ((App)((NavigationPage)Parent).Parent).MainPage = new MainPage(((App)((NavigationPage)Parent).Parent));
              
            }
            else if (text =="add")
            {
                await Navigation.PushAsync(new AddRecipePage());
            }
            else if (text == "edit")
            {
                await Navigation.PushAsync(new RecipesToEditPage(((App)((NavigationPage)Parent).Parent)));
            }
            else if (text == "recipes")
            {
                await Navigation.PushAsync(new TopRecipesPage(((App)((NavigationPage)Parent).Parent)));

            }
            else if (text == "Zmień hasło")
            {
                await Navigation.PushAsync(new ChangePasswordPage(((App)((NavigationPage)Parent).Parent)));

            }
        }
    }
}

    
