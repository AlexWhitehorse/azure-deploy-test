namespace AzureAppToDeploy
{
    public interface IConfig
    {
        string DatabaseConnectionString { get; }
        string AzureStorageConnectionString { get; }
    }

    public class Configuration : IConfig
    {
        public string DatabaseConnectionString { get; }
        public string AzureStorageConnectionString { get; }

        public Configuration(string databaseConnectionString, string azureStorageConnectionString)
        {
            DatabaseConnectionString = databaseConnectionString;
            AzureStorageConnectionString = azureStorageConnectionString;
        }
    }
}
