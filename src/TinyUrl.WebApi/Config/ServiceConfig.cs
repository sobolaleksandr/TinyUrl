namespace TinyUrl.WebApi.Config;

/// <summary>
/// Класс с настройками приложения.
/// Поддерживает иерархическую структуру и позволяет легко загружать данные из appsettings.json.
/// Регистрацию класса в IoC и привязку к секции appsettings.json смотри в методе Program.RegisterOptions().
/// 
/// Подробнее о загрузке настроек смотри в https://docs.microsoft.com/ru-ru/aspnet/core/fundamentals/configuration
/// </summary>
public class ServiceConfig
{
	public string ServerUrl { get; set; }

	public int UrlLength { get; set; }

	public string ImagesFolderPath { get; set; }
}