using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApp.Models
{
    public class TopRecipe
    {
        public int Place { get; set; }
        public int Oid { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public int Note { get; set; }
        public string ImageBase64 { get; set; }
    }
}
