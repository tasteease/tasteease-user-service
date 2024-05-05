using Fiap.TasteEase.Domain.Aggregates.ClientAggregate;
using Fiap.TasteEase.Domain.Aggregates.ClientAggregate.ValueObjects;
using Fiap.TasteEase.Domain.Models;
using FluentAssertions;

namespace Fiap.TasteEase.Domain.Tests
{
    public partial class AggregateTests
    {
        [Fact(DisplayName = "Constructor Should Set Properties Correctly")]
        [Trait("Aggregate", "Client")]
        public void Constructor_Should_Set_Properties_Correctly()
        {
            // Arrange
            var props = new ClientProps("Test Client", "123456789", DateTime.UtcNow, DateTime.UtcNow);

            // Act
            var client = new Client(props);

            // Assert
            client.Props.Should().NotBeNull();
            client.Name.Should().Be(props.Name);
            client.TaxpayerNumber.Should().Be(props.TaxpayerNumber);
            client.CreatedAt.Should().Be(props.CreatedAt);
            client.UpdatedAt.Should().Be(props.UpdatedAt);
        }

        [Fact(DisplayName = "Create Method Should Create Client")]
        [Trait("Aggregate", "Client")]
        public void Create_Method_Should_Create_Client()
        {
            // Arrange
            var props = new CreateClientProps("Test Client", "123456789");

            // Act
            var result = Client.Create(props);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var client = result.Value;
            client.Should().NotBeNull();
            client.Props.Should().NotBeNull();
            client.Name.Should().Be(props.Name);
            client.TaxpayerNumber.Should().Be(props.TaxpayerNumber);
            client.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(5));
            client.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, precision: TimeSpan.FromSeconds(5)); 
        }

        [Fact(DisplayName = "Rehydrate Method Should Rehydrate From Props And Id")]
        [Trait("Aggregate", "Client")]
        public void Rehydrate_Method_Should_Rehydrate_Client_From_Props_And_Id()
        {
            // Arrange
            var props = new ClientProps("Test Client", "123456789", DateTime.UtcNow, DateTime.UtcNow);
            var id = new ClientId(Guid.NewGuid());

            // Act
            var result = Client.Rehydrate(props, id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var client = result.Value;
            client.Should().NotBeNull();
            client.Props.Should().NotBeNull();
            client.Name.Should().Be(props.Name);
            client.TaxpayerNumber.Should().Be(props.TaxpayerNumber);
            client.CreatedAt.Should().Be(props.CreatedAt);
            client.UpdatedAt.Should().Be(props.UpdatedAt);
            client.Id.Should().Be(id);
        }

        [Fact(DisplayName = "Rehydrate From Model Method Should Rehydrate Client From Model")]
        [Trait("Aggregate", "Client")]
        public void Rehydrate_From_Model_Method_Should_Rehydrate_Client_From_Model()
        {
            // Arrange
            var model = new ClientModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Client",
                TaxpayerNumber = "123456789",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            var result = Client.Rehydrate(model);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var client = result.Value;
            client.Should().NotBeNull();
            client.Props.Should().NotBeNull();
            client.Name.Should().Be(model.Name);
            client.TaxpayerNumber.Should().Be(model.TaxpayerNumber);
            client.CreatedAt.Should().Be(model.CreatedAt);
            client.UpdatedAt.Should().Be(model.UpdatedAt);
            client.Id.Should().Be(new ClientId(model.Id));
        }
    }
}