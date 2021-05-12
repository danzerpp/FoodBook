
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.Json;
using System.Net.Http;

namespace FoodApp.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddRecipePage : ContentPage
    {

        ObservableCollection<Ingredient> _ingredientList = new ObservableCollection<Ingredient>();

        public AddRecipePage()
        {

            InitializeComponent();
          
            

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (_ingredientList.Where(i => i.Name == txtProduct.Text).FirstOrDefault() != null)
            {
                txtProduct.Text = String.Empty;
                txtProduct.Placeholder = "Składnik juz istnieje!";
            }
            else  if ( string.IsNullOrEmpty(txtProduct.Text))
            {
                txtProduct.Text = String.Empty;
                txtProduct.Placeholder = "Podaj nazwę!";
            }
            else if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                txtQuantity.Text = String.Empty;
                txtQuantity.Placeholder = "Podaj ilość!";
            }
            else if (string.IsNullOrEmpty(txtUnit.Text))
            {
                txtUnit.Text = String.Empty;
                txtUnit.Placeholder = "Podaj jednostkę!";
            }
            else if (txtUnit.Text != "Podaj jednostkę!" && txtQuantity.Text != "Podaj ilość!" && txtProduct.Text != "Podaj nazwę!")
            {
                _ingredientList.Add(new Ingredient()
                {
                    Name = txtProduct.Text,
                    Quantity = txtQuantity.Text,
                    Unit = txtUnit.Text
                });
                txtUnit.Text = String.Empty;
                txtQuantity.Text = String.Empty;
                txtProduct.Text = String.Empty;

                txtUnit.Placeholder = String.Empty;
                txtQuantity.Placeholder = String.Empty;
                txtProduct.Placeholder = String.Empty;
                ingredients.ItemsSource = _ingredientList;

            }
        }

        public void Delete(Object Sender, EventArgs args)
        {
            var SenderButton = (ImageButton)Sender;
            var className = SenderButton.ClassId;
            var ingredient = _ingredientList.Where(i => i.Name == className).First();
            _ingredientList.Remove(ingredient);
            ingredients.ItemsSource = _ingredientList;
        }
        
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //lets the Entry be empty
            if (string.IsNullOrEmpty(e.NewTextValue)) return;

            if (!int.TryParse(e.NewTextValue, out int value))
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
        }

        private void OnUnitTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >6)
            {
                ((Entry)sender).Text = e.OldTextValue;

            }
        }



        async private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (_ingredientList.Count > 0 && !string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtDescription.Text))
            {
                Recipe recipe = new Recipe()
                {
                    UserOid = ((App)((NavigationPage)Parent).Parent).userOid,
                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    Ingredients = _ingredientList.ToList()
                };
                await Navigation.PushAsync(new AddRecipeImage(recipe));
            }
        }
    }
}