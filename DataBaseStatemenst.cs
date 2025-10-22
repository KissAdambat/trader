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
            try
            {
                conn.Connection.Open();

                string sql = "SELECT * FROM `users` WHERE `UserName` = @username";
                MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);

                var logUser = user.GetType().GetProperties();
                string username = logUser[0].GetValue(user)?.ToString();
                string plainPassword = logUser[1].GetValue(user)?.ToString();

                cmd.Parameters.AddWithValue("@username", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHash = reader.GetString("Password");
                        string storedSalt = reader.GetString("Salt");

                        string computedHash = ComputeHmacSha256(plainPassword, storedSalt);

                        return storedHash == computedHash
                            ? new { message = "Sikeres bejelentkezés." }
                            : new { message = "Hibás jelszó." };
                    }
                    else
                    {
                        return new { message = "Felhasználó nem található." };
                    }
                }
            }
            catch (Exception ex)
            {
                return new { message = "Hiba történt a bejelentkezés során.", error = ex.Message };
            }
            finally
            {
                conn.Connection.Close();
            }
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

        public string DeleteUser(int userId)
        {
            try
            {
                conn.Connection.Open();
                string sql = "DELETE FROM `users` WHERE `UserID` = @userid";
                MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
                cmd.Parameters.AddWithValue("@userid", userId);
                cmd.ExecuteNonQuery();
                conn.Connection.Close();
                return "Sikeres törlés.";
            }
            catch (Exception)
            {
                return "Hiba történt a törlés során.";
            }
        }

        public string UserEdit(int userId, string fullName, string email)
        {
            try
            {
                conn.Connection.Open();
                string sql = "UPDATE `users` SET `FullName` = @fullname, `Email` = @email WHERE `UserID` = @userid";
                MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);
                cmd.Parameters.AddWithValue("@fullname", fullName);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@userid", userId);
                cmd.ExecuteNonQuery();
                conn.Connection.Close();
                return "Sikeres módosítás.";
            }
            catch (Exception)
            {
                return "Hiba történt a módosítás során.";
            }
        }
    }
}
