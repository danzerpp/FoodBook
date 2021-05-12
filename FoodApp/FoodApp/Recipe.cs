using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApp
{
    public class Recipe
    {
        public int UserOid { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public string ImageBase64 { get; set; }
    }
}
