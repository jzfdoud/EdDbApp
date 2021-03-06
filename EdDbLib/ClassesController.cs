﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace EdDbLib
{
    public class ClassesController : BaseController
    {
        public struct ClassesAndInstruct
        {
            public Class Clas { get; set; }
            public Instructor Instructor { get; set; }
        }
        public IEnumerable<ClassesAndInstruct> GetClassesAndInstructors()
        {
           
            var instCtrl = new InstructorController(Connection);
            var clas = from c in GetAll()
                       join i in instCtrl.GetAll()
                       on c.InstructorId equals i.Id
                       select new ClassesAndInstruct
                       {
                           Class =c, Instructor = i
                       };

            return clas;

        }
        
        public ClassesController(Connection connection) : base(connection) {}

        public List<Clas> GetAll()
        {
            var sql = "Select * From Class;";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            var classes = new List<Clas>();

            while (reader.Read())
            {
                //classes.Add(class);
            }
            reader.Close();
            return classes;
        }

        public Clas GetByPk(int Id)
        {
            var sql = $"Select * From Class Where Id = {Id}.";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return null;
            }

            reader.Close();
            return ClassParameters(reader);
        }

        private static Clas ClassParameters(SqlDataReader reader)
        {
            var id = Convert.ToInt32(reader["Id"]);
            var clas = new Clas(id);
            clas.Code = reader["Code"].ToString();
            clas.Subject = reader["Subject"].ToString();
            clas.Section = Convert.ToInt32(reader["Section"]);
            clas.InstructorId = DBNull.Value.Equals(reader["InstructorId"]) 
                ? null : (int?)reader["InstructorId"];
            return clas;
        }

        //public Clas GetByPk(int id)
        //{
        //    var sql = $"Select * From Class Where Id = {id};";
        //    var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
        //    var reader = sqlCmd.ExecuteReader();
        //    if (!reader.HasRows)
        //    {
        //        reader.Close();
        //        return null;
        //    }

        //    Clas clas = ClassParameters(reader);
            
        //}
    }
}
  

