using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Confluence.Server.ApiClient.Net;

public class ConfluenceSpacesResponse
{
    [JsonPropertyName("results")]
    public List<ConfluenceSpace> Results { get; set; } = new();

    [JsonPropertyName("start")]
    public int Start { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("_links")]
    public ConfluenceLinks Links { get; set; } = new();
}

public class ConfluenceSpace
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("_links")]
    public ConfluenceLinks Links { get; set; } = new();

    [JsonPropertyName("_expandable")]
    public ConfluenceExpandable Expandable { get; set; } = new();
}

public class ConfluenceLinks
{
    [JsonPropertyName("webui")]
    public string Webui { get; set; } = string.Empty;

    [JsonPropertyName("self")]
    public string Self { get; set; } = string.Empty;

    [JsonPropertyName("next")]
    public string Next { get; set; } = string.Empty;

    [JsonPropertyName("base")]
    public string Base { get; set; } = string.Empty;

    [JsonPropertyName("context")]
    public string Context { get; set; } = string.Empty;
}

public class ConfluenceExpandable
{
    [JsonPropertyName("metadata")]
    public string Metadata { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("retentionPolicy")]
    public string RetentionPolicy { get; set; } = string.Empty;

    [JsonPropertyName("homepage")]
    public string Homepage { get; set; } = string.Empty;
}