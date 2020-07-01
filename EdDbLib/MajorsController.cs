using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace EdDbLib
{
    public class MajorsController
    {
        public Connection Connection { get; private set; } = null;
        //connection has the sqlconnection instance that is open
        // create sql statement
        public MajorsController(Connection connection)
        {
            Connection = connection;
        }
        //create the sql command passing the sql statement and open sql connection
        public List<Major> GetAll()
        {
            var sql = "Select * From Major;";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            // execute the sql statement and return result set in reader
            var reader = sqlCmd.ExecuteReader();
            //Create collection class instance
            var majors = new List<Major>(10);
            //read looks at rows, if true move on to next row, if return false there are no more rows
            while (reader.Read())
            {
                var id = Convert.ToInt32(reader["Id"]);
                //do primary key first
                var major = new Major(id);
                //id first then instance because the id is private so you cant set it after creating it
                major.Code = reader["Code"].ToString();
                major.Description = reader["Description"].ToString();
                major.MinSAT = Convert.ToInt32(reader["MinSAT"]);
                majors.Add(major);
                //add puts major in the collection of instances?
            }
            reader.Close();
            return majors;
        }
        
        public Major GetByPk(int Id)
        {
            var sql = $"Select * From Major Where Id = {Id}.";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            if(!reader.HasRows)
            {
                reader.Close();
                return null;
            }
            // if we get here we found major!

            reader.Read();
            var id = Convert.ToInt32(reader["Id"]);
            var major = new Major(id);
            major.Code = reader["Code"].ToString();
            major.Description = reader["Description"].ToString();
            major.MinSAT = Convert.ToInt32(reader["MinSAT"]);

            reader.Close();
            return major;
        }

        public bool Delete(int Id)
        {
            var sql = $"Delete from Major where Id = @Id;";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Id", Id);
            try {
                int rowsAffected = sqlCmd.ExecuteNonQuery();
                    switch (rowsAffected)
                    {
                    case 0: return false;
                    case 1: return true;
                    default: throw new Exception($"Error: deleted {rowsAffected} rows");
                    }
                //if (rowsAffected == 1) return true;
                ////one row deleted
                //if (rowsAffected == 0) return false;
                ////nothing deleted
                //throw new Exception($"Error: deleted {rowsAffected} rows");
                ////any other amount of rows deleted, showing a lot more than expected was deleted- purpose of exception
                }
            catch(SqlException ex) 
            {
                var refIntEx = new Exceptions.ReferentialIntegrityException("Cannot delete major used by student", ex);
                throw refIntEx;
                //trapping sql exception, adding our own exception while showing user the sql exception?
            }
        }

        public bool Update(Major major)
        {
            var sql = "Update Major Set " +
                "Code = @Code, " +
                "Description = @Description, " +
                "MinSAT = @MinSAT " +
                "Where Id = @Id; ";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Code", major.Code);
            sqlCmd.Parameters.AddWithValue("@Description", major.Description);
            sqlCmd.Parameters.AddWithValue("@MinSAT", major.MinSAT);
            sqlCmd.Parameters.AddWithValue("@Id", major.Id);

            var rowsAffected = sqlCmd.ExecuteNonQuery();
            switch (rowsAffected)
            {
                case 0: return false;
                case 1: return true;
                default: throw new Exception($"Error: Updated {rowsAffected} rows");
            }
        }

        public bool Insert(Major major)
        {
            var sql = "Insert Major " +
                "(Code, Description, MinSAT) " +
                "Values (@Code, @Description, @MinSAT);";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Code", major.Code);
            sqlCmd.Parameters.AddWithValue("@Description", major.Description);
            sqlCmd.Parameters.AddWithValue("@MinSAT", major.MinSAT);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            switch (rowsAffected)
            {
                case 0: return false;
                case 1: return true;
                default: throw new Exception($"Error: Inserted {rowsAffected} rows");
            }

        }

        public Major GetByCode(string code)
        {
            var sql = $"Select * From Major Where Code = @Code;";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Code", code);
            var reader = sqlCmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }
            // if we get here we found major!

            reader.Read();
            var id = Convert.ToInt32(reader["Id"]);
            var major = new Major(id);
            major.Code = reader["Code"].ToString();
            major.Description = reader["Description"].ToString();
            major.MinSAT = Convert.ToInt32(reader["MinSAT"]);

            reader.Close();
            return major;
        }
    }
}
