using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowRecipePage : ContentPage
    {
        public int _recipeOid;
        public App _app;
        Recipe _recipe;
        public ShowRecipePage(int recipeOid, App app)
        {
            _app = app;
            _recipeOid = recipeOid;
            InitializeComponent();
        }

       async void  Appears(Object Sender, EventArgs args)
        {
            GetRecipe(_recipeOid);
        }

        async void GetRecipe(int oid)
        {

            var client = new System.Net.Http.HttpClient();
            string uri = _app.restApiUrl + "user/GetRecipe?recipeOid=" + oid;
            var result = await client.GetStringAsync(uri);
            Recipe recipe = JsonSerializer.Deserialize<Recipe>(result);
            _recipe = recipe;
            if (oid == 0)
            {
                recipeImage.Source = null;
            }
            else
            {
                uri = _app.restApiUrl + "user/HasRecipeNote?recipeOid=" + oid + "&userOid=" + _app.userOid;
                result = await client.GetStringAsync(uri);
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
    }
}