﻿using BlogApp.Web.Models;
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
		public async Task<List<Blog>> SearchAsync(string searchText)
		{
			var filteredBlogList = await _blogRepository.SearchAsync(searchText);
			return filteredBlogList;
		}
	}
}
