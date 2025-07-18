using Refit;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Confluence.Server.ApiClient.Net;

public class ConfluenceApiClient : IConfluenceApiClient
{
    private readonly IConfluenceApiClient _api;
    public HttpClient HttpClient { get; }

    public ConfluenceApiClient(ConfluenceApiConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.BaseUrl))
            throw new ArgumentNullException(nameof(config.BaseUrl));
        if (string.IsNullOrWhiteSpace(config.ApiToken))
            throw new ArgumentNullException(nameof(config.ApiToken));
        HttpClient = new HttpClient
        {
            BaseAddress = new System.Uri(config.BaseUrl)
        };
        HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config.ApiToken);
        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _api = RestService.For<IConfluenceApiClient>(HttpClient);
    }

    public async Task<ApiResponse<ConfluenceSpacesResponse>> GetSpacesAsync(int start, int limit) => await _api.GetSpacesAsync(start, limit);

    public async Task<ApiResponse<ConfluenceSearchResponse>> SearchContentAsync(string cql) => await _api.SearchContentAsync(cql);

    public async Task<ApiResponse<ConfluencePagesResponse>> GetPagesAsync(string spaceKey, int start = 0, int limit = 100) => await _api.GetPagesAsync(spaceKey, start, limit);

    public async Task<ApiResponse<ConfluencePagesResponse>> GetPagesMetadataAsync(string spaceKey, int start = 0, int limit = 100) => await _api.GetPagesMetadataAsync(spaceKey, start, limit);

    public async Task<ApiResponse<ConfluencePageResponse>> GetPageByIdAsync(string id)
    {
        return await _api.GetPageByIdAsync(id);
    }

    public async Task<ApiResponse<ConfluencePagesResponse>> GetChildPagesAsync(string id, int start = 0, int limit = 100)
    {
        return await _api.GetChildPagesAsync(id, start, limit);
    }

    public async Task<ApiResponse<ConfluenceSearchResponse>> SearchContentWithViewAsync(string cql, int limit = 100, int start = 0)
    {
        return await _api.SearchContentWithViewAsync(cql, limit, start);
    }

    public async Task<ApiResponse<ConfluenceSearchResponse>> SearchPagesWithViewAsync(string cql, int limit = 100, int start = 0)
    {
        return await _api.SearchPagesWithViewAsync(cql, limit, start);
    }

    public async Task<ConfluenceApiClientResult<ConfluencePageTitlesAndBodiesResponse>> GetConfluencePageTitleAndBodyListAsync(string spaceKey, int start = 0, int limit = 100)
    {
        var allPages = new List<ConfluencePageTitleAndBody>();
        int currentStart = start;
        bool hasNext = true;
        ApiException? lastError = null;
        System.Net.HttpStatusCode lastStatusCode = System.Net.HttpStatusCode.OK;

        while (hasNext)
        {
            var response = await GetPagesAsync(spaceKey, currentStart, limit);

            lastStatusCode = response.StatusCode;
            lastError = response.Error;

            if (response.IsSuccessful && response.Content != null)
            {
                var pageBatch = response.Content.Results
                    .Select(r => new ConfluencePageTitleAndBody
                    {
                        Title = r.Title,
                        BodyHtml = r.Body.Storage.Value
                    })
                    .ToList();

                allPages.AddRange(pageBatch);

                // Check for next page using the _links.next property
                if (!string.IsNullOrEmpty(response.Content.Links?.Next))
                {
                    currentStart += limit;
                }
                else
                {
                    hasNext = false;
                }
            }
            else
            {
                // If any batch fails, return what we have with the error
                return new ConfluenceApiClientResult<ConfluencePageTitlesAndBodiesResponse>
                {
                    HttpStatusCode = lastStatusCode,
                    Error = lastError,
                    Result = new ConfluencePageTitlesAndBodiesResponse
                    {
                        TitleAndBodyList = allPages
                    }
                };
            }
        }

        return new ConfluenceApiClientResult<ConfluencePageTitlesAndBodiesResponse>
        {
            HttpStatusCode = lastStatusCode,
            Error = lastError,
            Result = new ConfluencePageTitlesAndBodiesResponse
            {
                TitleAndBodyList = allPages
            }
        };
    }

    public async Task<ConfluenceApiClientResult<ConfluencePageTitlesAndBodiesResponse>> GetConfluencePageTitleAndBodyListByIdsAsync(List<string> pageIds)
    {
        var allPages = new List<ConfluencePageTitleAndBody>();
        ApiException? lastError = null;
        System.Net.HttpStatusCode lastStatusCode = System.Net.HttpStatusCode.OK;

        foreach (var id in pageIds)
        {
            var response = await _api.GetPageByIdAsync(id);
            lastStatusCode = response.StatusCode;
            lastError = response.Error;

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                allPages.Add(new ConfluencePageTitleAndBody
                {
                    Title = response.Content.Title,
                    BodyHtml = response.Content.Body?.Storage?.Value ?? string.Empty
                });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                continue; // Skip pages that are not found
            }
            else
            {
                // If any batch fails, return what we have with the error
                return new ConfluenceApiClientResult<ConfluencePageTitlesAndBodiesResponse>
                {
                    HttpStatusCode = lastStatusCode,
                    Error = lastError,
                    Result = new ConfluencePageTitlesAndBodiesResponse
                    {
                        TitleAndBodyList = allPages
                    }
                };
            }
        }

        return new ConfluenceApiClientResult<ConfluencePageTitlesAndBodiesResponse>
        {
            HttpStatusCode = lastStatusCode,
            Error = lastError,
            Result = new ConfluencePageTitlesAndBodiesResponse
            {
                TitleAndBodyList = allPages
            }
        };
    }

    /// <summary>
    /// Fetches HTML for the parent page and all its descendants in one or more paged calls.
    /// </summary>
    public async Task<Dictionary<string, string>> GetAllHtmlAsync(string parentId, string spaceKey)
    {
        var pages = new Dictionary<string, string>();
        int totalFetched = 0;

        // Build CQL including space filter
        var cqlRaw = $"(id={parentId} OR ancestor={parentId}) AND type=page AND space=\"{spaceKey}\"";
        const int pageSize = 100;

        do
        {
            var response = await SearchContentWithViewAsync(cqlRaw, pageSize, totalFetched);

            if (!response.IsSuccessStatusCode || response.Content == null)
            {
                break;
            }

            foreach (var result in response.Content.Results)
            {
                if (!string.IsNullOrEmpty(result.Id))
                {
                    var html = result.Body?.View?.Value ?? "";
                    pages[result.Id] = html;
                }
            }

            totalFetched += response.Content.Results.Count;

            // Check if we've fetched all pages
            if (response.Content.Results.Count < pageSize ||
                !string.IsNullOrEmpty(response.Content.Links?.Next) == false)
            {
                break;
            }
        }
        while (true);

        return pages;
    }

    /// <summary>
    /// Returns the total number of pages in <spaceKey> under parentId (incl. the parent itself).
    /// </summary>
    public async Task<int> GetPageCountAsync(string parentId, string spaceKey)
    {
        var rawCql = $"space=\"{spaceKey}\" AND (id={parentId} OR ancestor={parentId}) AND type=page";
        var response = await _api.GetConfluenceSearhLimitZero(rawCql);
        if (response.IsSuccessStatusCode && response.Content != null)
        {
            // totalSize is the total matching pages count
            return response.Content.Size;
        }
        return 0;
    }

    /// <summary>
    /// Fetches HTML for the parent page and all its descendants in one or more paged calls.
    /// </summary>
    public async Task<List<ConfluencePageInfo>> GetAllPageHtmlAsync(string parentId, string spaceKey, DateTime? since = null)
    {
        var pages = new List<ConfluencePageInfo>();
        int totalFetched = 0;
        const int pageSize = 100;

        // Build the base CQL for parent/ancestor, type and space
        var cqlParts = new List<string>
        {
            $"space=\"{spaceKey}\"",
            $"(id={parentId} OR ancestor={parentId})",
            "type=page"
        };

        // Only add lastmodified filter if 'since' has a value
        if (since.HasValue)
        {
            // Format as UTC ISO8601 for CQL
            var sinceStr = since.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss") + "Z";
            cqlParts.Add($"lastmodified > \"{sinceStr}\"");
        }

        var cqlRaw = string.Join(" AND ", cqlParts);

        do
        {
            var response = await _api.SearchPagesWithViewAsync(cqlRaw, pageSize, totalFetched);
            if (!response.IsSuccessStatusCode || response.Content == null)
                break;

            foreach (var result in response.Content.Results)
            {
                var lastModified = DateTime.MinValue;
                if (result.Version is not null)
                {
                    var versionObj = result.Version as System.Text.Json.JsonElement?;
                    if (versionObj.HasValue && versionObj.Value.TryGetProperty("when", out var whenProp))
                    {
                        var whenStr = whenProp.GetString();
                        if (!string.IsNullOrEmpty(whenStr))
                        {
                            DateTime.TryParse(whenStr, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out lastModified);
                        }
                    }
                }
                if (since.HasValue && lastModified <= since.Value)
                    continue;

                pages.Add(new ConfluencePageInfo
                {
                    Id = result.Id,
                    Title = result.Title,
                    LastModified = lastModified,
                    Html = result.Body?.View?.Value ?? ""
                });
            }

            totalFetched += response.Content.Results.Count;
            if (response.Content.Results.Count < pageSize)
                break;
        }
        while (true);

        return pages;
    }

    public async Task<ApiResponse<ConfluenceSearchResponse>> GetConfluenceSearhLimitZero(string cql)
    {
        return await _api.GetConfluenceSearhLimitZero(cql);
    }
}