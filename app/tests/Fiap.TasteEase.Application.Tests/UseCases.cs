using Amazon.CognitoIdentityProvider;
using Fiap.TasteEase.Application.Ports;
using Fiap.TasteEase.Application.UseCases.ClientUseCase;
using Fiap.TasteEase.Domain.Aggregates.ClientAggregate;
using Fiap.TasteEase.Domain.DTOs;
using Fiap.TasteEase.Domain.Models;
using Fiap.TasteEase.Domain.Ports;
using FluentAssertions;
using FluentResults;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Expressions;

namespace Fiap.TasteEase.Application.Tests
{
    public class UseCases
    {
        [Fact]
        public async Task Handle_Should_Return_Success_Result_With_Client_Id_When_Client_Creation_Succeeds()
        {
            // Arrange
            var request = new Create{ Name = "Test Client", TaxpayerNumber = "123456789" };
            var clientId = Guid.NewGuid();
            var clientRepositoryMock = new Mock<IClientRepository>();
            var identityProviderMock = new Mock<IAmazonCognitoIdentityProvider>();
            var awsSettingsMock = new Mock<IOptions<AwsSettings>>();
            var clientListResult = Result.Ok(new List<Client>().AsEnumerable());
            var clientResult = Result.Ok(new Client(new ClientProps("", "", DateTime.Now, DateTime.Now)));

            awsSettingsMock.Setup(x => x.Value).Returns(new AwsSettings());
            clientRepositoryMock.Setup(
                x => x.Get(It.IsAny<Expression<Func<ClientModel, bool>>>(), It.IsAny<Expression<Func<ClientModel, EntityModel>>[]>()))
                .Returns(Task.FromResult(clientListResult));

            clientRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(clientResult);

            var clientHandler = new ClientHandler(clientRepositoryMock.Object, identityProviderMock.Object, awsSettingsMock.Object);

            // Act
            var result = await clientHandler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();
        }
    }
}