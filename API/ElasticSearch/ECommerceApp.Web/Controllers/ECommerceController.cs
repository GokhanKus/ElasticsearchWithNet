using ECommerceApp.Web.Services;
using ECommerceApp.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Web.Controllers
{
	public class ECommerceController(ECommerceService _eCommerceService) : Controller
	{
		public async Task<IActionResult> Search([FromQuery] SearchPageViewModel searchPageView)
		{
			var (eCommerceList,totalItemCount, pageLinkCount) 
				= await _eCommerceService.SearchAsync(searchPageView.SearchViewModel, searchPageView.Page, searchPageView.PageSize);

			searchPageView.List = eCommerceList;
			searchPageView.TotalItemCount = totalItemCount;
			searchPageView.PageLinkCount = pageLinkCount;

			return View(searchPageView);
		}
	}
}
