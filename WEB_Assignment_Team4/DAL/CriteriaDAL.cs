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
            //Specify the SELECT SQL statments
            cmd.CommandText = @"SELECT * FROM Criteria WHERE CompetitionID = @competitionID";
            cmd.Parameters.AddWithValue("@competitionID", compID);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
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
        public int GetCriteria(int criteriaID)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
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
        public int GetCriteriaTotal(int compID)
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
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

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM Criteria WHERE CriteriaID = @criteriaID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
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
                    // Fill staff object with values from the data reader
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
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO Criteria (CompetitionID, CriteriaName, Weightage)
                                OUTPUT INSERTED.CriteriaID
                                VALUES(@competitionID, @cName, @weightage)";

            cmd.Parameters.AddWithValue("@competitionID", criteria.CompetitionID);
            cmd.Parameters.AddWithValue("@cName", criteria.CriteriaName);
            cmd.Parameters.AddWithValue("@weightage", criteria.Weightage);

            conn.Open();
            criteria.CriteriaID = (int)cmd.ExecuteScalar();
            conn.Close();

            return criteria.CriteriaID;
        }
        public int Update(Criteria criteria)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
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
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a staff record specified by a Staff ID
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Criteria
                                WHERE CriteriaID = @criteriaID";
            cmd.Parameters.AddWithValue("@criteriaID", criteriaID);
            //Open a database connection
            conn.Open();
            int rowAffected = 0;
            //Execute the DELETE SQL to remove the staff record
            rowAffected += cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
            //Return number of row of staff record updated or deleted
            return rowAffected;
        }
    }
}
