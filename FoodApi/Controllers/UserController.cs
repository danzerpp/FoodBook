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

        [HttpGet]
        [Route("GetUserRecipes")]
        public string GetRecipes(int userOid)
        {

            UnitOfWork _uow = new UnitOfWork();
            List<Recipe> recipes = _uow.GetUserRecipes(userOid);
            string jsonString = JsonSerializer.Serialize(recipes);
            return jsonString;
        }

        [HttpDelete]
        [Route("DeleteRecipe")]
        public void DeleteRecipe(int recipeOid)
        {
            UnitOfWork _uow = new UnitOfWork();
            _uow.DeleteRecipe(recipeOid);
        }

        [HttpGet]
        [Route("GetRecipe")]
        public string GetRecipe(int recipeOid)
        {
            UnitOfWork _uow = new UnitOfWork();
            return _uow.GetRecipe(recipeOid);
        }

        [HttpPost]
        [Route("EditRecipe")]
        public void EditRecipe(Recipe recipe)
        {
            UnitOfWork _uow = new UnitOfWork();
            _uow.EditRecipe(recipe);
        }

        [HttpGet] 
        [Route("CheckRecipeSelected")]
        public int CheckForRecipe(string date,int userOid)
        {
            UnitOfWork _uow = new UnitOfWork();
            return _uow.CheckForRecipe(date, userOid);
        }

        [HttpGet]
        [Route("HasRecipeNote")]
        public bool CheckNote(int recipeOid, int userOid)
        {
            UnitOfWork _uow = new UnitOfWork();
            return _uow.CheckNote(recipeOid, userOid);
        }

        [HttpGet]
        [Route("DoNote")]
        public void NewNote(int recipeOid, int userOid, bool isLiked)
        {
            UnitOfWork _uow = new UnitOfWork();
            _uow.NewNote(recipeOid, userOid, isLiked);
        }

        [HttpGet]
        [Route("DoLottery")]
        public int Lottery(int userOid, string date)
        {
            UnitOfWork _uow = new UnitOfWork();
           return _uow.DoLottery( userOid, date);
        }

        [HttpGet]
        [Route("GetTopRecipes")]
        public string  GetBestRecipes()
        {
            UnitOfWork _uow = new UnitOfWork();
            return JsonSerializer.Serialize(_uow.GetTopRecipes()) ;
        }
    }
}
