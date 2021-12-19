using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace WSReceiver.DAL
{
    internal static class DAService
    {
        private static SqlConnection _connection;

        public static void Connect(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }
        public static bool OpenConnection()
        {
            _connection.Open();
            return _connection.State == ConnectionState.Open;
        }

        public static bool CloseConnection()
        {
            _connection.Close();
            return _connection.State == ConnectionState.Closed;
        }

        public static bool ExecuteSP(string spName, SqlParameter[] parameters, out string json)
        {
            json = String.Empty;
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                using (SqlCommand sqlCommand = new SqlCommand(spName, _connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddRange(parameters);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        json = ToJson(reader);
                        return true;
                    }
                }
            }
            return false;
        }

        private static string ToJson(this SqlDataReader reader)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartArray();

                while (reader.Read())
                {
                    jsonWriter.WriteStartObject();

                    int fields = reader.FieldCount;

                    for (int i = 0; i < fields; i++)
                    {
                        jsonWriter.WritePropertyName(reader.GetName(i));
                        jsonWriter.WriteValue(reader[i]);
                    }

                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndArray();

                return sw.ToString();
            }
        }
    }
}
