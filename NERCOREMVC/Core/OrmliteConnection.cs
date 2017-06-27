using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Core
{
    public class OrmliteConnection
    {
        public IDbConnection openConn()
        {
            var dbFactory = new OrmLiteConnectionFactory("Data Source=(local);Initial Catalog=NETCOREMVC;Integrated Security=True", SqlServerDialect.Provider);
            IDbConnection dbConn = dbFactory.OpenDbConnection();
            return dbConn;
        }
    }
}
