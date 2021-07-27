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
    public class CriteriaDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        //Constructor
        public CriteriaDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
                "CJPConnectionString");

            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<Criteria> GetCompetitionCriteria(int compID)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments to get all the Criteria Details based on the CompetitionID specified
            cmd.CommandText = @"SELECT * FROM Criteria WHERE CompetitionID = @competitionID";
            cmd.Parameters.AddWithValue("@competitionID", compID);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a Criteria list
            List<Criteria> cList = new List<Criteria>();
            while (reader.Read())
            {
                cList.Add(
                    new Criteria
                    {
                        CriteriaID = reader.GetInt32(0),
                        CompetitionID = reader.GetInt32(1),
                        CriteriaName = reader.GetString(2),
                        Weightage = reader.GetInt32(3),
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return cList;
        }
        public int GetCriteriaWeightage(int criteriaID)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statment to get the Weightage of the Criteria based on the CriteriaID specified
            cmd.CommandText = @"SELECT Weightage FROM Criteria WHERE CriteriaID = @criteriaID";
            cmd.Parameters.AddWithValue("@criteriaID", criteriaID);

            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            int count = 0;
            while (reader.Read())
            {
                count += reader.GetInt32(0);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return count;
        }
        public int GetWeightageTotal(int compID)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statment to get the Weightage of all Criteria based on the CompetitionID specified
            cmd.CommandText = @"SELECT Weightage FROM Criteria WHERE CompetitionID = @competitionID";
            cmd.Parameters.AddWithValue("@competitionID", compID);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            int count = 0;
            while (reader.Read())
            {
                count += reader.GetInt32(0);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return count;
        }
        public Criteria GetDetails(int criteriaID)
        {
            Criteria criteria = new Criteria();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statment to get the details of a Criteria based on the CriteriaID specified specified
            cmd.CommandText = @"SELECT * FROM Criteria WHERE CriteriaID = @criteriaID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “criteriaID”
            cmd.Parameters.AddWithValue("@criteriaID", criteriaID);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    criteria.CriteriaID = criteriaID;
                    criteria.CompetitionID = reader.GetInt32(1);
                    criteria.CriteriaName = reader.GetString(2);
                    criteria.Weightage = reader.GetInt32(3);
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();

            return criteria;
        }
        public int Add(Criteria criteria)
        {
            //Create a SqlCommand objects from connection objects
            SqlCommand cmd1 = conn.CreateCommand();
            SqlCommand cmd2 = conn.CreateCommand();
            SqlCommand cmd3 = conn.CreateCommand();

            //Specify the INSERT SQL Statement to Insert the new Criteria details and output the CriteriaID Generated
            cmd1.CommandText = @"INSERT INTO Criteria (CompetitionID, CriteriaName, Weightage)
                                OUTPUT INSERTED.CriteriaID
                                VALUES(@competitionID, @cName, @weightage)";

            cmd1.Parameters.AddWithValue("@competitionID", criteria.CompetitionID);
            cmd1.Parameters.AddWithValue("@cName", criteria.CriteriaName);
            cmd1.Parameters.AddWithValue("@weightage", criteria.Weightage);

            //Specify the SELECT SQL Statement to get the CompetitiorID's of those who are in the same competition as the criteria competition & has a valid file submission
            cmd2.CommandText = @"SELECT Competitor.CompetitorID FROM Competitor
                                INNER JOIN CompetitionSubmission ON Competitor.CompetitorID=CompetitionSubmission.CompetitorID
                                WHERE CompetitionSubmission.FileSubmitted IS NOT NULL";

            //Specify the INSERT SQL Statement to Insert the new CriteriaID along with the many CompetitorID's, the CompetitionID and the default Score of 0
            cmd3.CommandText = @"INSERT INTO CompetitionScore (CriteriaID, CompetitorID, CompetitionID, Score) VALUES (@criteriaID,@competitorID,@competitionID,@score)";
            //Open a database connection
            conn.Open();
            int rowAffected = 0;
            //Assign the CriteriaID from outputed int
            criteria.CriteriaID = (int)cmd1.ExecuteScalar();

            //ExecuteReader to iterate through all the CompetitorID in the Competition with valid FileSubmissions
            SqlDataReader reader = cmd2.ExecuteReader();
            //In order to make this possible, MultipleActiveResultSets=true; was set in the ConnectionString
            if (reader.HasRows)
            {
                //Set the parameters of cmd3 & then execute. After that, clear the params so it can be reused in the next reader.read()
                while (reader.Read())
                {
                    cmd3.Parameters.AddWithValue("@criteriaID", criteria.CriteriaID);
                    cmd3.Parameters.AddWithValue("@competitorID", reader.GetInt32(0));
                    cmd3.Parameters.AddWithValue("@competitionID", criteria.CompetitionID);
                    cmd3.Parameters.AddWithValue("@score", 0);

                    rowAffected += cmd3.ExecuteNonQuery();
                    cmd3.Parameters.Clear();
                }
            }
            //Close reader
            reader.Close();
            //Close database connection
            conn.Close();

            return criteria.CriteriaID;
        }
        public int Update(Criteria criteria)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement based on the CriteriaID specified
            cmd.CommandText = @"UPDATE Criteria SET CriteriaName = @name,
                                Weightage = @weightage
                                WHERE CriteriaID = @criteriaID";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", criteria.CriteriaName);
            cmd.Parameters.AddWithValue("@weightage", criteria.Weightage);
            cmd.Parameters.AddWithValue("@criteriaID", criteria.CriteriaID);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();

            return count;
        }
        public int Delete(int criteriaID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd1 = conn.CreateCommand();
            SqlCommand cmd2 = conn.CreateCommand();
            //Specify an DELETE SQL statement based on the CriteriaID specified
            cmd1.CommandText = @"DELETE FROM Criteria
                                WHERE CriteriaID = @criteriaID";
            cmd1.Parameters.AddWithValue("@criteriaID", criteriaID);
            cmd2.CommandText = @"DELETE FROM CompetitionScore WHERE CriteriaID = @criteriaID";
            cmd2.Parameters.AddWithValue("@criteriaID", criteriaID);
            //Open a database connection
            conn.Open();
            int rowAffected = 0;
            //Execute the DELETE SQL to remove the Criteria record
            rowAffected += cmd2.ExecuteNonQuery();
            rowAffected += cmd1.ExecuteNonQuery();
            //Close database connection
            conn.Close();
            //Return number of row of Criteria record updated or deleted
            return rowAffected;
        }
        public CriteriaViewModel GetSubmissionCriteriaDetail(int competitionID, int competitorID, int criteriaID)
        {
            CriteriaViewModel cVM = new CriteriaViewModel();

            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments to get all the Criteria Details based on the CompetitionID specified
            cmd.CommandText = @"SELECT x.CompetitionID, x.CriteriaID, y.CriteriaName, y.Weightage, x.Score FROM CompetitionScore x 
                                INNER JOIN Criteria y ON x.CriteriaID=y.CriteriaID 
                                INNER JOIN CompetitionSubmission z ON x.CompetitorID=z.CompetitorID 
                                WHERE z.CompetitorID=@competitorID AND z.CompetitionID=@competitionID AND x.CriteriaID=@criteriaID";
            cmd.Parameters.AddWithValue("@competitorID", competitorID);
            cmd.Parameters.AddWithValue("@competitionID", competitionID);
            cmd.Parameters.AddWithValue("@criteriaID", criteriaID);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a Criteria object           
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    cVM.CompetitionID = reader.GetInt32(0);
                    cVM.CriteriaID = reader.GetInt32(1);
                    cVM.CriteriaName = reader.GetString(2);
                    cVM.Weightage = reader.GetInt32(3);
                    cVM.Score = reader.GetInt32(4);
                    cVM.CompetitorID = competitorID;
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return cVM;
        }
        public List<CriteriaViewModel> GetSubmissionCriteria(int competitionID, int competitorID)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments to get all the Criteria Details based on the CompetitionID specified
            cmd.CommandText = @"SELECT x.CompetitionID, x.CriteriaID, y.CriteriaName, y.Weightage, x.Score FROM CompetitionScore x 
                                INNER JOIN Criteria y ON x.CriteriaID=y.CriteriaID 
                                INNER JOIN CompetitionSubmission z ON x.CompetitorID=z.CompetitorID 
                                WHERE z.CompetitorID=@competitorID AND z.CompetitionID=@competitionID";
            cmd.Parameters.AddWithValue("@competitorID", competitorID);
            cmd.Parameters.AddWithValue("@competitionID", competitionID);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a Criteria list
            List<CriteriaViewModel> cVMList = new List<CriteriaViewModel>();
            while (reader.Read())
            {
                cVMList.Add(
                    new CriteriaViewModel
                    {
                        CompetitionID = reader.GetInt32(0),
                        CriteriaID = reader.GetInt32(1),
                        CriteriaName = reader.GetString(2),
                        Weightage = reader.GetInt32(3),
                        Score = reader.GetInt32(4),
                        CompetitorID = competitorID,
                    }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return cVMList;
        }
        public double GetSubmissionCriteriaTotal(int competitionID, int competitorID)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments to get all the Criteria Details based on the CompetitionID specified
            cmd.CommandText = @"SELECT y.Weightage, x.Score FROM CompetitionScore x 
                                INNER JOIN Criteria y ON x.CriteriaID=y.CriteriaID 
                                INNER JOIN CompetitionSubmission z ON x.CompetitorID=z.CompetitorID 
                                WHERE z.CompetitorID=@competitorID AND z.CompetitionID=@competitionID";
            cmd.Parameters.AddWithValue("@competitorID", competitorID);
            cmd.Parameters.AddWithValue("@competitionID", competitionID);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a Criteria list
            double totalScore = 0;
            while (reader.Read())
            {
                totalScore += Convert.ToDouble(reader.GetInt32(0)) * Convert.ToDouble(reader.GetInt32(1))/10;
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return totalScore;
        }
        public int UpdateCriteriaScore(CriteriaViewModel cVM)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement based on the CriteriaID specified
            cmd.CommandText = @"UPDATE CompetitionScore SET Score = @score 
                                WHERE CompetitionID = @competitionID
                                AND CompetitorID = @competitorID
                                AND CriteriaID = @criteriaID";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@competitionID", cVM.CompetitionID);
            cmd.Parameters.AddWithValue("@competitorID", cVM.CompetitorID);
            cmd.Parameters.AddWithValue("@criteriaID", cVM.CriteriaID);
            cmd.Parameters.AddWithValue("@score", cVM.Score);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();

            return count;
        }
    }
}
