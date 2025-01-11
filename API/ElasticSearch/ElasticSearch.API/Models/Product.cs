using System.Text.Json.Serialization;

namespace ElasticSearch.API.Models
{
	public class Product
	{
		//elastic default olarak Id'yi string atar o yuzden string Id ve bu Id response'da bulunmaz metadata'da bulunur
		[JsonPropertyName("_id")]
		public string Id { get; set; } = null!; 
		public string Name { get; set; } = null!;
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Updated { get; set; }
		public ProductFeature? Feature { get; set; }
	}
}
