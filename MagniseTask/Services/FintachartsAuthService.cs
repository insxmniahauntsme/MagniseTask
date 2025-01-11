using System.Text.Json;
using MagniseTask.Data;
using MagniseTask.Interfaces;

namespace MagniseTask.Services;

public class FintachartsAuthService : IFintachartsAuthService
{
	private readonly HttpClient _httpClient;
	private readonly IConfiguration _configuration;

	public FintachartsAuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
	{
		_httpClient = httpClientFactory.CreateClient("FintachartsClient");
		_configuration = configuration;
	}

	public async Task<AuthData> AuthenticateAsync()
	{
		var requestBody = new FormUrlEncodedContent(new Dictionary<string, string>
		{
			{ "grant_type", _configuration.GetSection("FintachartsAPI").GetValue<string>("grant_type")! },
			{ "client_id", _configuration.GetSection("FintachartsAPI").GetValue<string>("client_id")! },
			{ "username", _configuration.GetSection("FintachartsAPI").GetValue<string>("username")! },
			{ "password", _configuration.GetSection("FintachartsAPI").GetValue<string>("password")! },
		});
		var response = await _httpClient.PostAsync("/identity/realms/fintatech/protocol/openid-connect/token", requestBody);
		
		response.EnsureSuccessStatusCode();
		
		var content = await response.Content.ReadAsStringAsync();

		Console.WriteLine(content);

		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
		};
		var authResponse = System.Text.Json.JsonSerializer.Deserialize<AuthData>(content, options);

		Console.WriteLine(authResponse!.AccessToken);
		
		return authResponse!;
	}
}