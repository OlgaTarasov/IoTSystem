using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace MonitoringSystem.DAL
{
    public class DAService
    {
        private DAInstance _daService = DAInstance.GetInstance();

        public List<T> ExecuteSP<T>(string spName, SqlParameter[] parameters)
        {
            List<T> list = new List<T>();
            if ((_daService.Connection.State == ConnectionState.Closed))
            {
                _daService.Connection.Open();
            }
                try
                {
                    using (SqlCommand sqlCommand = new SqlCommand(spName, _daService.Connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddRange(parameters);

                        T obj = default(T);

                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj = Activator.CreateInstance<T>();
                                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                                {
                                    if (!object.Equals(reader[prop.Name], DBNull.Value))
                                    {
                                        prop.SetValue(obj, reader[prop.Name], null);
                                    }
                                }
                                list.Add(obj);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            
            return list;
        }
    }
}
