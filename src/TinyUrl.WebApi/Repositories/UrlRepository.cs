using Microsoft.EntityFrameworkCore;

using TinyUrl.WebApi.Data;
using TinyUrl.WebApi.Models;


namespace TinyUrl.WebApi.Repositories;

public class UrlRepository : IUrlRepository
{
	private readonly UrlContext _context;

	public UrlRepository(UrlContext context) => _context = context;

	public async Task<UrlModel?> GetByFullUrl(string url) => await _context.Urls.FirstOrDefaultAsync(x => x.FullAddress == url);

	public async Task<UrlModel?> GetByShortUrl(string url) => await _context.Urls.FirstOrDefaultAsync(x => x.ShortAddress == url);

	public async Task CreateUrl(UrlModel url)
	{
		await _context.AddAsync(url);
		await _context.SaveChangesAsync();
	}
}