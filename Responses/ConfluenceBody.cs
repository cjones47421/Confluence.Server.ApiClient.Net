using System.Text.Json.Serialization;

namespace Confluence.Server.ApiClient.Net;

public class ConfluenceBody
{
    [JsonPropertyName("storage")]
    public ConfluenceBodyStorage Storage { get; set; } = new();

    [JsonPropertyName("view")]
    public ConfluenceBodyView View { get; set; } = new();

    [JsonPropertyName("_expandable")]
    public ConfluenceBodyExpandable Expandable { get; set; } = new();
}

public class ConfluenceBodyStorage
{
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("representation")]
    public string Representation { get; set; } = string.Empty;

    [JsonPropertyName("_expandable")]
    public ConfluenceBodyStorageExpandable Expandable { get; set; } = new();
}

public class ConfluenceBodyView
{
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("representation")]
    public string Representation { get; set; } = string.Empty;

    [JsonPropertyName("_expandable")]
    public ConfluenceBodyViewExpandable Expandable { get; set; } = new();
}

public class ConfluenceBodyStorageExpandable
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

public class ConfluenceBodyViewExpandable
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

public class ConfluenceBodyExpandable
{
    [JsonPropertyName("editor")]
    public string Editor { get; set; } = string.Empty;

    [JsonPropertyName("view")]
    public string View { get; set; } = string.Empty;

    [JsonPropertyName("export_view")]
    public string ExportView { get; set; } = string.Empty;

    [JsonPropertyName("styled_view")]
    public string StyledView { get; set; } = string.Empty;

    [JsonPropertyName("anonymous_export_view")]
    public string AnonymousExportView { get; set; } = string.Empty;
}