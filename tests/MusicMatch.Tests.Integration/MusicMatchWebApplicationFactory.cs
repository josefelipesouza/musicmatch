using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MusicMatch.Application.Interfaces;
using MusicMatch.Infrastructure.Persistence;
using Moq;

namespace MusicMatch.Tests.Integration;

public class MusicMatchWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<AppDbContext>();
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("MusicMatchTestDb_" + Guid.NewGuid()));

            services.RemoveAll<IMessageService>();
            services.AddSingleton(new Mock<IMessageService>().Object);

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
        });

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Authentication:Google:ClientId"] = "test-client-id",
                ["Authentication:Google:ClientSecret"] = "test-client-secret",
                ["Authentication:Jwt:Issuer"] = "test-issuer",
                ["Authentication:Jwt:Audience"] = "test-audience",
                ["Authentication:Jwt:Secret"] = "test-secret-key-com-pelo-menos-32-caracteres",
                ["RabbitMQ:Host"] = "localhost",
                ["ConnectionStrings:DefaultConnection"] = "test"
            });
        });
    }
}
