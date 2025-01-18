using BlogApp.Web.Services;
using BlogApp.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Web.Controllers
{
	public class BlogController(BlogService _blogService) : Controller
	{
		public IActionResult Save()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SaveAsync(BlogCreateViewModel model)
		{
			var isSuccess = await _blogService.SaveIndexAsync(model);

			if (!isSuccess)
			{
				TempData["result"] = "kayit basarisiz";
				return RedirectToAction(nameof(Save));
			}
			TempData["result"] = "kayit basarili";
			return RedirectToAction(nameof(Save));
		}
	}
}
