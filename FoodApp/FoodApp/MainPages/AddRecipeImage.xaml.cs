using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    public partial class AddRecipeImage : ContentPage
    {
        Recipe _recipe;
        byte[] imageArray;
        public AddRecipeImage(Recipe recipe)
        {
            _recipe = recipe;
            InitializeComponent();
        }

       async void UploadImage(object sender, System.EventArgs e)
        {
           await  CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Upload zdjęcia", "Aplikacja nie ma dostępu do galerii", "OK");
            }
            else
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                    CompressionQuality = 40
                });

                if (file != null)
                {
                    imageArray = System.IO.File.ReadAllBytes(file.Path);
                    selectedImage.Source = ImageSource.FromFile(file.Path);
                }
                
            }
        }

        async void DeleteImage(object sender, System.EventArgs e)
        {
            if (imageArray != null)
            {
                imageArray = null;
                selectedImage.Source = null;

            }
        }

        async void AddRecipe(object sender, System.EventArgs e)
        {
            if (imageArray != null)
            {
                _recipe.ImageBase64 = Convert.ToBase64String(imageArray);
            }
            else
            {
                _recipe.ImageBase64 = "";
            }
            string jsonString = JsonSerializer.Serialize(_recipe);
            var data = new StringContent(jsonString, Encoding.UTF8, "application/json");



            string uri = ((App)((NavigationPage)Parent).Parent).restApiUrl + "user/AddNewRecipe";
            var client = new HttpClient();
            var response = await client.PostAsync(uri, data);
            var result = response.Content.ReadAsStringAsync().Result;

            ((App)((NavigationPage)Parent).Parent).MainPage = new NavigationPage(new MainUserPage());
        }
    }
}