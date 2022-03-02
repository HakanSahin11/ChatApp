namespace Chat_Application.Models
{
    public class DBElements : IDBElements
    {
        public string ConnectionString { get; set; } = "Default Connection";
        public string Database { get; set; } = "Default DB";
    }
    public interface IDBElements
    {
        string ConnectionString { get; set; }
        string Database { get; set; }
    }
}
