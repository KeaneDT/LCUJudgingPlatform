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
    public class CompetitorDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;
        //Constructor
        public CompetitorDAL()
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
        public List<Competitor> GetAllCompetitors()
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
            cmd.CommandText = @"SELECT * FROM Competitor ORDER BY CompetitorID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<Competitor> competitorList = new List<Competitor>();
            while (reader.Read())
            {
                competitorList.Add(
                    new Competitor
                    {
                        CompetitorID = reader.GetInt32(0),
                        CompetitorName = reader.GetString(1),
                        Salutation = reader.GetString(2),
                        EmailAddr = reader.GetString(4),
                        Password = reader.GetString(5),
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return competitorList;
        }
        public bool IsCompetitorExist(string email, int competitorId) //Create new validation
        {
            bool emailFound = false;

            //Create a SqlCommand object and specify the SQL statement
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CompetitorID FROM Competitor WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);

            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) //Records Found
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != competitorId)
                    {
                        //The email address is used by another judge
                        emailFound = true;
                    }
                }
            }
            else
            {
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return emailFound;
        }
        public bool ValidCompetitorLogin(string email, string pass)
        {
            bool validLogin = false;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CompetitorID FROM Competitor WHERE EmailAddr=@email AND Password=@pass";
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@pass", pass);
            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) //Records Found
            {
                validLogin = true;
            }
            else
            {
                validLogin = false;
            }
            reader.Close();
            conn.Close();

            return validLogin;
        }
        public int Add(Competitor competitor)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO Competitor (CompetitorName, Salutation, EmailAddr, Password)
                                OUTPUT INSERTED.CompetitorID
                                VALUES(@name, @salutation, @email, @password)";

            cmd.Parameters.AddWithValue("@name", competitor.CompetitorName);
            cmd.Parameters.AddWithValue("@salutation", competitor.Salutation);
            cmd.Parameters.AddWithValue("@email", competitor.EmailAddr);
            cmd.Parameters.AddWithValue("@password", competitor.Password);

            conn.Open();
            competitor.CompetitorID = (int)cmd.ExecuteScalar();
            conn.Close();

            return competitor.CompetitorID;
        }

    }
}
