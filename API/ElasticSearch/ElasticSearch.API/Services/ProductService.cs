using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models;
using ElasticSearch.API.Repositories;
using System.Collections.Immutable;
using System.Net;

namespace ElasticSearch.API.Services
{
	public class ProductService(ProductRepository _productRepository)
	{
		public async Task<ResponseDto<ProductDto>> SaveChangesAsync(ProductCreateDto request)
		{
			var product = request.CreateProduct();
			var response = await _productRepository.SaveChangesAsync(product);

			if (response is null)
				return ResponseDto<ProductDto>.Fail("an error occured", HttpStatusCode.InternalServerError);

			var productDto = response.CreateDto();
			return ResponseDto<ProductDto>.Success(productDto, HttpStatusCode.Created);
		}
		public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
		{
			var products = await _productRepository.GetAllAsync();

			var productListDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock,
				new ProductFeatureDto(p.Feature?.Width, p.Feature?.Height, p.Feature?.Color.ToString() ?? ""))).ToList();

			return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);
		}
		public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
		{
			var product = await _productRepository.GetByIdAsync(id);

			if (product is null)
				return ResponseDto<ProductDto>.Fail("no product found", HttpStatusCode.NotFound);

			var productDto = product.CreateDto();

			return ResponseDto<ProductDto>.Success(productDto, HttpStatusCode.OK);
		}
	}
}
