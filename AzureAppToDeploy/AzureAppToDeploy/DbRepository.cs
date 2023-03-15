using System.Data.SqlClient;

namespace AzureAppToDeploy
{
    public interface IDbRepository
    {
        List<MessageEntity> GetAllMessagesFromDb();
        void CreateMessage(string message);
        int GetLastMessageId();
    }

    public class DbRepository : IDbRepository
    {
        IConfig _config;

        public DbRepository(IConfig config)
        {
            _config = config;
        }

        public List<MessageEntity> GetAllMessagesFromDb()
        {
            var result = new List<MessageEntity>();
            string querry = "select id, msg, insertionDate, processValue, processResult from message_log order by insertionDate desc";


            using (SqlConnection connection = new SqlConnection(_config.DatabaseConnectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(querry, connection);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var res = new MessageEntity() 
                        { 
                            Id = (int)reader[0],
                            Message = (string)reader[1],
                            InsertionDate = (DateTime)reader[2],
                            ProcessValue = reader[3].GetType() == typeof(System.DBNull) ? null : (int)reader[3],
                            ProcessResult = reader[4].GetType() == typeof(System.DBNull) ? null : (string)reader[4]
                        };
                        result.Add(res);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public int GetLastMessageId()
        {
            int result = 0;
            string querry = "select top 1 id from message_log order by id desc";


            using (SqlConnection connection = new SqlConnection(_config.DatabaseConnectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(querry, connection);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result = (int)reader[0];
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public void CreateMessage(string message)
        {
            string sqlExpression = $"insert into message_log (msg, insertionDate) values ('{message}', getdate())";

            using (SqlConnection connection = new SqlConnection(_config.DatabaseConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
            }
        }
    }
}
