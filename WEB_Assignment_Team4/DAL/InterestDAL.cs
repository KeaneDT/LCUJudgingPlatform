using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using WEB_Assignment_Team4.Models;

namespace WEB_Assignment_Team4.DAL
{
    public class InterestDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        // Constructor
        public InterestDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJPConnectionString");

            //Instantiate a Sqlconnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }
        
        public List<Interest> GetAllInterest()
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
            cmd.CommandText = @"SELECT * FROM AreaInterest ORDER BY AreaInterestID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<Interest> interestList = new List<Interest>();
            while (reader.Read())
            {
                interestList.Add(
                    new Interest
                    {
                        AreaInterestID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return interestList;
        }

        public int Add(Interest interest)
        {
            // Create a Sqlcommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify an Insert SQL statments which will
            // return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO AreaInterest(AreaInterestID,Name)
                               OUTPUT INSERTED.AreaInterestID
                               VALUES(@Name)";

            // Define the parameters used in SQL statment, value for each parameter
            // is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@Name", interest.Name);

            // A connection to database must be opened before any operations made.
            conn.Open();

            // ExecuteScalar is used to retrieve the auto-generated 
            // AreaInterestID after executing the INSERT SQL statments
            interest.AreaInterestID = (int)cmd.ExecuteScalar();

            // A connection should be closed after operations.
            conn.Close();

            // return id with no error occured.
            return interest.AreaInterestID;
        }
    }
}
