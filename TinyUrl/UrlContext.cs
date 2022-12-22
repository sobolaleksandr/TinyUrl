using Microsoft.EntityFrameworkCore;

using TinyUrl.WebApi.Models;


namespace TinyUrl
{
    public sealed class UrlContext : DbContext
	{
		public DbSet<Url> Urls { get; set; }

		public UrlContext(DbContextOptions<UrlContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
	}
}
