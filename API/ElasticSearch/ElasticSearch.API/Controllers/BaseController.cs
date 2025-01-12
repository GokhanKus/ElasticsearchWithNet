using ElasticSearch.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ElasticSearch.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BaseController : ControllerBase
	{
		[NonAction]//bunlar birer endpoint degil, bunlar yardımcı metotlar, o yuzden swagger endpoint gibi algılamasın
		public IActionResult CreateActionResult<T>(ResponseDto<T> response)
		{
			if (response.StatusCode == HttpStatusCode.NoContent)
				return new ObjectResult(null) { StatusCode = response.StatusCode.GetHashCode() };

			return new ObjectResult(response) { StatusCode = response.StatusCode.GetHashCode() };
		}
	}
}
