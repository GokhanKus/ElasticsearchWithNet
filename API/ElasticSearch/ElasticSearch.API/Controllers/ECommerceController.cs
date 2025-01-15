using ElasticSearch.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ECommerceController(ECommerceRepository _eCommerceRepository): ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> TermQueryAsync(string customerFirstName)
		{
			var results = await _eCommerceRepository.TermQueryAsync(customerFirstName);
			return Ok(results);
		}
		
		[HttpPost]
		public async Task<IActionResult> TermsQueryAsync(List<string> customerFirstNameList)
		{
			var results = await _eCommerceRepository.TermsQueryAsync(customerFirstNameList);
			return Ok(results);
		}

		[HttpGet]
		public async Task<IActionResult> PrefixQueryAsync(string customerFullNamePrefix)
		{
			var results = await _eCommerceRepository.PrefixQueryAsync(customerFullNamePrefix);
			return Ok(results);
		}
		
		[HttpGet]
		public async Task<IActionResult> RangeQueryAsync(double minTaxfulTotalPrice, double maxTaxfulTotalPrice)
		{
			var results = await _eCommerceRepository.RangeQueryAsync(minTaxfulTotalPrice, maxTaxfulTotalPrice);
			return Ok(results);
		}
	}
}
