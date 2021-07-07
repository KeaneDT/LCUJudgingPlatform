﻿using System;
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

            //Read all records until the end, save data into a staff list
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
            //to get a staff record with the email address to be validated
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
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO Judge (JudgeName, Salutation, AreaInterestID, EmailAddr, Password)
                                OUTPUT INSERTED.JudgeID
                                VALUES(@name, @salutation, @interestID, @email, @password)";

            cmd.Parameters.AddWithValue("@name", judge.JudgeName);
            cmd.Parameters.AddWithValue("@salutation", judge.Salutation);
            cmd.Parameters.AddWithValue("@interestID", judge.AreaInterestID);
            cmd.Parameters.AddWithValue("@email", judge.EmailAddr);
            cmd.Parameters.AddWithValue("@password", judge.Password);

            conn.Open();
            judge.JudgeID = (int)cmd.ExecuteScalar();
            conn.Close();

            return judge.JudgeID;
        }
        public int GetJudgeID(string email)
        {
            int judgeID = 0;

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT JudgeID FROM Judge WHERE EmailAddr = @email";
            cmd.Parameters.AddWithValue("@email", email);

            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                judgeID = reader.GetInt32(0);
            }

            //Close DataReader
            reader.Close();

            //Close database connection
            conn.Close();

            return judgeID;
        }
    }
}
