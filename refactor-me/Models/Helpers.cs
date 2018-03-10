using System.Data.SqlClient;
using System.Web;

namespace refactor_me.Models
{
    public class Helpers
    {
        private const string ConnectionString = @"Data Source=.\sqlexpress01;Initial Catalog=C:\DEV\REFACTOR-ME\REFACTOR-ME\APP_DATA\DATABASE.MDF;Integrated Security=True";

        public static SqlConnection NewConnection()
        {
            var connstr = ConnectionString.Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data"));
            return new SqlConnection(connstr);
        }
    }
}