using FoodApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipesToEditPage : ContentPage
    {
        ObservableCollection<RecipeToEdit> _recipesToEdit = new ObservableCollection<RecipeToEdit>();
        App _app;
        public RecipesToEditPage(App app)
        {
            _app = app;
            InitializeComponent();
        }
        void PageAppears(Object Sender, EventArgs args)
        {
            GetRecipes();
        }

        async public void Edit(Object Sender, EventArgs args)
        {
            var SenderButton = (ImageButton)Sender;
            var className = SenderButton.ClassId;


            var client = new HttpClient();

            string uri = _app.restApiUrl + "user/GetRecipe?recipeOid=" + className;
            var result = await client.GetStringAsync(uri);
            Recipe recipe = JsonSerializer.Deserialize<Recipe>(result);

            await Navigation.PushAsync(new EditRecipePage(((App)((NavigationPage)Parent).Parent), recipe));

        }
        async public void Delete(Object Sender, EventArgs args)
        {
            List<RecipeToEdit> recipes = new List<RecipeToEdit>();

            foreach (var item in _recipesToEdit)
            {
                recipes.Add(item);
            }
            var wantUserDelete = await DisplayAlert("Usuń przepis", "Usunąć wybrany przepis?", "Tak", "Nie");

            if (wantUserDelete)
            {
                var SenderButton = (ImageButton)Sender;
                var className = SenderButton.ClassId;
                var client = new HttpClient();


                string uri = _app.restApiUrl + "user/DeleteRecipe?recipeOid=" + className;
                var result = await client.DeleteAsync(uri);


                var recipe = recipes.Where(i => i.RecipeOid.ToString() == className).First();
                recipes.Remove(recipe);
            }
            recipesList.ItemsSource = recipes;
            _recipesToEdit.Clear();
            foreach (var item in recipes)
            {
                _recipesToEdit.Add(item);
            }     
        }

        async void GetRecipes()
        {
            _recipesToEdit = new ObservableCollection<RecipeToEdit>();
            var client = new HttpClient();
            string uri = _app.restApiUrl + "user/GetUserRecipes?userOid=" + _app.userOid;
            var result = await client.GetStringAsync(uri);

            List<Recipe> recipes = JsonSerializer.Deserialize<List<Recipe>>(result);
            foreach (var recipe in recipes)
            {
                _recipesToEdit.Add(new RecipeToEdit()
                {
                    Name = recipe.Name,
                    RecipeOid = recipe.Oid,
                    Image = Xamarin.Forms.ImageSource.FromStream(
                () => new MemoryStream(Convert.FromBase64String(recipe.ImageBase64)))
                });
            }
            recipesList.ItemsSource = _recipesToEdit;

        }
    }
}