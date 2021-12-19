using System.Data.SqlClient;

namespace MonitoringSystem.DAL
{
    public class DAInstance
    {
        private static readonly DAInstance _daService = new DAInstance();

        private static SqlConnection _connection;
        public SqlConnection Connection { get { return _connection; } }
        private DAInstance()
        {

        }
        public static void Instance(string connectionString)
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(connectionString);
                _daService.Connection.Open();
            }
        }

        public static void OpenConnection()
        {
            if (_daService.Connection.State == System.Data.ConnectionState.Closed) _daService.Connection.Open();
        }
        public static DAInstance GetInstance()
        {
            if (_daService.Connection.State == System.Data.ConnectionState.Closed) _daService.Connection.Open();
            return _daService;
        }
    }
}
