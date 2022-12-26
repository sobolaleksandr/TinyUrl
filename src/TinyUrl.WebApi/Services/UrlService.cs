using IronBarCode;

using Microsoft.Extensions.Options;

using TinyUrl.WebApi.Config;
using TinyUrl.WebApi.Models;
using TinyUrl.WebApi.Repositories;


namespace TinyUrl.WebApi.Services;

public class UrlService : IUrlService
{
	private static readonly Random _random = new();
	private readonly IWebHostEnvironment _appEnvironment;
	private readonly ServiceConfig _config;
	private readonly IUrlRepository _urlRepository;

	public UrlService(IOptions<ServiceConfig> options, IUrlRepository urlRepository, IWebHostEnvironment appEnvironment)
	{
		_urlRepository = urlRepository;
		_appEnvironment = appEnvironment;
		_config = options.Value;
	}

	public async Task<UrlModel> CreateUrl(string url)
	{
		UrlModel newUrl = await CreateShortUrl(url);
		await _urlRepository.CreateUrl(newUrl);

		return newUrl;
	}

	public async Task<UrlModel?> GetByFullUrl(string url) => await _urlRepository.GetByFullUrl(url);

	public async Task<UrlModel?> GetByShortUrl(string url) => await _urlRepository.GetByShortUrl(url);

	private string CreateQrCode(string shortUrl, string randomString)
	{
		GeneratedBarcode qrCode = QRCodeWriter.CreateQrCode(shortUrl);
		string imagePath = @$"{_appEnvironment.WebRootPath}\Images\{randomString}.jpeg";
		qrCode.SaveAsJpeg(imagePath);

		return imagePath;
	}

	private async Task<UrlModel> CreateShortUrl(string fullUrl)
	{
		while (true)
		{
			string randomString = RandomString(_config.UrlLength);
			string shortUrl = $"{_config.ServerUrl}?u={randomString}";
			UrlModel? url = await _urlRepository.GetByFullUrl(shortUrl);
			if (url == null)
				return new UrlModel
				{
					FullAddress = fullUrl,
					ShortAddress = shortUrl,
					QrCodePath = CreateQrCode(shortUrl, randomString)
				};
		}
	}

	private static string RandomString(int length)
	{
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[_random.Next(s.Length)]).ToArray());
	}
}