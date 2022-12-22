using System.Reflection;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;

using Serilog;

using Swashbuckle.AspNetCore.SwaggerGen;

using TinyUrl.WebApi.Config;

string? projectName = Assembly.GetEntryAssembly()?.GetName().Name;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

ConfigureServices(builder.Services);
ConfigureApplication(builder.Build());

void ConfigureServices(IServiceCollection services)
{
	// Add services to the container.
	services.AddControllers();

	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	services.AddEndpointsApiExplorer();
	services.AddMvcCore().AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	}).AddApiExplorer();

	AddSwaggerGen(services);
	AddAuthentication(services);
	AddCors(services);
	RegisterOptions(services, builder.Configuration);
}

static void AddAuthentication(IServiceCollection services)
{
	// Аутентификация Negotiate позволяет пробрасывать пользователя на веб-сервер под управлением Linux
	// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/windowsauth#kestrel

	services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
	services.AddAuthorization(options =>
	{
		// By default, all incoming requests will be authorized according to the default policy.
		options.FallbackPolicy = options.DefaultPolicy;
	});
}

void AddCors(IServiceCollection services)
{
	// https://docs.microsoft.com/ru-ru/aspnet/core/security/cors
	services.AddCors(options => options.AddDefaultPolicy(b => ConfigureCorsPolicy(b, builder.Configuration)));
}

void ConfigureCorsPolicy(CorsPolicyBuilder corsPolicyBuilder, IConfiguration configuration)
{
	string[]? origins = configuration.GetSection("AllowedOrigins").Get<string[]>();
	if (origins?.Any() != true)
		return;

	corsPolicyBuilder.WithOrigins(origins)
		.AllowAnyHeader()
		.AllowAnyMethod();
}

static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
{
	services.AddOptions<ServiceConfig>().Bind(configuration.GetSection(nameof(ServiceConfig)));
}

void AddSwaggerGen(IServiceCollection services)
{
	services.AddSwaggerGen(c =>
	{
		c.SwaggerDoc("v1", new OpenApiInfo
		{
			Title = projectName,
			Version = "v1",
			Description = "Шаблон сервиса REST API",
			Contact = new OpenApiContact
			{
				Name = "GitLab Repository",
				Url = new Uri("http://gitlab.antereal.com/repos/templates")
			}
		});
		c.SupportNonNullableReferenceTypes();

		IncludeXmlComments(c);
	});
}

void IncludeXmlComments(SwaggerGenOptions options)
{
	string baseDirectory = AppContext.BaseDirectory;
	var xmlPaths = new List<string>
	{
		Path.Combine(baseDirectory, $"{projectName}.xml"),
	};

	xmlPaths.ForEach(s =>
	{
		if (File.Exists(s))
			options.IncludeXmlComments(s);
	});
}

void ConfigureApplication(WebApplication app)
{
	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseDeveloperExceptionPage();
	}

	app.UseExceptionHandler("/error");

	app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
	app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", $"{projectName} v1"));

	app.UseSerilogRequestLogging();

	app.UseHttpsRedirection();
	app.UseRouting();
	app.UseCors();

	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();

	app.Run();
}
