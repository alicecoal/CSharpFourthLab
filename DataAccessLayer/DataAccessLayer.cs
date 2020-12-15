using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public class DataAccessLayer
    {
        SqlConnection connection;

        public DataAccessLayer(Options.ConnectionOptions options)
        {
            string connectionString = $"Data Source={options.DataSource}; Initial Catalog={options.InitialCatalog}; Integrated Security={options.IntegratedSecurity}";

            using (TransactionScope scope = new TransactionScope())
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                scope.Complete();
            }
        }

        public List<T> GetListItems<T>(string sqlExpression)
        {
            List<T> items = new List<T>();                      

            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            using (TransactionScope scope = new TransactionScope())
            {
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        T item = (T)Activator.CreateInstance(typeof(T));
                        Type type = item.GetType();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object value = reader.GetValue(i);
                            if (value.GetType() == typeof(DBNull))
                            {
                                value = null;
                            }

                            PropertyInfo info = type.GetProperty(reader.GetName(i));                            
                            info.SetValue(item, value);
                        }

                        items.Add(item);
                    }

                    reader.Close();
                }

                scope.Complete();
                return items;
            }
        }

        public void Log(DateTime date, string message)
        {
            SqlCommand command = new SqlCommand("Log", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("date", date));
            command.Parameters.Add(new SqlParameter("message", message));

            using (TransactionScope scope = new TransactionScope())
            {
                command.ExecuteNonQuery();
                scope.Complete();
            }
        }
    }
}
