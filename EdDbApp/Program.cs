using System;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;

namespace EdDbApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connStr = @"server=localhost\sqlexpress;database=EdDb;trusted_connection=true;";
            //if having trouble connecting to sql-- check connection string!
            var sqlConn = new SqlConnection(connStr);
            sqlConn.Open();
            if (sqlConn.State != ConnectionState.Open)
            {
                throw new Exception("Connection did not open.");
            }
            // if connetion not made, will throw it and show error, if not continue.
            var sql = "Select * From Student;";
            var sqlCmd = new SqlCommand(sql, sqlConn);
            //creating sql variable
            var reader = sqlCmd.ExecuteReader();
            while(reader.Read())
            {
                var firstname = reader["Firstname"].ToString();
                var lastname = reader["Lastname"].ToString();
                Console.WriteLine($"{firstname} {lastname}");
            }
            //if true you are sitting on a valid row, if not skips?
            //can only have one datareader open at a time, will tell you one is open.
            reader.Close();



            sqlConn.Close();
            //dont let sql kill the connection string
        }
    }
}
