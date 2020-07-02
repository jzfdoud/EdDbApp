using System;
using System.Collections.Generic;
using System.Text;

namespace EdDbLib
{
    public class BaseController
    {
        public Connection Connection { get; protected set; } = null;
        public BaseController(Connection connection)
        {
            Connection = connection;
        }
    }
}
