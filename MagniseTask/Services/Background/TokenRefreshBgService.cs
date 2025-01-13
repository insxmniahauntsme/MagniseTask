using MagniseTask.Interfaces;

namespace MagniseTask.Services.Background;

public class TokenRefreshBgService : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;

	public TokenRefreshBgService(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var authService = scope.ServiceProvider.GetRequiredService<IFintachartsAuthService>();
				await authService.EnsureAuthenticatedAsync();
				Console.WriteLine("TokenRefreshBgService is running.");
			}

			await Task.Delay(TimeSpan.FromMinutes(29), stoppingToken);
		}
	}
}