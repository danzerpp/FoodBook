using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FoodApi.Database
{
    public class AccountConnection
    {
       
        
        
        public string CreateAccount(SqlConnection sqlConnection, string login,string password, string userName)
        {
            string queryToCheck = "Select UserName from UserLogin Where Login ='" +login.ToLower()+"'" ;
            using (SqlCommand cmd = new SqlCommand(queryToCheck, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        return "Podany email już istnieje!";
                    }
                }
            }

            string query = "INSERT INTO UserLogin(Login,Password,UserName,IsActivated,NumberToActivate) VALUES(@login,@password,@username,@isActive,@activeNumber)";

            string activateNumber = "";
            int count = 0;
           

            try
            {
                while (count < 6)
                {
                    Random rand = new Random();
                    activateNumber += rand.Next(0, 10);
                    count++;
                }

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("dominik199811@gmail.com", "kinimod11"),
                    EnableSsl = true
                };
                client.Send("dominik199811@gmail.com", login, "FoodBook - Kod do aktywacji", "Kod aktywacyjny: " + activateNumber);
            }
            catch (Exception)
            {

                return "Podany email nie istnieje, proszę podać prawidłowy";
            }
            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.Add("@login", SqlDbType.VarChar, 50).Value = login.ToLower();
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 50).Value = password;
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = userName;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@activeNumber", SqlDbType.Int).Value = int.Parse(activateNumber);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }

            
            return "Done";
        }

        internal string ActivateUser(SqlConnection sqlConnection, string login, int code)
        {
            int number;
            string queryToCheck = "Select NumberToActivate from UserLogin Where Login ='" + login.ToLower() +"'";
            using (SqlCommand cmd = new SqlCommand(queryToCheck, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    number = Convert.ToInt32(reader["NumberToActivate"]);
                    
                }
            }

            if (number == code)
            {
                string queryToUpdate = "Update UserLogin Set IsActivated = 1 where Login ='" + login.ToLower() + "'";
                using (SqlCommand cmd = new SqlCommand(queryToUpdate, sqlConnection))
                {
                    cmd.ExecuteNonQuery();
                }
                return "Done";
            }
            else
            {
                return "Podano błędny kod";
            }


        }

        public string GetUserData(SqlConnection sqlConnection, string login, string password)
        {
            string queryToCheck = "Select Oid,UserName,IsActivated from UserLogin Where Login ='" + login.ToLower() + "' AND Password='"+password+"'";
            using (SqlCommand cmd = new SqlCommand(queryToCheck, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        return "Błędny login lub hasło";
                    }
                    else
                    {
                        reader.Read();
                        if (reader["IsActivated"].ToString() == "True")
                        {
                            return  "True;"+ reader["Oid"].ToString() + ";"+reader["UserName"].ToString();
                        }
                        else
                        {
                            return "False;"+ reader["Oid"].ToString() + ";" + reader["UserName"].ToString();
                        }
                    }
                }
            }
        }

        public void SendNumberToActivateAgain(SqlConnection sqlConnection, string login)
        {
            string queryToCheck = "Select NumberToActivate from UserLogin Where Login ='" + login.ToLower() + "'";
            using (SqlCommand cmd = new SqlCommand(queryToCheck, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var client = new SmtpClient("smtp.gmail.com", 587)
                        {
                            Credentials = new NetworkCredential("dominik199811@gmail.com", "kinimod11"),
                            EnableSsl = true
                        };
                        client.Send("dominik199811@gmail.com", login, "FoodBook - Kod do aktywacji", "Kod aktywacyjny: " + reader["NumberToActivate"]);
                    }
                }
            }
        }


        public bool SetNewPassword(SqlConnection sqlConnection,string login, string password, string oldPassword)
        {
            string queryToCheck = "Select * From UserLogin where Login = '" + login +"' AND password = '" + oldPassword + "'";
            using (SqlCommand cmd = new SqlCommand(queryToCheck, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        return false;
                    }
                  
                }
            }


            string queryToUpdate = "Update UserLogin Set Password='" +  password + "' where Login ='" + login + "'";
            SqlCommand command = new SqlCommand(queryToUpdate, sqlConnection);
            command.ExecuteNonQuery();
            return true;
        }
        public string ResetPassword(SqlConnection sqlConnection, string login)
        {
            string queryToCheck = "Select NumberToActivate from UserLogin Where Login ='" + login.ToLower() + "'";
            using (SqlCommand cmd = new SqlCommand(queryToCheck, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        return "Konto z podanym emailem nie istnieje!";
                    }
                }
            }

            Random rand = new Random();
            int length = rand.Next(5, 10);
            string password = CreatePassword(length);

            string queryToUpdate = "Update UserLogin Set Password='" + password + "' where Login ='" + login + "'";
            SqlCommand command = new SqlCommand(queryToUpdate, sqlConnection);
            command.ExecuteNonQuery();

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("dominik199811@gmail.com", "kinimod11"),
                EnableSsl = true
            };
            client.Send("dominik199811@gmail.com", login, "FoodBook - Reset hasła", "Nowe hasło: " + password);
            return "Done";
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
