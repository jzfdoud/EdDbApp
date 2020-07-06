using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using System.Dynamic;

namespace EdDbLib
{
    public class StudentsController : BaseController
    {

        public struct StudentAndMajor
        {
            public int Id { get; set; }
            public string Fullname { get; set; }
            public string Major { get; set; }
        }

        public IEnumerable<StudentAndMajor> GetStudentWithMajor()
        {
            var students = GetAll();
            var majorCtrl = new MajorsController(Connection);
            var studentMajor = from s in students
                               join m in majorCtrl.GetAll()
                               on s.MajorId equals m.Id
                               select new StudentAndMajor
                               {
                                   Id = s.Id,
                                   Fullname = $"{s.Firstname} {s.Lastname}",
                                   Major = m.Description
                               };
            return studentMajor;
        }

        //structures similar to classes but cannot be null. Used when you have tp dream up some data used in one place. Can be called models-, something called a view like in sql?
        public struct StudentsPerState
        {
            public string StateCode { get; set; }
            public int Count { get; set; }
        }
        public IEnumerable<StudentsPerState> GetStudentsPerState()
        {
            var studentsPerState = from s in GetAll()
                                   group s by s.Statecode into sc
                                   select new StudentsPerState
                                   {
                                       StateCode = sc.Key, Count = sc.Count()
                                   };
            return studentsPerState;
        }

        //using LINQ to find students
        public IEnumerable<Student> GetByLastname(string Lastname)
        {
            var students = GetAll(); /* students is now a collection of all students in the database*/
            var someStudents = from s in students
                               where s.Lastname.StartsWith(Lastname)
                               orderby s.Lastname
                               select s;

            return someStudents;

        }

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

        public bool Insert(Student student, string MajorCode)
        {
            var majorCtrl = new MajorsController(this.Connection);
            //this.connection is for using the already initialized connection
            var major = majorCtrl.GetByCode(MajorCode);
            student.MajorId = major?.Id; /* <--if major is null, add it as major id?*/
            return Insert(student);

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

            Student student = StudentParameters(reader);
            if (!reader["MajorId"].Equals(DBNull.Value))
            {
                student.MajorId = Convert.ToInt32(reader["MajorId"]);
            }
            reader.Close();
            return student;

        }

        private static Student StudentParameters(SqlDataReader reader)
        {
            reader.Read();
            var id = Convert.ToInt32(reader["Id"]);
            var student = new Student(id);
            student.Firstname = reader["Firstname"].ToString();
            student.Lastname = reader["Lastname"].ToString();
            student.Statecode = reader["StateCode"].ToString();
            student.SAT = Convert.ToInt32(reader["SAT"]);
            student.GPA = Convert.ToDecimal(reader["GPA"]);
            student.MajorId = null;
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
                var student = StudentParameters(reader);

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

        public StudentsController(Connection connection) : base(connection)
        { }


    }
}
