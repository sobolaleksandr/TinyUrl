using TinyUrl.WebApi.Models;


namespace TinyUrl.WebApi.Services;

public interface IUrlService
{
	Task<UrlModel> CreateUrl(string url);

	Task<UrlModel?> GetByUrl(string url);
}