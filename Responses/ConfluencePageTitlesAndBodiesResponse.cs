using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Confluence.Server.ApiClient.Net;

public class ConfluencePageTitleAndBody
{
    public string Title { get; set; } = string.Empty;
    public string BodyHtml { get; set; } = string.Empty;
}

public class ConfluencePageTitlesAndBodiesResponse
{
    public List<ConfluencePageTitleAndBody> TitleAndBodyList { get; set; } = new();
}