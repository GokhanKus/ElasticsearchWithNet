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
		public async Task<IActionResult> TermQuery(string customerFirstName)
		{
			var results = await _eCommerceRepository.TermQuery(customerFirstName);
			return Ok(results);
		}
	}
}
