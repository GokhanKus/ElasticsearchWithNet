using ElasticSearch.API.DTOs;
using System.Text.Json.Serialization;

namespace ElasticSearch.API.Models
{
	public class Product
	{
		public string Id { get; set; } = null!;
		public string Name { get; set; } = null!;
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Updated { get; set; }
		public ProductFeature? Feature { get; set; }

		//auto mapper kullanmak yerine bu sekilde farklı yaklasimlar da olabilir DDD...
		public ProductDto CreateDto()
		{
			return Feature is null ?
				new ProductDto(Id, Name, Price, Stock, null) :
				new ProductDto(Id, Name, Price, Stock, new ProductFeatureDto(Feature.Width, Feature.Height, Feature.Color.ToString() ?? ""));
		}
	}
}
