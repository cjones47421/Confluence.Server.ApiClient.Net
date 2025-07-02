using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using System.Collections.Generic;

namespace ConfluenceApiClient;

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
}