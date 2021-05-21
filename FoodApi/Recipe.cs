using System;
using System.Collections.Generic;
using System.Text;

namespace FoodApi
{
    public class Recipe
    {
        public int Oid { get; set; }
        public int UserOid { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public string ImageBase64 { get; set; }
        public int Likes { get; set; }
        public int UnLikes { get; set; }
        public bool HasNote { get; set; }
    }
}
