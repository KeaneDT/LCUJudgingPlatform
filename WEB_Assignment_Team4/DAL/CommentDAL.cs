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
    public class CommentDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        public CommentDAL()
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

        public List<Comment> GetAllComment()
        {
            //Create a SqlCommand object from connection object 
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statments
            cmd.CommandText = @"SELECT * FROM Comment ORDER BY CompetitionID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
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
            //Close the database connection
            conn.Close();

            return commentList;
        }

        public int Add(Comment comment, int value)
        {
            //Create a SqlCommand Object object from connection object
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO Comment (CompetitionID, Description,
                                DateTimePosted)
                                OUTPUT INSERTED.CommentID
                                VALUES(@competitionno, @text, @dateposted)";

            //Define the parameters used in the SQL statment, value for each parameter
            //is retrieved from the respective class's property
            DateTime localDate = DateTime.Now;
            cmd.Parameters.AddWithValue("@competitionno", value);
            cmd.Parameters.AddWithValue("@text", comment.Description);
            cmd.Parameters.AddWithValue("@dateposted", localDate);
            
            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //CompetitionID after executing the INSERT SQL statments
            comment.CommentID = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations/
            conn.Close();

            //Return id with no error occours
            return comment.CommentID;

        }


    }

}
