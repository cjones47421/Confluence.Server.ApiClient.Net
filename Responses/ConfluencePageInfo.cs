namespace Confluence.Server.ApiClient.Net;

public class ConfluencePageInfo
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime LastModified { get; set; }
    public string Html { get; set; } = string.Empty;
}