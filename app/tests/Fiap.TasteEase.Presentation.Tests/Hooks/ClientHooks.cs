using BoDi;
using Fiap.TasteEase.Application.Ports;
using Fiap.TasteEase.Domain.Aggregates.ClientAggregate;
using Fiap.TasteEase.Domain.DTOs;
using Fiap.TasteEase.Domain.Models;
using Fiap.TasteEase.Domain.Ports;
using Fiap.TasteEase.Presentation.Tests.Repositories;
using FluentResults;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq.Expressions;
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
            _objectContainer.RegisterInstanceAs(factory);
            var jsonFilesRepo = new JsonFilesRepository();
            _objectContainer.RegisterInstanceAs(jsonFilesRepo);

            var client = Client.Create(new CreateClientProps("test", "test"));
            var list = new List<Client>();
            list.Add(client.Value);
            IEnumerable<Client> clients = list;
            var clientRepositoryMock = new Mock<IClientRepository>();
            clientRepositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<ClientModel, bool>>>(), It.IsAny<Expression<Func<ClientModel, EntityModel>>[]>()))
                        .Returns(Task.FromResult(
                             Result.Ok(clients)
                            ));
        }

        private WebApplicationFactory<Program> GetWebApplicationFactory() =>
            new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    IConfigurationSection? configSection = null;
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        builder.UseEnvironment("Test");

                        config.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), AppSettingsFile));
                        configSection = context.Configuration.GetSection(nameof(AwsSettings));
                    });
                    builder.ConfigureTestServices(services =>
                    {
                        services.Configure<AwsSettings>(configSection);

                        var list = new List<Client>();
                        IEnumerable<Client> clients = list;
                        var clientRepositoryMock = new Mock<IClientRepository>();
                        clientRepositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<ClientModel, bool>>>(), It.IsAny<Expression<Func<ClientModel, EntityModel>>[]>()))
                                    .Returns(Task.FromResult(
                                         Result.Ok(clients)
                                        ));

                        services.AddSingleton(clientRepositoryMock.Object);
                    }
                    );
                });
    }
}