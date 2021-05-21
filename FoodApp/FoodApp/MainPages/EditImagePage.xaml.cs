using Plugin.Media;
using System;
using System.Collections.Generic;
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
    public partial class EditImagePage : ContentPage
    {
        Recipe _recipe;
        App _app;
        byte[] imageArray;
        public EditImagePage(App app, Recipe recipe)
        {
            _recipe = recipe;
            _app = app;
            InitializeComponent();
            imageArray = Convert.FromBase64String(_recipe.ImageBase64);
            var stream = new MemoryStream(imageArray);
            selectedImage.Source = ImageSource.FromStream(() => stream);

        }

        async void UploadImage(object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();

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
                string jsonString = JsonSerializer.Serialize(_recipe);
              
                var data = new StringContent(jsonString, Encoding.UTF8, "application/json");

                string uri = _app.restApiUrl + "user/EditRecipe";
                var client = new HttpClient();
                var response = await client.PostAsync(uri, data);
                var result = response.Content.ReadAsStringAsync().Result;

                Navigation.RemovePage(Navigation.NavigationStack[2]);
                await Navigation.PopAsync();

                

            }
            else
            {
                await DisplayAlert("Uwaga", "Przepis musi zawierać zdjęcie!", "OK");
            }

        }
    }
}