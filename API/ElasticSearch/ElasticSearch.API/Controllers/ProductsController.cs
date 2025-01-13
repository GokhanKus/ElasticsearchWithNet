using ElasticSearch.API.DTOs;
using ElasticSearch.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers
{
	public class ProductsController(ProductService _productService) : BaseController
	{
		[HttpPost]
		public async Task<IActionResult> SaveAsync(ProductCreateDto request)
		{
			var product = await _productService.SaveChangesAsync(request);
			return CreateActionResult(product);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateAsync(ProductUpdateDto request)
		{
			var product = await _productService.UpdateAsync(request);
			return CreateActionResult(product);
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAsync(string id)
		{
			var product = await _productService.DeleteAsync(id);
			return CreateActionResult(product);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var productList = await _productService.GetAllAsync();
			return CreateActionResult(productList);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetByIdAsync(string id)
		{
			var productDto = await _productService.GetByIdAsync(id);
			return CreateActionResult(productDto);
		}
	}
}
