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

        internal void EditRecipe(SqlConnection sqlConnection, Recipe recipe)
        {
            string query = "UPDATE Recipe SET Name = @name, Description = @description, RecipeImage = @image Where Oid = " + recipe.Oid;

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@name", recipe.Name);
                cmd.Parameters.AddWithValue("@description", recipe.Description);
                cmd.Parameters.AddWithValue("@image", Convert.FromBase64String(recipe.ImageBase64));
                cmd.ExecuteNonQuery();
            }

            foreach (var item in recipe.Ingredients.Where(i=>i.Oid >0))
            {
                query = "Delete FROM Ingredient Where Oid = " + item.Oid;
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@name", recipe.Name);
                    cmd.Parameters.AddWithValue("@description", recipe.Description);
                    cmd.Parameters.AddWithValue("@image", Convert.FromBase64String(recipe.ImageBase64));
                    cmd.ExecuteNonQuery();
                }
            }

            foreach (var ingredient in recipe.Ingredients.Where(i=>i.Oid <0))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Ingredient(Name,Quantity,Unit,RecipeOid) VALUES(@name,@quantity,@unit,@recipeOid)", sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@name", ingredient.Name);
                    cmd.Parameters.AddWithValue("@quantity", ingredient.Quantity);
                    cmd.Parameters.AddWithValue("@unit", ingredient.Unit);
                    cmd.Parameters.AddWithValue("@recipeOid", recipe.Oid);
                    cmd.ExecuteNonQuery();
                }
            }


        }

        internal List<Recipe> GetTopRecipes(SqlConnection sqlConnection)
        {
            List<Recipe> recipes = new List<Recipe>();

            string query = "Select TOP 100 * From Recipe";

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var z = "";
                    }

                }
            }

            return recipes;
        }

        internal int Lottery(SqlConnection sqlConnection, int userOid,string date)
        {

            var splitDate = date.Split('/');
            string query = "SELECT * FROM DayRecipe where UserOid = " + userOid + " AND ChooseDate = '" + (splitDate[2] + "-" + splitDate[0] + "-" + splitDate[1]) + "'";
            int recipeOid = 0;
            bool exist = false;
            List<int> ingredientsOids = new List<int>();
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        exist = true;
                        recipeOid = int.Parse(reader["RecipeOid"].ToString());
                    }

                }
            }
            int newOid = -1 ;
            List<int> oids = new List<int>();

            using (SqlCommand cmd = new SqlCommand("Select Oid from Recipe", sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        oids.Add(int.Parse(reader["Oid"].ToString()));
                    }

                }
            }
            
            while (newOid < 0 || newOid == recipeOid)
            {
                Random rand = new Random();
                newOid = oids[rand.Next(0, oids.Count)];
            }

            if (exist)
            {
                SqlCommand cmd = new SqlCommand("Update DayRecipe Set RecipeOid = " + newOid, sqlConnection);
                cmd.ExecuteNonQuery();
            }
            else
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO DayRecipe(RecipeOid,UserOid,ChooseDate) VALUES(@recipeOid,@userOid,@chooseDate)", sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@recipeOid", newOid);
                    cmd.Parameters.AddWithValue("@userOid", userOid);
                    cmd.Parameters.AddWithValue("@chooseDate", (splitDate[2] + "-" + splitDate[0] + "-" + splitDate[1]));
                    cmd.ExecuteNonQuery();
                }
            }

            return recipeOid;

        }

        internal void NewNote(SqlConnection sqlConnection, int recipeOid, int userOid, bool isLiked)
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO Opinion(RecipeOid,UserOid,IsLiked) VALUES(@recipeOid,@userOid,@isLiked)", sqlConnection))
            {
                cmd.Parameters.AddWithValue("@recipeOid", recipeOid);
                cmd.Parameters.AddWithValue("@userOid", userOid);
                cmd.Parameters.AddWithValue("@isLiked", isLiked);
                cmd.ExecuteNonQuery();
            }
        }

        internal bool CheckForRecipeNote(SqlConnection sqlConnection, int recipeOid, int userOid)
        {
            string query = "Select * from Opinion where RecipeOid = " + recipeOid +" AND UserOid =" + userOid;
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    else return false;

                }
            }
        }

        internal int RecipeChecked(SqlConnection sqlConnection, string date, int userOid)
        {
            var splitDate = date.Split('/');
            string query = "SELECT RecipeOid FROM DayRecipe where UserOid = " + userOid +" AND ChooseDate = '"+ (splitDate[2] + "-"+ splitDate[0]+ "-"+splitDate[1])+"'";

            List<int> ingredientsOids = new List<int>();
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return int.Parse(reader["RecipeOid"].ToString());
                    }
                    else return 0;
                    
                }
            }
        }

        internal void DeleteRecipe(SqlConnection sqlConnection, int recipeOid)
        {
            string query = "SELECT Oid FROM Ingredient where RecipeOid = " + recipeOid;

            List<int> ingredientsOids = new List<int>();  
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ingredientsOids.Add(int.Parse(reader["Oid"].ToString()));
                        
                    }
                }
            }

            foreach (int oid in ingredientsOids)
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Ingredient Where Oid =" + oid, sqlConnection))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            using (SqlCommand cmd = new SqlCommand("DELETE FROM Recipe Where Oid =" + recipeOid, sqlConnection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        

        public List<Recipe> GetUserRecipes(SqlConnection sqlConnection, int userOid)
        {
            string query = "SELECT * FROM Recipe where UserOid = " + userOid;

            List<Recipe> recipes = new List<Recipe>();

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Recipe recipe = new Recipe();
                        recipe.Name = reader["Name"].ToString();
                        recipe.Oid = int.Parse(reader["Oid"].ToString());
                        recipe.ImageBase64 = Convert.ToBase64String((byte[])reader["RecipeImage"]);

                            recipes.Add(recipe);
                    }
                }
            }
            return recipes;
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
                        recipe.Oid = int.Parse(reader["Oid"].ToString());
                        recipe.Name = reader["Name"].ToString();
                        recipe.Description = reader["Description"].ToString();
                        userOid = int.Parse(reader["UserOid"].ToString());
                        recipe.UserOid = userOid;
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
                            Oid = int.Parse(reader["Oid"].ToString()),
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

            query = "SELECT * FROM Opinion where RecipeOid = " + recipeOid;
            recipe.Likes = 0;
            recipe.UnLikes = 0;
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bool like = Convert.ToBoolean(reader["IsLiked"].ToString());
                        if (like)
                        {
                            recipe.Likes++;
                        }
                        else
                        {
                            recipe.UnLikes--;
                        }
                    }
                }
            }
            string jsonString = JsonSerializer.Serialize(recipe);

            return jsonString;
        }
    }
}
