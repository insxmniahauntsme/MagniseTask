using System.Net.WebSockets;
using System.Text;
using MagniseTask.Data;
using MagniseTask.Interfaces;

namespace MagniseTask.Services;

public class PriceUpdateService : IPriceUpdateService
{
	private readonly MarketDbContext _context;
	private readonly IConfiguration _configuration;

	public PriceUpdateService(MarketDbContext context, IConfiguration configuration)
	{
		_context = context;
		_configuration = configuration;
	}

	public async Task ConnectWebSocketAsync()
	{
		var uri = _configuration.GetSection("FintachartsAPI").GetValue<string>("WSS_Uri");
		using var client = new ClientWebSocket();
		await client.ConnectAsync(new Uri(uri!), CancellationToken.None);
		
		var buffer = new byte[1024 * 4];
		while (client.State == WebSocketState.Open)
		{
			var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
			Console.WriteLine($"Received message: {message}");
		}
	}
	
	
}