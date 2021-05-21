using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApp.Models
{
    public class TopRecipe
    {
        public int Oid { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public int Likes { get; set; }
        public int UnLikes { get; set; }
    }
}
