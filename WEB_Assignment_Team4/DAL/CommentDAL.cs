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

            //Read all records until the end, save data into a staff list
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


    }

}
