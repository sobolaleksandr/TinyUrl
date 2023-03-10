using System.Reflection;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Serilog;

using Swashbuckle.AspNetCore.SwaggerGen;

using TinyUrl.WebApi.Config;
using TinyUrl.WebApi.Data;
using TinyUrl.WebApi.Repositories;
using TinyUrl.WebApi.Services;


string? projectName = Assembly.GetEntryAssembly()?.GetName().Name;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

ConfigureServices(builder.Services);
ConfigureApplication(builder.Build());

void ConfigureServices(IServiceCollection services)
{
	string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
	services.AddDbContext<UrlContext>(options => options.UseSqlServer(connection));
	services.AddScoped<IUrlRepository, UrlRepository>();
	services.AddScoped<IUrlService, UrlService>();

	// Add services to the container.
	services.AddControllers();

	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	services.AddEndpointsApiExplorer();
	services.AddMvcCore().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); }).AddApiExplorer();

	AddSwaggerGen(services);
	AddCors(services);
	RegisterOptions(services, builder.Configuration);
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
			Description = "Шаблон сервиса REST API"
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
		Path.Combine(baseDirectory, $"{projectName}.xml")
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
		app.UseDeveloperExceptionPage();

	app.UseExceptionHandler("/error");

	app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
	app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", $"{projectName} v1"));

	app.UseSerilogRequestLogging();

	app.UseHttpsRedirection();
	app.UseRouting();
	app.UseCors();

	app.UseAuthorization();

	app.MapControllers();

	app.Run();
}