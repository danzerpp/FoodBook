using FoodApi.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("AddNewRecipe")]
        public string AddRecipe(Recipe recipe)
        {

            UnitOfWork _uow = new UnitOfWork();
            _uow.AddRecipeToDatabase(recipe);
            return "Done";
        }
    }
}
