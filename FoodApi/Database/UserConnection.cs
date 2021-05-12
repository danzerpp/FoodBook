using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodApi.Database
{
    public class UserConnection
    {
        public void AddRecipe(SqlConnection sqlConnection, Recipe recipe)
        {
            int recipeOid;
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Recipe(Name,Description,UserOid,RecipeImage) output INSERTED.Oid VALUES(@name,@description,@userOid,@image)", sqlConnection))
            {
                cmd.Parameters.AddWithValue("@name", recipe.Name);
                cmd.Parameters.AddWithValue("@description", recipe.Description);
                cmd.Parameters.AddWithValue("@userOid", recipe.UserOid);
                if (!string.IsNullOrEmpty(recipe.ImageBase64))
                {
                    cmd.Parameters.AddWithValue("@image", Convert.FromBase64String(recipe.ImageBase64));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@image", new byte[1]);
                }

                recipeOid = (int)cmd.ExecuteScalar();
            }

            foreach (var ingredient in recipe.Ingredients)
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Ingredient(Name,Quantity,Unit,RecipeOid) VALUES(@name,@quantity,@unit,@recipeOid)", sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@name", ingredient.Name);
                    cmd.Parameters.AddWithValue("@quantity", ingredient.Quantity);
                    cmd.Parameters.AddWithValue("@unit", ingredient.Unit);
                    cmd.Parameters.AddWithValue("@recipeOid", recipeOid);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string GetRecipeData(SqlConnection sqlConnection, int recipeOid)
        {
            int userOid = 0;
            string query = "SELECT * FROM Recipe where Oid = "+ recipeOid;
            Recipe recipe = new Recipe();
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        recipe.Name = reader["Name"].ToString();
                        recipe.Description = reader["Description"].ToString();
                        userOid = int.Parse(reader["UserOid"].ToString());
                        recipe.ImageBase64 = Convert.ToBase64String((byte[])reader["RecipeImage"]);
                    }
                }
            }
            query = "SELECT * FROM Ingredient where RecipeOid = " + recipeOid;

            List<Ingredient> ingredients = new List<Ingredient>();
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ingredients.Add(new Ingredient()
                        {
                            Name = reader["Name"].ToString(),
                            Unit = reader["Unit"].ToString(),
                            Quantity = reader["Quantity"].ToString()
                        });
                    }
                }
            }
            recipe.Ingredients = ingredients;

            query = "SELECT * FROM UserLogin where Oid = " + userOid;

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        recipe.UserName = reader["UserName"].ToString();
                    }
                }
            }
            string jsonString = JsonSerializer.Serialize(recipe);

            return jsonString;
        }
    }
}
