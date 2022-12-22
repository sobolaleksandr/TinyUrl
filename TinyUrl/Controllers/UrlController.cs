using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TinyUrl.WebApi.Models;


namespace TinyUrl.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlController : ControllerBase
{
	private readonly UrlContext _context;

	public UrlController(UrlContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<ActionResult<Url>> GetFullUrl(string shortUrl)
	{
		Url? fullUrl = await _context.Urls.FirstOrDefaultAsync(x => x.ShortAddress == shortUrl);
		if (fullUrl == null)
		{
			var notFoundError = new RestApiError
			{
				Message = "Объект не найден."
			};

			return NotFound(notFoundError);
		}
		return fullUrl;
	}
}