using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace DBSD.Services
{
    public class UserServices : CommandManager
    {
        protected override DbCommand GetCommand(string sql)
        {
            SqlConnection connect = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
            return new SqlCommand(sql, connect);
        }
        public string RandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        public string AddItem(UserProperties user)
        {
            if (Login(user.UserName, user.Password).Count == 0)
            {
                var fullSql = string.Format("Insert into Users VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", user.FirstName, user.LastName, user.Email, user.Phone, user.Token, user.UserName, user.Password);
                var command = GetCommand(fullSql);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
                return "Success";
            }
            return "Fail";
        }
        public List<string> Login(string UserName, string Password)
        {
            var fullSql = string.Format("Select FirstName FROM Users Where UserName ='{0}' and UserPassword='{1}'", UserName, Password);
            var command = GetCommand(fullSql);
            List<string> result = new List<string>();
            command.Connection.Open();
            DbDataReader reader = command.ExecuteReader();
            string FirstName = "";
            while (reader.Read())
            {
                FirstName = Convert.ToString(reader.GetValue(0));
                result.Add(FirstName);
            }
            return result;
        }
        public void WriteToken(UserProperties user)
        {
            var fullSql = string.Format("Update Users set Token='{0}' where UserName='{1}'", user.Token, user.UserName);
            var command = GetCommand(fullSql);
            command.Connection.Open();
            DbDataReader reader = command.ExecuteReader();
        }

        public List<string> CheckToken(string Token)
        {
            var fullSql = string.Format("Select UserName FROM Users Where Token='{0}'", Token);
            var command = GetCommand(fullSql);
            List<string> result = new List<string>();
            command.Connection.Open();
            DbDataReader reader = command.ExecuteReader();
            string username = "";
            while (reader.Read())
            {
                username = Convert.ToString(reader.GetValue(0));
                result.Add(username);
            }
            return result;
        }

        public List<int> GetUserIdByToken(string token)
        {
            var fullSql = string.Format("Select UserId FROM Users Where Token='{0}'", token);
            var command = GetCommand(fullSql);
            List<int> result = new List<int>();
            command.Connection.Open();
            DbDataReader reader = command.ExecuteReader();
            int i;
            while (reader.Read())
            {
                i = Convert.ToInt32(reader.GetValue(0));
                result.Add(i);
            }
            return result;
        }

        public void CompleteOrder(Int64 id)
        {
            var fullSql = string.Format("Update Orders set OrderStatus ='Complete' Where OrderStatus='Incomplete' and UserId ='{0}'",id);
            var command = GetCommand(fullSql);
            command.Connection.Open();
            DbDataReader reader = command.ExecuteReader();
         }
    }
}
