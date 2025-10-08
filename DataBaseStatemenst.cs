using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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

            string sql = "INSERT INTO `users`(`UserName`, `FullName`, `Password`, `Salt`, `Email`) VALUES (@username,@fullname,@password,@salt,@email)";

            MySqlCommand cmd = new MySqlCommand(sql, conn.Connection);

            var newUser = user.GetType().GetProperties();

            cmd.Parameters.AddWithValue("@username", newUser[0].GetValue(user));
            cmd.Parameters.AddWithValue("@fullname", newUser[1].GetValue(user));
            cmd.Parameters.AddWithValue("@password", newUser[2].GetValue(user));
            cmd.Parameters.AddWithValue("@salt", newUser[3].GetValue(user));
            cmd.Parameters.AddWithValue("@email", newUser[4].GetValue(user));

            cmd.ExecuteNonQuery();

            conn.Connection.Close();

            return new { message = "Sikeres hozzáadás."};
        }
    }
}
