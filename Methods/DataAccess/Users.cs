using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;


namespace timepunch
{
    public partial class DataAccess
    {

        #region Gets
        public static List<User> GetUsers()
        {
            List<User> users = new List<User>();

            using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();

                string statement = "SELECT id, name, email, password, salt, role FROM Users;";

                using (SqliteCommand cmd = new SqliteCommand(statement, connection))
                {
                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read()){
                            User u = new User(){
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                PasswordHash = reader.GetString(3),
                                Salt = reader.GetString(4),
                                Role = (User.Roles)reader.GetInt32(5)
                            };
                            users.Add(u);
                        }
                    }
                }

                connection.Close();
            }

            return users;
        }

        public static User GetUserByEmail(string email)
        {

            User u = null;

            using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();

                string statement =
                "SELECT id, name, email, password, salt, role FROM Users WHERE email = @e ;";

                using (SqliteCommand cmd = new SqliteCommand(statement, connection))
                {
                    cmd.Parameters.AddWithValue("@e", email);
                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read()){ // Returns false if a user with that email doesn't exist.
                            u = new User()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                PasswordHash = reader.GetString(3),
                                Salt = reader.GetString(4),
                                Role = (User.Roles)reader.GetInt32(5)
                            };
                        }
                    }
                }

                connection.Close();
            }

            return u;
        }

        #endregion

        #region Sets

        public static void UpdatePassword(User u, string newPassword)
        {

            using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();

                string statement = "UPDATE User SET password = " + newPassword +
                                   " WHERE email = " + u.Email + " ;";

                using (SqliteCommand cmd = new SqliteCommand(statement, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }


        #endregion

        #region Inserts

        public static void InsertUser(User u)
        {

            using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();

                string statement = "INSERT INTO User(name, email, password, salt, role) VALUES(@name, @email, @pass, @salt, @role);";

                using (SqliteCommand cmd = new SqliteCommand(statement, connection))
                {
                    cmd.Parameters.AddWithValue("@name", u.Name);
                    cmd.Parameters.AddWithValue("@email", u.Email);
                    cmd.Parameters.AddWithValue("@pass", u.PasswordHash);
                    cmd.Parameters.AddWithValue("@salt", u.Salt);
                    cmd.Parameters.AddWithValue("@role", u.Role);
                    
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        #endregion


    }

}