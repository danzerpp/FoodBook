using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodApp.Models
{
    public class RecipeToEdit
    {
        public int RecipeOid { get; set; }
        public int Place { get; set; }
        public string Name { get; set; }
        public ImageSource Image { get; set; }
        public string BackColor { get; set; }
    }
}
