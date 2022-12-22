namespace TinyUrl.WebApi.Config;

/// <summary>
/// Пример класса с настройками приложения.
/// Поддерживает иерархическую структуру и позволяет легко загружать данные из appsettings.json.
/// Регистрацию класса в IoC и привязку к секции appsettings.json смотри в методе Program.RegisterOptions().
/// 
/// Подробнее о загрузке настроек смотри в https://docs.microsoft.com/ru-ru/aspnet/core/fundamentals/configuration
/// </summary>
public class ServiceConfig
{
	/// <summary>
	/// Пример поля, получаемого из настроек.
	/// </summary>
	public string ServerUrl { get; set; }
}