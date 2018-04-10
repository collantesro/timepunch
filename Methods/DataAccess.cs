using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;

namespace timepunch
{
    public class DataAccess
    {

        static private string GetConnectionString()
        {
            // Get the directory of the current assembly:
            string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return $"Data Source={baseDir}/databases/timepunch.sqlite;";
        }

        #region Gets

        public List<string> GetUsers()
        {

            List<string> users = new List<string>();

            using (SqliteConnection connect = new SqliteConnection(GetConnectionString()))
            {
                connect.Open();
                using (SqliteCommand qry = connect.CreateCommand())
                {
                    qry.CommandText = @"SELECT * FROM Users";
                    //qry.CommandType = CommandType.Text
                    SqliteDataReader r = qry.ExecuteReader();

                    while (r.Read())
                    {
                        //ImportedFiles.Add(Convert.ToString(r["FileName"]));

                        //User u = new User(); 
                        //u.name = (string)r["FileName"];


                        //users.add(u); 
                    }
                }
            }

            if (users.Count == 0)
            {
                throw new Exception("List is empty");
            }
            else
            {
                return (users);
            }
        }

        #endregion

        #region Sets

        #endregion

        #region Inserts

        #endregion


    }

}