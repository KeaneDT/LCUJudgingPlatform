using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB_Assignment_Team4.DAL;
using WEB_Assignment_Team4.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace WEB_Assignment_Team4.DAL
{
    public class CompetitionDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        public CompetitionDAL()
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

        public List<Competition> GetAllCompetition()
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
            cmd.CommandText = @"SELECT * FROM Competition ORDER BY CompetitionID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<Competition> competitionList = new List<Competition>();
            while (reader.Read())
            {
                competitionList.Add(
                    new Competition
                    {
                        CompetitionID = reader.GetInt32(0),
                        AreaInterestID = reader.GetInt32(1),
                        Name = reader.GetString(2),
                        StartDate = reader.GetDateTime(3),
                        EndDate = reader.GetDateTime(4),
                        ResultReleaseDate = reader.GetDateTime(5),
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return competitionList;
        }

        public List<Submissions> GetCompetitionSubmissions(int competitionID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SQL statement that select all branches
            cmd.CommandText = @"SELECT * FROM CompetitionSubmissions WHERE CompetitionID = @selectedCompetition";
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

        public List<Comment> GetCompetitionComment(int competitionID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SQL statement that select all branches
            cmd.CommandText = @"SELECT * FROM Comment WHERE CompetitionID = @selectedCompetition";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “branchNo”.
            cmd.Parameters.AddWithValue("@selectedCompetition", competitionID);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            List<Comment> commentList = new List<Comment>();
            while (reader.Read())
            {
                commentList.Add(
                    new Comment
                    {
                        CommentID = reader.GetInt32(0),
                        CompetitionID = reader.GetInt32(1),
                        Description = reader.GetString(2),
                        DateTimePosted = reader.GetDateTime(3),
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close database connection
            conn.Close();
            return commentList;
        }
        public bool IsNameExist(string name, int competitonId)
        {
            bool nameFound = false;
            //Create a SqlCommand object and specify the SQL Statment
            //to get a competition record with the name to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CompetitionID From Competition
                                WHERE CompetitionName=@selectedCompetitionName";
            cmd.Parameters.AddWithValue(@"selectedCompetitionName", name);

            //Open a database connection and execute the SQL statment
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows) //Records Found
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != competitonId)
                        // The competition name is used by another competition record.
                        nameFound = true;
                }
            }
            else // No records Found
            {
                nameFound = false; // The name given does not exist
            }
            reader.Close();
            conn.Close();

            return nameFound;

        }
        public int Add(Competition competition)
        {
            //Create a SqlCommand Object object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specifiy an INSERT SQL statment which will 
            //return an auto-generated CompetitionID after insertion
            cmd.CommandText = @"INSERT INTO Competition (AreaInterestID, CompetitionName,
                                StartDate, EndDate, ResultReleasedDate)
                                OUTPUT INSERTED.CompetitionID
                                VALUES(@interesttype, @name, @startdate, 
                                @enddate, @resultreleased)";
           
            //Define the parameters used in the SQL statment, value for each parameter
            //is retrieved from the respective class's property
            cmd.Parameters.AddWithValue("@interesttype", competition.AreaInterestID);
            cmd.Parameters.AddWithValue("@name", competition.Name);
            cmd.Parameters.AddWithValue("@startdate", competition.StartDate);
            cmd.Parameters.AddWithValue("@enddate", competition.EndDate);
            cmd.Parameters.AddWithValue("@resultreleased", competition.ResultReleaseDate);

           //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //CompetitionID after executing the INSERT SQL statments
            competition.CompetitionID = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations/
            conn.Close();

            //Return id with no error occours
            return competition.CompetitionID;

        }
        public Competition GetDetails(int competitionId)
        {
            Competition competition = new Competition();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that     
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM Competition                         
                                WHERE CompetitionID = @selectedCompetitionID";

            //Define the parameter used in SQL statement, value for the   
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    competition.CompetitionID= competitionId;
                    competition.AreaInterestID = reader.GetInt32(1);
                    competition.Name = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    competition.StartDate = !reader.IsDBNull(3) ?
                                            reader.GetDateTime(3) : (DateTime?)null;
                    competition.EndDate = !reader.IsDBNull(4) ?
                                          reader.GetDateTime(4) : (DateTime?)null;
                    competition.ResultReleaseDate = !reader.IsDBNull(5) ?
                                                    reader.GetDateTime(5) : (DateTime?)null;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();

            return competition;
        }
        public int Update(Competition competition)
        {
            //Create a Sqlcommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specifiy an UPDATE SQL Statments
            cmd.CommandText = @"IF EXISTS(SELECT c.*
                                FROM Competition c
                                INNER JOIN CompetitionSubmission cs
                                ON c.CompetitionID = cs.CompetitionID
                                where cs.CompetitionID = @selectedCompID)
                                BEGIN
                                PRINT 'Records Found' 
                                END
                                ELSE
                                UPDATE Competition SET AreaInterestID=@interestID,CompetitionName=@name,
                                StartDate=@startdate,EndDate=@enddate,
                                ResultReleasedDate=@daterelease
                                WHERE CompetitionID=@selectedCompID";

            //Define the parameters used in SQL statment, value for each parameter
            //is retrieved from the respective class's property.
            cmd.Parameters.AddWithValue("@name", competition.Name);
            cmd.Parameters.AddWithValue("@startdate", competition.StartDate);
            cmd.Parameters.AddWithValue("@enddate", competition.EndDate);
            cmd.Parameters.AddWithValue("@daterelease", competition.ResultReleaseDate);

            if (competition.AreaInterestID != null && competition.AreaInterestID != 0)
                //A InterestID is assigned
                cmd.Parameters.AddWithValue("@interestID", competition.AreaInterestID.Value);
            else
                //No InterestID is assigned
                cmd.Parameters.AddWithValue("@interestID", DBNull.Value);
            cmd.Parameters.AddWithValue("@selectedCompID", competition.CompetitionID);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();

            return count;
        }
        public int Delete(int competitionID)
        {
            SqlCommand cmd = conn.CreateCommand();
           
            cmd.CommandText = @"DELETE Competition
                                FROM AreaInterest x INNER JOIN Competition y
                                ON x.AreaInterestID = y.AreaInterestID
                                WHERE y.CompetitionID = @selectcompetitionId";
            
            cmd.Parameters.AddWithValue("@selectCompetitionId", competitionID);
            
            //Open a database connection
            conn.Open();
            
            int rowAffected = 0;

            //Execute the DELETE SQL to remove the interest record
            rowAffected += cmd.ExecuteNonQuery();

            //Close a database connection
            conn.Close();

            //Return number of row of interest record updated or deleted
            return rowAffected;
        }
        public List<Competition> GetJudgeCompetition(string email)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
            cmd.CommandText = @"SELECT x.* FROM Competition x INNER JOIN CompetitionJudge y ON x.CompetitionID=y.CompetitionID
                                INNER JOIN Judge z ON y.JudgeID=z.JudgeID WHERE z.EmailAddr = @email";
            cmd.Parameters.AddWithValue("@email", email);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<Competition> competitionList = new List<Competition>();
            while (reader.Read())
            {
                competitionList.Add(
                    new Competition
                    {
                        CompetitionID = reader.GetInt32(0),
                        AreaInterestID = reader.GetInt32(1),
                        Name = reader.GetString(2),
                        StartDate = reader.GetDateTime(3),
                        EndDate = reader.GetDateTime(4),
                        ResultReleaseDate = reader.GetDateTime(5),
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return competitionList;
        }
    }
}
