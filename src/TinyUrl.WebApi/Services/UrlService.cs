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
		License.LicenseKey = "IRONBARCODE.U3AYASAN.5144-E989DDF446-B25XIO4JLZRIYB6-TOTV57ZAFRYS-PQZYOF7NMXAG-4H4KRO5QWKZN-LZ6ZZ74XK6XB-LZZQ5K-T7ZCE43GKPSIUA-DEPLOYMENT.TRIAL-OTIOZG.TRIAL.EXPIRES.24.JAN.2023";
	}

	public async Task<UrlModel> CreateUrl(string url)
	{
		UrlModel newUrl = await CreateShortUrl(url);
		await _urlRepository.CreateUrl(newUrl);

		return newUrl;
	}

	public async Task<UrlModel?> GetByUrl(string url) => await _urlRepository.GetByUrl(url);

	private string CreateQrCode(string url)
	{
		GeneratedBarcode qrCode = QRCodeWriter.CreateQrCode(url);
		string imagePath = @$"{_appEnvironment.WebRootPath}\Images\{url}.jpeg";
		qrCode.SaveAsImage(imagePath);

		return imagePath;
	}

	private async Task<UrlModel> CreateShortUrl(string fullUrl)
	{
		while (true)
		{
			string randomString = RandomString(_config.UrlLength);
			string shortUrl = $"{_config.ServerUrl}?u={randomString}";
			UrlModel? url = await _urlRepository.GetByUrl(shortUrl);
			if (url == null)
				return new UrlModel
				{
					FullAddress = fullUrl,
					ShortAddress = shortUrl,
					QrCodePath = CreateQrCode(randomString)
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