using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace EdDbLib
{
    public class StudentsController
    {
        public Connection Connection { get; private set; } = null;
        //all class instances are allowed to be null

        public bool Delete(int Id)
        {
            var sql = $"Delete From Student Where Id = {Id}";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            if (rowsAffected != 1)
            {
                throw new Exception("Delete return result not 1");
            }
            //if we get here it worked!
            return true;

        }

        public bool Update(Student student)
        {
            var majorid = (student.MajorId == null)
                ? " NULL "
                : $" {student.MajorId} ";
           
            var sql = $"Update Student Set " +
                    $" Firstname = '{student.Firstname}', Lastname = '{student.Lastname}', StateCode = '{student.Statecode}', " +
                    $" SAT = {student.SAT}, GPA = {student.GPA}, MajorId = {majorid} Where id = {student.Id};";

            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            // means no resultset- executenonquery
            if (rowsAffected != 1)
            {
                throw new Exception("Update return result not 1");
            }
            //if we get here it worked!
            return true;
        }

        public bool Insert(Student student)
        {
            var majorid = (student.MajorId == null)
                ? " NULL "
                : $" {student.MajorId} ";
            var sql = $"Insert Student " +
                " ( Firstname, Lastname, StateCode, SAT, GPA, MajorId) " +
                " Values " +
                $" ('{student.Firstname}', '{student.Lastname}', '{student.Statecode}', {student.SAT}, {student.GPA}, {majorid}); ";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
             var rowsAffected = sqlCmd.ExecuteNonQuery();
            // means no resultset- executenonquery
            if(rowsAffected != 1)
            {
                throw new Exception("Insert return result not 1");
            }
            //if we get here it worked!
            return true;
        }

        public Student GetByPk(int Id)
        {
            var sql = $"Select * From Student Where Id = {Id}.";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }
            // if we get to here we found a student, read the record.

            reader.Read();
            var id = Convert.ToInt32(reader["Id"]);
            var student = new Student(id);
            student.Firstname = reader["Firstname"].ToString();
            student.Lastname = reader["Lastname"].ToString();
            student.Statecode = reader["StateCode"].ToString();
            student.SAT = Convert.ToInt32(reader["SAT"]);
            student.GPA = Convert.ToDecimal(reader["GPA"]);
            student.MajorId = null;
            if (!reader["MajorId"].Equals(DBNull.Value))
            {
                student.MajorId = Convert.ToInt32(reader["MajorId"]);
            }
            reader.Close();
            return student;

        }

        public List<Student> GetAll()
        {
            var sql = "Select * From Student;";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            //creating sql variable
            var reader = sqlCmd.ExecuteReader();
            var students = new List<Student>(60);
            while (reader.Read())
            {
                var student = new Student();
                student.Id = Convert.ToInt32(reader["Id"]);
                student.Firstname = reader["Firstname"].ToString();
                student.Lastname = reader["Lastname"].ToString();
                student.Statecode = reader["StateCode"].ToString();
                student.SAT = Convert.ToInt32(reader["SAT"]);
                student.GPA = Convert.ToDecimal(reader["GPA"]);
                student.MajorId = null;
                if (!reader["MajorId"].Equals(DBNull.Value))
                //dbvalue is null in sql not c#. C# keyword null != <null> (DBNull.value)-sql null
                {
                    student.MajorId = Convert.ToInt32(reader["MajorId"]);
                }
                //if true you are sitting on a valid row, if not skips?
                students.Add(student);

            }

            reader.Close();
            return students;
        }

        public StudentsController(Connection connection)
        {
            this.Connection = connection;
        }


    }
}
