using BoDi;
using Fiap.TasteEase.Domain.DTOs;
using Fiap.TasteEase.Infra.Context;
using Fiap.TasteEase.Presentation.Tests.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace Fiap.TasteEase.Presentation.Tests.Hooks
{
    [Binding]
    public class ClientHooks
    {
        private readonly IObjectContainer _objectContainer;
        private const string AppSettingsFile = "appsettings.json";

        public ClientHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public async Task RegisterServices()
        {
            var factory = GetWebApplicationFactory();
            await ClearData(factory);
            _objectContainer.RegisterInstanceAs(factory);
            var jsonFilesRepo = new JsonFilesRepository();
            _objectContainer.RegisterInstanceAs(jsonFilesRepo);
        }

        private WebApplicationFactory<Program> GetWebApplicationFactory() =>
            new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    IConfigurationSection? configSection = null;
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), AppSettingsFile));
                        configSection = context.Configuration.GetSection(nameof(AwsSettings));
                    });
                    builder.ConfigureTestServices(services =>
                        services.Configure<AwsSettings>(configSection));
                });

        private async Task ClearData(WebApplicationFactory<Program> factory)
        {
            using var scope = factory.Services.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            appContext.RemoveRange(appContext.Clients);
            await appContext.SaveChangesAsync();
        }
    }
}