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

        public Interest GetInterestDetails(int interestId)
        {
            Interest interest = new Interest();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that     
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM AreaInterest                         
                                WHERE AreaInterestID = @selectedAreaInterestID";

            //Define the parameter used in SQL statement, value for the   
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedAreaInterestID", interestId);

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
                    interest.AreaInterestID = interestId;
                    interest.Name = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();

            return interest;
        }
        // Return number of rows updated

        public int Add(Interest interest)
        {
            // Create a Sqlcommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify an Insert SQL statments which will
            // return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO AreaInterest(Name)
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
        public bool IsInterestExist(string name, int AreaInterestId) //Create new validation
        {
            bool intrecordFound = false;

            //Create a SqlCommand object and specify the SQL statement
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT AreaInterestId FROM AreaInterest WHERE Name=@selectedName";
            cmd.Parameters.AddWithValue("@selectedName", name);

            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) //Records Found
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != AreaInterestId)
                    {
                        //The name is used by another user
                        intrecordFound = true;
                    }
                }
            }
            else
            {
                intrecordFound = false; // The name given does not exist
            }
            reader.Close();
            conn.Close();

            return intrecordFound;
        }
        public int Delete (int interestID)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL Statments
            //to delete a interest record specified by a interest ID
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM AreaInterest
                                WHERE AreaInterestID = @selectAreaInterestId";
            cmd.Parameters.AddWithValue("@selectAreaInterestId", interestID);
            // Open a database connection
            conn.Open();
            int rowAffected = 0;

            //Execute the DELETE SQL to remove the interest record
            rowAffected += cmd.ExecuteNonQuery();

            //
            conn.Close();
            //Return number of row of interest record updated or deleted
            return rowAffected;
        }
    }
}
