using ElasticSearch.API.DTOs;
using ElasticSearch.API.Repositories;
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
	}
}
