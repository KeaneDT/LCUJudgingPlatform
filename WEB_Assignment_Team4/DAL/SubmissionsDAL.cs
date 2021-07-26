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

                        FileName = !reader.IsDBNull(2) ?
                        reader.GetString(2) : (string)null,

                        UploadDateTime = !reader.IsDBNull(3) ?
                        reader.GetDateTime(3) : (DateTime?)null,

                        Ranking = !reader.IsDBNull(6) ?
                        reader.GetInt32(5) : (int?)null,
                    });
            }
            //Close DataReader
            reader.Close();
            //Close database connection
            conn.Close();
            return submissionsList;
        }

        public int IncreaseCount(Submissions submission)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Update CompetitionSubmission SET VoteCount = VoteCount + 1 Where CompetitionID = 1 AND CompetitorID = 1";


            conn.Open();
            int count = cmd.ExecuteNonQuery();
            conn.Close();
            return count;

        }

        public SubmissionViewModel GetSubmissionDetails(int competitionID, int competitorID)
        {
            SubmissionViewModel sVM = new SubmissionViewModel();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT y.CompetitorID,y.Salutation, y.CompetitorName,z.CompetitionID,
                                y.EmailAddr, x.FileSubmitted, x.DateTimeFileUpload, x.Appeal, 
                                x.VoteCount, x.Ranking FROM CompetitionSubmission x 
                                INNER JOIN Competitor y ON x.CompetitorID=y.CompetitorID 
                                INNER JOIN Competition z ON x.CompetitionID=z.CompetitionID 
                                WHERE x.CompetitionID = @competitionID AND x.CompetitorID = @competitorID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@competitionID", competitionID);
            cmd.Parameters.AddWithValue("@competitorID", competitorID);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    sVM.CompetitorID = reader.GetInt32(0);
                    sVM.Salutation = !reader.IsDBNull(1) ?
                        reader.GetString(1) : (string)null;
                    sVM.CompetitorName = reader.GetString(2);
                    sVM.CompetitionID = reader.GetInt32(3);
                    sVM.EmailAddr = reader.GetString(4);
                    sVM.FileName = !reader.IsDBNull(5) ?
                        reader.GetString(5) : (string)null;
                    sVM.UploadDateTime = !reader.IsDBNull(6) ?
                        reader.GetDateTime(6) : (DateTime?)null;
                    sVM.Appeal = !reader.IsDBNull(7) ?
                        reader.GetString(7) : (string)null;
                    sVM.VoteCount = reader.GetInt32(8);
                    sVM.Ranking = !reader.IsDBNull(9) ?
                        reader.GetInt32(9) : (int?)null;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return sVM;
        }
    }
}
