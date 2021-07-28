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
    public class JudgeDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        //Constructor
        public JudgeDAL()
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
        public List<Judge> GetAllJudges()
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
            cmd.CommandText = @"SELECT * FROM Judge ORDER BY JudgeID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a Judge list
            List<Judge> judgeList = new List<Judge>();
            while (reader.Read())
            {
                judgeList.Add(
                    new Judge
                    {
                        JudgeID = reader.GetInt32(0),
                        JudgeName = reader.GetString(1),
                        Salutation = reader.GetString(2),
                        AreaInterestID = reader.GetInt32(3),
                        EmailAddr = reader.GetString(4),
                        Password = reader.GetString(5),
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return judgeList;
        }
        public bool IsJudgeExist(string email, int judgeId) //Create new validation
        {
            bool emailFound = false;

            //Create a SqlCommand object and specify the SQL statement
            //to get a Judge record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT JudgeID FROM Judge WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);

            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) //Records Found
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != judgeId)
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
        public bool ValidJudgeLogin(string email, string pass)
        {
            bool validLogin = false;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT JudgeID FROM Judge WHERE EmailAddr=@email AND Password=@pass";
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
        public int Add(Judge judge)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the INSERT SQL statment to Insert the new Judge details and output the JudgeID Generated
            cmd.CommandText = @"INSERT INTO Judge (JudgeName, Salutation, AreaInterestID, EmailAddr, Password)
                                OUTPUT INSERTED.JudgeID
                                VALUES(@name, @salutation, @interestID, @email, @password)";

            cmd.Parameters.AddWithValue("@name", judge.JudgeName);
            cmd.Parameters.AddWithValue("@salutation", judge.Salutation);
            cmd.Parameters.AddWithValue("@interestID", judge.AreaInterestID);
            cmd.Parameters.AddWithValue("@email", judge.EmailAddr);
            cmd.Parameters.AddWithValue("@password", judge.Password);

            //Open a database connection
            conn.Open();
            //Assign the JudgeID from outputed int
            judge.JudgeID = (int)cmd.ExecuteScalar();
            //Close database connection
            conn.Close();

            return judge.JudgeID;
        }

        public int Assign(JudgeAssign assign)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the INSERT SQL statment to Insert the new Judge details and output the JudgeID Generated
            cmd.CommandText = @"INSERT INTO CompetitionJudge (CompetitionID,JudgeID)
                                VALUES(@compID,@judgeID)";

            cmd.Parameters.AddWithValue("@judgeID", assign.JudgeID);
            cmd.Parameters.AddWithValue("@compID", assign.CompetitionID);
            
            //Open a database connection
            conn.Open();

            //Assign the JudgeID from outputed int
            assign.JudgeID = cmd.ExecuteNonQuery();
            
            //Close database connection
            conn.Close();

            return assign.JudgeID;
        }

        public int AssignDelete(int assignId,int judgeId)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE CompetitionJudge
                                FROM CompetitionJudge y INNER JOIN Judge x 
                                ON x.JudgeID = y.JudgeID
                                WHERE y.CompetitionID = @selectedCompetitionID
                                AND y.JudgeID = @selectedJudgeID";
            
            cmd.Parameters.AddWithValue("@selectedCompetitionID", assignId);
            cmd.Parameters.AddWithValue("@selectedJudgeID", judgeId);

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

        public Judge GetDetails(int JudgeId)
        {
            Judge Judge = new Judge();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that     
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM Judge                       
                                WHERE JudgeID = @selectedJudgeID";

            //Define the parameter used in SQL statement, value for the   
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedJudgeID", JudgeId);

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
                    Judge.JudgeID = JudgeId;
                    Judge.JudgeName = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    Judge.Salutation = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    Judge.AreaInterestID = !reader.IsDBNull(3) ? 
                                            reader.GetInt32(3) :(int?)null;
                    Judge.EmailAddr = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                    Judge.Password = !reader.IsDBNull(5) ? reader.GetString(5) : null;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();

            return Judge;
        }
        public List<JudgeAssign> GetAllAssignJudges()
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
            cmd.CommandText = @"SELECT * FROM CompetitionJudge ORDER BY CompetitionID ASC";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<JudgeAssign> competitionList = new List<JudgeAssign>();
            while (reader.Read())
            {
                competitionList.Add(
                    new JudgeAssign
                    {
                        CompetitionID = reader.GetInt32(0),
                        JudgeID = reader.GetInt32(1)
                    }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return competitionList;
        }

        public JudgeAssign GetJudgesRole(int roleId)
        {
            JudgeAssign role = new JudgeAssign();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that     
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM CompetitionJudge                         
                                WHERE CompetitionID = @selectedCompetitionID";

            //Define the parameter used in SQL statement, value for the   
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", roleId);

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
                    role.CompetitionID = roleId;
                    role.JudgeID= reader.GetInt32(1);
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();

            return role;
        }
        public bool IsCompetitionJudgeExist(int name, int competitonId)
        {
            bool nameFound = false;
            //Create a SqlCommand object and specify the SQL Statment
            //to get a competition record with the name to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CompetitionID From CompetitionJudge
                                WHERE JudgeId=@selectedCompetitionName";
            cmd.Parameters.AddWithValue(@"selectedCompetitionName", name);

            //Open a database connection and execute the SQL statment
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows) //Records Found
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != name)
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
    }
}


