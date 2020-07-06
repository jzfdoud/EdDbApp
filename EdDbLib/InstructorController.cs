using System;
using System.Collections.Generic;
using System.Text;

namespace EdDbLib
{
    class InstructorController : BaseController
    {
        public List<Instructor> GetAll()
        {
            var sql = "Selct * From Instructor";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
        }


        public InstructorController(Connection connection) : base(connection) { }
    }
}
