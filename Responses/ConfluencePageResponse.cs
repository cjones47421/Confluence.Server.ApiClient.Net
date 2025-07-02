using System.Text.Json.Serialization;

namespace ConfluenceApiClient;

public class ConfluencePageResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("body")]
    public ConfluenceBody Body { get; set; } = new();

    [JsonPropertyName("version")]
    public object Version { get; set; } = new(); // You can create a more specific class if needed

    [JsonPropertyName("_links")]
    public ConfluenceLinks Links { get; set; } = new();

    [JsonPropertyName("_expandable")]
    public ConfluenceExpandable Expandable { get; set; } = new();
}