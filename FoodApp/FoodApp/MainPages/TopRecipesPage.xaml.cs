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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopRecipesPage : ContentPage
    {
        private App _app;
        ObservableCollection<RecipeToEdit> _recipesToEdit = new ObservableCollection<RecipeToEdit>();
        public TopRecipesPage(App app)
        {
            _app = app;
            InitializeComponent();
        }

        async void PageAppears(Object Sender, EventArgs args)
        {
            _recipesToEdit = new ObservableCollection<RecipeToEdit>();
            var client = new HttpClient();
            string uri = _app.restApiUrl + "user/GetTopRecipes?";
            var result = await client.GetStringAsync(uri);
            int i = 1;
            List<TopRecipe> recipes = JsonSerializer.Deserialize<List<TopRecipe>>(result);
            foreach (var recipe in recipes)
            {
                _recipesToEdit.Add(new RecipeToEdit()
                {
                    Place = i,
                    Name = recipe.Name,
                    RecipeOid = recipe.Oid,
                    Image = Xamarin.Forms.ImageSource.FromStream(
                () => new MemoryStream(Convert.FromBase64String(recipe.ImageBase64)))
                });
                i++;
            }
            foreach (var item in _recipesToEdit)
            {
                if (item.Place ==1)
                {
                    item.BackColor = "Gold";
                }
                else if (item.Place == 2)
                {
                    item.BackColor = "Silver";
                }
                else if (item.Place == 3)
                {
                    item.BackColor = "Brown";
                }
                else
                {
                    item.BackColor = "Transparent";
                }
            }
            recipesList.ItemsSource = _recipesToEdit;
        }
        async void Show(Object Sender, EventArgs args)
        {
            var SenderButton = (ImageButton)Sender;
            var className = SenderButton.ClassId;
            await Navigation.PushAsync(new ShowRecipePage(int.Parse(className),((App)((NavigationPage)Parent).Parent)));
        }
    }
}