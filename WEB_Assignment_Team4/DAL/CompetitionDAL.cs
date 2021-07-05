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
        // Return number of rows updated
    }
}
