namespace AzureAppToDeploy
{
    public class MessageEntity
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public DateTime? InsertionDate { get; set; }
        public int? ProcessValue { get; set; }
        public string? ProcessResult { get; set; }
    }
}
