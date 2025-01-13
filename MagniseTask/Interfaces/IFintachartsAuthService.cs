using MagniseTask.Data;

namespace MagniseTask.Interfaces;

public interface IFintachartsAuthService
{
	Task EnsureAuthenticatedAsync();
	Task AuthenticateAsync();
	
}