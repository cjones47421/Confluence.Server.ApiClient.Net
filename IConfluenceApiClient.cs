using Refit;

namespace ConfluenceApiClient;

public interface IConfluenceApiClient
{
    [Get("/rest/api/space?&start={start}&limit={limit}")]
    Task<ApiResponse<ConfluenceSpacesResponse>> GetSpacesAsync(int start, int limit);

    [Get("/rest/api/content/search?cql={cql}")]
    Task<ApiResponse<ConfluenceSearchResponse>> SearchContentAsync(string cql);

    [Get("/rest/api/content/search?cql=space={spaceKey} AND type=page&expand=title,body.storage&limit={limit}&start={start}")]
    Task<ApiResponse<ConfluencePagesResponse>> GetPagesAsync(string spaceKey, int start = 0, int limit = 100);

    [Get("/rest/api/content/{id}?expand=title,body.storage")]
    Task<ApiResponse<ConfluencePageResponse>> GetPageByIdAsync(string id);

    [Get("/rest/api/content/search?cql=space={spaceKey} AND type=page&expand=metadata&limit={limit}&start={start}")]
    Task<ApiResponse<ConfluencePagesResponse>> GetPagesMetadataAsync(string spaceKey, int start = 0, int limit = 100);

    //The implementation of this method is simplified to return a custom result type and is not use Refit's ApiResponse directly.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Refit", "RF001:Refit types must have Refit HTTP method attributes", Justification = "Utilizes refit on the method level")]
    Task<ConfluenceApiClientResult<ConfluencePageTitlesAndBodiesResponse>> GetConfluencePageTitleAndBodyListAsync(string spaceKey, int start = 0, int limit = 100);

    //The implementation of this method is simplified to return a custom result type and is not use Refit's ApiResponse directly.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Refit", "RF001:Refit types must have Refit HTTP method attributes", Justification = "Utilizes refit on the method level")]
    Task<ConfluenceApiClientResult<ConfluencePageTitlesAndBodiesResponse>> GetConfluencePageTitleAndBodyListByIdsAsync(List<string> pageIds);
}