using MagniseTask.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MagniseTask.Controllers;

[Route("api/authorization")]
public class AuthController : Controller
{
	private readonly IFintachartsAuthService _fintachartsAuthService;

	public AuthController(IFintachartsAuthService fintachartsAuthService)
	{
		_fintachartsAuthService = fintachartsAuthService;
	}
	
	[HttpGet]
	public async Task<ActionResult<string>> Authenticate()
	{
		var token = await _fintachartsAuthService.AuthenticateAsync();
		return Ok(token);
	}
	
}