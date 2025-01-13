using Elastic.Clients.Elasticsearch;
using ElasticSearch.API.DTOs;
using ElasticSearch.API.Repositories;
using System.Net;

namespace ElasticSearch.API.Services
{
	public class ProductService(ProductRepository _productRepository,ILogger<ProductService> _logger)
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
		public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto productDto)
		{
			var isSuccess = await _productRepository.UpdateAsync(productDto);

			return isSuccess == true ?
				ResponseDto<bool>.Success(isSuccess, HttpStatusCode.NoContent) :
				ResponseDto<bool>.Fail("an error occured during update", HttpStatusCode.InternalServerError);
		}
		public async Task<ResponseDto<DeleteResponse>> DeleteAsync(string id)
		{
			var deleteResponse = await _productRepository.DeleteAsync(id);

			if(!deleteResponse.IsValidResponse && deleteResponse.Result == Result.NotFound)
				return ResponseDto<DeleteResponse>.Fail("no product found", HttpStatusCode.NotFound);
			
			if (!deleteResponse.IsValidResponse)
			{
				_logger.LogError(deleteResponse.ElasticsearchServerError?.Error?.Reason ?? "Unknown Error");

				return ResponseDto<DeleteResponse>.Fail("an error occured", HttpStatusCode.InternalServerError);
			}

			return ResponseDto<DeleteResponse>.Success(deleteResponse, HttpStatusCode.NoContent);
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
