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

            cmd.Parameters.AddWithValue("@username", user);
            cmd.Parameters.AddWithValue("@fullname", user);
            cmd.Parameters.AddWithValue("@password", user);
            cmd.Parameters.AddWithValue("@salt", user);
            cmd.Parameters.AddWithValue("@email", user);

            cmd.ExecuteNonQuery();

            conn.Connection.Close();

            return null;
        }
    }
}
