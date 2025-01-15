using AutoMapper;
using MagniseTask.Data;
using MagniseTask.DTOs;
using MagniseTask.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagniseTask.Controllers;

[Route("api/assets")]
public class AssetsController : Controller
{
	private readonly IAssetsRepository _assetsRepository;
	private readonly IFintachartsDataService _fintachartsDataService;
	private readonly IMapper _mapper;
	public AssetsController(IAssetsRepository assetsRepository, IFintachartsDataService fintachartsDataService, IMapper mapper)
	{
		_assetsRepository = assetsRepository;
		_fintachartsDataService = fintachartsDataService;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Asset>>> GetSupportedAssets()
	{
		var supportedAssets = await _assetsRepository.GetSupportedAssets();
    
		if (supportedAssets == null || !supportedAssets.Any())
		{
			var authHeaderValue = Request.Headers["Authorization"].ToString();

			if (string.IsNullOrEmpty(authHeaderValue) || !authHeaderValue.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
			{
				return Unauthorized("Authorization token is missing or invalid.");
			}

			var token = authHeaderValue.Substring("Bearer ".Length).Trim();

			var assets = await _fintachartsDataService.GetAllAssets(token);

			await _assetsRepository.AddAssets(assets);

			return Ok(_mapper.Map<IEnumerable<AssetDto>>(assets));
		}

		return Ok(supportedAssets);
	}

	[HttpGet("{symbol}")]
	public Task<ActionResult<Asset>> GetAssetPrice(string symbol)
	{
		throw new NotImplementedException();
	}
}