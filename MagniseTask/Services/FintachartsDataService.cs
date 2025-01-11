using MagniseTask.Data;

namespace MagniseTask.Services;

public class FintachartsDataService
{
	private HttpClient _httpClient;

	public FintachartsDataService(IHttpClientFactory httpClientFactory)
	{
		_httpClient = httpClientFactory.CreateClient("FintachartsClient");
	}

	public Task<IEnumerable<Asset>> GetAllAssets(string accessToken)
	{
		throw new NotImplementedException();
	}
}