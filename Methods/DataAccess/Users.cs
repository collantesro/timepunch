using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;


namespace timepunch
{
    public partial class DataAccess
    {

        #region Gets
        public List<User> GetUsers() 
        {
            List<User> users = new List<User>(); 

            using(SqliteConnection connection = new SqliteConnection(GetConnectionString())) 
            {
                connection.Open(); 

                string statement = "SELECT name FROM Users;"; 

                using(SqliteCommand cmd = new SqliteCommand(statement, connection))
                {
                    using(SqliteDataReader reader = cmd.ExecuteReader()) 
                    {
                        //Gets the data while reading, add rows to users here
                        //Console.WriteLine(rdr.GetInt32(0) + " " + rdr.GetString(1) + " " + rdr.GetInt32(2));
                        // while(reader.Read()) {
                        //     users.Add(reader.GetString(0)); 
                        //}
                    }
                }

                connection.Close(); 
            }

            return users; 
        }

            #endregion

        #region Sets

        #endregion

        #region Inserts

        public void InsertUser(User u) 
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