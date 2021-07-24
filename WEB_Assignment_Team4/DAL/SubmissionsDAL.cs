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
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission WHERE FileSubmitted IS NOT NUll ORDER BY CompetitionID";
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
                        VoteCount = reader.GetInt32(5),
                        FileName = reader.GetString(2),
                        UploadDateTime = reader.GetDateTime(3),
                        Ranking = reader.GetInt32(5)
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return submissionsList;
        }

        public List<Submissions> GetAllSubmissionsLeaderboard()
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission WHERE FileSubmitted IS NOT NUll ORDER BY Ranking";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<Submissions> leaderboardSubmissionsList = new List<Submissions>();
            while (reader.Read())
            {
                leaderboardSubmissionsList.Add(
                    new Submissions
                    {
                        CompetitionID = reader.GetInt32(0),
                        CompetitorID = reader.GetInt32(1),
                        VoteCount = reader.GetInt32(5),
                        FileName = reader.GetString(2),
                        UploadDateTime = reader.GetDateTime(3),
                        Ranking = reader.GetInt32(5)
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return leaderboardSubmissionsList;
        }

        public List<Submissions> GetCompetitionSubmissions(int competitionID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SQL statement that select all branches
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission WHERE CompetitionID = @selectedCompetition AND FileSubmitted IS NOT NUll";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “branchNo”.
            cmd.Parameters.AddWithValue("@selectedCompetition", competitionID);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            List<Submissions> submissionsList = new List<Submissions>();
            while (reader.Read())
            {
                submissionsList.Add(
                    new Submissions
                    {
                        CompetitionID = reader.GetInt32(0),
                        CompetitorID = reader.GetInt32(1),
                        VoteCount = reader.GetInt32(5),
                        FileName = reader.GetString(2),
                        UploadDateTime = reader.GetDateTime(3),
                        Ranking = reader.GetInt32(5)
                    });
            }
            //Close DataReader
            reader.Close();
            //Close database connection
            conn.Close();
            return submissionsList;
        }
    }
}
