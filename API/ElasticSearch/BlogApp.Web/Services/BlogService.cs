using BlogApp.Web.Models;
using BlogApp.Web.Repositories;
using BlogApp.Web.ViewModel;
using System.Reflection.Metadata;

namespace BlogApp.Web.Services
{
	public class BlogService(BlogRepository _blogRepository)
	{
		public async Task<bool> SaveIndexAsync(BlogCreateViewModel blogCreateModel)
		{
			var blog = new Blog
			{
				Title = blogCreateModel.Title,
				Content = blogCreateModel.Content,
				Tags = blogCreateModel.Tags.Split(","),
				UserId = Guid.NewGuid(),
			};
			var isCreatedBlog = await _blogRepository.SaveChangesAsync(blog);
			return isCreatedBlog != null;
		}
		public async Task<List<BlogViewModel>> SearchAsync(string searchText)
		{
			var filteredBlogList = await _blogRepository.SearchAsync(searchText);
			var blogViewModels = filteredBlogList.Select(blog => new BlogViewModel
			{
				Id = blog.Id,
				Title = blog.Title,
				Content = blog.Content,
				Created = blog.Created.ToShortDateString(),
				Tags = string.Join(",",blog.Tags),
				UserId = blog.UserId.ToString(),
			}).ToList();

			return blogViewModels;
		}
	}
}
