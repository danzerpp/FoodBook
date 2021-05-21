using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodApp.MainPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditRecipePage : ContentPage
    {
        ObservableCollection<Ingredient> _ingredientList = new ObservableCollection<Ingredient>();
        List<Ingredient> _removedIngredients = new List<Ingredient>();
        Recipe _recipe;
        App _app;
        public EditRecipePage(App app,Recipe recipe)
        {
            InitializeComponent();
            _recipe = recipe;
            _app = app;
            RefreshPageValues();
        }

        void RefreshPageValues()
        {
            txtName.Text = _recipe.Name;
            txtDescription.Text = _recipe.Description;
            foreach (var item in _recipe.Ingredients)
            {
                _ingredientList.Add(item);
            }
            ingredients.ItemsSource = _ingredientList;
        }

        async void EditImage(object sender, EventArgs e)
        {
            if (_ingredientList.Count > 0 && !string.IsNullOrEmpty(txtName.Text) && !string.IsNullOrEmpty(txtDescription.Text))
            {
                var ingredients = new List<Ingredient>();
                ingredients.AddRange(_ingredientList.ToList());
                foreach (var item in ingredients)
                {
                    if (item.Oid >0)
                    {
                        item.Oid = 0;
                    }
                }   
                ingredients.AddRange(_removedIngredients.ToList());
                Recipe recipe = new Recipe()
                {
                    Oid = _recipe.Oid,
                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    Ingredients = ingredients,
                    ImageBase64 = _recipe.ImageBase64
                };
               await Navigation.PushAsync(new EditImagePage(_app, recipe));
            }


        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (_ingredientList.Where(i => i.Name == txtProduct.Text).FirstOrDefault() != null)
            {
                txtProduct.Text = String.Empty;
                txtProduct.Placeholder = "Składnik juz istnieje!";
            }
            else if (string.IsNullOrEmpty(txtProduct.Text))
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
                    Oid = -1,
                    Name = txtProduct.Text,
                    Quantity = txtQuantity.Text,
                    Unit = txtUnit.Text
                }); ;
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
                _removedIngredients.Add(ingredient);
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
            if (e.NewTextValue.Length > 6)
            {
                ((Entry)sender).Text = e.OldTextValue;

            }
        }
    }
}