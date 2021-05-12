﻿using System;
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
        public void GetRecipe(int recipeOid)
        {
            UserConnection userConnection = new UserConnection();
            userConnection.GetRecipeData(sqlConnection, recipeOid);
        }

        public void SetNewPassword(string login,string password)
        {
            AccountConnection accountConnection = new AccountConnection();
            accountConnection.SetNewPassword(sqlConnection, login, password);
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
            string query = "If not exists (select name from sysobjects where name = 'UserLogin') CREATE TABLE UserLogin(Oid int IDENTITY (1, 1) PRIMARY KEY,Login char(50),Password char(50),UserName char(50),IsActivated bit, NumberToActivate int)";

            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();

            query = "If not exists (select name from sysobjects where name = 'Recipe') CREATE TABLE Recipe(Oid int IDENTITY (1, 1) PRIMARY KEY,Name char(50),Description char(300),UserOid int FOREIGN KEY REFERENCES UserLogin(Oid),RecipeImage Image)";

            command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();

            query = "If not exists (select name from sysobjects where name = 'Ingredient') CREATE TABLE Ingredient(Oid int IDENTITY (1, 1) PRIMARY KEY,Name char(50),Quantity int,Unit char(50),RecipeOid int FOREIGN KEY REFERENCES Recipe(Oid))";

            command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();
        }
    }
}
