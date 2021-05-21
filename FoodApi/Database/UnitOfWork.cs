using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace FoodApi.Database
{
    
    public class UnitOfWork
    {
        private string _connectionString = @"data source=DESKTOP-DCN4A6S\SQLEXPRESS;integrated security=SSPI;initial catalog=FoodApiDomain;";
        SqlConnection sqlConnection;
        public UnitOfWork()
        {
            sqlConnection = new SqlConnection(_connectionString);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                string s = "d";
            }
        }

        public void AddRecipeToDatabase(Recipe recipe)
        {
            UserConnection userConnection = new UserConnection();
            userConnection.AddRecipe(sqlConnection, recipe);
        }

        public List<Recipe> GetUserRecipes(int userOid)
        {
            UserConnection userConnection = new UserConnection();
            return userConnection.GetUserRecipes(sqlConnection, userOid);
        }

        public string GetRecipe(int recipeOid)
        {
            UserConnection userConnection = new UserConnection();
            return userConnection.GetRecipeData(sqlConnection, recipeOid);
        }

        internal void DeleteRecipe(int recipeOid)
        {
            UserConnection userConnection = new UserConnection();
            userConnection.DeleteRecipe(sqlConnection, recipeOid);
        }

        public bool SetNewPassword(string login,string password, string oldPassword)
        {
            AccountConnection accountConnection = new AccountConnection();
            return accountConnection.SetNewPassword(sqlConnection, login, password, oldPassword);
        }

        internal int CheckForRecipe(string date,int userOid)
        {
            UserConnection userConnection = new UserConnection();
            return  userConnection.RecipeChecked(sqlConnection, date, userOid);
        }

        internal bool CheckNote(int recipeOid, int userOid)
        {
            UserConnection userConnection = new UserConnection();
            return userConnection.CheckForRecipeNote(sqlConnection, recipeOid, userOid);
        }

        internal void NewNote(int recipeOid, int userOid, bool isLiked)
        {
            UserConnection userConnection = new UserConnection();
            userConnection.NewNote(sqlConnection, recipeOid, userOid, isLiked);
        }

        internal List<Recipe> GetTopRecipes()
        {
            UserConnection userConnection = new UserConnection();
            return userConnection.GetTopRecipes(sqlConnection);
        }

        internal int DoLottery(int userOid, string date)
        {
            UserConnection userConnection = new UserConnection();
            return userConnection.Lottery(sqlConnection, userOid,date);
        }

        internal void EditRecipe(Recipe recipe)
        {
            UserConnection userConnection = new UserConnection();
            userConnection.EditRecipe(sqlConnection, recipe);
        }

        public string ResetPassword(string login)
        {
            AccountConnection accountConnection = new AccountConnection();
            return accountConnection.ResetPassword(sqlConnection, login);
        }

        public string ActivateUser(string login, int code)
        {
            AccountConnection accountConnection = new AccountConnection();
            return accountConnection.ActivateUser(sqlConnection, login, code);
        }

        public string LoginUser(string login, string password)
        {
            AccountConnection accountConnection = new AccountConnection();
            return accountConnection.GetUserData(sqlConnection, login, password);
        }

        public void SendCodeAgain(string login)
        {
            AccountConnection accountConnection = new AccountConnection();
            accountConnection.SendNumberToActivateAgain(sqlConnection, login);
        }

        public string AddUser(string login , string password, string userName)
        {
            AccountConnection accountConnection = new AccountConnection();
            return accountConnection.CreateAccount(sqlConnection, login, password, userName);
            //accountConnection.SetNewPassword(sqlConnection, "dominik199811@gmail.com","dodzi");
        }

        public void CreateTables()
        {
            string query = "If not exists (select name from sysobjects where name = 'UserLogin') CREATE TABLE UserLogin(Oid int IDENTITY (1, 1) PRIMARY KEY,Login varchar(50),Password varchar(50),UserName varchar(50),IsActivated bit, NumberToActivate int)";

            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();

            query = "If not exists (select name from sysobjects where name = 'Recipe') CREATE TABLE Recipe(Oid int IDENTITY (1, 1) PRIMARY KEY,Name varchar(50),Description varchar(300),UserOid int FOREIGN KEY REFERENCES UserLogin(Oid),RecipeImage Image)";

            command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();

            query = "If not exists (select name from sysobjects where name = 'Ingredient') CREATE TABLE Ingredient(Oid int IDENTITY (1, 1) PRIMARY KEY,Name varchar(50),Quantity int,Unit varchar(50),RecipeOid int FOREIGN KEY REFERENCES Recipe(Oid))";

            command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();

            query = "If not exists (select name from sysobjects where name = 'DayRecipe') CREATE TABLE DayRecipe(Oid int IDENTITY (1, 1) PRIMARY KEY,ChooseDate date,RecipeOid int FOREIGN KEY REFERENCES Recipe(Oid),UserOid int FOREIGN KEY REFERENCES UserLogin(Oid))";

            command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();

            query = "If not exists (select name from sysobjects where name = 'Opinion') CREATE TABLE Opinion(Oid int IDENTITY (1, 1) PRIMARY KEY,IsLiked bit,RecipeOid int FOREIGN KEY REFERENCES Recipe(Oid),UserOid int FOREIGN KEY REFERENCES UserLogin(Oid))";

            command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();
        }
    }
}
