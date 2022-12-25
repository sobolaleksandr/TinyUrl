using TinyUrl.WebApi.Models;


namespace TinyUrl.WebApi.Repositories;

public interface IUrlRepository
{
	Task<UrlModel?> GetByUrl(string url);

	Task CreateUrl(UrlModel url);
}