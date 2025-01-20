using System.ComponentModel.DataAnnotations;

namespace BlogApp.Web.ViewModel
{
	public class ECommerceSearchViewModel
	{
		[Display(Name = "Category")]
		public string? Category { get; set; }

		[Display(Name = "Gender")]
		public string? Gender { get; set; }

		[Display(Name = "Order Date (Start)")]
		[DataType(DataType.Date)]
		public DateTime? OrderDateStart { get; set; }

		[Display(Name = "Order Date (End)")]
		[DataType(DataType.Date)]
		public DateTime? OrderDateEnd { get; set; }

		[Display(Name = "CustomerFullName")]
		public string CustomerFullName { get; set; } = null!;
	}
}
