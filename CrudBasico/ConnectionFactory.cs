using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudBasico.ConnectionFactory
{
    public static class ConnectionFactory
    {
        public static DbConnection CreateConnection()
        {
            var factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["CNN"].ProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["CNN"].ConnectionString;
            return connection;
        }
    }
}
