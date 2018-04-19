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

            using(SqliteConnection connection = new SqliteConnection(GetConnectionString())) 
            {
                connection.Open(); 

                string statement = "SELECT id, name, email, password, salt, role FROM Users;"; 

                using(SqliteCommand cmd = new SqliteCommand(statement, connection))
                {
                    using(SqliteDataReader reader = cmd.ExecuteReader()) 
                    {

                        User u = new User(); 
                        u.id = reader.GetInt32(0); 
                        u.name = reader.GetString(1); 
                        u.email = reader.GetString(2);
                        u.passwordHash = reader.GetString(3); 
                        u.salt = reader.GetString(4); 
                        u.r = User.role.Default; 
                    }
                }

                connection.Close(); 
            }

            return users; 
        }

        public static User GetUserByEmail(string email) {
            
            User u = new User(); 

            using(SqliteConnection connection = new SqliteConnection(GetConnectionString())) 
            {
                connection.Open(); 

                string statement = 
                "SELECT id, name, email, password, salt, role FROM Users WHERE email = " + email + ";"; 

                using(SqliteCommand cmd = new SqliteCommand(statement, connection))
                {
                    using(SqliteDataReader reader = cmd.ExecuteReader()) 
                    {

                        u.id = reader.GetInt32(0); 
                        u.name = reader.GetString(1); 
                        u.email = reader.GetString(2);
                        u.passwordHash = reader.GetString(3); 
                        u.salt = reader.GetString(4); 
                        u.r = User.role.Default; 
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

            using(SqliteConnection connection = new SqliteConnection(GetConnectionString())) 
            {
                connection.Open(); 

                string statement = "UPDATE User SET password = " + newPassword + 
                                   " WHERE email = " + u.email + " ;"; 

                using(SqliteCommand cmd = new SqliteCommand(statement, connection))
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

            using(SqliteConnection connection = new SqliteConnection(GetConnectionString())) 
            {
                connection.Open(); 

                string statement = "INSERT INTO User(name, email, password, salt, role) VALUES(" +
                                    u.name + "," + u.passwordHash + ", salt, 0);"; 

                using(SqliteCommand cmd = new SqliteCommand(statement, connection))
                {
                    cmd.ExecuteNonQuery(); 
                }

                connection.Close(); 
            }
        }

        #endregion


        }

    }