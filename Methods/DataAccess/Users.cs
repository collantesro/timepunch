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

        /// <summary>
        /// This method queries for a user by the specified email address.
        /// </summary>
        /// <param name="email">Email address of desired user.</param>
        /// <returns>Returns a User object if found, null otherwise.</returns>
        public static User GetUserByEmail(string email)
        {

            User u = null;

            using (SqliteConnection connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();

                string statement =
                "SELECT id, name, email, password, salt, role FROM Users WHERE email = @e COLLATE NOCASE;";
                // COLLATE NOCASE means search case-insensitive

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

                if(String.IsNullOrEmpty(u.Email))
                    throw new ArgumentNullException("Email is null or empty");

                string statement = "UPDATE User SET password = @password WHERE email = @email ;";

                using (SqliteCommand cmd = new SqliteCommand(statement, connection))
                {
                    cmd.Parameters.AddWithValue("@password", newPassword);
                    cmd.Parameters.AddWithValue("@email", u.Email);
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

                string statement = "INSERT INTO Users(name, email, password, salt, role) VALUES(@name, @email, @pass, @salt, @role);";

                using (SqliteCommand cmd = new SqliteCommand(statement, connection))
                {
                    cmd.Parameters.AddWithValue("@name", u.Name);
                    cmd.Parameters.AddWithValue("@email", u.Email);
                    cmd.Parameters.AddWithValue("@pass", u.PasswordHash);
                    cmd.Parameters.AddWithValue("@salt", u.Salt);
                    cmd.Parameters.AddWithValue("@role", User.Roles.Unapproved);
                    
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        #endregion


    }

}