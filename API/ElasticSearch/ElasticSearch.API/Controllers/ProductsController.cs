using ElasticSearch.API.DTOs;
using ElasticSearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController(ProductService _productService) : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> SaveAsync(ProductCreateDto request)
		{
			var product = await _productService.SaveChangesAsync(request);
			return Ok(product);
		}
	}
}
