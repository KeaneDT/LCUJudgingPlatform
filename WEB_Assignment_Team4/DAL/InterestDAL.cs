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
    }
}
