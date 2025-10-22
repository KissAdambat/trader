using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace trader
{
    internal class DataBaseStatemenst
    {
        Connect conn = new Connect();

        public object AddNewUser(object user)
        {
            conn.Connection.Open();
            var newUser = user.GetType().GetProperties();
            string salt = GenerateSalt();
            string hashedPassword = ComputeHmacSha256((string)newUser[2].GetValue(user), salt);

            string sql = "INSERT INTO `users`(`UserName`, `FullName`, `Password`, `Salt`, `Email`) VALUES (@username,@fullname,@password,@salt,@email)";

            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);


            cmd.Parameters.AddWithValue("@username", newUser[0].GetValue(user));
            cmd.Parameters.AddWithValue("@fullname", newUser[1].GetValue(user));
            cmd.Parameters.AddWithValue("@password", hashedPassword);
            cmd.Parameters.AddWithValue("@salt", salt);
            cmd.Parameters.AddWithValue("@email", newUser[4].GetValue(user));

            cmd.ExecuteNonQuery();

            conn.Connection.Close();

            return new { message = "Sikeres hozzáadás."};
        }
        public object LoginUser(object user)
        {
            conn.Connection.Open();
            string sql = "SELECT * FROM `users` WHERE `UserName` = @username AND `Password` = @password";
            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
            var logUser = user.GetType().GetProperties();
            cmd.Parameters.AddWithValue("@username", logUser[0].GetValue(user));
            cmd.Parameters.AddWithValue("@password", logUser[1].GetValue(user));
            MySqlDataReader reader = cmd.ExecuteReader();
            object IsRegistered = reader.Read() ? new { message = "Regisztrált"}:new {message = "Nem regisztrált" };
            conn.Connection.Close();
            return IsRegistered;
        }

        public DataView GetAllUsers()
        {
            try
            {
                conn.Connection.Open();
                string sql = "SELECT * FROM `users`";
                MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conn.Connection.Close();
                return dt.DefaultView;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        public string ComputeHmacSha256(string password, string salt)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
