using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Confluence.Server.ApiClient.Net;

public class ConfluenceSearchResponse
{
    [JsonPropertyName("results")]
    public List<ConfluencePageResponse> Results { get; set; } = new();

    [JsonPropertyName("start")]
    public int Start { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("_links")]
    public ConfluenceLinks Links { get; set; } = new();
}