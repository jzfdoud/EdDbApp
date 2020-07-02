using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EdDbLib;


namespace EdDbApp
{
    public class Program
    {
        static void Main() 
        {
            testClassesController();
            //remember to change to which method you actually want to call.
        }

        static void testClassesController()
        {
            var conn = new Connection("localhost", "sqlexpress", "EdDb");
            var classesCtrl = new ClassesController(conn);

            var classes = classesCtrl.GetAll();
            //var newClas = new Clas()
            //{
            //    Code = "ART119",
            //    Subject = "ART",
            //    Section = 1
            //};
            conn.Close();

        }

        static void testMajorController()
        {
            var connection = new Connection("localhost", "sqlexpress", "EdDb");
            var majorsCtrl = new MajorsController(connection);

            var MajorARTC = majorsCtrl.GetByCode("ARTC");

            //var newMajor = new Major(9)
            //{
            //    Code = "ARTC",
            //    Description = "Ceramics",
            //    MinSAT = 1237,
                
            //};
            //var success1 = majorsCtrl.Insert(newMajor);


            ////var majors3 = majorsCtrl.Delete(8);

            //var majors = majorsCtrl.GetAll();
            //var majors1 = majorsCtrl.GetByPk(3);
            //var majors2 =  majorsCtrl.GetByPk(9);
        }
       
      
        static void TestStudentController()
        {
            var conn = new Connection("localhost", "sqlexpress", "EdDb");
            var studentsCtrl = new StudentsController(conn);

            var newStudent = new Student() 
            {
                Firstname = "Victoria",
                Lastname = "Doud",
                Statecode = "OH",
                SAT = 1600,
                GPA = 4.0m,
                MajorId = null
            };
            // 'm' means treat double as a decimal. could also use convert.decimal  

            var itWorked = studentsCtrl.Insert(newStudent, "ARTC");

            //var Eden = new Student(61)
            //{
            //    Firstname = "Eden",
            //    Lastname = "Doud",
            //    Statecode = "OH",
            //    SAT = 1600,
            //    GPA = 4.0m,
            //    MajorId = 3
            //};
            //var itWorked = studentsCtrl.Update(Eden);
            ////var itWorked = studentsCtrl.Insert(newStudent);

            //itWorked = studentsCtrl.Delete(61);

            //var student = studentsCtrl.GetByPk(10);
            //var noStudent = studentsCtrl.GetByPk(-1);
            //var students = studentsCtrl.GetAll();

            conn.Close();
        }

        static void Test1()
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
            var students = new List<Student>(60);
            while(reader.Read())
            {
                var student = new Student();
                student.Id = Convert.ToInt32(reader["Id"]);
                student.Firstname = reader["Firstname"].ToString();
                student.Lastname = reader["Lastname"].ToString();
                student.Statecode = reader["StateCode"].ToString();
                student.SAT = Convert.ToInt32(reader["SAT"]);
                student.GPA = Convert.ToDecimal(reader["GPA"]);
                student.MajorId = null;
                if(!reader["MajorId"].Equals(DBNull.Value))
                    //dbvalue is null in sql not c#. C# keyword null != <null> (DBNull.value)-sql null
                {
                    student.MajorId = Convert.ToInt32(reader["MajorId"]);
                }
            //if true you are sitting on a valid row, if not skips?
                students.Add(student);

            }
           
            reader.Close();



            sqlConn.Close();
           
        }
    }
}
/* int64 is like long, int32 is like int. 
As long as there are no alphabet characters or symbols you can do this, same for decimal. 
if three are it will give back error*/
//can only have one datareader open at a time, will tell you one is open.
//dont let sql kill the connection string. 
//Sql connections are limited so you may not be able to connect if on a shared database after losing connection.
