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
    public class CompetitionDAL
    {
        private IConfiguration Configuration { get; }
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
        public bool IsNameExist(string name, int competitonId)
        {
            bool nameFound = false;
            //
            //
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CompetitionID From Competition
                                WHERE CompetitionName=@selectedCompetitionName";
            cmd.Parameters.AddWithValue(@"selectedCompetitionName", name);

            //
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != competitonId)
                        //
                        nameFound = true;
                }
            }
            else //
            {
                nameFound = false;
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
            cmd.Parameters.AddWithValue("@selectedAreaInterestID", competitionId);

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
        public List<Competition> GetJudgeCompetition(int judgeID)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
            cmd.CommandText = @"SELECT * FROM Competition INNER JOIN CompetitionJudge ON Competition.CompetitionID=CompetitionJudge.CompetitionID 
                                WHERE CompetitionJudge.JudgeID = @judgeID";
            cmd.Parameters.AddWithValue("@judgeID", judgeID);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Competition> judgeCompList = new List<Competition>();
            while (reader.Read())
            {
                judgeCompList.Add(
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
            reader.Close();
            conn.Close();

            return judgeCompList;
        }
    }
}
