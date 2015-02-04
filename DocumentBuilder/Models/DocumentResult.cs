namespace DocumentBuilder.Models
{
    public interface IDocumentResult
    {
        string Path { get; set; }
        int PageCount { get; set; }
    }

    public class DocumentResult : IDocumentResult
    {
        public string Path { get; set; }
        public int PageCount { get; set; }
    }
}