# Confluence.Server.ApiClient.Net

A .NET 8+ client library for interacting with Atlassian Confluence Server's REST API. This package is designed for Confluence Server (not Confluence Cloud) and provides strongly-typed access to spaces, pages, and search endpoints, including support for Confluence Query Language (CQL).

## Features
- Authenticate using API tokens
- Retrieve spaces and pages
- Search content using CQL
- Fetch page bodies and metadata
- Strongly-typed response models

## Requirements
- .NET 8 or later
- Confluence Server instance with REST API enabled

## Installation
Install via NuGet Package Manager:
Install-Package Confluence.Server.ApiClient.Net
Or via .NET CLI:
dotnet add package Confluence.Server.ApiClient.Net
## Usage

### 1. Configure the clientusing ConfluenceApiClient;

var config = new ConfluenceApiConfig
{
    BaseUrl = "https://your-confluence-server/rest/api/",
    ApiToken = "YOUR_API_TOKEN"
};

var client = new ConfluenceApiClient(config);
### 2. List spacesvar spacesResponse = await client.GetSpacesAsync(start: 0, limit: 10);
if (spacesResponse.IsSuccessStatusCode)
{
    foreach (var space in spacesResponse.Content.Results)
    {
        Console.WriteLine($"Space: {space.Key} - {space.Name}");
    }
}
### 3. Search content using CQLstring cql = "type=page AND space=DEV";
var searchResponse = await client.SearchContentAsync(cql);
if (searchResponse.IsSuccessStatusCode)
{
    foreach (var page in searchResponse.Content.Results)
    {
        Console.WriteLine($"Page: {page.Title}");
    }
}
### 4. Get page by IDvar pageResponse = await client.GetPageByIdAsync("123456");
if (pageResponse.IsSuccessStatusCode)
{
    Console.WriteLine(pageResponse.Content.Title);
    Console.WriteLine(pageResponse.Content.Body.Storage.Value); // HTML content
}
### 5. Get all page titles and bodies in a spacevar result = await client.GetConfluencePageTitleAndBodyListAsync("SPACEKEY");
if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
{
    foreach (var item in result.Result.TitleAndBodyList)
    {
        Console.WriteLine($"Title: {item.Title}\nBody: {item.BodyHtml}");
    }
}
## Confluence Query Language (CQL)
This client supports CQL for advanced searching. See [CONFLUENCE_CQL.md](EA.Confluence.API.Client/CONFLUENCE_CQL.md) for a quick reference and examples.

## Contributing
Contributions are welcome! Please open issues or submit pull requests for improvements and bug fixes.

## License
[MIT](LICENSE)

---

> This package is not affiliated with Atlassian. For Confluence Cloud, use the official Atlassian SDKs.
