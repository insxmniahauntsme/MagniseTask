using MagniseTask.Data;

namespace MagniseTask.Services;

public class AuthDataManager
{
	private AuthData AuthData { get; set; }

	public AuthData GetAuthData()
	{
		return AuthData;
	}

	public void SetAuthData(AuthData authData)
	{
		AuthData = authData;
	}
}