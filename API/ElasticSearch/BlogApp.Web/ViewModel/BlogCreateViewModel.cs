using BlogApp.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogApp.Web.ViewModel
{
	public class BlogCreateViewModel
	{
		[Required]
		public string Title { get; set; } = null!;

		[Required]
		public string Content { get; set; } = null!;
		public List<string> Tags { get; set; } = new();
	}
}
