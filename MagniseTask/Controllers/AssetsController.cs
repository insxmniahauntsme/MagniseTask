using MagniseTask.Data;
using MagniseTask.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagniseTask.Controllers;

[Route("api/assets")]
public class AssetsController : Controller
{
	private readonly IAssetsRepository _assetsRepository;

	public AssetsController(IAssetsRepository assetsRepository)
	{
		_assetsRepository = assetsRepository;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Asset>>> GetSupportedAssets()
	{
		var supportedAssets = await _assetsRepository.GetSupportedAssets();
		if (supportedAssets == Array.Empty<Asset>())
		{
			return Ok(supportedAssets);
		}
		return Ok(supportedAssets);
	}

	[HttpGet("{symbol}")]
	public Task<ActionResult<Asset>> GetAssetPrice(string symbol)
	{
		throw new NotImplementedException();
	}
}