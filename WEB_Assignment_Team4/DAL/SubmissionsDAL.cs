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
    public class SubmissionsDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public SubmissionsDAL()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("CJPConnectionString");

            //Instantiate a Sqlconnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<Submissions> GetAllSubmissions()
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
            cmd.CommandText = @"SELECT * FROM CompetitionSubmssions ORDER BY CompetitionID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<Submissions> submissionsList = new List<Submissions>();
            while (reader.Read())
            {
                submissionsList.Add(
                    new Submissions
                    {
                        CompetitionID = reader.GetInt32(0),
                        CompetitorID = reader.GetInt32(1),
                        FileName = reader.GetString(2),
                        UploadDateTime = reader.GetDateTime(3),
                        VoteCount = reader.GetInt32(5),
                        Ranking = reader.GetInt32(6)
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return submissionsList;
        }
    }
}
